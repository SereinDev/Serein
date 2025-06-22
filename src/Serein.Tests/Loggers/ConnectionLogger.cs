using Microsoft.Extensions.Logging;
using Serein.Core.Models.Abstractions;

namespace Serein.Tests.Loggers;

public class ConnectionLogger : ConnectionLoggerBase
{
    public override void Log(LogLevel level, string message) { }

    public override void LogReceivedData(string data) { }

    public override void LogReceivedMessage(string line) { }

    public override void LogSentMessage(string data) { }

    public override void LogSentData(string line) { }
}
