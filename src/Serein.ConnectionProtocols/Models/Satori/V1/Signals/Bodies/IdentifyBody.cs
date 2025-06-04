using System.Text.Json.Serialization;

namespace Serein.ConnectionProtocols.Models.Satori.V1.Signals.Bodies;

public class IdentifyBody
{
    public string? Token { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long? Sn { get; init; }
}
