using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using Serein.ConnectionProtocols.Models.OneBot.V11.Messages;
using Serein.ConnectionProtocols.Models.OneBot.V11.Packets;
using Serein.Core.Models.Network.Connection;
using Serein.Core.Models.Plugins;
using Serein.Core.Models.Settings;
using Serein.Core.Utils.Json;

namespace Serein.Core.Services.Network.Connection;

public partial class PacketHandler
{
    private void HandleOneBotV11Packet(JsonNode jsonNode)
    {
        switch (jsonNode["post_type"]?.ToString())
        {
            case "message":
            case "message_sent":
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

    private void HandleNoticePacket(NoticePacket? packet)
    {
        if (packet is null || !IsListenedId(TargetType.Group, packet.GroupId.ToString()))
        {
            return;
        }

        if (
            packet.NoticeType == "group_decrease"
            || packet.NoticeType == "group_increase"
            || packet.NoticeType == "notify"
        )
        {
            if (
                packet.NoticeType == "notify"
                && (packet.SubType != "poke" || packet.TargetId != packet.SelfId)
            )
            {
                return;
            }

            reactionTrigger.Trigger(
                packet.NoticeType == "group_increase" ? ReactionType.GroupIncreased
                    : packet.NoticeType == "group_decrease" ? ReactionType.GroupDecreased
                    : ReactionType.GroupPoke,
                new(GroupId: packet.GroupId.ToString(), UserId: packet.UserId.ToString()),
                new Dictionary<string, string?> { ["sender.id"] = packet.UserId.ToString() }
            );
        }
    }

    private void HandleMessagePacket(MessagePacket? packet)
    {
        if (packet is null)
        {
            return;
        }

        _connectionLogger.Value.LogReceivedMessage(
            $"[{(packet.MessageType == MessageType.Group ? $"群聊({packet.GroupId})" : "私聊")}] {packet.Sender.Nickname}({packet.UserId}): {packet.RawMessage} (id={packet.MessageId})"
        );

        if (
            packet.MessageType == MessageType.Group
            && !IsListenedId(TargetType.Group, packet.GroupId.ToString())
        )
        {
            return;
        }

        if (
            packet.MessageType == MessageType.Group
                && !eventDispatcher.Dispatch(Event.GroupMessageReceived, packet)
            || packet.MessageType == MessageType.Private
                && !eventDispatcher.Dispatch(Event.PrivateMessageReceived, packet)
        )
        {
            return;
        }

        matcher.QueueMsg(packet);
    }
}
