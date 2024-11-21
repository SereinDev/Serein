using Microsoft.Extensions.Logging;

namespace Serein.Core.Models.Output;

public interface IConnectionLogger
{
    void Log(LogLevel level, string message);

    void LogSentMessage(string data);

    void LogReceivedMessage(string line);

    void LogSentData(string line);

    void LogReceivedData(string data);
}
