using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serein.Core.Models.Output;
using Serein.Core.Models.Plugins.Info;

namespace Serein.Core.Services.Plugins.Net;

public sealed class NetPluginLoader(
    IServiceProvider serviceProvider,
    ILogger<NetPluginLoader> logger,
    IPluginLogger pluginLogger
) : IPluginLoader<PluginBase>
{
    private readonly List<WeakReference<PluginAssemblyLoadContext>> _contexts = [];
    private readonly ConcurrentDictionary<string, PluginBase> _plugins = new();
    public IReadOnlyDictionary<string, PluginBase> Plugins => _plugins;

    public void Load(PluginInfo pluginInfo, string dir)
    {
        PluginBase? plugin = null;

        try
        {
            var entry = Path.GetFullPath(
                Path.Join(dir, pluginInfo.EntryFile ?? (pluginInfo.Id + ".dll"))
            );

            if (!File.Exists(entry))
            {
                throw new FileNotFoundException("插件入口点文件不存在", entry);
            }

            var context = new PluginAssemblyLoadContext(entry);
            context.Unloading += (_) => logger.LogDebug("插件\"{}\"上下文已卸载", pluginInfo.Id);

            _contexts.Add(new(context, true));

            var assembly = context.LoadFromAssemblyPath(entry);
            plugin = CreatePluginInstance(assembly.GetExportedTypes());
            plugin.FileName = entry;
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
            {
                _plugins.TryAdd(pluginInfo.Id, plugin);
            }
        }
    }

    private PluginBase CreatePluginInstance(Type[] allTypes)
    {
        var types = allTypes.Where(type => type.BaseType == typeof(PluginBase)).ToArray();
        var count = types.Length;

        if (count > 1)
        {
            throw new InvalidOperationException("该程序集存在多个插件入口点");
        }
        if (count == 0)
        {
            throw new InvalidOperationException("未找到有效的插件入口点");
        }

        var type = types.First();

        foreach (var ctor in type.GetConstructors())
        {
            if (ctor.IsStatic || !ctor.IsPublic)
            {
                continue;
            }

            var args = new List<object?>();

            foreach (var parameterInfo in ctor.GetParameters())
            {
                if (parameterInfo.ParameterType == typeof(IServiceProvider))
                {
                    args.Add(serviceProvider);
                }
                else
                {
                    args.Add(serviceProvider.GetRequiredService(parameterInfo.ParameterType));
                }
            }
            return ctor.Invoke([.. args]) as PluginBase ?? throw new NotSupportedException();
        }

        throw new InvalidOperationException("未找到插件入口点构造函数");
    }

    public void Unload()
    {
        foreach ((_, PluginBase plugin) in _plugins)
        {
            try
            {
                plugin.Dispose();
            }
            catch (Exception e)
            {
                pluginLogger.Log(
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
                {
                    context.Unload();
                }
            }
            catch (Exception e)
            {
                logger.LogDebug(e, "尝试卸载插件上下文时异常");
            }
        }

        _plugins.Clear();
    }
}
