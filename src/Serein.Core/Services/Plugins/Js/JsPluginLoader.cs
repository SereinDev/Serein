using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

using Microsoft.Extensions.Logging;

using Serein.Core.Models.Output;
using Serein.Core.Models.Plugins.Info;
using Serein.Core.Models.Plugins.Js;
using Serein.Core.Services.Data;
using Serein.Core.Services.Plugins.Net;
using Serein.Core.Utils;
using Serein.Core.Utils.Extensions;
using Serein.Core.Utils.Json;

namespace Serein.Core.Services.Plugins.Js;

public class JsPluginLoader(
    IServiceProvider serviceProvider,
    IPluginLogger pluginLogger,
    NetPluginLoader netPluginLoader,
    SettingProvider settingProvider
) : IPluginLoader<JsPlugin>
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly IPluginLogger _pluginLogger = pluginLogger;
    private readonly NetPluginLoader _netPluginLoader = netPluginLoader;
    private readonly SettingProvider _settingProvider = settingProvider;

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
        var entry = Path.Join(dir, pluginInfo.EntryFile ?? "index.js");

        if (!File.Exists(entry))
            throw new FileNotFoundException("找不到指定的入口点文件");

        JsPluginConfig? jsConfig = null;

        var configPath = Path.Join(dir, PathConstants.JsPluginConfigFileName);
        if (File.Exists(configPath))
            jsConfig = JsonSerializer.Deserialize<JsPluginConfig>(
                File.ReadAllText(configPath),
                JsonSerializerOptionsFactory.CamelCase
            );

        JsPlugin? jsPlugin = null;
        try
        {
            jsPlugin = new JsPlugin(
                _serviceProvider,
                pluginInfo,
                entry,
                jsConfig ?? JsPluginConfig.Default
            );
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
            if (
                _settingProvider.Value.Application.JSPatternToSkipLoadingSingleFile.Any(
                    file.EndsWith
                )
            )
                continue;

            var name = Path.GetFileNameWithoutExtension(file);
            var id = Guid.NewGuid().ToString("N");

            JsPlugin? jsPlugin = null;
            try
            {
                jsPlugin = new JsPlugin(
                    _serviceProvider,
                    new() { Id = id, Name = name },
                    file,
                    JsPluginConfig.Default
                );

                if (_netPluginLoader.Plugins.ContainsKey(name))
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
                    JsPlugins.TryAdd(id, jsPlugin);
            }
        }
    }
}
