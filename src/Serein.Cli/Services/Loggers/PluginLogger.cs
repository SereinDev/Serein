using Microsoft.Extensions.Logging;
using Serein.Cli.Utils;
using Serein.Core.Models.Output;

namespace Serein.Cli.Services.Loggers;

public sealed class PluginLogger(ILogger<PluginLogger> logger) : IPluginLogger
{
    private readonly ILogger _logger = logger;

    public void Log(LogLevel level, string name, string message)
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
