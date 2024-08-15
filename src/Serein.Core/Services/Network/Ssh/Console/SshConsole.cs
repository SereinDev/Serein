using System.Text;

using Serein.Core.Utils;

using Spectre.Console;
using Spectre.Console.Rendering;

namespace Serein.Core.Services.Network.Ssh.Console;

public class SshConsole : IAnsiConsole
{
    private readonly object _lock = new();
    private readonly SshPty _sshPty;

    public SshConsole(SshPty sshPty)
    {
        _sshPty = sshPty;

        Pipeline = new();
        Input = new SshConsoleInput(_sshPty);
        Cursor = new SshConsoleCursor(_sshPty);
        Profile = new(new SshConsoleOutput(_sshPty), EncodingMap.UTF8);
        ExclusivityMode = new SshExclusivityMode();

        if (sshPty.WidthChars > 0)
            Profile.Width = (int)sshPty.WidthChars;
        if (sshPty.HeightChars > 0)
            Profile.Height = (int)sshPty.HeightChars;
    }

    public Profile Profile { get; }
    public IAnsiConsoleCursor Cursor { get; }
    public IAnsiConsoleInput Input { get; }
    public IExclusivityMode ExclusivityMode { get; }
    public RenderPipeline Pipeline { get; }

    public void Clear(bool home)
    {
        lock (_lock)
            if (home)
                _sshPty.Clear();
    }

    public void Write(IRenderable renderable)
    {
        var stringBuilder = new StringBuilder();

        foreach (var segment in renderable.GetSegments(this))
        {
            // if (segment.IsControlCode)
            // {
            //     stringBuilder.Append(segment.Text);
            //     continue;
            // }

            stringBuilder.Append(segment.Text);

            // var parts = segment.Text.Normalize(NormalizationForm.FormC)
        }

        Profile.Out.Writer.Write(stringBuilder);
        Profile.Out.Writer.Flush();
    }
}