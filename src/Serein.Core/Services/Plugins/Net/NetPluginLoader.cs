using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

using Microsoft.Extensions.Hosting;

using Serein.Core.Models.Output;
using Serein.Core.Models.Plugins.Info;
using Serein.Core.Models.Plugins.Net;
using Serein.Core.Utils;

namespace Serein.Core.Services.Plugins.Net;

public class NetPluginLoader : IPluginLoader<PluginBase>
{
    private static readonly string Name = typeof(NetPluginLoader).FullName!;
    private readonly IHost _host;
    private readonly IPluginLogger _logger;
    private AssemblyLoadContext _assemblyLoadContext;

    public ConcurrentDictionary<string, PluginBase> NetPlugins { get; } = new();
    public IReadOnlyDictionary<string, PluginBase> Plugins => NetPlugins;

    public NetPluginLoader(IHost host, IPluginLogger logger)
    {
        _host = host;
        _logger = logger;
        _assemblyLoadContext = CreateNew();
    }

    private AssemblyLoadContext CreateNew()
    {
        var context = new AssemblyLoadContext(Name, true);
        context.Resolving += ResolvingHandler;
        context.SetProfileOptimizationRoot(Path.GetFullPath(PathConstants.PluginsDirectory));

        return context;
    }

    private Assembly? ResolvingHandler(AssemblyLoadContext context, AssemblyName name)
    {
        return null;
    }

    public void Load(PluginInfo pluginInfo, string dir)
    {
        var entry = pluginInfo.EntryFile ?? (pluginInfo.Id + ".dll");
        var assembly = _assemblyLoadContext.LoadFromAssemblyPath(Path.Combine(dir, entry));

        PluginBase? plugin = null;

        try
        {
            plugin = CreatePluginInstance(assembly.GetExportedTypes());
            plugin.FileName = Path.Combine(dir, entry);
            plugin.Info = pluginInfo;
            plugin.Services = _host.Services;
        }
        catch
        {
            plugin?.Disable();
            throw;
        }
        finally
        {
            if (plugin is not null)
                NetPlugins.TryAdd(pluginInfo.Id, plugin);
        }
    }

    private static PluginBase CreatePluginInstance(Type[] allTypes)
    {
        var t = allTypes.Where(type => type.BaseType != typeof(PluginBase));
        var count = t.Count();

        return count > 1
            ? throw new InvalidOperationException("存在多个插件入口点")
            : count == 0 || Activator.CreateInstance(t.First()) is not PluginBase plugin
            ? throw new InvalidOperationException("未找到有效的插件入口点")
            : plugin;
    }

    public void Unload()
    {
        foreach ((_, PluginBase plugin) in NetPlugins)
        {
            try
            {
                plugin.Dispose();
            }
            catch (Exception) { }
            GC.ReRegisterForFinalize(plugin);
        }
        _assemblyLoadContext.Unload();

        _assemblyLoadContext = CreateNew();
    }
}
