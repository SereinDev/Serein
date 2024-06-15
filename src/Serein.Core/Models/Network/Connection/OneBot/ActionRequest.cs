using System.Text.Json.Serialization;

using Serein.Core.Models.Network.Connection.OneBot.ActionParams;

namespace Serein.Core.Models.Network.Connection.OneBot;

public class ActionRequest<T>
    where T : notnull, IActionParams
{
    public string Action { get; set; } = string.Empty;

    public T? Params { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Echo { get; set; }
}
