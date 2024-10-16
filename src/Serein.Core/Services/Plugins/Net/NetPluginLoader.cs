using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Loader;

using Microsoft.Extensions.DependencyInjection;

using Microsoft.Extensions.Logging;

using Serein.Core.Models.Output;
using Serein.Core.Models.Plugins.Info;
using Serein.Core.Models.Plugins.Net;
using Serein.Core.Utils;

namespace Serein.Core.Services.Plugins.Net;

public class NetPluginLoader(IServiceProvider serviceProvider, IPluginLogger logger)
    : IPluginLoader<PluginBase>
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

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

    private PluginBase CreatePluginInstance(Type[] allTypes)
    {
        var types = allTypes.Where(type => type.BaseType == typeof(PluginBase));
        var count = types.Count();

        if (count > 1)
            throw new InvalidOperationException("该程序集存在多个插件入口点");
        if (count == 0)
            throw new InvalidOperationException("未找到有效的插件入口点");

        var type = types.First();

        foreach (var ctor in type.GetConstructors())
        {
            if (ctor.IsStatic || !ctor.IsPublic)
                continue;

            var args = new List<object?>();

            foreach (var parameterInfo in ctor.GetParameters())
                if (parameterInfo.ParameterType == typeof(IServiceProvider))
                    args.Add(_serviceProvider);
                else
                    args.Add(_serviceProvider.GetRequiredService(parameterInfo.ParameterType));

            return ctor.Invoke([.. args]) as PluginBase ?? throw new NotSupportedException();
        }

        throw new InvalidOperationException("未找到插件入口点构造函数");
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
