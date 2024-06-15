using System;

using Microsoft.Extensions.Logging;

namespace Serein.Tests.Loggers;

public class MainTestLogger : ILogger
{
    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter
    )
    {
        Console.WriteLine($"[{logLevel}] {eventId} {state} {exception}");
    }
}
