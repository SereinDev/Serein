using System.IO;
using System.Windows.Forms;

using Microsoft.Extensions.DependencyInjection;

using Serein.Core;
using Serein.Lite.Utils;

namespace Serein.Lite.Ui.Controls;

public class ConsoleWebBrowser : WebBrowser
{
    public ConsoleWebBrowser()
    {
        if (!File.Exists("Serein/console/index.html"))
            SereinApp.Current?.Services.GetRequiredService<ResourcesManager>().WriteConsoleHtml();

        Navigate(@"file:\\\" + Path.GetFullPath(@"Serein\console\index.html"));
    }

    public void AppendHtmlLine(string html)
    {
        Document?.InvokeScript("appendText", [html]);
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
        AppendHtmlLine("\u0006clear");
    }
}
