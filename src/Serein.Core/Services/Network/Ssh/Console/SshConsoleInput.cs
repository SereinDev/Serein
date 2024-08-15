using System;
using System.Threading;
using System.Threading.Tasks;

using Spectre.Console;

namespace Serein.Core.Services.Network.Ssh.Console;

public class SshConsoleInput(SshPty sshPty) : IAnsiConsoleInput
{
    private readonly SshPty _sshPty = sshPty;

    public bool IsKeyAvailable() => true;

    private ConsoleKeyInfo? WaitKey(bool intercept, CancellationToken cancellationToken = default)
    {
        var lockObj = new object();

        using var handle = new EventWaitHandle(false, EventResetMode.ManualReset);
        cancellationToken.Register(() => handle.Set());

        ConsoleKeyInfo? consoleKeyInfo = null;
        _sshPty.KeyRead += OnKeyRead;
        handle.WaitOne();

        return consoleKeyInfo;

        void OnKeyRead(object? sender, ConsoleKeyInfo? keyInfo)
        {
            lock (lockObj)
            {
                _sshPty.KeyRead -= OnKeyRead;
                consoleKeyInfo = keyInfo;
                handle.Set();

                if (!intercept && keyInfo.HasValue)
                    _sshPty.Send(keyInfo.Value.KeyChar.ToString());
            }
        }
    }

    public ConsoleKeyInfo? ReadKey(bool intercept)
    {
        return WaitKey(intercept);
    }



    public Task<ConsoleKeyInfo?> ReadKeyAsync(bool intercept, CancellationToken cancellationToken)
    {
        return Task.FromResult(WaitKey(intercept, cancellationToken));
    }
}