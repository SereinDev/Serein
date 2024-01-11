using System.ComponentModel;

namespace Serein.Core.Models.Settings;

public class PagesSetting : INotifyPropertyChanged
{
    public bool ServerPanel { get; set; } = true;

    public bool ServerPluginManager { get; set; } = true;

    public bool Match { get; set; } = true;

    public bool Schedule { get; set; } = true;

    public bool Bot { get; set; } = true;

    public bool Member { get; set; } = true;

    public bool JSPlugin { get; set; } = true;

    public bool Settings { get; set; } = true;

#pragma warning disable CS0067
    public event PropertyChangedEventHandler? PropertyChanged;
}
