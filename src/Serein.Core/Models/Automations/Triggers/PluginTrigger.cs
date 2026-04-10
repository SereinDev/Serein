namespace Serein.Core.Models.Automations.Triggers;

public sealed class PluginTrigger : TriggerBase
{
    public override TriggerType Type => TriggerType.Plugin;

    public string Key { get; set; } = string.Empty;
}
