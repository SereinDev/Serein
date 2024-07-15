using System;

using Microsoft.Extensions.Logging;

using Serein.Core.Services.Data;

using Spectre.Console;

namespace Serein.Cli.Loggers;

public class CliLogger(SettingProvider settingProvider) : ILogger
{
    private readonly LogLevel _logLevel = settingProvider.Value.Application.LogLevel;

    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return _logLevel <= logLevel;
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

        var text = state?.ToString();

        switch (logLevel)
        {
            case LogLevel.Trace:
                AnsiConsole.MarkupLineInterpolated($"{DateTime.Now:T} Trace [[Serein]] {text}");
                break;

            case LogLevel.Debug:
                AnsiConsole.MarkupLineInterpolated(
                    $"{DateTime.Now:T} [mediumpurple4]Debug[/] [[Serein]] {text}"
                );
                break;

            case LogLevel.Information:
                AnsiConsole.MarkupLineInterpolated(
                    $"{DateTime.Now:T} [cadetblue_1]Info[/]  [[Serein]] {text}"
                );
                break;

            case LogLevel.Warning:
                AnsiConsole.MarkupLineInterpolated(
                    $"{DateTime.Now:T} [yellow bold]Warn  [[Serein]] {text}[/]"
                );
                break;

            case LogLevel.Error:
                AnsiConsole.MarkupLineInterpolated(
                    $"{DateTime.Now:T} [red bold]Error [[Serein]] {text}[/]"
                );
                break;

            case LogLevel.Critical:
                AnsiConsole.MarkupLineInterpolated(
                    $"{DateTime.Now:T} [maroon blod]Critical[[Serein]]  {text}[/]"
                );
                break;

            case LogLevel.None:
                break;

            default:
                throw new NotSupportedException();
        }
    }
}
