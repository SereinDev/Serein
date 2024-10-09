using System;

using Microsoft.Extensions.Logging;

using Serein.Cli.Utils;
using Serein.Core.Utils.Extensions;

namespace Serein.Cli.Services.Loggers;

public class CliLogger(string categoryName) : ILogger
{
    private static readonly bool EnableDebug = Environment.CommandLine.Contains("--debug");
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
        return EnableDebug || logLevel >= LogLevel.Information;
    }

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter
    )
    {
        var text = state?.ToString() ?? string.Empty;

        if (exception != null)
            text += EnableDebug ? exception.ToString() : exception.GetDetailString();

        CliConsole.WriteLine(logLevel, $"[{_name}] {text}");
    }
}
