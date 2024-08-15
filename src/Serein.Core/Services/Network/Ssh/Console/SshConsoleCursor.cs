using Spectre.Console;

namespace Serein.Core.Services.Network.Ssh.Console;

public class SshConsoleCursor(SshPty sshPty) : IAnsiConsoleCursor
{
    private readonly SshPty _sshPty = sshPty;

    public void Move(CursorDirection direction, int steps)
    {
        _sshPty.MoveCursor(direction, steps);
    }

    public void SetPosition(int column, int line)
    {
        _sshPty.SetCursor(column, line);
    }

    public void Show(bool show)
    {
        if (show)
            _sshPty.ShowCursor();
        else
            _sshPty.HideCursor();
    }
}