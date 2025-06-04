namespace Serein.ConnectionProtocols.Models.OneBot.V12.Packets;

public class EventPacket
{
    public string Id { get; init; } = string.Empty;

    public double Time { get; init; }

    public EventType Type { get; init; }

    public string DetailType { get; init; } = string.Empty;

    public string SubType { get; init; } = string.Empty;
}
