using System;
using Microsoft.Extensions.Logging;

namespace Serein.Plus.Services.Loggers;

public class ConsoleLogger(string categoryName) : ILogger
{
    private readonly string _name = categoryName.Contains('.')
        ? categoryName[(categoryName.LastIndexOf('.') + 1)..]
        : categoryName;

    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public event EventHandler<(LogLevel Level, string Line)>? LogWritten;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter
    )
    {
        LogWritten?.Invoke(this, (logLevel, $"[{_name}] " + formatter(state, exception)));
    }
}
