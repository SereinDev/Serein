using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

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
    EventDispatcher eventDispatcher,
    ILogger logger
)
{
    private readonly IPluginLogger _pluginLogger = pluginLogger;
    private readonly JsPluginLoader _jsPluginLoader = jsPluginLoader;
    private readonly NetPluginLoader _netPluginLoader = netPluginLoader;
    private readonly EventDispatcher _eventDispatcher = eventDispatcher;
    private readonly ILogger _logger = logger;

    public ConcurrentDictionary<string, string> CommandVariables { get; } = new();
    public ConcurrentDictionary<string, object?> ExportedVariables { get; } = new();

    public event EventHandler? PluginsReloading;
    public event EventHandler? PluginsLoaded;

    public bool Loading { get; private set; }
    public bool Reloading { get; private set; }

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
        if (Loading)
            throw new InvalidOperationException("正在加载插件");

        Loading = true;

        if (!Directory.Exists(PathConstants.PluginsDirectory))
        {
            Directory.CreateDirectory(PathConstants.PluginsDirectory);
            Loading = false;
            return;
        }

        _logger.LogDebug("开始加载插件");
        _pluginLogger.Log(LogLevel.Trace, string.Empty, "开始加载插件");

        foreach (var dir in Directory.GetDirectories(PathConstants.PluginsDirectory))
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
                _pluginLogger.Log(LogLevel.Error, Path.GetFileName(dir), e.Message);
                continue;
            }

            try
            {
                if (
                    _jsPluginLoader.JsPlugins.ContainsKey(pluginInfo.Id)
                    || _netPluginLoader.NetPlugins.ContainsKey(pluginInfo.Id)
                )
                    throw new InvalidOperationException("插件Id重复");

                _pluginLogger.Log(LogLevel.Information, pluginInfo.Name, "正在加载，路径：" + dir);

                if (pluginInfo.Type == PluginType.Js)
                    _jsPluginLoader.Load(pluginInfo, dir);
                else if (pluginInfo.Type == PluginType.Net)
                    _netPluginLoader.Load(pluginInfo, dir);
                else
                    throw new NotSupportedException("未指定插件类型");
            }
            catch (Exception e)
            {
                _pluginLogger.Log(LogLevel.Error, pluginInfo.Name, e.GetDetailString());
            }
        }

        _logger.LogDebug("开始加载Js单文件插件");
        _jsPluginLoader.LoadSingle();

        Loading = false;
        _eventDispatcher.Dispatch(Event.PluginsLoaded);
        _pluginLogger.Log(
            LogLevel.Trace,
            string.Empty,
            $"所有插件加载完毕。已加载{_jsPluginLoader.Plugins.Count + _netPluginLoader.Plugins.Count}个插件"
            );

        PluginsLoaded?.Invoke(this, EventArgs.Empty);
    }

    public void Unload()
    {
        _eventDispatcher.Dispatch(Event.PluginsUnloading);

        _netPluginLoader.Unload();
        _jsPluginLoader.Unload();
        CommandVariables.Clear();
        ExportedVariables.Clear();

    }
    public void Reload()
    {
        if (Reloading || Loading)
            throw new InvalidOperationException("正在加载插件");

        Reloading = true;

        try
        {
            PluginsReloading?.Invoke(null, EventArgs.Empty);
            Task.Delay(100).Wait();
            Unload();
            Load();
        }
        finally
        {
            Reloading = false;
        }
    }
}
