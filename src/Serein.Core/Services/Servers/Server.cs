using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Serein.Core.Models.Commands;
using Serein.Core.Models.Plugins;
using Serein.Core.Models.Server;
using Serein.Core.Services.Commands;
using Serein.Core.Services.Data;
using Serein.Core.Services.Plugins;
using Serein.Core.Services.Servers.ProcessSpawners;
using Serein.Core.Utils;
using Serein.Core.Utils.Extensions;

namespace Serein.Core.Services.Servers;

public class Server
{
    [JsonIgnore]
    public string Id { get; }
    public RestartStatus RestartStatus { get; private set; }

    public virtual bool Status =>
        _commonProcessSpawner is { IsValueCreated: true, Value.Status: true }
        || _ptyProcessSpawner is { IsValueCreated: true, Value.Status: true };

    public int? Pid { get; private set; }
    public IServerInfo Info => _info;
    public IReadOnlyList<string> CommandHistory => _commandHistory;
    public int CommandHistoryIndex { get; internal set; }
    public Configuration Configuration { get; }
    public ServerPluginManager PluginManager { get; }
    public ServerLogger Logger { get; }

    private CancellationTokenSource? _restartCancellationTokenSource;
    private bool _isTerminated;
    private TimeSpan _prevProcessCpuTime = TimeSpan.Zero;
    private readonly LogWriter _logWriter;
    private readonly Matcher _matcher;
    private readonly EventDispatcher _eventDispatcher;
    private readonly ReactionTrigger _reactionManager;
    private readonly ILogger _logger;
    private readonly SereinApp _sereinApp;
    private readonly SettingProvider _settingProvider;
    private readonly List<string> _commandHistory;
    private readonly List<string> _cache;
    private readonly System.Timers.Timer _updateTimer;
    private readonly ServerInfo _info;
    protected readonly Lazy<PtyProcessSpawner> _ptyProcessSpawner;
    protected readonly Lazy<CommonProcessSpawner> _commonProcessSpawner;

    public event EventHandler? StatusChanged;

    protected internal Server(
        string id,
        ILogger<Server> logger,
        ILogger<LogWriter> writerLogger,
        SereinApp sereinApp,
        Matcher matcher,
        Configuration configuration,
        SettingProvider settingManager,
        EventDispatcher eventDispatcher,
        ReactionTrigger reactionManager
    )
    {
        Id = id;
        _logWriter = new(writerLogger, string.Format(PathConstants.ServerLogDirectory, id));
        _logger = logger;
        _sereinApp = sereinApp;
        Configuration = configuration;
        _settingProvider = settingManager;
        _matcher = matcher;
        _eventDispatcher = eventDispatcher;
        _reactionManager = reactionManager;
        _commandHistory = [];
        _cache = [];
        _updateTimer = new(2000) { AutoReset = true };
        _updateTimer.Elapsed += (_, _) => Task.Run(UpdateInfo);
        _info = new();
        PluginManager = new(this);
        Logger = new(id);

        StatusChanged += (_, _) => Task.Run(UpdateInfo);

        _ptyProcessSpawner = new(() =>
        {
            var spawner = new PtyProcessSpawner(Id, _sereinApp, _logWriter, Logger, logger);
            spawner.StatusChanged += (_, _) => OnServerStatusChanged();
            spawner.ProcessExited += (_, exitCode) => OnServerExit(exitCode);
            return spawner;
        });
        _commonProcessSpawner = new(() =>
        {
            var spawner = new CommonProcessSpawner(Logger);
            spawner.StatusChanged += (_, _) => OnServerStatusChanged();
            spawner.ProcessExited += (_, exitCode) => OnServerExit(exitCode);
            return spawner;
        });

        Logger.Output += (_, e) =>
        {
            if (e.Type == ServerOutputType.StandardOutput)
            {
                OnServerOutput(e.Data);
            }
        };
    }

    public virtual IProcessSpawner? GetCurrentProcessSpawner()
    {
        if (_commonProcessSpawner is { IsValueCreated: true, Value.Status: true })
        {
            return _commonProcessSpawner.Value;
        }
        else if (_ptyProcessSpawner is { IsValueCreated: true, Value.Status: true })
        {
            return _ptyProcessSpawner.Value;
        }

        return null;
    }

