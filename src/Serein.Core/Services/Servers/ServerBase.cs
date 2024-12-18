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

public abstract class ServerBase
{
    [JsonIgnore]
    public string Id { get; }
    public RestartStatus RestartStatus { get; protected set; }
    public abstract bool Status { get; }
    public abstract int? Pid { get; }
    public IServerInfo Info => _info;
    public IReadOnlyList<string> CommandHistory => _commandHistory;
    public int CommandHistoryIndex { get; internal set; }
    public Configuration Configuration { get; }
    public ServerPluginManager PluginManager { get; }

    protected CancellationTokenSource? _restartCancellationTokenSource;
    protected bool _isTerminated;
    protected Process? _process;
    protected TimeSpan _prevProcessCpuTime = TimeSpan.Zero;
    protected readonly LogWriter _logWriter;
    protected readonly Matcher _matcher;
    protected readonly EventDispatcher _eventDispatcher;
    protected readonly ReactionTrigger _reactionManager;
    protected readonly ILogger _logger;
    protected readonly SettingProvider _settingProvider;
    protected readonly List<string> _commandHistory;
    protected readonly List<string> _cache;
    protected readonly System.Timers.Timer _updateTimer;
    protected readonly ServerInfo _info;
    public event EventHandler? ServerStatusChanged;
    public event EventHandler<ServerOutputEventArgs>? ServerOutput;

    protected ServerBase(
        string id,
        Matcher matcher,
        ILogger<Server> logger,
        ILogger<LogWriter> writerLogger,
        Configuration configuration,
        SettingProvider settingManager,
        EventDispatcher eventDispatcher,
        ReactionTrigger reactionManager
    )
    {
        Id = id;
        _logWriter = new(writerLogger, string.Format(PathConstants.ServerLogDirectory, id));
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
        _info = new();
        PluginManager = new(this);

        ServerStatusChanged += (_, _) => UpdateInfo();
    }

    protected abstract void StartProcess();

    protected abstract void TerminateProcess();

    protected abstract void WriteLine(byte[] bytes);

    protected void OnServerStatusChanged()
    {
        ServerStatusChanged?.Invoke(this, EventArgs.Empty);

        if (Status)
        {
            WriteInfoLine($"“{Configuration.FileName}”启动中");
        }
    }

    protected void OnServerExit(int exitCode)
    {
        _updateTimer.Stop();
        _logger.LogDebug("Id={}: 进程（PID={}）退出：{}", Id, Pid, exitCode);

        if (Configuration.SaveLog)
        {
            _logWriter.WriteAsync(DateTime.Now.ToString("T") + " 进程退出：" + exitCode);
        }

        WriteInfoLine($"进程已退出，退出代码为 {exitCode} (0x{exitCode:x8})");

        if (
            RestartStatus == RestartStatus.Waiting
            || RestartStatus == RestartStatus.None
                && exitCode != 0
                && Configuration.AutoRestart
                && !_isTerminated
        )
        {
            Task.Run(WaitAndRestart);
        }

        ServerStatusChanged?.Invoke(this, EventArgs.Empty);

        if (!_eventDispatcher.Dispatch(Event.ServerExited, this, exitCode, DateTime.Now))
        {
            return;
        }

        _reactionManager.TriggerAsync(
            exitCode == 0
                ? ReactionType.ServerExitedNormally
                : ReactionType.ServerExitedUnexpectedly,
            new(Id)
        );
    }

    protected void WriteInfoLine(string line)
    {
        ServerOutput?.Invoke(this, new(ServerOutputType.Information, line));
    }

    protected void WriteErrorLine(string line)
    {
        ServerOutput?.Invoke(this, new(ServerOutputType.Error, line));
        _logger.LogError("Failed to start server with Pty: {}", line);
    }

