using Microsoft.Extensions.Logging;

using Serein.Core.Models.Output;

namespace Serein.Tests.Loggers;

public class ConnectionLogger : IConnectionLogger
{
    public void Log(LogLevel level, string message) { }

    public void LogReceivedData(string data)
    {
    }

    public void LogReceivedMessage(string line)
    {
    }

    public void LogSentData(string data)
    {
    }

    public void LogSentPacket(string line)
    {
    }
}
