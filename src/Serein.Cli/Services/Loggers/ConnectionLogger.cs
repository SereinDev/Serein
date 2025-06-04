using Microsoft.Extensions.Logging;
using Serein.Core.Models.Abstractions;

namespace Serein.Cli.Services.Loggers;

public sealed class ConnectionLogger(ILogger<ConnectionLogger> logger) : IConnectionLogger
{
    private readonly ILogger _logger = logger;

    public void Log(LogLevel level, string message)
    {
        _logger.Log(level, message);
    }

    public void LogReceivedMessage(string line)
    {
        _logger.LogInformation("[Recv] {}", line);
    }

    public void LogReceivedData(string data)
    {
        _logger.LogInformation("[Recv] {}", data);
    }

    public void LogSentMessage(string data)
    {
        _logger.LogInformation("[Sent] {}", data);
    }

    public void LogSentData(string line)
    {
        _logger.LogInformation("[Sent] {}", line);
    }
}
