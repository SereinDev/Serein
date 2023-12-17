using System.Text.Json.Nodes;

using Serein.Core.Models.OneBot.Packets;

namespace Serein.Core.Models;

public interface IOutputHandler
{
    void LogServerOriginalOutput(string line);
    void LogServerNotice(string line);
    void LogBotNotice(string line);
    void LogBotMessage(MessagePacket packet);
    void LogBotJsonPacket(JsonNode jsonNode);
    void LogPluginNotice(string line);
    void LogPluginInfo(string title, string line);
    void LogPluginWarn(string title, string line);
    void LogPluginError(string title, string line);
}
