using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Serein.Plus.Services.Loggers;

public class ConsoleLoggerProvider : ILoggerProvider
{
    private readonly Dictionary<string, ILogger> _loggers = [];

    public event EventHandler<(LogLevel Level, string Line)>? LogWritten;

    public ILogger CreateLogger(string categoryName)
    {
        if (_loggers.TryGetValue(categoryName, out var logger))
        {
            return logger;
        }

        var consoleLogger = new ConsoleLogger(categoryName);
        consoleLogger.LogWritten += (sender, args) => LogWritten?.Invoke(sender, args);
        _loggers[categoryName] = consoleLogger;

        return consoleLogger;
    }

    public void Dispose()
    {
        _loggers.Clear();
    }
}
