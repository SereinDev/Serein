using System.Text.Json.Nodes;

using Microsoft.Extensions.Logging;

using Serein.Core.Models.OneBot.Packets;

namespace Serein.Core.Models.Output;

public interface IOutputHandler : ILogger
{
    void LogServerRawOutput(string line);

    void LogServerInfo(string line);

    void LogBotNotice(string line);

    void LogBotMessage(MessagePacket packet);

    void LogBotJsonPacket(JsonNode jsonNode);

    void LogPluginNotice(string line);

    void LogPluginInfomation(string title, string line);

    void LogPluginWarn(string title, string line);

    void LogPluginError(string title, string line);
}
