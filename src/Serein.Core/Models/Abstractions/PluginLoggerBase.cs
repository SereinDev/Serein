using System;
using Microsoft.Extensions.Logging;

namespace Serein.Core.Models.Abstractions;

public abstract class PluginLoggerBase
{
    public event Action<LogLevel, string, string>? Logging;

    public abstract void Log(LogLevel level, string name, string message);

    protected void OnLogging(LogLevel level, string name, string message)
    {
        Logging?.Invoke(level, name, message);
    }
}
