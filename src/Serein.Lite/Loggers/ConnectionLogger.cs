using Microsoft.Extensions.Logging;

using Serein.Core.Models.Output;
using Serein.Lite.Ui.Function;

namespace Serein.Lite.Loggers;

public class ConnectionLogger(ConnectionPage connectionPage) : IConnectionLogger
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
}
