using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Serein.Core.Models.Abstractions;
using Serein.Core.Models.Plugins;
using Serein.Core.Services.Data;
using Serein.Core.Services.Plugins.Js;
using Serein.Core.Services.Plugins.Net;
using Serein.Core.Utils.Extensions;

namespace Serein.Core.Services.Plugins;

public sealed class EventDispatcher(
    ILogger<EventDispatcher> logger,
    PluginLoggerBase pluginLogger,
    SettingProvider settingProvider,
    NetPluginLoader netPluginLoader,
    JsPluginLoader jsPluginLoader
)
{
    private readonly ILogger _logger = logger;

    /// <summary>
    /// 分发事件
    /// </summary>
    /// <returns>如果此事件被拦截则返回false</returns>
    internal bool Dispatch(Event @event, params object[] args)
    {
        _logger.LogDebug("分发事件：{}", @event);

        if (jsPluginLoader.Plugins.Count == 0 && netPluginLoader.Plugins.Count == 0)
        {
            _logger.LogDebug("没有可用的插件，事件（{}）被忽略", @event);
            return true;
        }

        var tasks = new List<Task<bool>>();
        using var cancellationTokenSource = new CancellationTokenSource();

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
            return true;
        }

        if (settingProvider.Value.Application.PluginEventMaxWaitingTime > 0)
        {
            try
            {
                Task.WaitAll(
                    [.. tasks],
                    settingProvider.Value.Application.PluginEventMaxWaitingTime
                );
            }
            catch (OperationCanceledException)
            {
                _logger.LogDebug("事件（{}）任务被取消", @event);
            }
            catch (AggregateException e)
            {
                pluginLogger.Log(
                    LogLevel.Trace,
                    string.Empty,
                    $"事件（{@event}）任务执行时出现异常：\n{(e.InnerException ?? e).GetDetailString()}"
                );
            }
        }

        cancellationTokenSource.Cancel();

        var completedTasks = tasks.Where(task => task.Status == TaskStatus.RanToCompletion);
        _logger.LogDebug(
            "事件（{}）各任务结果：{}",
            @event,
            completedTasks.Select(task => task.Result)
        );

        var result = !completedTasks.Any() || completedTasks.All(task => task.Result);
        _logger.LogDebug(
            "事件（{}）分发完成，已完成的任务数：{}，结果：{}",
            @event,
            completedTasks.Count(),
            result
        );
        return result;
    }

    private void DispatchToJsPlugins(
        in List<Task<bool>> tasks,
        Event @event,
        CancellationToken cancellationToken,
        params object[] args
    )
    {
        foreach ((_, var jsPlugin) in jsPluginLoader.Plugins)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }

            if (jsPlugin.IsEnabled)
            {
                tasks.Add(Task.Run(() => jsPlugin.Invoke(@event, cancellationToken, args)));
            }
        }
    }

    private List<Task> DispatchToNetPlugins(
        Event @event,
        CancellationToken cancellationToken,
        params object[] args
    )
    {
        var tasks = new List<Task>();

        foreach ((var name, var plugin) in netPluginLoader.Plugins)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }

            if (plugin.IsEnabled)
            {
                try
                {
                    tasks.Add(plugin.Invoke(@event, args));
                }
                catch (Exception e)
                {
                    pluginLogger.Log(
                        LogLevel.Error,
                        name,
                        $"触发事件{name}时出现异常：\n{e.GetDetailString()}"
                    );
                }
            }
        }

        return tasks;
    }
}
