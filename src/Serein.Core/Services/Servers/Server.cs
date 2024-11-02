using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Serein.Core.Models.Plugins;
using Serein.Core.Models.Server;
using Serein.Core.Models.Settings;
using Serein.Core.Services.Commands;
using Serein.Core.Services.Data;
using Serein.Core.Services.Plugins;
using Serein.Core.Utils;
using Serein.Core.Utils.Extensions;

namespace Serein.Core.Services.Servers;

public class Server
{
    [JsonIgnore]
    public string Id { get; }
    public RestartStatus RestartStatus { get; private set; }
    public bool Status => _serverProcess is not null && !_serverProcess.HasExited;
    public int? Pid => _serverProcess?.Id;
    public IServerInfo Info => _serverInfo;
    public IReadOnlyList<string> CommandHistory => _commandHistory;
    public int CommandHistoryIndex { get; internal set; }
    public Configuration Configuration { get; }
    public ServerPluginManager PluginManager { get; }

    private CancellationTokenSource? _restartCancellationTokenSource;
    private BinaryWriter? _inputWriter;
    private Process? _serverProcess;
    private TimeSpan _prevProcessCpuTime = TimeSpan.Zero;
    private bool _isTerminated;

    private readonly Matcher _matcher;
    private readonly EventDispatcher _eventDispatcher;
    private readonly ReactionTrigger _reactionManager;
    private readonly ILogger _logger;
    private readonly SettingProvider _settingProvider;
    private readonly List<string> _commandHistory;
    private readonly List<string> _cache;
    private readonly System.Timers.Timer _updateTimer;
    private readonly ServerInfo _serverInfo;
    public event EventHandler? ServerStatusChanged;
    public event EventHandler<ServerOutputEventArgs>? ServerOutput;

    internal Server(
        string id,
        Matcher matcher,
        ILogger<Server> logger,
        Configuration configuration,
        SettingProvider settingManager,
        EventDispatcher eventDispatcher,
        ReactionTrigger reactionManager
    )
    {
        Id = id;
        _logger = logger;
        Configuration = configuration;
        _settingProvider = settingManager;
        _matcher = matcher;
        _eventDispatcher = eventDispatcher;
        _reactionManager = reactionManager;
        _commandHistory = [];
        _cache = [];
        _updateTimer = new(2000) { AutoReset = true };
        _updateTimer.Elapsed += (_, _) => UpdateInfo();
        _serverInfo = new();
        PluginManager = new(this);

        ServerStatusChanged += (_, _) => UpdateInfo();
    }

