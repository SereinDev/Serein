using System.Collections.Generic;

using Microsoft.Extensions.Logging;

namespace Serein.Cli.Services.Loggers;

public sealed class CliLoggerProvider : ILoggerProvider
{
    private readonly Dictionary<string, CliLogger> _loggers = [];

    public ILogger CreateLogger(string categoryName)
    {
        lock (_loggers)
        {
            if (!_loggers.TryGetValue(categoryName, out var logger))
            {
                logger = _loggers[categoryName] = new(categoryName);
            }
            return logger;
        }
    }

    public void Dispose()
    {
        _loggers.Clear();
    }
}
