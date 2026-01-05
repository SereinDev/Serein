using System.Text.Json.Serialization;

namespace Serein.Core.Models.Automations.Triggers;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(MatchTrigger), typeDiscriminator: "match")]
[JsonDerivedType(typeof(EventTrigger), typeDiscriminator: "event")]
[JsonDerivedType(typeof(PluginTrigger), typeDiscriminator: "plugin")]
[JsonDerivedType(typeof(ScheduleTrigger), typeDiscriminator: "schedule")]
public abstract class TriggerBase
{
    [JsonConverter(typeof(JsonStringEnumConverter<TriggerType>))]
    public abstract TriggerType Type { get; }

    public bool IsEnabled { get; set; }
}
