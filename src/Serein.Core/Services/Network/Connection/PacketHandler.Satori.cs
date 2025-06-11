using System.Text.Json;
using System.Text.Json.Nodes;
using Serein.ConnectionProtocols.Models.Satori.V1.Signals;
using Serein.ConnectionProtocols.Models.Satori.V1.Signals.Bodies;
using Serein.Core.Models.Network.Connection;
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
            $"[频道({eventBody.Channel.Id})] {eventBody.Message.User?.Nick ?? eventBody.Message.User?.Name}({eventBody.Message.User?.Id}: {eventBody.Message.Content} (id={eventBody.Message.Id})"
        );

        if (!IsListenedId(TargetType.Channel, eventBody.Channel.Id))
        {
            return;
        }

        matcher.QueueMsg(eventBody);
    }
}
