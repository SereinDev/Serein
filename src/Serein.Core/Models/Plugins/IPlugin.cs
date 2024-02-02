using System;

namespace Serein.Core.Models.Plugins;

public interface IPlugin : IDisposable
{
    public PluginInfo Info { get; }
}
