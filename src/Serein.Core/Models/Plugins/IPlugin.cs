using System;

using Serein.Core.Models.Plugins.Info;

namespace Serein.Core.Models.Plugins;

public interface IPlugin : IDisposable
{
    public string FileName { get; }

    public PluginInfo PluginInfo { get; }
}
