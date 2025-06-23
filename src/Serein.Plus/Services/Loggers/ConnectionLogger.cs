using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serein.Core.Models.Abstractions;
using Serein.Plus.Pages;

namespace Serein.Plus.Services.Loggers;

public sealed class ConnectionLogger(
    ILogger<ConnectionLogger> logger,
    IServiceProvider serviceProvider
) : ConnectionLoggerBase
{
    private readonly Lazy<ConnectionPage> _connectionPage = new(
        serviceProvider.GetRequiredService<ConnectionPage>
    );
    private readonly ILogger _logger = logger;

    public override void Log(LogLevel level, string message)
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

    public override void LogReceivedData(string data)
    {
        _connectionPage.Value.Dispatcher.Invoke(
            () => _connectionPage.Value.Console.AppendReceivedMsgLine(data)
        );

        OnLogging("received", data);
    }

    public override void LogReceivedMessage(string line)
    {
        _connectionPage.Value.Dispatcher.Invoke(
            () => _connectionPage.Value.Console.AppendReceivedMsgLine(line)
        );

        OnLogging("received", line);
    }

    public override void LogSentMessage(string data)
    {
        _connectionPage.Value.Dispatcher.Invoke(
            () => _connectionPage.Value.Console.AppendSentMsgLine(data)
        );

        OnLogging("sent", data);
    }

    public override void LogSentData(string line)
    {
        _connectionPage.Value.Dispatcher.Invoke(
            () => _connectionPage.Value.Console.AppendSentMsgLine(line)
        );

        OnLogging("sent", line);
    }
}
