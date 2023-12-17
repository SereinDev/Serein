namespace Serein.Core.Models.OneBot;

public abstract class Packet
{
    public string PostType { get; init; } = string.Empty;

    public long Time { get; init; }
}
