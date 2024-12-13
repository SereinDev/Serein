using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Logging;
using Serein.Core.Models.Server;
using Serein.Core.Services.Commands;
using Serein.Core.Services.Data;
using Serein.Core.Services.Plugins;
using Serein.Core.Utils;

namespace Serein.Core.Services.Servers;

public sealed class Server : ServerBase
{
    public override bool Status => _process is not null && !_process.HasExited;
    public override int? Pid => _process?.Id;
    private BinaryWriter? _inputWriter;

    internal Server(
        string id,
        Matcher matcher,
        ILogger<Server> logger,
        ILogger<LogWriter> writerLogger,
        Configuration configuration,
        SettingProvider settingManager,
        EventDispatcher eventDispatcher,
        ReactionTrigger reactionManager
    )
        : base(
            id,
            matcher,
            logger,
            writerLogger,
            configuration,
            settingManager,
            eventDispatcher,
            reactionManager
        ) { }

    protected override void StartProcess()
    {
        var psi = new ProcessStartInfo
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
        };
        foreach (var (key, value) in Configuration.Environment)
        {
            psi.Environment[key] = value;
        }

        _process = Process.Start(psi);
        _process!.EnableRaisingEvents = true;

        _inputWriter = new(_process.StandardInput.BaseStream);

        _process.BeginOutputReadLine();
        _process.BeginErrorReadLine();

        _process.Exited += OnExit;
        _process.ErrorDataReceived += OnOutputDataReceived;
        _process.OutputDataReceived += OnOutputDataReceived;

        OnServerStatusChanged();
    }

    protected override void TerminateProcess()
    {
        _process?.Kill(true);
    }

    private void OnExit(object? sender, EventArgs e)
    {
        OnServerExit(_process?.ExitCode ?? 0);
        _info.ExitTime = _process?.ExitTime;
        _process = null;
    }

    private void OnOutputDataReceived(object? sender, DataReceivedEventArgs e)
    {
        if (e.Data is null)
        {
            return;
        }

        OnServerOutput(e.Data);
    }

    protected override void WriteLine(byte[] bytes)
    {
        if (_inputWriter is null || !Status)
        {
            return;
        }

        _inputWriter.Write(bytes);
        _inputWriter.Flush();
    }
}
