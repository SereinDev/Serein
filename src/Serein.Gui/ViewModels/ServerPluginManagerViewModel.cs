using Serein.Core.Models.Abstractions;

namespace Serein.Gui.ViewModels;

public class ServerPluginManagerViewModel : NotifyPropertyChangedModelBase
{
    public required string Title { get; init; }
    public bool Remove { get; set; }
    public bool Enable { get; set; }
    public bool Disable { get; set; }
    public bool OpenInExplorer { get; set; }
}
