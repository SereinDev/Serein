using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Serein.Core.Models.Output;
using Serein.Core.Models.Plugins;
using Serein.Core.Services.Data;
using Serein.Core.Services.Plugins.Js;
using Serein.Core.Services.Plugins.Net;
using Serein.Core.Utils.Extensions;

namespace Serein.Core.Services.Plugins;

public sealed class EventDispatcher(
    ILogger<EventDispatcher> logger,
    IPluginLogger pluginLogger,
    SettingProvider settingProvider,
    NetPluginLoader netPluginLoader,
    JsPluginLoader jsPluginLoader
)
{
    private readonly ILogger _logger = logger;
    private readonly IPluginLogger _pluginLogger = pluginLogger;
    private readonly SettingProvider _settingProvider = settingProvider;
    private readonly NetPluginLoader _netPluginLoader = netPluginLoader;
    private readonly JsPluginLoader _jsPluginLoader = jsPluginLoader;

    /// <summary>
    /// 分发事件
    /// </summary>
    /// <returns>如果此事件被拦截则返回false</returns>
    internal bool Dispatch(Event @event, params object[] args)
    {
        _logger.LogDebug("分发事件：{}", @event);

        var tasks = new List<Task<bool>>();
        var cancellationTokenSource = new CancellationTokenSource();

        DispatchToJsPlugins(tasks, @event, cancellationTokenSource.Token, args);

        foreach (var t in DispatchToNetPlugins(@event, cancellationTokenSource.Token, args))
        {
            if (t is Task<bool> tb)
            {
                tasks.Add(tb);
            }
        }

        _logger.LogDebug("事件（{}）任务数：{}", @event, tasks.Count);

        if (tasks.Count == 0)
        {
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
            return true;
        }

        if (_settingProvider.Value.Application.PluginEventMaxWaitingTime > 0)
        {
            Task.WaitAll([.. tasks], _settingProvider.Value.Application.PluginEventMaxWaitingTime);
        }

        cancellationTokenSource.Cancel();
        return !tasks.Where((task) => task.IsCompleted).Any((task) => !task.Result);
    }

    private void DispatchToJsPlugins(
        in List<Task<bool>> tasks,
        Event @event,
        CancellationToken cancellationToken,
        params object[] args
    )
    {
        foreach ((_, var jsPlugin) in _jsPluginLoader.Plugins)
        {
            tasks.Add(Task.Run(() => jsPlugin.Invoke(@event, cancellationToken, args)));
        }
    }

    private List<Task> DispatchToNetPlugins(
        Event @event,
        CancellationToken cancellationToken,
        params object[] args
    )
    {
        var tasks = new List<Task>();

        foreach ((var name, var plugin) in _netPluginLoader.Plugins)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }

            try
            {
                tasks.Add(plugin.Invoke(@event, args));
            }
            catch (Exception e)
            {
                _pluginLogger.Log(
                    LogLevel.Error,
                    name,
                    $"触发事件{name}时出现异常：\n{e.GetDetailString()}"
                );
            }
        }

        return tasks;
    }
}
