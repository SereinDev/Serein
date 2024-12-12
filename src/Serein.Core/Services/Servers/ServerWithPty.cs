using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Pty.Net;
using Serein.Core.Models.Server;
using Serein.Core.Services.Commands;
using Serein.Core.Services.Data;
using Serein.Core.Services.Plugins;
using Serein.Core.Utils;

namespace Serein.Core.Services.Servers;

public class ServerWithPty(
    string id,
    Matcher matcher,
    ILogger<Server> logger,
    ILogger<LogWriter> writerLogger,
    Configuration configuration,
    SettingProvider settingManager,
    EventDispatcher eventDispatcher,
    ReactionTrigger reactionManager
)
    : ServerBase(
        id,
        matcher,
        logger,
        writerLogger,
        configuration,
        settingManager,
        eventDispatcher,
        reactionManager
    )
{
    private CancellationTokenSource _cancellationTokenSource = new();
    private IPtyConnection? _ptyConnection;
    private StreamReader? _streamReader;
    private bool _isPreparing;

    public override bool Status => _ptyConnection is not null && !_isPreparing;

    public override int? Pid => _ptyConnection?.Pid;

    protected override void StartProcess()
    {
        if (_cancellationTokenSource.IsCancellationRequested)
        {
            _cancellationTokenSource = new();
        }

        _isPreparing = true;
        PtyProvider
            .SpawnAsync(
                new()
                {
                    Name = Id,
                    App = Configuration.FileName,
                    CommandLine = [Configuration.Argument],
                    Cwd =
                        Path.GetDirectoryName(Configuration.FileName)
                        ?? Directory.GetCurrentDirectory(),
                    ForceWinPty = Environment.OSVersion.Platform == PlatformID.Win32NT,

                    Rows =
                        SereinApp.Type == AppType.Cli
                        && Environment.OSVersion.Platform == PlatformID.Win32NT
                        && Configuration.TerminalHeight is null
                            ? Console.WindowHeight
                            : Configuration.TerminalHeight ?? 80,
                    Cols =
                        SereinApp.Type == AppType.Cli
                        && Environment.OSVersion.Platform == PlatformID.Win32NT
                        && Configuration.TerminalHeight is null
                            ? Console.WindowHeight
                            : Configuration.TerminalWidth ?? 150,
                },
                _cancellationTokenSource.Token
            )
            .ContinueWith(
                (task) =>
                {
                    if (task.IsFaulted)
                    {
                        if (Configuration.SaveLog)
                        {
                            _logWriter.WriteAsync(task.Exception.ToString());
                        }
                        WriteErrorLine(task.Exception.Message);
                        return;
                    }

                    _isPreparing = false;
                    _ptyConnection = task.Result;
                    _process = Process.GetProcessById(_ptyConnection.Pid);
                    _ptyConnection.ProcessExited += (_, e) =>
                    {
                        _ptyConnection = null;
                        _process = null;
                        OnServerExit(e.ExitCode);
                        _cancellationTokenSource.Cancel();
                    };
                    _streamReader = new(
                        _ptyConnection.ReaderStream,
                        EncodingMap.GetEncoding(Configuration.OutputEncoding)
                    );

                    OnServerStatusChanged();
                    WriteInfoLine(
                        "正在使用虚拟终端启动服务器进程。若出现问题请尝试关闭虚拟终端功能。"
                    );
                    ReadLineLoop();
                }
            );
    }

    private async Task ReadLineLoop()
    {
        while (
            Status && !_cancellationTokenSource.IsCancellationRequested && _streamReader is not null
        )
        {
            try
            {
                var line = await _streamReader.ReadLineAsync(_cancellationTokenSource.Token);
                if (line is not null)
                {
                    OnServerOutput(line);
                }
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }

        _streamReader?.Dispose();
    }

    protected override void TerminateProcess()
    {
        _process?.Kill(true);
    }

    protected override void WriteLine(byte[] bytes)
    {
        _ptyConnection?.WriterStream.Write(bytes);
    }
}
