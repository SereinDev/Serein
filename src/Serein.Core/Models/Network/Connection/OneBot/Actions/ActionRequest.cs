using System.Text.Json.Serialization;

namespace Serein.Core.Models.Network.Connection.OneBot.Actions;

public class ActionRequest<T>
    where T : notnull, IActionParams
{
    public string Action { get; set; } = string.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public T? Params { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Echo { get; set; }
}
