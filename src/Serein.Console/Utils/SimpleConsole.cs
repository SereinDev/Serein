using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace Serein.Console.Utils;

public static class SimpleConsole
{
    public static bool IsColorful { get; } =
        !Environment.GetCommandLineArgs().Contains("--no-color")
        && Environment.GetEnvironmentVariable("SEREIN_NO_COLOR") is null;

    private static string GetCallerFileName(this string callerFilePath)
    {
        return Path.GetFileNameWithoutExtension(callerFilePath);
    }

    public static void Log(
        LogLevel logLevel,
        string content,
        [CallerFilePath] string callerFilePath = ""
    )
    {
        WriteLine(
            logLevel,
            new Markup(
                $"[{GetContentStyle(logLevel)}][[{callerFilePath.GetCallerFileName().EscapeMarkup()}]] {content.EscapeMarkup()}[/] "
            )
        );
    }

    public static void Log(
        LogLevel logLevel,
        string message,
        IRenderable renderable,
        [CallerFilePath] string callerFilePath = ""
    )
    {
        WriteLine(
            logLevel,
            new Rows(
                new Markup(
                    $"[{GetContentStyle(logLevel)}][[{callerFilePath.GetCallerFileName().EscapeMarkup()}]] {message.EscapeMarkup()}[/]"
                ),
                renderable
            ).Expand()
        );
    }

    public static void Log(
        LogLevel logLevel,
        IRenderable renderable,
        [CallerFilePath] string callerFilePath = ""
    )
    {
        IRenderable[] items =
        [
            new Markup(
                $"[{GetContentStyle(logLevel)}][[{callerFilePath.GetCallerFileName().EscapeMarkup()}]][/]"
            ),
            renderable,
        ];

        WriteLine(
            logLevel,
            renderable is Text or Markup ? new Columns(items).Expand() : new Rows(items).Expand()
        );
    }

    public static void WriteLine()
    {
        AnsiConsole.WriteLine();
    }

    public static void WriteLine(LogLevel logLevel, string content)
    {
        WriteLine(
            logLevel,
            new Markup($"[{GetContentStyle(logLevel)}]{content.EscapeMarkup()}[/] ")
        );
    }

    public static void WriteLine(LogLevel logLevel, IRenderable renderable)
    {
        (string? tagStyle, string? tag) = logLevel switch
        {
            LogLevel.Trace => ("grey", "Trace"),
            LogLevel.Debug => ("blue", "Debug"),
            LogLevel.Information => ("CadetBlue_1", "Info"),
            LogLevel.Warning => ("yellow", "Warn"),
            LogLevel.Error => ("red", "Error"),
            LogLevel.Critical => ("bold rapidblink on Red3_1", "Critical"),
            _ => (null, null),
        };

        if (string.IsNullOrEmpty(tag))
        {
            return;
        }

        var table = new Table()
            .HideHeaders()
            .HideFooters()
            .NoBorder()
            .NoSafeBorder()
            .HideRowSeparators()
            .AddColumns("_time", "_tag", "_message")
            .AddRow(
                new Text(DateTime.Now.ToString("HH:mm:ss"), new Style(Color.Silver)),
                new Markup($"[{tagStyle}]{tag.EscapeMarkup()}[/]"),
                renderable
            );

        AnsiConsole.Write(table);
    }

    private static readonly ExceptionSettings ExceptionSettings = new()
    {
        Format = ExceptionFormats.ShortenPaths,
        Style =
        {
            Exception = new Style(Color.Red, Color.Default, Decoration.Bold),
            Message = Color.Red,
            Path = new Style(Color.Grey, Color.Default, Decoration.Italic),
            LineNumber = new Style(Color.Grey, Color.Default, Decoration.Italic),
            Dimmed = Color.Grey,
            NonEmphasized = Color.FromHex("#DCDCDC"),
            Method = Color.FromHex("#DCDCAA"),
            ParameterType = Color.FromHex("#4EC9B0"),
            ParameterName = Color.FromHex("#9CDCFE"),
        },
    };

    public static void WriteException(Exception ex)
    {
        AnsiConsole.WriteException(ex, ExceptionSettings);
    }

    private static string? GetContentStyle(LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Trace => "grey",
            LogLevel.Debug => "blue",
            LogLevel.Information => null,
            LogLevel.Warning => "yellow",
            LogLevel.Error => "red",
            LogLevel.Critical => "bold red",
            _ => null,
        };
    }
}
