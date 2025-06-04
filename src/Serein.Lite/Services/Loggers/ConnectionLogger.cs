using Microsoft.Extensions.Logging;
using Serein.Core.Models.Abstractions;
using Serein.Lite.Ui.Function;
using Serein.Lite.Utils;

namespace Serein.Lite.Services.Loggers;

public sealed class ConnectionLogger(ConnectionPage connectionPage) : IConnectionLogger
{
    private readonly object _lock = new();

    public void Log(LogLevel level, string message)
    {
        connectionPage.Invoke(() =>
        {
            lock (_lock)
            {
                switch (level)
                {
                    case LogLevel.Information:
                        connectionPage.ConsoleWebBrowser.AppendLine($"[Info] {message}");
                        break;
                    case LogLevel.Warning:
                        connectionPage.ConsoleWebBrowser.AppendWarn(message);
                        break;
                    case LogLevel.Error:
                        connectionPage.ConsoleWebBrowser.AppendError(message);
                        break;
                    default:
                        return;
                }
            }
        });
    }

    public void LogReceivedMessage(string line)
    {
        connectionPage.Invoke(() =>
        {
            lock (_lock)
            {
                connectionPage.ConsoleWebBrowser.AppendHtmlLine(
                    "<span style=\"color:#239B56;font-weight: bold;\">[↓]</span>"
                        + LogColorizer.EscapeLog(line)
                );
            }
        });
    }

    public void LogReceivedData(string data)
    {
        connectionPage.Invoke(() =>
        {
            lock (_lock)
            {
                connectionPage.ConsoleWebBrowser.AppendHtmlLine(
                    "<span style=\"color:#239B56;font-weight: bold;\">[↓]</span>"
                        + LogColorizer.EscapeLog(data)
                );
            }
        });
    }

    public void LogSentData(string line)
    {
        connectionPage.Invoke(() =>
        {
            lock (_lock)
            {
                connectionPage.ConsoleWebBrowser.AppendHtmlLine(
                    "<span style=\"color:#2874A6;font-weight: bold;\">[↑]</span>"
                        + LogColorizer.EscapeLog(line)
                );
            }
        });
    }

    public void LogSentMessage(string data)
    {
        connectionPage.Invoke(() =>
        {
            lock (_lock)
            {
                connectionPage.ConsoleWebBrowser.AppendHtmlLine(
                    "<span style=\"color:#2874A6;font-weight: bold;\">[↑]</span>"
                        + LogColorizer.EscapeLog(data)
                );
            }
        });
    }
}
