using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Serein.Core.Models.Output;
using Serein.Core.Models.Plugins;
using Serein.Core.Models.Plugins.Info;
using Serein.Core.Services.Permissions;
using Serein.Core.Services.Plugins.Js;
using Serein.Core.Services.Plugins.Net;
using Serein.Core.Services.Plugins.Storages;
using Serein.Core.Utils;
using Serein.Core.Utils.Extensions;
using Serein.Core.Utils.Json;

namespace Serein.Core.Services.Plugins;

public sealed class PluginManager(
    ILogger<PluginManager> logger,
    IPluginLogger pluginLogger,
    JsPluginLoader jsPluginLoader,
    NetPluginLoader netPluginLoader,
    EventDispatcher eventDispatcher,
    SessionStorage sessionStorage,
    PermissionManager permissionManager
)
{
    private static readonly JsonSerializerOptions Options =
        new(JsonSerializerOptionsFactory.Common)
        {
            Converters = { new JsonStringEnumConverter() },
        };

    private readonly IPluginLogger _pluginLogger = pluginLogger;
    private readonly JsPluginLoader _jsPluginLoader = jsPluginLoader;
    private readonly NetPluginLoader _netPluginLoader = netPluginLoader;
    private readonly EventDispatcher _eventDispatcher = eventDispatcher;
    private readonly SessionStorage _sessionStorage = sessionStorage;
    private readonly PermissionManager _permissionManager = permissionManager;
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
        try
        {
            if (Loading)
            {
                throw new InvalidOperationException("正在加载插件");
            }

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
                if (!File.Exists(Path.Join(dir, PathConstants.PluginInfoFileName)))
                {
                    continue;
                }

                PluginInfo pluginInfo;
                try
                {
                    pluginInfo =
                        JsonSerializer.Deserialize<PluginInfo>(
                            File.ReadAllText(Path.Join(dir, PathConstants.PluginInfoFileName)),
                            Options
                        ) ?? throw new InvalidDataException("插件信息为空");

                    if (string.IsNullOrWhiteSpace(pluginInfo.Id))
                    {
                        throw new InvalidOperationException("Id不可为空");
                    }
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
                    {
                        throw new InvalidOperationException("插件Id重复");
                    }

                    _pluginLogger.Log(
                        LogLevel.Trace,
                        string.Empty,
                        $"\"{pluginInfo.Name}\"正在加载"
                    );

                    if (pluginInfo.Type == PluginType.Js)
                    {
                        _jsPluginLoader.Load(pluginInfo, dir);
                    }
                    else if (pluginInfo.Type == PluginType.Net)
                    {
                        _netPluginLoader.Load(pluginInfo, dir);
                    }
                    else
                    {
                        throw new NotSupportedException("未指定插件类型");
                    }
                    _pluginLogger.Log(
                        LogLevel.Trace,
                        string.Empty,
                        $"\"{pluginInfo.Name}\"加载成功并已启用"
                    );
                }
                catch (Exception e)
                {
                    _pluginLogger.Log(LogLevel.Error, pluginInfo.Name, e.GetDetailString());
                }
            }

            _logger.LogDebug("开始加载Js单文件插件");
            _jsPluginLoader.LoadSingle();

            _eventDispatcher.Dispatch(Event.PluginsLoaded);
            _pluginLogger.Log(
                LogLevel.Trace,
                string.Empty,
                $"所有插件加载完毕。已加载{_jsPluginLoader.Plugins.Count + _netPluginLoader.Plugins.Count}个插件"
            );

            PluginsLoaded?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception e)
        {
            _logger.LogDebug(e, "[{}] 插件加载时出现异常", nameof(PluginManager));
            throw;
        }
        finally
        {
            Loading = false;
            _logger.LogDebug("插件加载结束");
        }
    }

    public void Unload()
    {
        _eventDispatcher.Dispatch(Event.PluginsUnloading);

        _netPluginLoader.Unload();
        _jsPluginLoader.Unload();
        _sessionStorage.Clear();
        _permissionManager.Clear();
        CommandVariables.Clear();
        ExportedVariables.Clear();
    }

    public void Reload()
    {
        if (Reloading || Loading)
        {
            throw new InvalidOperationException("正在加载插件");
        }

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

    public static string Resolve(IPlugin plugin, params string[] paths)
    {
        var path = Path.Combine(paths);
        return Path.IsPathRooted(path)
            ? path
            : Path.GetFullPath(Path.Join(Path.GetDirectoryName(plugin.FileName), path));
    }
}
