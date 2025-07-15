using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Serein.Core.Models.Abstractions;
using Serein.Core.Models.Plugins.Info;
using Serein.Core.Models.Plugins.Js;
using Serein.Core.Services.Data;
using Serein.Core.Services.Plugins.Net;
using Serein.Core.Utils;
using Serein.Core.Utils.Extensions;
using Serein.Core.Utils.Json;

namespace Serein.Core.Services.Plugins.Js;

public sealed class JsPluginLoader(
    IServiceProvider serviceProvider,
    PluginLoggerBase pluginLogger,
    NetPluginLoader netPluginLoader,
    SettingProvider settingProvider
) : IPluginLoader<JsPlugin>
{
    private readonly ConcurrentDictionary<string, JsPlugin> _plugins = new();
    public IReadOnlyDictionary<string, JsPlugin> Plugins => _plugins;

    public void Unload()
    {
        foreach ((_, var jsPlugin) in _plugins)
        {
            jsPlugin.Dispose();
        }
        _plugins.Clear();
    }

    public void Load(PluginInfo pluginInfo, string dir)
    {
        pluginInfo.EntryFile ??= "index.js";
        var entry = Path.GetFullPath(Path.Join(dir, pluginInfo.EntryFile));

        if (!File.Exists(entry))
        {
            throw new FileNotFoundException("找不到指定的入口点文件");
        }

        JsPluginConfig? jsConfig = null;

        var configPath = Path.Join(dir, PathConstants.JsPluginConfigFileName);
        if (File.Exists(configPath))
        {
            jsConfig = JsonSerializer.Deserialize<JsPluginConfig>(
                File.ReadAllText(configPath),
                JsonSerializerOptionsFactory.Common
            );
        }

        JsPlugin? jsPlugin = null;
        try
        {
            jsPlugin = new JsPlugin(
                serviceProvider,
                pluginInfo,
                entry,
                jsConfig ?? JsPluginConfig.Default
            );
            jsPlugin.Execute(File.ReadAllText(entry));
        }
        finally
        {
            if (jsPlugin is not null)
            {
                _plugins.TryAdd(pluginInfo.Id, jsPlugin);
            }
        }
    }

    public void LoadSingle()
    {
        foreach (var file in Directory.GetFiles(PathConstants.PluginsDirectory, "*.js"))
        {
            if (
                settingProvider.Value.Application.JsFilesToExcludeFromLoading.Any(
                    file.EndsWith
                )
            )
            {
                continue;
            }

            var name = Path.GetFileNameWithoutExtension(file);
            var id = Guid.NewGuid().ToString("N");

            JsPlugin? jsPlugin = null;
            try
            {
                jsPlugin = new JsPlugin(
                    serviceProvider,
                    new()
                    {
                        Id = id,
                        Name = name,
                        EntryFile = Path.GetFileName(file),
                        Type = PluginType.Js,
                    },
                    Path.GetFullPath(file),
                    JsPluginConfig.Default
                );

                if (netPluginLoader.Plugins.ContainsKey(name))
                {
                    throw new NotSupportedException($"尝试加载“{file}”插件时发现Id重复");
                }

                jsPlugin.Execute(File.ReadAllText(file));
            }
            catch (Exception e)
            {
                pluginLogger.Log(LogLevel.Error, name, e.GetDetailString());
                jsPlugin?.Disable();
            }
            finally
            {
                if (jsPlugin is not null)
                {
                    _plugins.TryAdd(id, jsPlugin);
                }
            }
        }
    }
}
