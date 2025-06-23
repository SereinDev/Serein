using Serein.ConnectionProtocols.Models.Satori.V1.Signals.Bodies;
using V11 = Serein.ConnectionProtocols.Models.OneBot.V11.Packets;
using V12 = Serein.ConnectionProtocols.Models.OneBot.V12.Packets;

namespace Serein.Core.Models.Network.Connection;

public readonly record struct Packets
{
    public V11.MessagePacket? OneBotV11 { get; init; }

    public V12.MessagePacket? OneBotV12 { get; init; }

    public EventBody? SatoriV1 { get; init; }
}
