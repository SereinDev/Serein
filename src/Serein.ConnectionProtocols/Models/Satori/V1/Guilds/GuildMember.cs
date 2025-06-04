namespace Serein.ConnectionProtocols.Models.Satori.V1.Guilds;

public class GuildMember
{
    public User? User { get; init; }

    public string? Nick { get; init; }

    public string? Avatar { get; init; }

    public double? JoinedAt { get; init; }
}
