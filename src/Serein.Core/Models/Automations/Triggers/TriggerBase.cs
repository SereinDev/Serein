using System.Text.Json.Serialization;
using Serein.Core.Models.Abstractions;

namespace Serein.Core.Models.Automations.Triggers;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(MatchTrigger), typeDiscriminator: "match")]
[JsonDerivedType(typeof(EventTrigger), typeDiscriminator: "event")]
[JsonDerivedType(typeof(PluginTrigger), typeDiscriminator: "plugin")]
[JsonDerivedType(typeof(ScheduleTrigger), typeDiscriminator: "schedule")]
public abstract class TriggerBase : NotifyPropertyChangedModelBase
{
    public abstract TriggerType Type { get; } // 方便js插件判断

    public bool IsEnabled { get; set; } = true;
}