    protected void OnServerOutput(string line)
    {
        _logger.LogDebug("Id={}: 输出'{}'", Id, line);
        if (Configuration.SaveLog)
        {
            _logWriter.WriteAsync(line);
        }

        _info.OutputLines++;

        ServerOutput?.Invoke(this, new(ServerOutputType.Raw, line));

        if (!_eventDispatcher.Dispatch(Event.ServerRawOutput, this, line))
        {
            return;
        }

        var filtered = OutputFilter.Clear(line);

        if (!_eventDispatcher.Dispatch(Event.ServerOutput, this, filtered))
        {
            return;
        }

        _cache.Add(filtered);

        if (_cache.Count > 1)
        {
            _matcher.QueueServerOutputLine(Id, string.Join('\n', _cache));
        }

        if (
            !_settingProvider.Value.Application.PattenForEnableMatchingMuiltLines.Any(
                filtered.Contains
            )
        )
        {
            _cache.Clear();
        }
        else if (_cache.Count > 100)
        {
            _cache.RemoveRange(0, _cache.Count - 100);
        }

        _matcher.QueueServerOutputLine(Id, filtered);
    }

    public void Start()
    {
        _logger.LogDebug("Id={}: 请求启动", Id);

        if (Status)
        {
            throw new InvalidOperationException("服务器已在运行");
        }

        if (string.IsNullOrEmpty(Configuration.FileName))
        {
            throw new InvalidOperationException("启动文件为空");
        }

        if (!_eventDispatcher.Dispatch(Event.ServerStarting, this))
        {
            return;
        }

        StartProcess();

        RestartStatus = RestartStatus.None;

        _info.OutputLines = 0;
        _info.InputLines = 0;
        _info.StartTime = DateTime.Now;
        _info.ExitTime = null;
        _info.FileName = File.Exists(Configuration.FileName)
            ? Path.GetFileName(Configuration.FileName)
            : Configuration.FileName;

        _isTerminated = false;
        _prevProcessCpuTime = TimeSpan.Zero;
        _commandHistory.Clear();
        _restartCancellationTokenSource?.Cancel();

        _reactionManager.TriggerAsync(ReactionType.ServerStart, new(Id));
        _eventDispatcher.Dispatch(Event.ServerStarted, this);
        _updateTimer.Start();

        _logger.LogDebug("Id={}: 正在启动", Id);

        if (Configuration.SaveLog)
        {
            _logWriter.WriteAsync(DateTime.Now.ToString("T") + " 服务器已启动");
        }
    }

    public void Stop()
    {
        _logger.LogDebug("Id={}: 请求关闭", Id);

        if (CancelRestart())
        {
            return;
        }

        if (!Status)
        {
            throw new InvalidOperationException("服务器未运行");
        }

        if (!_eventDispatcher.Dispatch(Event.ServerStopping, this))
        {
            return;
        }

        if (Configuration.StopCommands.Length == 0)
        {
            if (
                SereinApp.Type is AppType.Lite or AppType.Plus
                && Environment.OSVersion.Platform == PlatformID.Win32NT
                && Pid is not null
            )
            {
                WriteInfoLine("当前未设置关服命令，将发送Ctrl+C事件作为替代");

                var pid = (uint)Pid.Value;

                NativeMethods.AttachConsole(pid);
                NativeMethods.GenerateConsoleCtrlEvent(NativeMethods.CtrlTypes.CTRL_C_EVENT, pid);
                NativeMethods.FreeConsole();

                _logger.LogDebug("Id={}: 尝试发送Ctrl+C事件", Id);
            }
            else
            {
                throw new NotSupportedException("关服命令为空");
            }
        }

        foreach (string command in Configuration.StopCommands)
        {
            if (!string.IsNullOrEmpty(command))
            {
                Input(command);
            }
        }

        _logger.LogDebug("Id={}: 正在关闭", Id);
    }

