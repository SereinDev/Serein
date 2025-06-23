using System;
using Microsoft.Extensions.Logging;

namespace Serein.Core.Models.Abstractions;

public abstract class ConnectionLoggerBase
{
    public event Action<(string Type, string Data)>? Logging;

    public abstract void Log(LogLevel level, string message);

    public abstract void LogSentMessage(string data);

    public abstract void LogReceivedMessage(string line);

    public abstract void LogSentData(string line);

    public abstract void LogReceivedData(string data);

    protected void OnLogging(string type, string data)
    {
        Logging?.Invoke((type, data));
    }
}
