using System;

using Microsoft.Extensions.Logging;

using Serein.Core.Models.Output;

using Spectre.Console;

namespace Serein.Cli.Loggers;

public class PluginLogger : IPluginLogger
{
    public void Log(LogLevel level, string name, string message)
    {
        switch (level)
        {
            case LogLevel.Debug:
                AnsiConsole.MarkupLineInterpolated(
                    $"{DateTime.Now:T} [mediumpurple4]Debug[/] [[name]] {message}"
                );
                break;

            case LogLevel.Information:
                AnsiConsole.MarkupLineInterpolated(
                    $"{DateTime.Now:T} [cadetblue_1]Info [/] [[name]] {message}"
                );
                break;

            case LogLevel.Warning:
                AnsiConsole.MarkupLineInterpolated(
                    $"{DateTime.Now:T} [yellow bold]Warn  [[name]] {message}[/]"
                );
                break;

            case LogLevel.Critical:
            case LogLevel.Error:
                AnsiConsole.MarkupLineInterpolated(
                    $"{DateTime.Now:T} [red bold]Error [[name]] {message}[/]"
                );
                break;

            case LogLevel.None:
                break;

            default:
                throw new NotSupportedException();
        }
    }
}
