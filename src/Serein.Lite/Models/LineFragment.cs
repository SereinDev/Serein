using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serein.Lite.Models;

public record LineFragment(string Text)
{
    public List<string> Classes { get; } = new();

    public Dictionary<string, string> Styles { get; } = new();

    public void SetBold() => Styles["font-weight"] = "bold";

    public void SetItalic() => Styles["font-style"] = "italic";

    public void SetUnderline() => Styles["text-decoration"] = "underline";

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine("<span ");

        if (Classes.Count > 0)
            sb.Append($"class=\"{string.Join('\x20', Classes)}\" ");

        if (Styles.Count > 0)
            sb.Append(
                $"style=\"{string.Join('\x20', Styles.Select((s) => $"{s.Key}: {s.Key};"))}\" "
            );

        sb.AppendLine(">");
        sb.AppendLine(Text);
        sb.AppendLine("</span>");

        return sb.ToString();
    }
}
