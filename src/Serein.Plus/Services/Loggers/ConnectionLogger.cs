using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Serein.Core.Models.Output;
using Serein.Plus.Pages;

namespace Serein.Plus.Services.Loggers;

public sealed class ConnectionLogger(ILogger<ConnectionLogger> logger, IServiceProvider serviceProvider)
    : IConnectionLogger
{
    private readonly Lazy<ConnectionPage> _connectionPage =
        new(serviceProvider.GetRequiredService<ConnectionPage>);
    private readonly ILogger _logger = logger;

    public void Log(LogLevel level, string message)
    {
        _logger.Log(level, "{}", message);

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

    public void LogReceivedData(string data)
    {
        _connectionPage.Value.Dispatcher.Invoke(
            () => _connectionPage.Value.Console.AppendReceivedMsgLine(data)
        );
    }

    public void LogReceivedMessage(string line)
    {
        _connectionPage.Value.Dispatcher.Invoke(
            () => _connectionPage.Value.Console.AppendReceivedMsgLine(line)
        );
    }

    public void LogSentMessage(string data)
    {
        _connectionPage.Value.Dispatcher.Invoke(
            () => _connectionPage.Value.Console.AppendSentMsgLine(data)
        );
    }

    public void LogSentData(string line)
    {
        _connectionPage.Value.Dispatcher.Invoke(
            () => _connectionPage.Value.Console.AppendSentMsgLine(line)
        );
    }
}
