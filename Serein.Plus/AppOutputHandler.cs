using System;
using System.Text.Json.Nodes;

using Microsoft.Extensions.Logging;

using Serein.Core.Models;
using Serein.Core.Models.OneBot.Packets;

namespace Serein.Plus;

public class AppOutputHandler : IOutputHandler
{
    private readonly LogLevel _logLevel;

    public AppOutputHandler(LogLevel logLevel)
    {
        _logLevel = logLevel;
    }

    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return _logLevel <= logLevel;
    }

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter
    )
    { }

    public void LogBotJsonPacket(JsonNode jsonNode) { }

    public void LogBotMessage(MessagePacket packet) { }

    public void LogBotNotice(string line) { }

    public void LogPluginError(string title, string line) { }

    public void LogPluginInfomation(string title, string line) { }

    public void LogPluginNotice(string line) { }

    public void LogPluginWarn(string title, string line) { }

    public void LogServerNotice(string line) { }

    public void LogServerRawOutput(string line) { }
}
