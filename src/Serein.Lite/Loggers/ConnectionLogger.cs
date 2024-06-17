using System.Drawing;

using Microsoft.Extensions.Logging;

using Serein.Core.Models.Output;
using Serein.Lite.Extensions;
using Serein.Lite.Ui.Function;

namespace Serein.Lite.Loggers;

public class ConnectionLogger(ConnectionPage connectionPage) : IConnectionLogger
{
    public readonly object Lock = new();
    private readonly ConnectionPage _connectionPage = connectionPage;

    public void Log(LogLevel level, string message)
    {
        _connectionPage.Invoke(() =>
        {
            lock (Lock)
            {
                switch (level)
                {
                    case LogLevel.Information:
                        _connectionPage.ConsoleRichTextBox.AppendText($"[Info] ");
                        break;
                    case LogLevel.Warning:
                        _connectionPage.ConsoleRichTextBox.AppendTextWithColor(
                            "[Warn] ",
                            Color.Red
                        );
                        break;
                    case LogLevel.Error:
                        _connectionPage.ConsoleRichTextBox.AppendTextWithColor(
                            "[Error]",
                            Color.Red
                        );
                        break;
                    default:
                        return;
                }
                _connectionPage.ConsoleRichTextBox.AppendText(message);
                _connectionPage.ConsoleRichTextBox.ScrollToEnd();
            }
        });
    }
}
