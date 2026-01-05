namespace Serein.Core.Models.Automations.Triggers;

public sealed class EventTrigger : TriggerBase
{
    public override TriggerType Type => TriggerType.Event;

    public Events[] Events { get; set; } = [];
}
