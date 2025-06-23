using System.Text.Json;
using System.Text.Json.Nodes;
using Serein.ConnectionProtocols.Models.Satori.V1.Channels;
using Serein.ConnectionProtocols.Models.Satori.V1.Signals;
using Serein.ConnectionProtocols.Models.Satori.V1.Signals.Bodies;
using Serein.Core.Models.Network.Connection;
using Serein.Core.Models.Plugins;
using Serein.Core.Utils.Json;

namespace Serein.Core.Services.Network.Connection;

public partial class PacketHandler
{
    private void HandleSatoriPacket(JsonNode jsonNode)
    {
        var signal = JsonSerializer.Deserialize<Signal<JsonNode>>(
            jsonNode,
            JsonSerializerOptionsFactory.PacketStyle
        );

        if (signal?.Op != Opcode.Event)
        {
            return;
        }

        var eventBody = JsonSerializer.Deserialize<EventBody>(
            signal.Body,
            JsonSerializerOptionsFactory.PacketStyle
        );

        // https://satori.js.org/zh-CN/resources/message.html
        if (
            eventBody?.Type != "message-created"
            || string.IsNullOrEmpty(eventBody.Channel?.Id)
            || eventBody.Message is null
        )
        {
            return;
        }

        _connectionLogger.Value.LogReceivedMessage(
            $"[{(eventBody.Channel.Type == ChannelType.Direct ? "私聊" : "群聊")}({eventBody.Channel.Id})] {eventBody.User?.Nick ?? eventBody.User?.Name}({eventBody.User?.Id}): {eventBody.Message.Content} (id={eventBody.Message.Id})"
        );

        var packets = new Packets { SatoriV1 = eventBody };

        if (!eventDispatcher.Dispatch(Event.ChannelMessageReceived, packets))
        {
            return;
        }

        if (eventBody.Channel.Type != ChannelType.Direct)
        {
            if (
                !IsListenedId(TargetType.Group, eventBody.Channel.Id)
                || !eventDispatcher.Dispatch(Event.GroupMessageReceived, packets)
            )
            {
                return;
            }
        }
        else if (!eventDispatcher.Dispatch(Event.GroupMessageReceived, packets))
        {
            return;
        }

        matcher.QueueMsg(packets);
    }
}
