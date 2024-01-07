namespace Serein.Core.Models.OneBot.Packets;

public abstract class Packet
{
    public string PostType { get; init; } = string.Empty;

    public long Time { get; init; }
}
