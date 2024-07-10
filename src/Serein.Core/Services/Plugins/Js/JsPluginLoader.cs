using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    : IPluginLoader<JsPlugin>
{
    private readonly IHost _host = host;
    private readonly IPluginLogger _pluginLogger = pluginLogger;
    private readonly NetPluginLoader _netPluginLoader = netPluginLoader;

    internal ConcurrentDictionary<string, JsPlugin> JsPlugins { get; } = new();
    public IReadOnlyDictionary<string, JsPlugin> Plugins => JsPlugins;

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

        JsPlugin? jsPlugin = null;
        try
        {
            jsPlugin = new JsPlugin(_host, pluginInfo, entry, jsConfig ?? JsPluginConfig.Default);
            jsPlugin.Execute(File.ReadAllText(entry));
        }
        finally
        {
            if (jsPlugin is not null)
                JsPlugins.TryAdd(pluginInfo.Id, jsPlugin);
        }
    }

    public void LoadSingle()
    {
        foreach (var file in Directory.GetFiles(PathConstants.PluginsDirectory, "*.js"))
        {
            var name = Path.GetFileNameWithoutExtension(file);

            JsPlugin? jsPlugin = null;
            try
            {
                jsPlugin = new JsPlugin(
                   _host,
                   new() { Id = name, Name = name },
                   file,
                   JsPluginConfig.Default
               );

                if (_netPluginLoader.NetPlugins.ContainsKey(name))
                    throw new NotSupportedException($"尝试加载“{file}”插件时发现Id重复");

                jsPlugin.Execute(File.ReadAllText(file));
            }
            catch (Exception e)
            {
                _pluginLogger.Log(LogLevel.Error, name, e.GetDetailString());
                jsPlugin?.Disable();
            }
            finally
            {
                if (jsPlugin is not null)
                    JsPlugins.TryAdd(name, jsPlugin);
            }
        }
    }
}
