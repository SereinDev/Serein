using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serein.Core.Models.Output;
using Serein.Core.Models.Plugins;
using Serein.Core.Models.Plugins.Info;
using Serein.Core.Models.Plugins.Js;
using Serein.Core.Services.Plugins.Net;
using Serein.Core.Utils;
using Serein.Core.Utils.Extensions;
using Serein.Core.Utils.Json;

namespace Serein.Core.Services.Plugins.Js;

public class JsPluginLoader(IHost host, IPluginLogger pluginLogger, NetPluginLoader netPluginLoader)
    : IPluginLoader
{
    private readonly IHost _host = host;
    private readonly IPluginLogger _pluginLogger = pluginLogger;
    private readonly NetPluginLoader _netPluginLoader = netPluginLoader;

    internal ConcurrentDictionary<string, JsPlugin> JsPlugins { get; } = new();
    public IReadOnlyDictionary<string, IPlugin> Plugins =>
        (IReadOnlyDictionary<string, IPlugin>)JsPlugins;

    public void Unload()
    {
        foreach ((_, var jsPlugin) in JsPlugins)
        {
            jsPlugin.Dispose();
        }
        JsPlugins.Clear();
    }

    public void Load(PluginInfo pluginInfo, string dir)
    {
        var entry = pluginInfo.EntryFile ?? "index.js";

        if (!File.Exists(Path.Combine(dir, entry)))
            throw new FileNotFoundException("找不到指定的入口点文件");

        JsPluginConfig? jsConfig = null;

        var configPath = Path.Combine(dir, PathConstants.JsPluginConfigFileName);
        if (File.Exists(configPath))
            jsConfig = JsonSerializer.Deserialize<JsPluginConfig>(
                File.ReadAllText(configPath),
                JsonSerializerOptionsFactory.CamelCase
            );

        var plugin = new JsPlugin(_host, pluginInfo, entry, jsConfig ?? JsPluginConfig.Default);
        var text = File.ReadAllText(entry);

        JsPlugins.TryAdd(pluginInfo.Id, plugin);
        plugin.Loaded = true;
        plugin.Execute(text);
    }

    public void LoadSingle()
    {
        foreach (var file in Directory.GetFiles(PathConstants.PluginDirectory, "*.js"))
        {
            var name = Path.GetFileNameWithoutExtension(file);
            try
            {
                var plugin = new JsPlugin(
                    _host,
                    new() { Id = name, Name = name },
                    file,
                    JsPluginConfig.Default
                );
                var text = File.ReadAllText(file);

                if (_netPluginLoader.NetPlugins.ContainsKey(name))
                    throw new NotSupportedException($"尝试加载“{file}”插件时发现Id重复");

                JsPlugins.AddOrUpdate(
                    name,
                    plugin,
                    (_, _) => throw new NotSupportedException($"尝试加载“{file}”插件时发现Id重复")
                );
                plugin.Loaded = true;
                plugin.Execute(text);
            }
            catch (Exception e)
            {
                _pluginLogger.Log(LogLevel.Error, name, e.GetDetailString());
            }
        }
    }
}
