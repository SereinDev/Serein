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

        var eventData = JsonSerializer.Deserialize<EventBody>(
            signal.Body,
            JsonSerializerOptionsFactory.PacketStyle
        );

        // https://satori.js.org/zh-CN/resources/message.html
        if (
            eventData?.Type != "message-created"
            || string.IsNullOrEmpty(eventData.Channel?.Id)
            || eventData.Message is null
        )
        {
            return;
        }

        _connectionLogger.Value.LogReceivedMessage(
            $"[频道({eventData.Channel.Id})] {eventData.Message.User?.Nick ?? eventData.Message.User?.Name}({eventData.Message.User?.Id}: {eventData.Message.Content} (id={eventData.Message.Id})"
        );

        if (!IsListenedId(TargetType.Channel, eventData.Channel.Id))
        {
            return;
        }
    }
}
