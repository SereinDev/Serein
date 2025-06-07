using System.Text.Json.Serialization;
using Serein.ConnectionProtocols.Models.OneBot.V12.Messages;

namespace Serein.ConnectionProtocols.Models.OneBot.V12.Packets;

public class MessagePacket : EventPacket
{
    public string MessageId { get; init; } = string.Empty;

    public MessageSegment[] Message { get; init; } = [];

    public string AltMessage { get; init; } = string.Empty;

    public string UserId { get; init; } = string.Empty;

    public string? GroupId { get; init; }

    public string? GuildId { get; init; }

    public string? ChannelId { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter<MessageDetailType>))]
    public new MessageDetailType DetailType { get; init; }

    [JsonIgnore]
    public string FriendlyMessage
    {
        get
        {
            _friendlyMessage ??=
                Message.Length == 0 || !string.IsNullOrEmpty(AltMessage)
                    ? AltMessage
                    : string.Join<MessageSegment>("", Message);

            return _friendlyMessage;
        }
    }

    private string? _friendlyMessage;
}
