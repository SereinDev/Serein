using System.Text.Json.Nodes;

using Microsoft.Extensions.Logging;

using Serein.Core.Models.Network.OneBot.Packets;

namespace Serein.Core.Models.Output;

public interface ISereinLogger : ILogger
{
    void LogBotConsole(LogLevel logLevel, string line);

    void LogBotReceivedMessage(MessagePacket packet);

    void LogBotJsonPacket(JsonNode jsonNode);

    void LogPlugin(LogLevel logLevel, string title, string line);
}
