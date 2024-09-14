using System.ComponentModel;

using PropertyChanged;

namespace Serein.Plus.ViewModels;
public class PluginConsoleViewModel : INotifyPropertyChanged
{
    public int Total => JavaScriptPluginCount + NetPluginCount;

    [AlsoNotifyFor(nameof(Total))]
    public int JavaScriptPluginCount { get; internal set; }

    [AlsoNotifyFor(nameof(Total))]
    public int NetPluginCount { get; internal set; }

#pragma warning disable CS0067
    public event PropertyChangedEventHandler? PropertyChanged;
}
