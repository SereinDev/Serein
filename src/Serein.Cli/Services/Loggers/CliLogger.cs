using System;

using Microsoft.Extensions.Logging;

using Spectre.Console;

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
        var text = state?.ToString();

        switch (logLevel)
        {
            case LogLevel.Trace:
                AnsiConsole.MarkupLineInterpolated($"{DateTime.Now:T} Trace [[{_name}]] {text}");
                break;

            case LogLevel.Debug:
                AnsiConsole.MarkupLineInterpolated(
                    $"{DateTime.Now:T} [mediumpurple4]Debug[/] [[{_name}]] {text}"
                );
                break;

            case LogLevel.Information:
                AnsiConsole.MarkupLineInterpolated(
                    $"{DateTime.Now:T} [cadetblue_1]Info[/] [[{_name}]] {text}"
                );
                break;

            case LogLevel.Warning:
                AnsiConsole.MarkupLineInterpolated(
                    $"{DateTime.Now:T} [yellow bold]Warn [[{_name}]] {text}[/]"
                );
                break;

            case LogLevel.Error:
                AnsiConsole.MarkupLineInterpolated(
                    $"{DateTime.Now:T} [red bold]Error [[{_name}]] {text}[/]"
                );
                break;

            case LogLevel.Critical:
                AnsiConsole.MarkupLineInterpolated(
                    $"{DateTime.Now:T} [maroon bold]Critical [[{_name}]]  {text}[/]"
                );
                break;

            case LogLevel.None:
                break;

            default:
                throw new NotSupportedException();
        }
    }
}
