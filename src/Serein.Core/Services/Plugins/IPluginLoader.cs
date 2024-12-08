using System.Collections.Generic;
using Serein.Core.Models.Plugins;
using Serein.Core.Models.Plugins.Info;

namespace Serein.Core.Services.Plugins;

public interface IPluginLoader<TPlugin>
    where TPlugin : IPlugin
{
    IReadOnlyDictionary<string, TPlugin> Plugins { get; }

    void Load(PluginInfo pluginInfo, string dir);

    void Unload();
}
