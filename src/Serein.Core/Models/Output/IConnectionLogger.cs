using System.Text.Json.Nodes;

using Microsoft.Extensions.Logging;

namespace Serein.Core.Models.Output;

public interface IConnectionLogger
{
    void Log(LogLevel level, string message);

    void LogReceivedMessage(string line);

    void LogSentPacket(string line);

    void LogReceivedData(string data);

    void LogSentData(string data);
}
