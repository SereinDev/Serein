namespace Serein.ConnectionProtocols.Models.OneBot.V11.Packets;

public class NoticePacket : Packet
{
    public string NoticeType { get; init; } = string.Empty;

    public string SubType { get; init; } = string.Empty;

    public long UserId { get; init; }

    public long OperatorId { get; init; }

    public long GroupId { get; init; }

    public long TargetId { get; init; }
}
