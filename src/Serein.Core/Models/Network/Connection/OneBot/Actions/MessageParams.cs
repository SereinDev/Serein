using System.Text.Json.Serialization;

namespace Serein.Core.Models.Network.Connection.OneBot.Actions;

public class MessageParams : IActionParams
{
    public string Type => UserId is not null ? "private" : "group";

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? UserId { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? GroupId { get; init; }

    public string Message { get; init; } = string.Empty;

    public bool AutoEscape { get; init; }
}
