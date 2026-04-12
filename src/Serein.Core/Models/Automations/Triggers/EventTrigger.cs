using System.Text.Json.Serialization;

namespace Serein.Core.Models.Automations.Triggers;

public sealed class EventTrigger : TriggerBase
{
    [JsonIgnore]
    public override TriggerType Type => TriggerType.Event;

    public Events Event { get; set; } = Events.None;
}
