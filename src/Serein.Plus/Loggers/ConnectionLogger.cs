using System;

using Microsoft.Extensions.Logging;

using Serein.Core.Models.Output;
using Serein.Plus.Ui.Pages.Function;

namespace Serein.Plus.Loggers;

public class ConnectionLogger(ConnectionPage connectionPage) : IConnectionLogger
{
    private readonly ConnectionPage _connectionPage = connectionPage;

    public void Log(LogLevel level, string message)
    {
        _connectionPage.Dispatcher.Invoke(() =>
        {
            switch (level)
            {
                case LogLevel.Information:
                    _connectionPage.Console.AppendInfoLine(message);
                    break;
                case LogLevel.Warning:
                    _connectionPage.Console.AppendWarnLine(message);
                    break;
                case LogLevel.Error:
                case LogLevel.Critical:
                    _connectionPage.Console.AppendErrorLine(message);
                    break;
                default:
                    throw new NotSupportedException();
            }
        });
    }

    public void LogReceivedData(string data) { }

    public void LogReceivedMessage(string line) { }

    public void LogSentData(string data) { }

    public void LogSentPacket(string line) { }
}
