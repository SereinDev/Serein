using System.Collections.Generic;
using Serein.Core.Models.Automations.Triggers;
using Serein.Core.Models.Commands;

namespace Serein.Core.Models.Automations;

public sealed class AutomationTask
{
    public string Name { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public List<TriggerBase> Triggers { get; init; } = [];

    public List<Command> Commands { get; init; } = [];
}
