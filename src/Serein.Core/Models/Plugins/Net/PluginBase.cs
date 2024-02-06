using System;

namespace Serein.Core.Models.Plugins.Net;

public abstract partial class PluginBase : IPlugin
{
    public abstract PluginInfo Info { get; }

    public abstract void Dispose();
}