    public void Start()
    {
        _logger.LogDebug("Id={}: 请求启动", Id);

        if (Status)
            throw new InvalidOperationException("服务器已在运行");

        if (string.IsNullOrEmpty(Configuration.FileName))
            throw new InvalidOperationException("启动文件为空");

        if (!_eventDispatcher.Dispatch(Event.ServerStarting, Id))
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
                Arguments = Configuration.Argument,
            }
        );
        _serverProcess!.EnableRaisingEvents = true;
        _isTerminated = false;
        RestartStatus = RestartStatus.None;
        _serverInfo.OutputLines = 0;
        _serverInfo.InputLines = 0;
        _serverInfo.StartTime = _serverProcess.StartTime;
        _serverInfo.ExitTime = null;
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

        ServerStatusChanged?.Invoke(this, EventArgs.Empty);
        _restartCancellationTokenSource?.Cancel();
        ServerOutput?.Invoke(
            this,
            new(ServerOutputType.Information, $"“{Configuration.FileName}”启动中")
        );
        _reactionManager.TriggerAsync(ReactionType.ServerStart, new(Id));
        _eventDispatcher.Dispatch(Event.ServerStarted, Id);
        _updateTimer.Start();

        _logger.LogDebug("Id={}: 正在启动", Id);
    }

    public void Stop()
    {
        _logger.LogDebug("Id={}: 请求关闭", Id);

        if (CancelRestart())
            return;

        if (!Status || _serverProcess is null)
            throw new InvalidOperationException("服务器未运行");

        if (!_eventDispatcher.Dispatch(Event.ServerStopping, Id))
            return;

        if (Configuration.StopCommands.Length == 0)
            if (
                SereinApp.Type is AppType.Lite or AppType.Plus
                && Environment.OSVersion.Platform == PlatformID.Win32NT
            )
            {
                ServerOutput?.Invoke(
                    this,
                    new(
                        ServerOutputType.Information,
                        "当前未设置关服命令，将发送Ctrl+C事件作为替代"
                    )
                );

                NativeMethods.AttachConsole((uint)_serverProcess.Id);
                NativeMethods.GenerateConsoleCtrlEvent(
                    NativeMethods.CtrlTypes.CTRL_C_EVENT,
                    (uint)_serverProcess.Id
                );
                NativeMethods.FreeConsole();

                _logger.LogDebug("Id={}: 发送Ctrl+C事件", Id);
            }
            else
                throw new NotSupportedException("关服命令为空");

        foreach (string command in Configuration.StopCommands)
            if (!string.IsNullOrEmpty(command))
                Input(command);

        _logger.LogDebug("Id={}: 正在关闭", Id);
    }

    internal void InputFromCommand(string command, EncodingMap.EncodingType? encodingType = null)
    {
        if (Status)
            Input(command, encodingType);
        else if (command == "start")
            Start();
        else if (command == "stop")
        {
            RestartStatus = RestartStatus.None;
            _restartCancellationTokenSource?.Cancel();
        }
    }

    public void Input(string command)
    {
        Input(command, null, false);
    }

    internal void Input(
        string command,
        EncodingMap.EncodingType? encodingType = null,
        bool fromUser = false
    )
    {
        _logger.LogDebug(
            "Id={}: command='{}'; encodingType={}; fromUser={}",
            Id,
            command,
            encodingType,
            fromUser
        );

        if (_inputWriter is null || !Status)
            return;

        if (!_eventDispatcher.Dispatch(Event.ServerInput, Id, command))
            return;

        _inputWriter.Write(
            EncodingMap
                .GetEncoding(encodingType ?? Configuration.InputEncoding)
                .GetBytes(
                    (Configuration.UseUnicodeChars ? command.ToUnicode() : command)
                        + Configuration.LineTerminator
                )
        );
        _inputWriter.Flush();

        _serverInfo.InputLines++;

        if (fromUser && !string.IsNullOrEmpty(command))
        {
            if (
                _commandHistory.Count > 0 && _commandHistory[^1] != command
                || _commandHistory.Count == 0
            )
                _commandHistory.Add(command);

            if (SereinApp.Type != AppType.Cli)
                ServerOutput?.Invoke(this, new(ServerOutputType.InputCommand, command));
        }

        CommandHistoryIndex = CommandHistory.Count;

        _matcher.MatchServerInputAsync(Id, command);
    }

    public void RequestRestart()
    {
        _logger.LogDebug("Id={}: 请求重启", Id);

        if (RestartStatus != RestartStatus.None)
            throw new InvalidOperationException("正在等待重启");

        Stop();

        RestartStatus = RestartStatus.Waiting;
    }

    public void Terminate()
    {
        _logger.LogDebug("Id={}: 请求强制结束", Id);

        if (CancelRestart())
            return;

        if (!Status)
            throw new InvalidOperationException("服务器未运行");

        _serverProcess?.Kill(true);
        _isTerminated = true;
    }

    private void OnExit(object? sender, EventArgs e)
    {
        _updateTimer.Stop();
        var exitCode = _serverProcess?.ExitCode ?? 0;
        _logger.LogDebug("Id={}: 进程（PID={}）退出：{}", Id, _serverProcess?.Id, exitCode);

        ServerOutput?.Invoke(
            this,
            new(
                ServerOutputType.Information,
                $"进程已退出，退出代码为 {exitCode} (0x{exitCode:x8})"
            )
        );

        if (
            RestartStatus == RestartStatus.Waiting
            || RestartStatus == RestartStatus.None
                && exitCode != 0
                && Configuration.AutoRestart
                && !_isTerminated
        )
            Task.Run(WaitAndRestart);

        _serverInfo.ExitTime = _serverProcess?.ExitTime;
        _serverProcess = null;

        ServerStatusChanged?.Invoke(this, EventArgs.Empty);

        if (!_eventDispatcher.Dispatch(Event.ServerExited, Id, exitCode, DateTime.Now))
            return;

        _reactionManager.TriggerAsync(
            exitCode == 0
                ? ReactionType.ServerExitedNormally
                : ReactionType.ServerExitedUnexpectedly,
            new(Id)
        );
    }

    private void OnOutputDataReceived(object? sender, DataReceivedEventArgs e)
    {
        if (e.Data is null)
            return;

        _logger.LogDebug("Id={}: 输出'{}'", Id, e.Data);
        _serverInfo.OutputLines++;

        ServerOutput?.Invoke(this, new(ServerOutputType.Raw, e.Data));

        if (!_eventDispatcher.Dispatch(Event.ServerRawOutput, Id, e.Data))
            return;

        var filtered = OutputFilter.Clear(e.Data);

        if (!_eventDispatcher.Dispatch(Event.ServerOutput, Id, filtered))
            return;

        if (
            _settingProvider.Value.Application.PattenForEnableMatchingMuiltLines.Any(
                filtered.Contains
            )
        )
        {
            _cache.Add(filtered);
            _matcher.MatchServerOutputAsync(Id, string.Join('\n', _cache));
        }
        else
            _cache.Clear();

        _matcher.MatchServerOutputAsync(Id, filtered);
    }

    private bool CancelRestart()
    {
        if (
            _restartCancellationTokenSource is not null
            && !_restartCancellationTokenSource.IsCancellationRequested
        )
        {
            _restartCancellationTokenSource.Cancel();
            _logger.LogDebug("Id={}: 取消重启", Id);
            ServerOutput?.Invoke(this, new(ServerOutputType.Information, "重启已取消"));
            return true;
        }

        return false;
    }

    private void WaitAndRestart()
    {
        RestartStatus = RestartStatus.Preparing;
        _restartCancellationTokenSource = new();

        ServerOutput?.Invoke(
            this,
            new(
                ServerOutputType.Information,
                $"将在五秒后({DateTime.Now.AddSeconds(5):T})重启服务器"
            )
        );

        Task.Delay(5000, _restartCancellationTokenSource.Token)
            .ContinueWith(
                (task) =>
                {
                    RestartStatus = RestartStatus.None;
                    if (!task.IsCanceled)
                        try
                        {
                            Start();
                        }
                        catch (Exception e)
                        {
                            ServerOutput?.Invoke(this, new(ServerOutputType.Error, e.Message));
                        }
                }
            );
    }

    private async Task UpdateInfo()
    {
        if (!Status || _serverProcess is null)
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

        _serverInfo.CPUUsage = (int)(
            (_serverProcess.TotalProcessorTime - _prevProcessCpuTime).TotalMilliseconds
            / 2000
            / Environment.ProcessorCount
            * 100
        );
        _prevProcessCpuTime = _serverProcess.TotalProcessorTime;

        if (Configuration.PortIPv4 >= 0)
            await Task.Run(
                () => _serverInfo.Stat = new("127.0.0.1", (ushort)Configuration.PortIPv4)
            );
    }
}
