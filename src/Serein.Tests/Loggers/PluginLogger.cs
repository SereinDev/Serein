using Microsoft.Extensions.Logging;

using Serein.Core.Models.Output;

namespace Serein.Tests.Loggers;

public class PluginLogger : IPluginLogger
{
    public void Log(LogLevel level, string name, string message) { }
}
