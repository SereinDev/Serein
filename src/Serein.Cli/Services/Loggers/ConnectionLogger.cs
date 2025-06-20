using Microsoft.Extensions.Logging;
using Serein.Core.Models.Abstractions;

namespace Serein.Cli.Services.Loggers;

public sealed class ConnectionLogger(ILogger<ConnectionLogger> logger) : ConnectionLoggerBase
{
    private readonly ILogger _logger = logger;

    public override void Log(LogLevel level, string message)
    {
        _logger.Log(level, message);
    }

    public override void LogReceivedMessage(string line)
    {
        _logger.LogInformation("[Recv] {}", line);
    }

    public override void LogReceivedData(string data)
    {
        _logger.LogInformation("[Recv] {}", data);
    }

    public override void LogSentMessage(string data)
    {
        _logger.LogInformation("[Sent] {}", data);
    }

    public override void LogSentData(string line)
    {
        _logger.LogInformation("[Sent] {}", line);
    }
}
