namespace Serein.ConnectionProtocols.Models.OneBot.V11.Packets;

public abstract class Packet
{
    public string PostType { get; init; } = string.Empty;

    public long Time { get; init; }

    public long SelfId { get; init; }
}
