using System.Windows.Media;

using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;

namespace Serein.Plus.Ui.Utils;

public class LogLevelColorizer : DocumentColorizingTransformer
{
    public const char Prefix = '\a';

    protected override void ColorizeLine(DocumentLine line)
    {
        var text = CurrentContext.Document.GetText(line);
        var i = text.IndexOf(']');

        if (!text.StartsWith(Prefix) || text.Length < 3 || text[1] != '[' | i < 0)
            return;

        var level = text[2..i];
        var brush = level.ToLowerInvariant() switch
        {
            "info" => Info,
            "warn" => Warn,
            "error" => Error,
            "sent" => Sent,
            "recv" => Received,
            "received" => Received,
            _ => null,
        };

        if (brush is null)
            return;

        ChangeLinePart(
            line.Offset + 1,
            line.Offset + i + 1,
            (element) => element.TextRunProperties.SetForegroundBrush(brush)
        );
    }

    private static readonly SolidColorBrush Info = new(Color.FromRgb(0x00, 0xaf, 0xff));
    private static readonly SolidColorBrush Warn = new(Color.FromRgb(0xaf, 0x5f, 0x00));
    private static readonly SolidColorBrush Error = new(Color.FromRgb(0xd7, 0x00, 0x00));
    private static readonly SolidColorBrush Sent = new(Color.FromRgb(0x00, 0x5f, 0xd7));
    private static readonly SolidColorBrush Received = new(Color.FromRgb(0x5f, 0xd7, 0x00));
}
