using System;
using System.Text.Json.Nodes;

using Microsoft.Extensions.Logging;

using Serein.Core.Models.Network.OneBot.Packets;
using Serein.Core.Models.Output;

using Spectre.Console;

namespace Serein.Cli;

public class CliLogger(string categoryName, LogLevel logLevel = LogLevel.Information) : ISereinLogger
{
    private readonly string _categoryName = categoryName;
    private readonly LogLevel _logLevel = logLevel;

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
                AnsiConsole.MarkupLineInterpolated($"{DateTime.Now:T} [[{_categoryName}]] {text}");
                break;

            case LogLevel.Debug:
                AnsiConsole.MarkupLineInterpolated(
                    $"{DateTime.Now:T} [mediumpurple4]Debug[/] [[{_categoryName}]] {text}"
                );
                break;

            case LogLevel.Information:
                AnsiConsole.MarkupLineInterpolated(
                    $"{DateTime.Now:T} [cadetblue_1]Info [/] [[{_categoryName}]] {text}"
                );
                break;

            case LogLevel.Warning:
                AnsiConsole.MarkupLineInterpolated(
                    $"{DateTime.Now:T} [yellow bold]Warn  [[{_categoryName}]] {text}[/]"
                );
                break;

            case LogLevel.Error:
                AnsiConsole.MarkupLineInterpolated(
                    $"{DateTime.Now:T} [red bold]Error [[{_categoryName}]] {text}[/]"
                );
                break;

            case LogLevel.Critical:
                AnsiConsole.MarkupLineInterpolated(
                    $"{DateTime.Now:T} [maroon blod]Fatal [[{_categoryName}]]  {text}[/]"
                );
                break;

            case LogLevel.None:
                break;

            default:
                throw new NotSupportedException();
        }
    }

    public void LogBotJsonPacket(JsonNode jsonNode) { }

    public void LogBotReceivedMessage(MessagePacket packet) { }

    public void LogBotConsole(LogLevel logLevel, string line)
    {
        this.Log(logLevel, "{}", line);
    }

    public void LogPlugin(LogLevel logLevel, string title, string line)
    {
        switch (logLevel)
        {
            case LogLevel.Trace:
                this.LogInformation("{}", line);
                break;

            case LogLevel.Information:
                this.LogInformation("[{}] {}", title, line);
                break;

            case LogLevel.Warning:
                this.LogWarning("[{}] {}", title, line);
                break;

            case LogLevel.Error:
                this.LogError("[{}] {}", title, line);
                break;

            default:
                throw new NotSupportedException();
        }
    }

    public void LogServerInfo(string line)
    {
        this.LogInformation("{}", line);
    }

    public void LogServerRawOutput(string line)
    {
        Console.WriteLine(line);
    }
}
