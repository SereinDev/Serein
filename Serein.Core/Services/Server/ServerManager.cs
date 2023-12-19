using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using System.Timers;

using Microsoft.Extensions.Hosting;

using Serein.Core.Models;
using Serein.Core.Models.Server;
using Serein.Core.Services.Data;
using Serein.Core.Utils;

namespace Serein.Core.Services.Server;

public class ServerManager
{
    public ServerStatus Status =>
        _serverProcess is null
            ? ServerStatus.Unknown
            : _serverProcess.HasExited
                ? ServerStatus.Stopped
                : ServerStatus.Running;
    public int? PID => _serverProcess?.Id;
    public DateTime? StartTime => _serverProcess?.StartTime;
    public IServerInfo? ServerInfo => _serverInfo;

    public IReadOnlyList<string> CommandHistory => _commandHistory;
    public int CommandHistoryIndex { get; private set; }
    private readonly List<string> _commandHistory = new();

    private readonly Timer _updateTimer;
    private StreamWriter? _inputWriter;
    private Process? _serverProcess;
    private RestartStatus _restartStatus;
    private ServerInfo? _serverInfo;
    private TimeSpan _prevProcessCpuTime = TimeSpan.Zero;

    private readonly IHost _host;
    private IServiceProvider Services => _host.Services;
    private readonly IOutputHandler _output;
    private readonly Matcher _matcher;
    private readonly SettingProvider _settingProvider;

    public ServerManager(
        IHost host,
        IOutputHandler output,
        SettingProvider settingManager,
        Matcher matcher
    )
    {
        _host = host;
        _settingProvider = settingManager;
        _output = output;
        _matcher = matcher;
        _updateTimer = new(2000) { AutoReset = true };
        _updateTimer.Elapsed += (_, _) => UpdateInfo();
        _updateTimer.Start();
    }

    public void Start()
    {
        if (Status == ServerStatus.Running)
            throw new InvalidOperationException("服务器已在运行");

        _serverProcess = Process.Start(
            new ProcessStartInfo
            {
                FileName = _settingProvider.Value.Server.Path,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                StandardOutputEncoding = EncodingMap.GetEncoding(
                    _settingProvider.Value.Server.OutputEncoding
                ),
                StandardErrorEncoding = EncodingMap.GetEncoding(
                    _settingProvider.Value.Server.OutputEncoding
                ),
                WorkingDirectory = Path.GetDirectoryName(_settingProvider.Value.Server.Path),
                Arguments = _settingProvider.Value.Server.Argument ?? string.Empty
            }
        );
        _serverProcess!.EnableRaisingEvents = true;

        _restartStatus = RestartStatus.None;
        _serverInfo = new();
        _commandHistory.Clear();
        _prevProcessCpuTime = TimeSpan.Zero;

        _inputWriter = new(
            _serverProcess.StandardInput.BaseStream,
            EncodingMap.GetEncoding(_settingProvider.Value.Server.InputEncoding)
        )
        {
            AutoFlush = true,
            NewLine = _settingProvider.Value.Server.LineTerminator
                .Replace("\\n", "\n")
                .Replace("\\r", "\r")
        };

        _serverProcess.BeginOutputReadLine();
        _serverProcess.BeginErrorReadLine();
        _serverProcess.OutputDataReceived += OnOutputDataReceived;
        _serverProcess.ErrorDataReceived += OnOutputDataReceived;
        _serverProcess.Exited += OnExit;

        _output.LogServerNotice($"“{_settingProvider.Value.Server.Path}”启动中");
    }

    public void Stop(CallerType callerType)
    {
        if (Status != ServerStatus.Running)
            throw new InvalidOperationException("服务器未运行");

        foreach (string command in _settingProvider.Value.Server.StopCommands)
        {
            if (!string.IsNullOrEmpty(command))
            {
                Input(command, callerType);
            }
        }
    }

    public void Input(string command, CallerType callerType)
    {
        if (_inputWriter is null || Status != ServerStatus.Running)
        {
            if (callerType == CallerType.Command)
                if (command == "start")
                    Start();
                else if (command == "stop")
                    _restartStatus = RestartStatus.None;
            return;
        }

        _inputWriter.WriteLine(command);

        if (
            (
                _commandHistory.Count > 0 && _commandHistory[^1] != command
                || _commandHistory.Count == 0
            )
            && callerType == CallerType.User
            && !string.IsNullOrEmpty(command)
        )
            _commandHistory.Add(command);
        CommandHistoryIndex = CommandHistory.Count;

        _matcher.MatchServerInputAsync(command);
    }

    public bool TryStart()
    {
        try
        {
            Start();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool TryStart([NotNullWhen(false)] out Exception? e)
    {
        e = null;
        try
        {
            Start();
            return true;
        }
        catch (Exception ee)
        {
            e = ee;
            return false;
        }
    }

    private void OnExit(object? sender, EventArgs e)
    {
        var exitCode = _serverProcess?.ExitCode;
        _serverProcess = null;
        _output.LogServerNotice($"进程已退出，退出代码为 {exitCode} (0x{exitCode:x8})");

        if (
            _restartStatus == RestartStatus.Waiting
            || _restartStatus == RestartStatus.None && exitCode != 0
        )
            _ = WaitAndRestartAsync();
    }

    private void OnOutputDataReceived(object? sender, DataReceivedEventArgs e)
    {
        if (e.Data is null)
            return;

        _output.LogServerOriginalOutput(e.Data);
        _matcher.MatchServerOutputAsync(OutputFilter.Clear(e.Data));
    }

    private async Task WaitAndRestartAsync()
    {
        var i = 0;

        _restartStatus = RestartStatus.Preparing;
        _output.LogServerNotice($"将在五秒后({DateTime.Now.AddSeconds(5):T})重启服务器");
        while (i < 50 && _restartStatus == RestartStatus.Preparing)
        {
            await Task.Delay(100);
            i++;
        }

        TryStart();
    }

    private void UpdateInfo()
    {
        _serverInfo ??= new();

        if (Status != ServerStatus.Running || _serverProcess is null)
        {
            _serverInfo.Argument = null;
            _serverInfo.FileName = null;
            _serverInfo.ExitTime = null;
            _serverInfo.StartTime = null;
            _serverInfo.Motd = null;
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
    }
}