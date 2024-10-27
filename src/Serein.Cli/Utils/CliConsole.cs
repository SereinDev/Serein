using System;
using System.Linq;

using Microsoft.Extensions.Logging;

namespace Serein.Cli.Utils;

public static class CliConsole
{
    public static bool IsColorful { get; } =
        !Environment.GetCommandLineArgs().Contains("--no-color")
        && Environment.GetEnvironmentVariable("SEREIN_NO_COLOR") is null;

    public static void WriteLine(LogLevel logLevel, string line)
    {
        switch (logLevel)
        {
            case LogLevel.Trace:
                Console.WriteLine($"{DateTime.Now:T} Trace {line}");
                break;

            case LogLevel.Debug:
                if (IsColorful)
                    Console.WriteLine(
                        $"\x1b[0m{DateTime.Now:T} \x1b[38;2;95;95;135mDebug\x1b[0m {line}"
                    );
                else
                    Console.WriteLine($"{DateTime.Now:T} Debug {line}");
                break;

            case LogLevel.Information:
                if (IsColorful)
                    Console.WriteLine(
                        $"\x1b[0m{DateTime.Now:T} \x1b[38;2;95;175;175mInfo\x1b[0m {line}"
                    );
                else
                    Console.WriteLine($"{DateTime.Now:T} Info {line}");
                break;

            case LogLevel.Warning:
                if (IsColorful)
                    Console.WriteLine($"\x1b[0m{DateTime.Now:T} \x1b[1;93mWarn {line}\x1b[0m");
                else
                    Console.WriteLine($"{DateTime.Now:T} Warn {line}");
                break;

            case LogLevel.Error:
                if (IsColorful)
                    Console.WriteLine($"\x1b[0m{DateTime.Now:T} \x1b[1;91mError {line}\x1b[0m");
                else
                    Console.WriteLine($"{DateTime.Now:T} Error {line}");
                break;

            case LogLevel.Critical:
                if (IsColorful)
                    Console.WriteLine($"\x1b[0m{DateTime.Now:T} \x1b[1;31mCritical {line}\x1b[0m");
                else
                    Console.WriteLine($"{DateTime.Now:T} Critical {line}");
                break;
            case LogLevel.None:
                break;
        }
    }
}
