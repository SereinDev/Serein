using System.Text.Json.Serialization;

namespace Serein.ConnectionProtocols.Models.OneBot.Shared;

public class ActionRequest<T>
    where T : notnull
{
    public string Action { get; set; } = string.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public T? Params { get; set; }
}
