using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

using Microsoft.Extensions.Logging;

using Serein.Core.Models.Output;
using Serein.Core.Models.Plugins;
using Serein.Core.Models.Server;
using Serein.Core.Models.Settings;
using Serein.Core.Services.Data;
using Serein.Core.Services.Plugins;
using Serein.Core.Utils;

namespace Serein.Core.Services.Servers;

public class Server
{
    public ServerStatus Status =>
        _serverProcess is null
            ? ServerStatus.Unknown
            : _serverProcess.HasExited
                ? ServerStatus.Stopped
                : ServerStatus.Running;
    public int? Pid => _serverProcess?.Id;
    public IServerInfo ServerInfo => _serverInfo;
    public IReadOnlyList<string> CommandHistory => _commandHistory;
    public int CommandHistoryIndex { get; private set; }
    public Configuration Configuration { get; }

    private readonly List<string> _commandHistory;
    private readonly List<string> _cache;
    private readonly Timer _updateTimer;
    private readonly ServerInfo _serverInfo;
    private BinaryWriter? _inputWriter;
    private Process? _serverProcess;
    private RestartStatus _restartStatus;
    private TimeSpan _prevProcessCpuTime = TimeSpan.Zero;
    private bool _isTerminated;
    private readonly Matcher _matcher;
    private readonly EventDispatcher _eventDispatcher;
    private readonly ReactionManager _reactionManager;
    private readonly string _id;
    private readonly ISereinLogger _logger;
    private readonly SettingProvider _settingProvider;
    public event EventHandler? ServerStatusChanged;
    public event EventHandler<ServerOutputEventArgs>? ServerOutput;

    public Server(
        string id,
        ISereinLogger logger,
        Configuration configuration,
        SettingProvider settingManager,
        Matcher matcher,
        EventDispatcher eventDispatcher,
        ReactionManager reactionManager
    )
    {
        _id = id;
        _logger = logger;
        Configuration = configuration;
        _settingProvider = settingManager;
        _matcher = matcher;
        _eventDispatcher = eventDispatcher;
        _reactionManager = reactionManager;
        _commandHistory = new();
        _cache = new();
        _updateTimer = new(2000) { AutoReset = true };
        _updateTimer.Elapsed += (_, _) => UpdateInfo();
        _updateTimer.Start();
        _serverInfo = new();
    }

    public void Start()
    {
        if (Status == ServerStatus.Running)
            throw new InvalidOperationException("服务器已在运行");

        if (string.IsNullOrEmpty(Configuration.FileName))
            throw new InvalidOperationException("启动文件为空");

        if (!_eventDispatcher.Dispatch(Event.ServerStarting, _id))
            return;

        _serverProcess = Process.Start(
            new ProcessStartInfo
            {
                FileName = Configuration.FileName,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                StandardOutputEncoding = EncodingMap.GetEncoding(Configuration.OutputEncoding),
                StandardErrorEncoding = EncodingMap.GetEncoding(Configuration.OutputEncoding),
                WorkingDirectory = Path.GetDirectoryName(Configuration.FileName),
                Arguments = Configuration.Argument ?? string.Empty
            }
        );
        _serverProcess!.EnableRaisingEvents = true;
        _isTerminated = true;
        _restartStatus = RestartStatus.None;
        _serverInfo.OutputLines = 0;
        _serverInfo.InputLines = 0;
        _serverInfo.StartTime = _serverProcess.StartTime;
        _serverInfo.FileName = File.Exists(Configuration.FileName)
            ? Path.GetFileName(Configuration.FileName)
            : Configuration.FileName;
        _commandHistory.Clear();
        _prevProcessCpuTime = TimeSpan.Zero;

        _inputWriter = new(_serverProcess.StandardInput.BaseStream);

        _serverProcess.BeginOutputReadLine();
        _serverProcess.BeginErrorReadLine();

        _serverProcess.Exited += OnExit;
        _serverProcess.ErrorDataReceived += OnOutputDataReceived;
        _serverProcess.OutputDataReceived += OnOutputDataReceived;

        ServerStatusChanged?.Invoke(null, EventArgs.Empty);
        ServerOutput?.Invoke(
            this,
            new(ServerOutputType.Information, $"“{Configuration.FileName}”启动中")
        );
        _reactionManager.TriggerAsync(ReactionType.ServerStart);
        _eventDispatcher.Dispatch(Event.ServerStarted, _id);
    }

    public void Stop()
    {
        if (Status != ServerStatus.Running)
            throw new InvalidOperationException("服务器未运行");

        if (!_eventDispatcher.Dispatch(Event.ServerStopping, _id))
            return;

        foreach (string command in Configuration.StopCommands)
        {
            if (!string.IsNullOrEmpty(command))
            {
                Input(command);
            }
        }
    }

