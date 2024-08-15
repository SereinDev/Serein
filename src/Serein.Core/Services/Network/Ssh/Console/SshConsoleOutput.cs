using System.IO;
using System.Text;

using Spectre.Console;

namespace Serein.Core.Services.Network.Ssh.Console;

public class SshConsoleOutput(SshPty sshPty) : IAnsiConsoleOutput
{
    private readonly SshPty _sshPty = sshPty;

    public TextWriter Writer { get; } = new SshConsoleTextWriter(sshPty);

    public bool IsTerminal => true;

    public int Width => (int)_sshPty.WidthChars;

    public int Height => (int)_sshPty.HeightChars;

    public void SetEncoding(Encoding encoding) { }
}