    internal void InputFromCommand(string command, EncodingMap.EncodingType? encodingType = null)
    {
        if (Status)
        {
            Input(command, encodingType, false);
        }
        else if (command == "start")
        {
            Start();
        }
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

    internal void Input(string command, EncodingMap.EncodingType? encodingType, bool fromUser)
    {
        if (!Status)
        {
            return;
        }

        _logger.LogDebug(
            "Id={}: command='{}'; encodingType={}; fromUser={}",
            Id,
            command,
            encodingType,
            fromUser
        );

        if (!_eventDispatcher.Dispatch(Event.ServerInput, this, command))
        {
            return;
        }

        WriteLine(
            EncodingMap
                .GetEncoding(encodingType ?? Configuration.InputEncoding)
                .GetBytes(
                    (Configuration.UseUnicodeChars ? command.ToUnicode() : command)
                        + Configuration.LineTerminator
                )
        );

        _info.InputLines++;

        if (fromUser && !string.IsNullOrEmpty(command))
        {
            if (
                _commandHistory.Count > 0 && _commandHistory[^1] != command
                || _commandHistory.Count == 0
            )
            {
                _commandHistory.Add(command);
            }

            if (SereinApp.Type != AppType.Cli)
            {
                ServerOutput?.Invoke(this, new(ServerOutputType.InputCommand, command));
            }
        }

        CommandHistoryIndex = CommandHistory.Count;

        _matcher.QueueServerInputLine(Id, command);
    }

    public void Terminate()
    {
        _logger.LogDebug("Id={}: 请求强制结束", Id);

        if (CancelRestart())
        {
            return;
        }

        if (!Status)
        {
            throw new InvalidOperationException("服务器未运行");
        }

        TerminateProcess();
        _isTerminated = true;
    }

    protected bool CancelRestart()
    {
        if (
            _restartCancellationTokenSource is not null
            && !_restartCancellationTokenSource.IsCancellationRequested
        )
        {
            _restartCancellationTokenSource.Cancel();
            _logger.LogDebug("Id={}: 取消重启", Id);
            WriteInfoLine("重启已取消");
            return true;
        }

        return false;
    }

    public void RequestRestart()
    {
        _logger.LogDebug("Id={}: 请求重启", Id);

        if (RestartStatus != RestartStatus.None)
        {
            throw new InvalidOperationException("正在等待重启");
        }

        Stop();

        RestartStatus = RestartStatus.Waiting;
    }

    private void WaitAndRestart()
    {
        RestartStatus = RestartStatus.Preparing;
        _restartCancellationTokenSource?.Dispose();
        _restartCancellationTokenSource = new();

        WriteInfoLine($"将在五秒后({DateTime.Now.AddSeconds(5):T})重启服务器");

        Task.Delay(5000, _restartCancellationTokenSource.Token)
            .ContinueWith(
                (task) =>
                {
                    RestartStatus = RestartStatus.None;
                    if (!task.IsCanceled)
                    {
                        try
                        {
                            Start();
                        }
                        catch (Exception e)
                        {
                            ServerOutput?.Invoke(this, new(ServerOutputType.Error, e.Message));
                        }
                    }
                }
            );
    }

    private async Task UpdateInfo()
    {
        if (!Status && _process is null)
        {
            _info.Argument = null;
            _info.FileName = null;
            _info.StartTime = null;
            _info.Stat = null;
            _info.OutputLines = 0;
            _info.InputLines = 0;
            _info.CPUUsage = 0;
            return;
        }
        else if (_process is null)
        {
            return;
        }

        _info.CPUUsage = (int)(
            (_process.TotalProcessorTime - _prevProcessCpuTime).TotalMilliseconds
            / 2000
            / Environment.ProcessorCount
            * 100
        );
        _prevProcessCpuTime = _process.TotalProcessorTime;

        if (Configuration.PortIPv4 >= 0)
        {
            await Task.Run(() => _info.Stat = new("127.0.0.1", (ushort)Configuration.PortIPv4));
        }
    }
}
