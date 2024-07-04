using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serein.Core.Models.Output;
using Serein.Core.Models.Plugins;
using Serein.Core.Services.Data;
using Serein.Core.Services.Plugins.Js;
using Serein.Core.Services.Plugins.Net;
using Serein.Core.Utils.Extensions;

namespace Serein.Core.Services.Plugins;

public class EventDispatcher(IHost host)
{
    private readonly IHost _host = host;
    private IServiceProvider Services => _host.Services;
    private NetPluginLoader Loader => Services.GetRequiredService<NetPluginLoader>();
    private JsPluginLoader JsManager => Services.GetRequiredService<JsPluginLoader>();
    private SettingProvider SettingProvider => Services.GetRequiredService<SettingProvider>();
    private IPluginLogger Logger => Services.GetRequiredService<IPluginLogger>();

    public bool Dispatch(Event @event, params object[] args)
    {
        var tasks = new List<Task<bool>>();
        var cancellationTokenSource = new CancellationTokenSource();

        DispatchToJsPlugins(tasks, @event, cancellationTokenSource.Token, args);

        foreach (var t in DispatchToNetPlugins(@event, cancellationTokenSource.Token, args))
            if (t is Task<bool> tb)
                tasks.Add(tb);

        if (tasks.Count == 0)
        {
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
            return true;
        }

        if (SettingProvider.Value.Application.PluginEventMaxWaitingTime > 0)
            Task.WaitAll(
                tasks.ToArray(),
                SettingProvider.Value.Application.PluginEventMaxWaitingTime
            );

        cancellationTokenSource.Cancel();
        return tasks.Select((t) => !t.IsCompleted || t.Result).Any((b) => !b);
    }

    private void DispatchToJsPlugins(
        in List<Task<bool>> tasks,
        Event @event,
        CancellationToken cancellationToken,
        params object[] args
    )
    {
        foreach ((_, var jsPlugin) in JsManager.JsPlugins)
            tasks.Add(Task.Run(() => jsPlugin.Invoke(@event, cancellationToken, args)));
    }

    private List<Task> DispatchToNetPlugins(
        Event @event,
        CancellationToken cancellationToken,
        params object[] args
    )
    {
        var tasks = new List<Task>();

        foreach ((var name, var plugin) in Loader.NetPlugins)
        {
            if (cancellationToken.IsCancellationRequested)
                break;

            try
            {
                tasks.Add(plugin.Invoke(@event, args));
            }
            catch (Exception e)
            {
                Logger.Log(LogLevel.Error, name, $"触发事件{name}时出现异常：\n{e.GetDetailString()}");
            }
        }

        return tasks;
    }
}
