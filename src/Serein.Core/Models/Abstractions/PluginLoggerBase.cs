using Microsoft.Extensions.Logging;

namespace Serein.Core.Models.Abstractions;

public abstract class PluginLoggerBase
{
    public abstract void Log(LogLevel level, string name, string message);
}
