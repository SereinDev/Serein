using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serein.Core.Models.Output;
using Serein.Core.Models.Plugins.Net;
using Serein.Core.Utils;

namespace Serein.Core.Services.Plugins.Net;

public class NetManager(IHost host) : IManager
{
    private const string Name = "";

    private readonly IHost _host = host;
    private IServiceProvider Services => _host.Services;
    private ISereinLogger Logger => Services.GetRequiredService<ISereinLogger>();
    private AssemblyLoadContext? _assemblyLoadContext;

    public ConcurrentDictionary<string, PluginBase> Plugins { get; } = new();

    private Assembly? ResolvingHandler(AssemblyLoadContext context, AssemblyName name)
    {
        var fileName = name.Name;
        return !string.IsNullOrEmpty(fileName) ? context.LoadFromAssemblyPath(fileName) : null;
    }

    public void Load()
    {
        _assemblyLoadContext = new(Name);
        _assemblyLoadContext.Resolving += ResolvingHandler;
        _assemblyLoadContext.SetProfileOptimizationRoot(PathConstants.PluginDirectory);

        foreach (var file in Directory.GetFiles(PathConstants.PluginDirectory, "*.dll"))
        {
            var name = Path.GetFileNameWithoutExtension(file);
            try
            {
                var assembly = _assemblyLoadContext.LoadFromAssemblyPath(file);
                var plugin = CreatePluginInstance(assembly.GetExportedTypes());

                if (Plugins.TryAdd(name, plugin))
                    throw new InvalidOperationException("插件名称重复");
            }
            catch (Exception e)
            {
                Logger.LogPlugin(LogLevel.Error, name, $"插件初始化失败：{e.Message}");
            }
        }
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
        foreach ((_, PluginBase plugin) in Plugins)
        {
            plugin.Dispose();
            GC.ReRegisterForFinalize(plugin);
        }
        _assemblyLoadContext?.Unload();
    }
}
