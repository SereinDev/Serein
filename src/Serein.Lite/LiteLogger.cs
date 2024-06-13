using System;
using System.Text.Json.Nodes;

using Microsoft.Extensions.Logging;

using Serein.Core.Models.Network.OneBot.Packets;
using Serein.Core.Models.Output;

namespace Serein.Lite;
public class LiteLogger : ISereinLogger
{
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
    }

    public void LogBotConsole(LogLevel logLevel, string line)
    {
    }

    public void LogBotJsonPacket(JsonNode jsonNode)
    {
    }

    public void LogBotReceivedMessage(MessagePacket packet)
    {
    }

    public void LogPlugin(LogLevel logLevel, string title, string line)
    {
    }
}
