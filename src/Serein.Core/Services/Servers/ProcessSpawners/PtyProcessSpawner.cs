using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Pty.Net;
using Serein.Core.Models.Server;
using Serein.Core.Services.Data;
using Serein.Core.Utils;

namespace Serein.Core.Services.Servers.ProcessSpawners;

internal sealed class PtyProcessSpawner(
    string id,
    SereinApp sereinApp,
    LogWriter logWriter,
    ServerLogger serverLogger,
    ILogger<Server> logger
) : IProcessSpawner
{
    public Process? CurrentProcess { get; private set; }

    public bool Status => _ptyConnection is not null && !_isPreparing;

    public event EventHandler? StatusChanged;
    public event EventHandler<int>? ProcessExited;

    private CancellationTokenSource _cancellationTokenSource = new();
    private IPtyConnection? _ptyConnection;
    private StreamReader? _streamReader;
    private bool _isPreparing;

    public void Start(Configuration configuration)
    {
        if (_cancellationTokenSource.IsCancellationRequested)
        {
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = new();
        }

        _isPreparing = true;

        var cwd = Path.GetDirectoryName(configuration.FileName);
        if (string.IsNullOrEmpty(cwd))
        {
            cwd = Directory.GetCurrentDirectory();
        }

        PtyProvider
            .SpawnAsync(
                new()
                {
                    Name = id,
                    App = configuration.FileName,
                    CommandLine = [configuration.Argument],
                    Cwd = cwd,
                    ForceWinPty = Environment.OSVersion.Platform == PlatformID.Win32NT,
                    Environment = configuration.Environment,

                    Rows =
                        sereinApp.Type == AppType.Cli
                        && Environment.OSVersion.Platform == PlatformID.Win32NT
                        && configuration.Pty.TerminalHeight is null
                            ? Console.WindowHeight
                            : configuration.Pty.TerminalHeight ?? 80,
                    Cols =
                        sereinApp.Type == AppType.Cli
                        && Environment.OSVersion.Platform == PlatformID.Win32NT
                        && configuration.Pty.TerminalHeight is null
                            ? Console.WindowHeight
                            : configuration.Pty.TerminalWidth ?? 150,
                },
                _cancellationTokenSource.Token
            )
            .ContinueWith(
                (task) =>
                {
                    if (task.IsFaulted)
                    {
                        if (configuration.SaveLog)
                        {
                            logWriter.WriteAsync(task.Exception.ToString());
                        }
                        serverLogger.WriteInternalError(task.Exception.Message);
                        return;
                    }

                    _isPreparing = false;
                    _ptyConnection = task.Result;
                    CurrentProcess = Process.GetProcessById(_ptyConnection.Pid);
                    _ptyConnection.ProcessExited += (_, e) =>
                    {
                        try
                        {
                            _ptyConnection?.Dispose();
                        }
                        catch (Exception ex)
                        {
                            serverLogger.WriteInternalError(
                                "在处理进程退出时发生异常：" + ex.Message
                            );
                            logger.LogError(ex, "在处理进程退出时发生异常");
                        }

                        _ptyConnection = null;
                        _streamReader?.Close();
                        _cancellationTokenSource.Cancel();

                        ProcessExited?.Invoke(this, e.ExitCode);
                    };
                    _streamReader = new(
                        _ptyConnection.ReaderStream,
                        EncodingMap.GetEncoding(configuration.OutputEncoding)
                    );

                    StatusChanged?.Invoke(this, EventArgs.Empty);
                    serverLogger.WriteInternalInfo(
                        "正在使用虚拟终端启动服务器进程。若出现问题请尝试关闭虚拟终端功能。"
                    );
                    ReadLineLoop();
                }
            );
    }

    public void Terminate()
    {
        CurrentProcess?.Kill(true);
    }

    public void Write(byte[] bytes)
    {
        _ptyConnection?.WriterStream.Write(bytes);
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
                    serverLogger.WriteStandardOutput(line);
                }
            }
            catch (ObjectDisposedException)
            {
                break;
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception e)
            {
                serverLogger.WriteInternalError(e.Message);
                logger.LogDebug(e, "读取服务器输出时发生错误");
            }
        }

        _streamReader?.Dispose();
    }
}
