using Microsoft.Extensions.Logging;

using Serein.Core.Models.Output;

namespace Serein.Cli.Loggers;

public class ConnectionLogger(ILogger logger) : IConnectionLogger
{
    private readonly ILogger _logger = logger;

    public void Log(LogLevel level, string message)
    {
        _logger.Log(level, message);
    }
}
