using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Serein.Core.Models.Output;
using Serein.Plus.Pages;

namespace Serein.Plus.Services.Loggers;

public class ConnectionLogger(IServiceProvider serviceProvider) : IConnectionLogger
{
    private readonly Lazy<ConnectionPage> _connectionPage = new(serviceProvider.GetRequiredService<ConnectionPage>);

    public void Log(LogLevel level, string message)
    {
        _connectionPage.Value.Dispatcher.Invoke(() =>
        {
            switch (level)
            {
                case LogLevel.Information:
                    _connectionPage.Value.Console.AppendInfoLine(message);
                    break;
                case LogLevel.Warning:
                    _connectionPage.Value.Console.AppendWarnLine(message);
                    break;
                case LogLevel.Error:
                    _connectionPage.Value.Console.AppendErrorLine(message);
                    break;
            }
        });
    }

    public void LogReceivedData(string data) { }

    public void LogReceivedMessage(string line) { }

    public void LogSentData(string data) { }

    public void LogSentPacket(string line) { }
}
