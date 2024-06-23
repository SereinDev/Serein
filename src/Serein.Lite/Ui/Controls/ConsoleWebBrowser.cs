using System.Windows.Forms;

using Serein.Lite.Utils;

namespace Serein.Lite.Ui.Controls;

public class ConsoleWebBrowser : WebBrowser
{
    public void AppendHtmlLine(string html)
    {
        Document?.InvokeScript("appendText", new[] { html });
    }

    public void AppendLine(string text)
    {
        AppendHtmlLine(LogColorizer.EscapeLog(text));
    }

    public void AppendNotice(string text)
    {
        AppendHtmlLine(
            $"<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>{LogColorizer.EscapeLog(text)}"
        );
    }

    public void AppendInfo(string text)
    {
        AppendHtmlLine("[Info]" + LogColorizer.EscapeLog(text));
    }

    public void AppendWarn(string text)
    {
        AppendHtmlLine(
            $"<span style=\"color:#9c8022;font-weight: bold;\">[Warn]</span>{LogColorizer.EscapeLog(text)}"
        );
    }

    public void AppendError(string text)
    {
        AppendHtmlLine(
            $"<span style=\"color:#BA4A00;font-weight: bold;\">[Error]</span>{LogColorizer.EscapeLog(text)}"
        );
    }

    public void ClearLines()
    {
        AppendHtmlLine("\u1145clear");
    }
}
