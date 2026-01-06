using System.Text.Json.Serialization;

namespace Serein.Core.Models.Automations.Triggers;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(MatchTrigger), typeDiscriminator: "match")]
[JsonDerivedType(typeof(EventTrigger), typeDiscriminator: "event")]
[JsonDerivedType(typeof(PluginTrigger), typeDiscriminator: "plugin")]
[JsonDerivedType(typeof(ScheduleTrigger), typeDiscriminator: "schedule")]
public abstract class TriggerBase
{
    public bool IsEnabled { get; set; }
}
