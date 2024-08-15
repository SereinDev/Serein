using System.Collections.Generic;
using System.IO;
using System.Text;

using Serein.Core.Utils;

namespace Serein.Core.Services.Network.Ssh.Console;

public class SshConsoleTextWriter(SshPty sshPty) : TextWriter
{
    private readonly SshPty _sshPty = sshPty;

    public override Encoding Encoding { get; } = EncodingMap.UTF8;

    private readonly List<string> _buffer = [];

    public override void Write(string? value)
    {
        if (value != null)
            lock (_buffer)
                _buffer.Add(value);
    }

    public override void Flush()
    {
        lock (_buffer)
        {
            _sshPty.Send(string.Join(string.Empty, _buffer));
            _buffer.Clear();
        }
    }
}