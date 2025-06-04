using Serein.ConnectionProtocols.Models.Satori.V1.Channels;
using Serein.ConnectionProtocols.Models.Satori.V1.Guilds;

namespace Serein.ConnectionProtocols.Models.Satori.V1;

public class Message
{
    public string Id { get; init; } = string.Empty;

    public string Content { get; init; } = string.Empty;

    public Channel? Channel { get; init; }

    public Guild? Guild { get; init; }

    public GuildMember? Member { get; init; }

    public User? User { get; init; }

    public double CreatedAt { get; init; }

    public double? UpdatedAt { get; init; }
}
