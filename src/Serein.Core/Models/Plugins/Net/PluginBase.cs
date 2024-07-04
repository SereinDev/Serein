using System;

using Serein.Core.Models.Plugins.Info;

namespace Serein.Core.Models.Plugins.Net;

public abstract partial class PluginBase : IPlugin
{
    public string FileName { get; internal set; } = null!;
    public PluginInfo PluginInfo { get; internal set; } = null!;
    public IServiceProvider Services { get; internal set; } = null!;

    public abstract void Dispose();
}
