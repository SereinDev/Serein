using Microsoft.Extensions.Logging;
using Serein.Core.Models.Abstractions;

namespace Serein.Tests.Loggers;

public class PluginLogger : PluginLoggerBase
{
    public override void Log(LogLevel level, string name, string message) { }
}
