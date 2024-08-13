using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Loader;

using Microsoft.Extensions.Logging;

using Serein.Core.Models.Output;
using Serein.Core.Models.Plugins.Info;
using Serein.Core.Models.Plugins.Net;
using Serein.Core.Utils;

namespace Serein.Core.Services.Plugins.Net;

public class NetPluginLoader(IPluginLogger logger) : IPluginLoader<PluginBase>
{
    private readonly IPluginLogger _logger = logger;
    private readonly List<WeakReference<AssemblyLoadContext>> _contexts = [];
    public ConcurrentDictionary<string, PluginBase> NetPlugins { get; } = new();
    public IReadOnlyDictionary<string, PluginBase> Plugins => NetPlugins;
    public void Load(PluginInfo pluginInfo, string dir)
    {
        PluginBase? plugin = null;

        try
        {
            var entry = pluginInfo.EntryFile ?? (pluginInfo.Id + ".dll");

            var context = new AssemblyLoadContext(pluginInfo.Id, true);
            context.SetProfileOptimizationRoot(Path.GetFullPath(PathConstants.PluginsDirectory));

            _contexts.Add(new(context, true));

            var assembly = context.LoadFromAssemblyPath(Path.GetFullPath(Path.Join(dir, entry)));
            plugin = CreatePluginInstance(assembly.GetExportedTypes());
            plugin.FileName = Path.Join(dir, entry);
            plugin.Info = pluginInfo;
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
        var types = allTypes.Where(type => type.BaseType == typeof(PluginBase));
        var count = types.Count();

        return count > 1
            ? throw new InvalidOperationException("该程序集存在多个插件入口点")
            : count == 0 || Activator.CreateInstance(types.First()) is not PluginBase plugin
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
            catch (Exception e)
            {
                _logger.Log(
                    LogLevel.Error,
                    plugin.Info.Name,
                    "卸载插件时出现异常：" + Environment.NewLine + e.Message
               );
            }
            GC.ReRegisterForFinalize(plugin);
        }

        foreach (var reference in _contexts)
        {
            try
            {
                if (reference.TryGetTarget(out var context))
                    context.Unload();
            }
            catch { }
        }

        NetPlugins.Clear();
    }
}
