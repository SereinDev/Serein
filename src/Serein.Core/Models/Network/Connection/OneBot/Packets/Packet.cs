namespace Serein.Core.Models.Network.Connection.OneBot.Packets;

public abstract class Packet
{
    public string PostType { get; init; } = string.Empty;

    public long Time { get; init; }
}
