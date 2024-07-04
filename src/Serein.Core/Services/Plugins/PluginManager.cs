using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Serein.Core.Models.Output;
using Serein.Core.Models.Plugins;
using Serein.Core.Models.Plugins.Info;
using Serein.Core.Services.Plugins.Js;
using Serein.Core.Services.Plugins.Net;
using Serein.Core.Utils;
using Serein.Core.Utils.Extensions;
using Serein.Core.Utils.Json;

namespace Serein.Core.Services.Plugins;

public class PluginManager(
    IPluginLogger pluginLogger,
    JsPluginLoader jsPluginLoader,
    NetPluginLoader netPluginLoader,
    EventDispatcher eventDispatcher
)
{
    private readonly IPluginLogger _logger = pluginLogger;
    private readonly JsPluginLoader _jsPluginLoader = jsPluginLoader;
    private readonly NetPluginLoader _netPluginLoader = netPluginLoader;
    private readonly EventDispatcher _eventDispatcher = eventDispatcher;

    public ConcurrentDictionary<string, string> CommandVariables { get; } = new();
    public ConcurrentDictionary<string, object?> ExportedVariables { get; } = new();
    public event EventHandler? PluginsReloading;

    public void ExportVariables(string key, object? value)
    {
        ExportedVariables.AddOrUpdate(key, value, (_, _) => value);
    }

    public void SetCommandVariable(string key, object? value)
    {
        var str = value?.ToString() ?? string.Empty;
        CommandVariables.AddOrUpdate(key, str, (_, _) => str);
    }

    public void Load()
    {
        if (!Directory.Exists(PathConstants.PluginDirectory))
            return;

        foreach (var dir in Directory.GetDirectories(PathConstants.PluginDirectory))
        {
            if (!File.Exists(Path.Combine(dir, PathConstants.PluginInfoFileName)))
                continue;

            PluginInfo pluginInfo;
            try
            {
                pluginInfo =
                    JsonSerializer.Deserialize<PluginInfo>(
                        File.ReadAllText(Path.Combine(dir, PathConstants.PluginInfoFileName)),
                        JsonSerializerOptionsFactory.SnakeCase
                    ) ?? throw new InvalidDataException("插件信息为空");

                if (string.IsNullOrWhiteSpace(pluginInfo.Id))
                    throw new InvalidOperationException("Id不可为空");
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, Path.GetFileName(dir), e.Message);
                continue;
            }

            try
            {
                if (
                    _jsPluginLoader.JsPlugins.ContainsKey(pluginInfo.Id)
                    || _netPluginLoader.NetPlugins.ContainsKey(pluginInfo.Id)
                )
                    throw new InvalidOperationException("插件Id重复");

                _logger.Log(LogLevel.Information, pluginInfo.Name, "正在加载");

                if (pluginInfo.PluginType == PluginType.Js)
                    _jsPluginLoader.Load(pluginInfo, dir);
                else if (pluginInfo.PluginType == PluginType.Net)
                    _netPluginLoader.Load(pluginInfo, dir);
                else
                    throw new NotSupportedException("未指定插件类型");
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, pluginInfo.Name, e.GetDetailString());
            }
        }

        _jsPluginLoader.LoadSingle();
        _eventDispatcher.Dispatch(Event.PluginsLoaded);
    }

    public void Reload()
    {
        _eventDispatcher.Dispatch(Event.PluginsUnloading);

        _netPluginLoader.Unload();
        _jsPluginLoader.Unload();
        CommandVariables.Clear();
        ExportedVariables.Clear();

        PluginsReloading?.Invoke(null, EventArgs.Empty);
        Load();
    }
}
