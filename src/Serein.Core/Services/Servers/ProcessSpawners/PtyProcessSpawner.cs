using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Pty.Net;
using Serein.Core.Models.Server;
using Serein.Core.Services.Data;
using Serein.Core.Utils;
using Serein.Core.Utils.Extensions;

namespace Serein.Core.Services.Servers.ProcessSpawners;

public sealed class PtyProcessSpawner(
    string id,
    SereinApp sereinApp,
    LogWriter logWriter,
    ServerLogger serverLogger,
    ILogger<Server> logger
) : IProcessSpawner
{
    public Process? CurrentProcess { get; private set; }
    public IPtyConnection? PtyConnection { get; private set; }
    public bool Status => PtyConnection is not null && !_isPreparing;

    public event EventHandler? StatusChanged;
    public event EventHandler<int>? ProcessExited;
    public event EventHandler<char>? CharOutput;

    private CancellationTokenSource _cancellationTokenSource = new();
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

        PtyProvider
            .SpawnAsync(
                new()
                {
                    Name = id,
                    App = configuration.FileName,
                    CommandLine = [configuration.Argument],
                    Cwd = StringExtension.SelectValueNotNullOrEmpty(
                        Path.GetDirectoryName(configuration.FileName),
                        Directory.GetCurrentDirectory()
                    )!,
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
                    PtyConnection = task.Result;
                    CurrentProcess = Process.GetProcessById(PtyConnection.Pid);
                    PtyConnection.ProcessExited += (_, e) =>
                    {
                        try
                        {
                            PtyConnection?.Dispose();
                        }
                        catch (Exception ex)
                        {
                            serverLogger.WriteInternalError(
                                "在处理进程退出时发生异常：" + ex.Message
                            );
                            logger.LogError(ex, "在处理进程退出时发生异常");
                        }

                        PtyConnection = null;
                        _streamReader?.Close();
                        _cancellationTokenSource.Cancel();

                        ProcessExited?.Invoke(this, e.ExitCode);
                    };
                    _streamReader = new(
                        PtyConnection.ReaderStream,
                        EncodingMap.GetEncoding(configuration.OutputEncoding)
                    );

                    StatusChanged?.Invoke(this, EventArgs.Empty);
                    serverLogger.WriteInternalInfo(
                        "正在使用虚拟终端启动服务器进程。若出现问题请尝试关闭虚拟终端功能。"
                    );
                    Task.Run(StartReadLineLoop);
                }
            );
    }

    public void Terminate()
    {
        CurrentProcess?.Kill(true);
    }

    public void Write(byte[] bytes)
    {
        PtyConnection?.WriterStream.Write(bytes);
    }

    private void StartReadLineLoop()
    {
        var list = new List<char>();

        while (
            Status && !_cancellationTokenSource.IsCancellationRequested && _streamReader is not null
        )
        {
            try
            {
                var code = _streamReader.Read();

                if (code < 0)
                {
                    break;
                }

                var c = (char)code;

                CharOutput?.Invoke(this, c);

                if (c == '\n' || c == '\r')
                {
                    if (list.Count > 0)
                    {
                        var line = new string([.. list]).TrimEnd('\r', '\n');
                        list.Clear();
                        serverLogger.WriteStandardOutput(line);
                    }
                }
                else
                {
                    list.Add(c);
                }
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
