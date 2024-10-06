using Microsoft.Extensions.Logging;

using Serein.Core.Models.Output;

namespace Serein.Cli.Services.Loggers;

public class ConnectionLogger(ILogger<ConnectionLogger> logger) : IConnectionLogger
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

    public void LogSentPacket(string line)
    {
        _logger.LogInformation("[Sent] {}", line);
    }

    public void LogSentData(string data)
    {
        _logger.LogInformation("[Sent] {}", data);
    }
}
