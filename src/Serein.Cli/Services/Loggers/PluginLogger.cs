using Microsoft.Extensions.Logging;

using Serein.Cli.Utils;
using Serein.Core.Models.Output;

namespace Serein.Cli.Services.Loggers;

public class PluginLogger(ILogger<PluginLogger> logger) : IPluginLogger
{
    private readonly ILogger _logger = logger;

    public void Log(LogLevel level, string name, string message)
    {
        switch (level)
        {
            case LogLevel.Trace:
                _logger.LogInformation(message);
                break;

            default:
                CliConsole.WriteLine(level, $"[{name}] [{message}]");
                break;
        }
    }
}