    internal void InputFromCommand(string command, EncodingMap.EncodingType? encodingType = null)
    {
        if (Status == ServerStatus.Running)
            Input(command, encodingType);
        else if (command == "start")
            Start();
        else if (command == "stop")
            _restartStatus = RestartStatus.None;
    }

    public void Input(
        string command,
        EncodingMap.EncodingType? encodingType = null,
        bool fromUser = false
    )
    {
        if (_inputWriter is null || Status != ServerStatus.Running)
            return;

        if (!_eventDispatcher.Dispatch(Event.ServerInput, _id, command))
            return;

        _inputWriter.Write(
            EncodingMap
                .GetEncoding(encodingType ?? Configuration.InputEncoding)
                .GetBytes(
                    command + Configuration.LineTerminator.Replace("\\n", "\n").Replace("\\r", "\r")
                )
        );
        _inputWriter.Flush();

        _serverInfo.InputLines++;

        if (
            (
                _commandHistory.Count > 0 && _commandHistory[^1] != command
                || _commandHistory.Count == 0
            )
            && fromUser
            && !string.IsNullOrEmpty(command)
        )
        {
            _commandHistory.Add(command);

            if (SereinApp.Type != AppType.Cli)
                ServerOutput?.Invoke(this, new(ServerOutputType.InputCommand, command));
        }

        CommandHistoryIndex = CommandHistory.Count;

        _matcher.MatchServerInputAsync(command);
    }

    public void Terminate()
    {
        if (Status != ServerStatus.Running)
            throw new InvalidOperationException("服务器未运行");

        _serverProcess?.Kill(true);
        _isTerminated = true;
    }

    private void OnExit(object? sender, EventArgs e)
    {
        var exitCode = _serverProcess?.ExitCode ?? 0;
        ServerOutput?.Invoke(
            this,
            new(ServerOutputType.Information, $"进程已退出，退出代码为 {exitCode} (0x{exitCode:x8})")
        );

        if (Configuration.AutoRestart && !_isTerminated)
            if (
                _restartStatus == RestartStatus.Waiting
                || _restartStatus == RestartStatus.None && exitCode != 0
            )
                _ = WaitAndRestartAsync();

        if (!_eventDispatcher.Dispatch(Event.ServerExited, _id, exitCode, DateTime.Now))
            return;

        _serverInfo.ExitTime = _serverProcess?.ExitTime;
        _serverProcess = null;

        _reactionManager.TriggerAsync(
            exitCode == 0
                ? ReactionType.ServerExitedNormally
                : ReactionType.ServerExitedUnexpectedly
        );
        ServerStatusChanged?.Invoke(null, EventArgs.Empty);
    }

    private void OnOutputDataReceived(object? sender, DataReceivedEventArgs e)
    {
        if (e.Data is null)
            return;

        _serverInfo.OutputLines++;

        ServerOutput?.Invoke(this, new(ServerOutputType.Raw, e.Data));

        if (!_eventDispatcher.Dispatch(Event.ServerRawOutput, _id, e.Data))
            return;

        var filtered = OutputFilter.Clear(e.Data);

        if (!_eventDispatcher.Dispatch(Event.ServerOutput, _id, filtered))
            return;

        if (
            _settingProvider.Value.Application.PattenForEnableMatchMuiltLines.Any(filtered.Contains)
        )
        {
            _cache.Add(filtered);
            _matcher.MatchServerOutputAsync(string.Join('\n', _cache));
        }
        else
            _cache.Clear();

        _matcher.MatchServerOutputAsync(filtered);
    }

    private async Task WaitAndRestartAsync()
    {
        var i = 0;

        _restartStatus = RestartStatus.Preparing;
        ServerOutput?.Invoke(
            this,
            new(ServerOutputType.Information, $"将在五秒后({DateTime.Now.AddSeconds(5):T})重启服务器")
        );
        while (i < 50 && _restartStatus == RestartStatus.Preparing)
        {
            await Task.Delay(100);
            i++;
        }

        try
        {
            Start();
        }
        catch (Exception e)
        {
            _logger.LogWarning("[{}] 重启失败：{}", _id, e.Message);
        }
    }

    private async Task UpdateInfo()
    {
        if (Status != ServerStatus.Running || _serverProcess is null)
        {
            _serverInfo.Argument = null;
            _serverInfo.FileName = null;
            _serverInfo.StartTime = null;
            _serverInfo.Stat = null;
            _serverInfo.OutputLines = 0;
            _serverInfo.InputLines = 0;
            _serverInfo.CPUUsage = 0;
            return;
        }

        _serverInfo.CPUUsage =
            (_serverProcess.TotalProcessorTime - _prevProcessCpuTime).TotalMilliseconds
            / 2000
            / Environment.ProcessorCount
            * 100;
        _prevProcessCpuTime = _serverProcess.TotalProcessorTime;

        if (Configuration.IPv4Port >= 0)
            await Task.Run(
                () => _serverInfo.Stat = new("127.0.0.1", (ushort)Configuration.IPv4Port)
            );
    }
}
