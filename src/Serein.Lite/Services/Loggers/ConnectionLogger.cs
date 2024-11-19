using Microsoft.Extensions.Logging;

using Serein.Core.Models.Output;
using Serein.Lite.Ui.Function;
using Serein.Lite.Utils;

namespace Serein.Lite.Services.Loggers;

public sealed class ConnectionLogger(ConnectionPage connectionPage) : IConnectionLogger
{
    private readonly object _lock = new();
    private readonly ConnectionPage _connectionPage = connectionPage;

    public void Log(LogLevel level, string message)
    {
        _connectionPage.Invoke(() =>
        {
            lock (_lock)
            {
                switch (level)
                {
                    case LogLevel.Information:
                        _connectionPage.ConsoleWebBrowser.AppendLine($"[Info] {message}");
                        break;
                    case LogLevel.Warning:
                        _connectionPage.ConsoleWebBrowser.AppendWarn(message);
                        break;
                    case LogLevel.Error:
                        _connectionPage.ConsoleWebBrowser.AppendError(message);
                        break;
                    default:
                        return;
                }
            }
        });
    }

    public void LogReceivedMessage(string line)
    {
        _connectionPage.Invoke(() =>
        {
            lock (_lock)
            {
                _connectionPage.ConsoleWebBrowser.AppendHtmlLine(
                    "<span style=\"color:#239B56;font-weight: bold;\">[↓]</span>" +
                    LogColorizer.EscapeLog(line)
                );
            }
        });
    }

    public void LogReceivedData(string data)
    {
        _connectionPage.Invoke(() =>
        {
            lock (_lock)
            {
                _connectionPage.ConsoleWebBrowser.AppendHtmlLine(
                    "<span style=\"color:#239B56;font-weight: bold;\">[↓]</span>" +
                    LogColorizer.EscapeLog(data)
                );
            }
        });
    }

    public void LogSentPacket(string line)
    {
        _connectionPage.Invoke(() =>
        {
            lock (_lock)
            {
                _connectionPage.ConsoleWebBrowser.AppendHtmlLine(
                   "<span style=\"color:#2874A6;font-weight: bold;\">[↑]</span>" +
                    LogColorizer.EscapeLog(line)
                );
            }
        });
    }

    public void LogSentData(string data)
    {
        _connectionPage.Invoke(() =>
        {
            lock (_lock)
            {
                _connectionPage.ConsoleWebBrowser.AppendHtmlLine(
                    "<span style=\"color:#2874A6;font-weight: bold;\">[↑]</span>" +
                    LogColorizer.EscapeLog(data)
                );
            }
        });
    }
}
