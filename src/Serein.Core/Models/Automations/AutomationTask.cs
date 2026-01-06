using System.Collections.ObjectModel;
using Serein.Core.Models.Abstractions;
using Serein.Core.Models.Automations.Triggers;
using Serein.Core.Models.Commands;

namespace Serein.Core.Models.Automations;

public sealed class AutomationTask : NotifyPropertyChangedModelBase
{
    public ObservableCollection<TriggerBase> Triggers { get; set; } = [];

    public ObservableCollection<Command> Commands { get; set; } = [];

    public string Description { get; set; } = string.Empty;
}
