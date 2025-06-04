using System.Text.Json.Serialization;

namespace Serein.ConnectionProtocols.Models.OneBot.V11.Actions;

public class MessageParams
{
    public string Type => UserId is not null ? "private" : "group";

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? UserId { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? GroupId { get; init; }

    public string Message { get; init; } = string.Empty;

    public bool AutoEscape { get; init; }
}
