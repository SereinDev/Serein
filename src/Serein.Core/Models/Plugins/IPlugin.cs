using System;
using System.ComponentModel;
using Serein.Core.Models.Plugins.Info;

namespace Serein.Core.Models.Plugins;

public interface IPlugin : IDisposable, INotifyPropertyChanged
{
    public string FileName { get; }

    public PluginInfo Info { get; }

    public bool IsEnabled { get; }

    public void Disable();
}
