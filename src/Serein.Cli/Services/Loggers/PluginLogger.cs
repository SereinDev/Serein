using Microsoft.Extensions.Logging;
using Serein.Cli.Utils;
using Serein.Core.Models.Abstractions;

namespace Serein.Cli.Services.Loggers;

public sealed class PluginLogger(ILogger<PluginLogger> logger) : PluginLoggerBase
{
    private readonly ILogger _logger = logger;

    public override void Log(LogLevel level, string name, string message)
    {
        if (level == LogLevel.Trace)
        {
            _logger.LogInformation(message);
        }
        else
        {
            CliConsole.WriteLine(level, $"[{name}] {message}");
        }
    }
}
