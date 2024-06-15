using Microsoft.Extensions.Logging;

using Serein.Core.Models.Output;

namespace Serein.Tests.Loggers;

public class ConnectionLogger : IConnectionLogger
{
    public void Log(LogLevel level, string message) { }
}
