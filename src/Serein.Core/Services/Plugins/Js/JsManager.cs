using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text.Json;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serein.Core.Models.Output;
using Serein.Core.Models.Plugins.Js;
using Serein.Core.Utils;
using Serein.Core.Utils.Extensions;
using Serein.Core.Utils.Json;

namespace Serein.Core.Services.Plugins.Js;

public class JsManager(IHost host) : IManager
{
    private readonly IHost _host = host;
    private IServiceProvider Services => _host.Services;
    private IPluginLogger Logger => Services.GetRequiredService<IPluginLogger>();
    public ConcurrentDictionary<string, JsPlugin> JsPlugins { get; } = new();
    public ConcurrentDictionary<string, object?> ExportedVariables { get; } = new();

    public void Unload()
    {
        foreach ((_, var jsPlugin) in JsPlugins)
        {
            jsPlugin.Dispose();
        }
        JsPlugins.Clear();
        ExportedVariables.Clear();
    }

    public void Load()
    {
        var files = Directory.GetFiles(PathConstants.PluginDirectory, "*.js");

        foreach (var file in files)
        {
            var name = Path.GetFileNameWithoutExtension(file);
            Config? preLoadConfig = null;
            var configPath = Path.Combine(
                PathConstants.PluginDirectory,
                name,
                PathConstants.PluginConfigFileName
            );

            if (File.Exists(configPath))
                preLoadConfig = JsonSerializer.Deserialize<Config>(
                    File.ReadAllText(configPath),
                    JsonSerializerOptionsFactory.CamelCase
                );

            var plugin = new JsPlugin(_host, name, preLoadConfig ?? new());
            var text = File.ReadAllText(file);

            JsPlugins.AddOrUpdate(name, plugin, (_, _) => plugin);
            try
            {
                plugin.Loaded = true;
                plugin.Execute(text);
            }
            catch (Exception e)
            {
                Logger.Log(LogLevel.Error, name, e.GetDetailString());
            }
        }
    }
}
