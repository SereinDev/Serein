using System.Text.Json.Serialization;

namespace Serein.Core.Models.Automations.Triggers;

public sealed class PluginTrigger : TriggerBase
{
    [JsonIgnore]
    public override TriggerType Type => TriggerType.Plugin;

    public string Key { get; set; } = string.Empty;
}
