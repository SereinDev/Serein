using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

using Microsoft.Extensions.Hosting;

using Serein.Core.Models.Output;
using Serein.Core.Models.Plugins;
using Serein.Core.Models.Plugins.Info;
using Serein.Core.Models.Plugins.Net;
using Serein.Core.Utils;

namespace Serein.Core.Services.Plugins.Net;

public class NetPluginLoader : IPluginLoader
{
    private static readonly string Name = typeof(NetPluginLoader).FullName!;
    private readonly IHost _host;
    private readonly IPluginLogger _logger;
    private AssemblyLoadContext _assemblyLoadContext;

    public ConcurrentDictionary<string, PluginBase> NetPlugins { get; } = new();
    public IReadOnlyDictionary<string, IPlugin> Plugins =>
        (IReadOnlyDictionary<string, IPlugin>)NetPlugins;

    public NetPluginLoader(IHost host, IPluginLogger logger)
    {
        _host = host;
        _logger = logger;
        _assemblyLoadContext = CreateNew();
    }

    private AssemblyLoadContext CreateNew()
    {
        var context = new AssemblyLoadContext(Name);
        context.Resolving += ResolvingHandler;
        context.SetProfileOptimizationRoot(Path.GetFullPath(PathConstants.PluginDirectory));

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

        var plugin = CreatePluginInstance(assembly.GetExportedTypes());
        plugin.FileName = Path.Combine(dir, entry);
        plugin.PluginInfo = pluginInfo;
        plugin.Services = _host.Services;

        if (!NetPlugins.TryAdd(pluginInfo.Id, plugin))
            throw new InvalidOperationException("插件名称重复");
    }

    private static PluginBase CreatePluginInstance(Type[] allTypes)
    {
        var t = allTypes.Where(type => type.BaseType != typeof(PluginBase));
        var count = t.Count();

        if (count == 0)
            throw new InvalidOperationException("未找到有效的插件入口点");
        if (count > 1)
            throw new InvalidOperationException("存在多个插件入口点");

        foreach (var type in t)
        {
            if (Activator.CreateInstance(type) is not PluginBase plugin)
                continue;

            return plugin;
        }

        throw new InvalidOperationException("未找到有效的插件入口点");
    }

    public void Unload()
    {
        foreach ((_, PluginBase plugin) in NetPlugins)
        {
            plugin.Dispose();
            GC.ReRegisterForFinalize(plugin);
        }
        _assemblyLoadContext.Unload();

        _assemblyLoadContext = CreateNew();
    }
}
