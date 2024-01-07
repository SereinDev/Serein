using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core.Models;
using Serein.Core.Models.Plugins.CSharp;
using Serein.Core.Utils;

namespace Serein.Core.Services.Plugins.CSharp;

public class Loader
{
    private const string Name = "";

    private readonly IHost _host;
    private IServiceProvider Services => _host.Services;
    private IOutputHandler Logger => Services.GetRequiredService<IOutputHandler>();
    private AssemblyLoadContext? _assemblyLoadContext;

    public readonly ConcurrentDictionary<string, PluginBase> Plugins;

    public Loader(IHost host)
    {
        _host = host;
        Plugins = new();
    }

    private Assembly? ResolvingHandler(AssemblyLoadContext context, AssemblyName name)
    {
        var fileName = name.Name;
        return null;
    }

    public void Load()
    {
        _assemblyLoadContext = new(Name);
        _assemblyLoadContext.Resolving += ResolvingHandler;
        _assemblyLoadContext.SetProfileOptimizationRoot(PathConstants.PluginDirectory);

        foreach (var file in Directory.GetFiles(PathConstants.PluginDirectory, "*.dll"))
        {
            var fileName = Path.GetFileName(file);
            try
            {
                var assembly = _assemblyLoadContext.LoadFromAssemblyPath(file);
                var types = assembly.GetExportedTypes();
                var plugin = CreatePluginInstance(types);

                if (Plugins.TryAdd(fileName, plugin))
                    throw new InvalidOperationException("插件名称重复");
            }
            catch (Exception e)
            {
                Logger.LogPluginError(fileName, $"插件初始化失败：{e.Message}");
            }
        }
    }

    private static PluginBase CreatePluginInstance(Type[] types)
    {
        var t = types.Where(type => type.BaseType != typeof(PluginBase));

        if (t.Count() != 1)
            throw new InvalidOperationException("未找到有效的插件入口点");

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
        foreach ((_, PluginBase plugin) in Plugins)
        {
            plugin.Dispose();
            GC.ReRegisterForFinalize(plugin);
        }
        _assemblyLoadContext?.Unload();
    }
}
