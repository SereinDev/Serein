using PropertyChanged;
using Serein.Core.Models.Abstractions;

namespace Serein.Plus.ViewModels;

public class PluginConsoleViewModel : NotifyPropertyChangedModelBase
{
    public int Total => JavaScriptPluginCount + NetPluginCount;

    [AlsoNotifyFor(nameof(Total))]
    public int JavaScriptPluginCount { get; internal set; }

    [AlsoNotifyFor(nameof(Total))]
    public int NetPluginCount { get; internal set; }
}
