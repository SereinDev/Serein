namespace Serein.ConnectionProtocols.Models.Satori.V1.Channels;

public class Channel
{
    public string Id { get; init; } = string.Empty;

    public ChannelType Type { get; init; }

    public string? Name { get; init; }

    public string? ParentId { get; init; }
}
