using System;

using Microsoft.Extensions.Logging;

using Serein.Cli.Utils;
using Serein.Core.Utils.Extensions;

namespace Serein.Cli.Services.Loggers;

public sealed class CliLogger(string categoryName) : ILogger
{
    private static readonly bool EnableDebug =
        Environment.CommandLine.Contains("--debug")
        || Environment.GetEnvironmentVariable("SEREIN_DEBUG") is not null;

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
        if (!IsEnabled(logLevel))
            return;

        var text = state?.ToString() ?? string.Empty;

        if (exception != null)
            text += Environment.NewLine + (EnableDebug ? exception.ToString() : exception.GetDetailString());

        CliConsole.WriteLine(logLevel, $"[{_name}] {text}");
    }
}
