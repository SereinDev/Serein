using System;
using System.Drawing;

using Microsoft.Extensions.Logging;

using Serein.Core.Models.Output;
using Serein.Lite.Extensions;
using Serein.Lite.Ui.Function;

namespace Serein.Lite.Loggers;

public class PluginLogger(PluginPage pluginPage) : IPluginLogger
{
    private readonly PluginPage _pluginPage = pluginPage;

    private readonly object _lock = new();

    public void Log(LogLevel level, string name, string message)
    {
        _pluginPage.Invoke(() =>
        {
            lock (_lock)
            {
                switch (level)
                {
                    case LogLevel.Information:
                        _pluginPage.ConsoleRichTextBox.AppendText($"[Info] ");
                        break;
                    case LogLevel.Warning:
                        _pluginPage.ConsoleRichTextBox.AppendTextWithColor("[Warn] ", Color.Red);
                        break;
                    case LogLevel.Error:
                        _pluginPage.ConsoleRichTextBox.AppendTextWithColor("[Error]", Color.Red);
                        break;
                    default:
                        return;
                }
                _pluginPage.ConsoleRichTextBox.AppendText(
                    $"[{name}] {message}{Environment.NewLine}"
                );
                _pluginPage.ConsoleRichTextBox.ScrollToEnd();
            }
        });
    }
}
