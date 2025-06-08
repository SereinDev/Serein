using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using Serein.ConnectionProtocols.Models.OneBot.V12.Messages;
using Serein.ConnectionProtocols.Models.OneBot.V12.Packets;
using Serein.Core.Models.Commands;
using Serein.Core.Models.Network.Connection;
using Serein.Core.Utils.Json;

namespace Serein.Core.Services.Network.Connection;

public partial class PacketHandler
{
    /// <summary>
    /// https://12.onebot.dev/connect/data-protocol/event/
    /// </summary>
    private void HandleOneBotV12Packet(JsonNode jsonNode)
    {
        switch (jsonNode["type"]?.ToString())
        {
            case "message":
                HandleMessagePacket(
                    JsonSerializer.Deserialize<MessagePacket>(
                        jsonNode,
                        JsonSerializerOptionsFactory.PacketStyle
                    )
                );
                break;

            case "notice":
                HandleNoticePacket(
                    JsonSerializer.Deserialize<NoticePacket>(
                        jsonNode,
                        JsonSerializerOptionsFactory.PacketStyle
                    )
                );
                break;
        }
    }

    private void HandleMessagePacket(MessagePacket? packet)
    {
        if (packet is null)
        {
            return;
        }

        _connectionLogger.Value.LogReceivedMessage(
            $"[{packet.DetailType switch
            {
                MessageDetailType.Private => "私聊",
                MessageDetailType.Group => "群聊",
                MessageDetailType.Channel => "频道",
                _ => throw new NotSupportedException()
            }}] {packet.UserId}: {packet.FriendlyMessage} (id={packet.MessageId})"
        );
    }

    private void HandleNoticePacket(NoticePacket? packet)
    {
        if (packet is null || !IsListenedId(TargetType.Group, packet.GroupId))
        {
            return;
        }

        reactionTrigger.Trigger(
            packet.DetailType switch
            {
                "group_increase" => ReactionType.GroupIncreased,
                "group_decrease" => ReactionType.GroupDecreased,
                _ => throw new NotSupportedException(),
            },
            new(GroupId: packet.GroupId, UserId: packet.UserId),
            new Dictionary<string, string?> { ["sender.id"] = packet.UserId }
        );
    }
}
