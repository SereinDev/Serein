using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serein.Lite.Models;

public record LineFragment(string Text)
{
    public LineFragment(string text, LineFragment from)
        : this(text)
    {
        Styles = new(from.Styles);
        Classes = [.. from.Classes];
    }

    public List<string> Classes { get; } = [];

    public Dictionary<string, string> Styles { get; } = [];

    public void Reset()
    {
        Classes.Clear();
        Styles.Clear();
    }

    public void SetBold() => Styles["font-weight"] = "bold";

    public void SetItalic() => Styles["font-style"] = "italic";

    public void SetUnderline() => Styles["text-decoration"] = "underline";

    public override string ToString()
    {
        if (string.IsNullOrEmpty(Text))
        {
            return string.Empty;
        }

        var sb = new StringBuilder();
        sb.Append("<span");

        if (Classes.Count > 0)
        {
            sb.Append($" class=\"{string.Join('\x20', Classes)}\"");
        }

        if (Styles.Count > 0)
        {
            sb.Append(
                $" style=\"{string.Join('\x20', Styles.Select((s) => $"{s.Key}: {s.Value};"))}\""
            );
        }

        sb.Append('>');
        sb.Append(Text);
        sb.Append("</span>");

        return sb.ToString();
    }
}
