namespace Serein.ConnectionProtocols.Models.OneBot.V12.Packets;

public class NoticePacket : EventPacket
{
    public string UserId { get; init; } = string.Empty;

    public string OperatorId { get; init; } = string.Empty;

    public string? GroupId { get; init; }

    public string? GuildId { get; init; }

    public string? ChannelId { get; init; }
}
