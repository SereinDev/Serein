using System.Text.Json.Serialization;
using Serein.ConnectionProtocols.Models.OneBot.V12.Messages;

namespace Serein.ConnectionProtocols.Models.OneBot.V12.Actions;

public class MessageParams
{
    public string DetailType { get; init; } = string.Empty;

    public MessageSegment[] Message { get; init; } = [];

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? UserId { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? GroupId { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? GuildId { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ChannelId { get; init; }
}
