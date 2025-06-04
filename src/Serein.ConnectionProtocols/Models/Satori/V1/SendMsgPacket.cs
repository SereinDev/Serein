namespace Serein.ConnectionProtocols.Models.Satori.V1;

public class SendMsgPacket
{
    public string ChannelId { get; init; } = string.Empty;

    public string Content { get; init; } = string.Empty;
}
