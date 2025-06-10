using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json.Nodes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serein.ConnectionProtocols.Models.OneBot;
using Serein.Core.Models.Abstractions;
using Serein.Core.Models.Network.Connection;
using Serein.Core.Models.Plugins;
using Serein.Core.Services.Commands;
using Serein.Core.Services.Data;
using Serein.Core.Services.Plugins;

namespace Serein.Core.Services.Network.Connection;

public sealed partial class PacketHandler(
    IHost host,
    Matcher matcher,
    SettingProvider settingProvider,
    EventDispatcher eventDispatcher,
    ReactionTrigger reactionTrigger
)
{
    private readonly Lazy<IConnectionLogger> _connectionLogger = new(
        host.Services.GetRequiredService<IConnectionLogger>
    );

    public Action<JsonNode>? PluginPacketHandler { get; set; }

    public void Handle(AdapterType adapter, JsonNode jsonNode)
    {
        if (!eventDispatcher.Dispatch(Event.PacketReceived, jsonNode))
        {
            return;
        }

        switch (adapter)
        {
            case AdapterType.OneBot_ForwardWebSocket:
            case AdapterType.OneBot_ReverseWebSocket:
                if (settingProvider.Value.Connection.OneBot.Version == OneBotVersion.V11)
                {
                    HandleOneBotV11Packet(jsonNode);
                }
                else
                {
                    HandleOneBotV12Packet(jsonNode);
                }
                break;

            case AdapterType.Satori:
                HandleSatoriPacket(jsonNode);
                break;

            case AdapterType.Plugin when PluginPacketHandler is not null:
                PluginPacketHandler(jsonNode);
                break;

            default:
                throw new NotSupportedException($"不支持处理的适配器类型: {adapter}");
        }
    }

    public bool IsListenedId(TargetType targetType, [NotNullWhen(true)] string? id)
    {
        if (targetType == TargetType.Auto || string.IsNullOrEmpty(id))
        {
            return false;
        }

        var key = targetType.ToString().ToLowerInvariant();
        var shortKey = targetType != TargetType.Guild ? key[0].ToString() : key;

        return settingProvider.Value.Connection.ListenedIds.Contains(id)
            || settingProvider.Value.Connection.ListenedIds.Contains($"{key}:*")
            || settingProvider.Value.Connection.ListenedIds.Contains($"{key}:{id}")
            || settingProvider.Value.Connection.ListenedIds.Contains($"{shortKey}:*")
            || settingProvider.Value.Connection.ListenedIds.Contains($"{shortKey}:{id}");
    }
}
