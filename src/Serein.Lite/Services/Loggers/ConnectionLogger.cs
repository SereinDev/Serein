using Microsoft.Extensions.Logging;
using Serein.Core.Models.Abstractions;
using Serein.Lite.Ui.Functions;
using Serein.Lite.Utils;

namespace Serein.Lite.Services.Loggers;

public sealed class ConnectionLogger(ConnectionPage connectionPage) : ConnectionLoggerBase
{
    private readonly object _lock = new();

    public override void Log(LogLevel level, string message)
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
        OnLogging(level.ToString().ToLowerInvariant(), message);
    }

    public override void LogReceivedMessage(string line)
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

        OnLogging("received", line);
    }

    public override void LogReceivedData(string data)
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

        OnLogging("received", data);
    }

    public override void LogSentData(string line)
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

        OnLogging("sent", line);
    }

    public override void LogSentMessage(string data)
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

        OnLogging("sent", data);
    }
}
