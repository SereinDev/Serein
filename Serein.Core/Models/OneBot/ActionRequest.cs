using System.Text.Json.Serialization;

namespace Serein.Core.Models.OneBot;

public class ActionRequest<T>
    where T : notnull
{
    public string Action { get; set; } = string.Empty;

    public T? Params { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Echo { get; set; }
}