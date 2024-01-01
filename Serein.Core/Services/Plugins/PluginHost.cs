using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

using Jint.Runtime;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core.Models;
using Serein.Core.Models.Plugins.Js;
using Serein.Core.Utils;
using Serein.Core.Utils.Json;

namespace Serein.Core.Services.Plugins;

public class PluginHost
{
    private readonly IHost _host;
    private IServiceProvider Services => _host.Services;
    private IOutputHandler Logger => Services.GetRequiredService<IOutputHandler>();
    public ConcurrentDictionary<string, string> Variables { get; }
    public List<JsPlugin> JsPlugins { get; private set; }

    public PluginHost(IHost host)
    {
        _host = host;
        Variables = new();
        JsPlugins = new();
    }

    public void SetVariable(string key, object? value)
    {
        var str = value?.ToString() ?? string.Empty;
        Variables.AddOrUpdate(key, str, (_, _) => str);
    }

    public void LoadAll()
    {
        Unload();
        Directory.CreateDirectory(PathConstants.PluginDirectory);
        LoadJs();
    }

    private void Unload()
    {
        JsPlugins.ForEach((jsPlugin) => jsPlugin.Dispose());
        Variables.Clear();
    }

    private void LoadJs()
    {
        var files = Directory.GetFiles(PathConstants.PluginDirectory, "*.js");
        var plugins = new List<JsPlugin>();

        foreach (var file in files)
        {
            var @namespace = Path.GetFileNameWithoutExtension(file);
            PreloadConfig? preLoadConfig = null;
            var configPath = Path.Combine(
                PathConstants.PluginDirectory,
                @namespace,
                PathConstants.PluginConfigFileName
            );

            if (File.Exists(configPath))
                preLoadConfig = JsonSerializer.Deserialize<PreloadConfig>(
                    File.ReadAllText(configPath),
                    JsonSerializerOptionsFactory.CamelCase
                );

            var plugin = new JsPlugin(_host, @namespace, preLoadConfig ?? new());
            var text = File.ReadAllText(file);

            try
            {
                plugin.Execute(text);
                plugin.Loaded = true;
            }
            catch (JavaScriptException e)
            {
                Logger.LogPluginError(@namespace, $"{e.Message}\n{e.JavaScriptStackTrace}");
            }
            catch (Exception e)
            {
                Logger.LogPluginError(@namespace, e.Message);
            }
        }

        JsPlugins = plugins;
    }
}