    private void OnServerStatusChanged()
    {
        StatusChanged?.Invoke(this, EventArgs.Empty);

        if (Status)
        {
            Logger.WriteInternalInfo($"“{Configuration.FileName}”启动中");
            Pid = GetCurrentProcessSpawner()!.CurrentProcess?.Id;
        }
    }

    private void OnServerExit(int exitCode)
    {
        _updateTimer.Stop();
        _logger.LogDebug("Id={}: 进程（PID={}）退出：{}", Id, Pid, exitCode);

        if (Configuration.SaveLog)
        {
            _logWriter.WriteAsync(DateTime.Now.ToString("T") + " 进程退出：" + exitCode);
        }

        Logger.WriteInternalInfo($"进程已退出，退出代码为 {exitCode} (0x{exitCode:x8})");

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

        StatusChanged?.Invoke(this, EventArgs.Empty);

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

        Pid = null;
    }

    private void OnServerOutput(string line)
    {
        _logger.LogDebug("Id={}: 输出'{}'", Id, line);
        if (Configuration.SaveLog)
        {
            _logWriter.WriteAsync(line);
        }

        _info.OutputLines++;

        if (!_eventDispatcher.Dispatch(Event.ServerRawOutput, this, line))
        {
            return;
        }

        var filtered = OutputFilter.Clean(line);

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
        Logger.ClearHistory();
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

    protected virtual void StartProcess()
    {
        if (Configuration.Pty.IsEnabled)
        {
            _ptyProcessSpawner.Value.Start(Configuration);
        }
        else
        {
            _commonProcessSpawner.Value.Start(Configuration);
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
                _sereinApp.Type is AppType.Lite or AppType.Plus
                && Environment.OSVersion.Platform == PlatformID.Win32NT
                && Pid is not null
            )
            {
                Logger.WriteInternalInfo("当前未设置关服命令，将发送Ctrl+C事件作为替代");

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

    internal void InputFromCommand(string command, bool? useUnicodeChars)
    {
        if (Status)
        {
            Input(command, useUnicodeChars);
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

    public void Input(string command, bool? useUnicodeChars = null)
    {
        Input(command, true, useUnicodeChars);
    }

    protected internal void Input(string command, bool fromUser, bool? useUnicodeChars = null)
    {
        if (!Status)
        {
            return;
        }

        _logger.LogDebug("Id={}: command='{}'; fromUser={}", Id, command, fromUser);

        if (!_eventDispatcher.Dispatch(Event.ServerInput, this, command))
        {
            return;
        }

        GetCurrentProcessSpawner()!
            .Write(
                EncodingMap
                    .GetEncoding(Configuration.InputEncoding)
                    .GetBytes(
                        (
                            useUnicodeChars ?? Configuration.UseUnicodeChars
                                ? command.ToUnicode()
                                : command
                        ) + Configuration.LineTerminator
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

            if (_sereinApp.Type != AppType.Cli)
            {
                Logger.WriteStandardInput(command);
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

        GetCurrentProcessSpawner()!.Terminate();
        _isTerminated = true;
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
            Logger.WriteInternalInfo("重启已取消");
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

        Logger.WriteInternalInfo($"将在五秒后({DateTime.Now.AddSeconds(5):T})重启服务器");

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
                            Logger.WriteInternalError(e.Message);
                        }
                    }
                }
            );
    }

    private void UpdateInfo()
    {
        var process = GetCurrentProcessSpawner()?.CurrentProcess;
        if (!Status && process is null)
        {
            _info.Argument = null;
            _info.FileName = null;
            _info.StartTime = null;
            _info.Stat = null;
            _info.OutputLines = 0;
            _info.InputLines = 0;
            _info.CpuUsage = 0;
            return;
        }
        else if (process is null || process.HasExited)
        {
            return;
        }

        _info.CpuUsage = (int)(
            (process.TotalProcessorTime - _prevProcessCpuTime).TotalMilliseconds
            / 2000
            / Environment.ProcessorCount
            * 100
        );
        _prevProcessCpuTime = process.TotalProcessorTime;

        if (Configuration.PortIPv4 != 0)
        {
            _info.Stat = new("127.0.0.1", Configuration.PortIPv4);
        }
    }
}
