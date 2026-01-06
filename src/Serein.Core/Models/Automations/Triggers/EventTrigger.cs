namespace Serein.Core.Models.Automations.Triggers;

public sealed class EventTrigger : TriggerBase
{
    public Events[] Events { get; set; } = [];
}
