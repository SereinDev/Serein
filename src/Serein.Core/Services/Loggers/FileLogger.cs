using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Serein.Core.Services.Loggers;

internal class FileLogger(string categoryName, IList<string> buffer) : ILogger
{
    private readonly string _categoryName = categoryName;
    private readonly IList<string> _buffer = buffer;

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
        var stringBuilder = new StringBuilder();
        stringBuilder.Append(DateTime.Now.ToString("HH:mm:ss.ffffff"));
        stringBuilder.Append(" [");
        stringBuilder.Append(_categoryName);
        stringBuilder.Append("] [");
        stringBuilder.Append(logLevel);
        stringBuilder.Append('/');
        stringBuilder.Append(eventId);
        stringBuilder.Append("] ");
        stringBuilder.Append(state);

        if (exception is not null)
        {
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append(exception);
        }

        _buffer.Add(stringBuilder.ToString());
    }
}
