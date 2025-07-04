using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Serein.Core.Models.Abstractions;
using Serein.Core.Models.Plugins;
using Serein.Core.Models.Plugins.Info;
using Serein.Core.Services.Network.Connection;
using Serein.Core.Services.Permissions;
using Serein.Core.Services.Plugins.Js;
using Serein.Core.Services.Plugins.Js.BuiltInModules;
using Serein.Core.Services.Plugins.Net;
using Serein.Core.Services.Plugins.Storages;
using Serein.Core.Utils;
using Serein.Core.Utils.Extensions;
using Serein.Core.Utils.Json;

namespace Serein.Core.Services.Plugins;

public sealed partial class PluginManager(
    ILogger<PluginManager> logger,
    SereinApp sereinApp,
    FileSystem fileSystem,
    PluginLoggerBase pluginLogger,
    PacketHandler packetHandler,
    SessionStorage sessionStorage,
    JsPluginLoader jsPluginLoader,
    NetPluginLoader netPluginLoader,
    EventDispatcher eventDispatcher,
    PermissionManager permissionManager
)
{
    [GeneratedRegex(@"^[a-zA-Z][a-zA-Z0-9\-]{2,24}$")]
    private static partial Regex GetIdRegex();

    private static readonly Regex IdRegex = GetIdRegex();
    private static readonly JsonSerializerOptions Options = new(JsonSerializerOptionsFactory.Common)
    {
        Converters = { new JsonStringEnumConverter() },
    };

    private readonly ILogger _logger = logger;

    public ConcurrentDictionary<string, string> CommandVariables { get; } = new();
    public ConcurrentDictionary<string, object?> ExportedVariables { get; } = new();

    public event EventHandler? PluginsReloading;
    public event EventHandler? PluginsLoaded;

    public bool IsLoading { get; private set; }
    public bool IsReloading { get; private set; }

    public void Load()
    {
        try
        {
            if (IsLoading)
            {
                throw new InvalidOperationException("正在加载插件");
            }

            IsLoading = true;

            if (!Directory.Exists(PathConstants.PluginsDirectory))
            {
                Directory.CreateDirectory(PathConstants.PluginsDirectory);
                IsLoading = false;
                return;
            }

            _logger.LogDebug("开始加载插件");
            pluginLogger.Log(LogLevel.Trace, string.Empty, "开始加载插件");

            foreach (var dir in Directory.GetDirectories(PathConstants.PluginsDirectory))
            {
                _logger.LogDebug("尝试从{}加载插件", dir);

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

                    if (string.IsNullOrEmpty(pluginInfo.Id))
                    {
                        throw new InvalidOperationException("Id不可为空");
                    }

                    if (!IdRegex.IsMatch(pluginInfo.Id))
                    {
                        throw new InvalidOperationException("Id不符合规范");
                    }
                }
                catch (Exception e)
                {
                    pluginLogger.Log(LogLevel.Error, Path.GetFileName(dir), e.Message);
                    continue;
                }

                if (pluginInfo.Targets is not null)
                {
                    if (
                        pluginInfo.Targets.Max is not null
                        && pluginInfo.Targets.Max
                            < new Version(
                                sereinApp.Version.Major,
                                sereinApp.Version.Minor,
                                sereinApp.Version.Build
                            ) // fix (2.1.0 < 2.1.0.0) == true
                    )
                    {
                        pluginLogger.Log(
                            LogLevel.Warning,
                            pluginInfo.Name,
                            $"插件版本可能不兼容，当前版本：{sereinApp.Version}，插件要求最高版本：{pluginInfo.Targets.Max}"
                        );
                    }

                    if (
                        pluginInfo.Targets.Min is not null
                        && pluginInfo.Targets.Min > sereinApp.Version
                    )
                    {
                        pluginLogger.Log(
                            LogLevel.Warning,
                            pluginInfo.Name,
                            $"插件版本可能不兼容，当前版本：{sereinApp.Version}，插件要求最低版本：{pluginInfo.Targets.Min}"
                        );
                    }
                }

                try
                {
                    if (
                        jsPluginLoader.Plugins.ContainsKey(pluginInfo.Id)
                        || netPluginLoader.Plugins.ContainsKey(pluginInfo.Id)
                    )
                    {
                        throw new InvalidOperationException("插件Id重复");
                    }

                    pluginLogger.Log(
                        LogLevel.Trace,
                        string.Empty,
                        $"\"{pluginInfo.Name}\"正在加载"
                    );

                    if (pluginInfo.Type == PluginType.Js)
                    {
                        jsPluginLoader.Load(pluginInfo, dir);
                    }
                    else if (pluginInfo.Type == PluginType.Net)
                    {
                        netPluginLoader.Load(pluginInfo, dir);
                    }
                    else
                    {
                        throw new NotSupportedException("未指定插件类型");
                    }
                    pluginLogger.Log(
                        LogLevel.Trace,
                        string.Empty,
                        $"\"{pluginInfo.Name}\"加载成功并已启用"
                    );
                }
                catch (Exception e)
                {
                    pluginLogger.Log(LogLevel.Error, pluginInfo.Name, e.GetDetailString());
                    logger.LogError(e, "加载插件\"{PluginName}\"时发生错误", pluginInfo.Name);
                }
            }

            _logger.LogDebug("开始加载Js单文件插件");
            jsPluginLoader.LoadSingle();

            eventDispatcher.Dispatch(Event.PluginsLoaded);
            pluginLogger.Log(
                LogLevel.Trace,
                string.Empty,
                $"所有插件加载完毕。已加载{jsPluginLoader.Plugins.Count + netPluginLoader.Plugins.Count}个插件"
            );

            PluginsLoaded?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception e)
        {
            _logger.LogDebug(e, "插件加载时出现异常");
            throw;
        }
        finally
        {
            IsLoading = false;
            _logger.LogDebug("插件加载结束");
        }
    }

    public void Unload()
    {
        packetHandler.PluginPacketHandler = null;
        eventDispatcher.Dispatch(Event.PluginsUnloading);

        fileSystem.DisposeAll();
        netPluginLoader.Unload();
        jsPluginLoader.Unload();
        sessionStorage.Clear();
        permissionManager.Clear();
        CommandVariables.Clear();
        ExportedVariables.Clear();
    }

    public void Reload()
    {
        if (IsReloading || IsLoading)
        {
            throw new InvalidOperationException("正在加载插件");
        }

        IsReloading = true;

        try
        {
            PluginsReloading?.Invoke(null, EventArgs.Empty);
            Task.Delay(100).Wait();
            Unload();
            Load();
        }
        finally
        {
            IsReloading = false;
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
