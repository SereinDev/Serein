using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serein.Core.Models.Network.Connection.OneBot.Messages;
using Serein.Core.Models.Network.Connection.OneBot.Packets;
using Serein.Core.Models.Output;
using Serein.Core.Models.Plugins;
using Serein.Core.Models.Settings;
using Serein.Core.Services.Commands;
using Serein.Core.Services.Data;
using Serein.Core.Services.Plugins;
using Serein.Core.Utils.Json;

namespace Serein.Core.Services.Network.Connection;

public sealed class PacketHandler(
    IHost host,
    Matcher matcher,
    SettingProvider settingProvider,
    EventDispatcher eventDispatcher,
    ReactionTrigger reactionTrigger
)
{
    private readonly Lazy<IConnectionLogger> _connectionLogger =
        new(host.Services.GetRequiredService<IConnectionLogger>);
    private readonly Matcher _matcher = matcher;
    private readonly SettingProvider _settingProvider = settingProvider;
    private readonly EventDispatcher _eventDispatcher = eventDispatcher;
    private readonly ReactionTrigger _reactionTrigger = reactionTrigger;

    public void Handle(JsonObject jsonObject)
    {
        if (!_eventDispatcher.Dispatch(Event.PacketReceived, jsonObject))
        {
            return;
        }

        switch (jsonObject["post_type"]?.ToString())
        {
            case "message":
            case "message_sent":
                HandleMessagePacket(
                    JsonSerializer.Deserialize<MessagePacket>(
                        jsonObject,
                        JsonSerializerOptionsFactory.PacketStyle
                    )
                );
                break;

            case "notice":
                HandleNoticePacket(
                    JsonSerializer.Deserialize<NoticePacket>(
                        jsonObject,
                        JsonSerializerOptionsFactory.PacketStyle
                    )
                );
                break;
        }
    }

    private void HandleNoticePacket(NoticePacket? packet)
    {
        if (packet is null || !_settingProvider.Value.Connection.Groups.Contains(packet.GroupId))
        {
            return;
        }

        switch (packet.NoticeType)
        {
            case "group_decrease":
            case "group_increase":
                _reactionTrigger.Trigger(
                    packet.NoticeType == "group_increase"
                        ? ReactionType.GroupIncreased
                        : ReactionType.GroupDecreased,
                    new(GroupId: packet.GroupId, UserId: packet.UserId),
                    new Dictionary<string, string?> { ["sender.id"] = packet.UserId.ToString() }
                );
                break;

            case "notify":
                if (packet.SubType == "poke" && packet.TargetId == packet.SelfId)
                {
                    _reactionTrigger.Trigger(
                        ReactionType.GroupPoke,
                        new(GroupId: packet.GroupId, UserId: packet.UserId),
                        new Dictionary<string, string?> { ["sender.id"] = packet.UserId.ToString() }
                    );
                }
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
            $"[{(packet.MessageType == MessageType.Group ? $"群聊({packet.GroupId})" : "私聊")}] {packet.Sender.Nickname}({packet.UserId}): {packet.RawMessage} (id={packet.MessageId})"
        );

        if (
            packet.MessageType == MessageType.Group
            && !_settingProvider.Value.Connection.Groups.Contains(packet.GroupId)
        )
        {
            return;
        }

        if (
            packet.MessageType == MessageType.Group
                && !_eventDispatcher.Dispatch(Event.GroupMessageReceived, packet)
            || packet.MessageType == MessageType.Private
                && !_eventDispatcher.Dispatch(Event.PrivateMessageReceived, packet)
        )
        {
            return;
        }

        _matcher.QueueMsg(packet);
    }
}
