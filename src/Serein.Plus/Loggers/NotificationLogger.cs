using System;
using System.Collections.ObjectModel;

using Microsoft.Extensions.Logging;

namespace Serein.Plus.Loggers;

public class NotificationLogger : ILogger
{
    public NotificationLogger()
    {
        Messages = new();
    }

    public ObservableCollection<(LogLevel LogLevel, string Message)> Messages { get; }

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
        Messages.Add((logLevel, formatter(state, exception)));
        if (Messages.Count > 50)
            Messages.RemoveAt(0);
    }
}
