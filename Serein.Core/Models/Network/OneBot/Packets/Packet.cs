namespace Serein.Core.Models.Network.OneBot.Packets;

public abstract class Packet
{
    public string PostType { get; init; } = string.Empty;

    public long Time { get; init; }
}
