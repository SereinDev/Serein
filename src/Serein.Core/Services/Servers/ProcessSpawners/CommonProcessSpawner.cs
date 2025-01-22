using System;
using System.Diagnostics;
using System.IO;
using Serein.Core.Models.Server;
using Serein.Core.Utils;

namespace Serein.Core.Services.Servers.ProcessSpawners;

internal sealed class CommonProcessSpawner(ServerLogger serverLogger) : IProcessSpawner
{
    public Process? CurrentProcess { get; private set; }

    public bool Status => CurrentProcess is not null && !CurrentProcess.HasExited;

    private BinaryWriter? _inputWriter;

    public event EventHandler? StatusChanged;
    public event EventHandler<int>? ProcessExited;

    public void Start(Configuration configuration)
    {
        var psi = new ProcessStartInfo
        {
            FileName = configuration.FileName,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            RedirectStandardInput = true,
            StandardOutputEncoding = EncodingMap.GetEncoding(configuration.OutputEncoding),
            StandardErrorEncoding = EncodingMap.GetEncoding(configuration.OutputEncoding),
            WorkingDirectory = Path.GetDirectoryName(configuration.FileName),
            Arguments = configuration.Argument,
        };
        foreach (var (key, value) in configuration.Environment)
        {
            psi.Environment[key] = value;
        }

        CurrentProcess = Process.Start(psi);
        CurrentProcess!.EnableRaisingEvents = true;

        StatusChanged?.Invoke(this, EventArgs.Empty);

        _inputWriter = new(CurrentProcess.StandardInput.BaseStream);

        CurrentProcess.BeginOutputReadLine();
        CurrentProcess.BeginErrorReadLine();

        CurrentProcess.Exited += (_, e) => ProcessExited?.Invoke(this, CurrentProcess.ExitCode);
        CurrentProcess.ErrorDataReceived += OnOutputDataReceived;
        CurrentProcess.OutputDataReceived += OnOutputDataReceived;
    }

    public void Terminate()
    {
        if (CurrentProcess is null)
        {
            return;
        }

        CurrentProcess.Kill();
    }

    public void Write(byte[] data)
    {
        _inputWriter?.Write(data);
        _inputWriter?.Flush();
    }

    private void OnOutputDataReceived(object? sender, DataReceivedEventArgs e)
    {
        if (e.Data is null)
        {
            return;
        }

        serverLogger.WriteStandardOutput(e.Data);
    }
}
