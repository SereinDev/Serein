using Microsoft.Extensions.Logging;
using Serein.Core.Models.Abstractions;

namespace Serein.Tests.Loggers;

public class ConnectionLogger : IConnectionLogger
{
    public void Log(LogLevel level, string message) { }

    public void LogReceivedData(string data) { }

    public void LogReceivedMessage(string line) { }

    public void LogSentMessage(string data) { }

    public void LogSentData(string line) { }
}
