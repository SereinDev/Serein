using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serein.Core.Models.Network.Connection.OneBot.Packets;
using Serein.Core.Models.Output;
using Serein.Core.Models.Plugins;
using Serein.Core.Models.Plugins.Net;
using Serein.Core.Services.Data;
using Serein.Core.Services.Plugins.Js;
using Serein.Core.Services.Plugins.Net;
using Serein.Core.Utils.Extensions;

namespace Serein.Core.Services.Plugins;

public class EventDispatcher(IHost host)
{
    private readonly IHost _host = host;
    private IServiceProvider Services => _host.Services;
    private NetManager Loader => Services.GetRequiredService<NetManager>();
    private JsManager JsManager => Services.GetRequiredService<JsManager>();
    private SettingProvider SettingProvider => Services.GetRequiredService<SettingProvider>();
    private IPluginLogger Logger => Services.GetRequiredService<IPluginLogger>();

    public bool Dispatch(Event @event, params object[] args)
    {
        var tasks = new List<Task<bool>>();
        var cancellationTokenSource = new CancellationTokenSource();

        foreach ((_, var jsPlugin) in JsManager.JsPlugins)
            tasks.Add(Task.Run(() => jsPlugin.Invoke(@event, cancellationTokenSource.Token, args)));

        foreach (var t in DispatchToDll(@event, cancellationTokenSource.Token, args))
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

    public List<Task> DispatchToDll(
        Event @event,
        CancellationToken cancellationToken,
        params object[] args
    )
    {
        var tasks = new List<Task>();
        Func<PluginBase, Task>? func = null;

        switch (@event)
        {
            case Event.ServerStarting:
                func = (p) => p.OnServerStarting(args.First().As<string>());
                break;

            case Event.ServerStopping:
                func = (p) => p.OnServerStopping(args.First().As<string>());
                break;

            case Event.GroupMessageReceived:
                func = (p) => p.OnGroupMessageReceived(args.First().As<MessagePacket>());
                break;

            case Event.PrivateMessageReceived:
                func = (p) => p.OnPrivateMessageReceived(args.First().As<MessagePacket>());
                break;

            case Event.WsDataReceived:
                func = (p) => p.OnWsDataReceived(args.First().As<string>());
                break;

            case Event.PacketReceived:
                func = (p) => p.OnPacketReceived(args.First().As<JsonNode>());
                break;

            case Event.ServerOutput:
                if (args.Length != 2)
                    throw new ArgumentException("缺少参数或类型不正确", nameof(args));

                func = (p) => p.OnServerOutput(args.First().As<string>(), args.Last().As<string>());
                break;

            case Event.ServerRawOutput:
                if (args.Length != 2)
                    throw new ArgumentException("缺少参数或类型不正确", nameof(args));

                func = (p) =>
                    p.OnServerRawOutput(args.First().As<string>(), args.Last().As<string>());
                break;

            case Event.ServerInput:
                if (args.Length != 2)
                    throw new ArgumentException("缺少参数或类型不正确", nameof(args));

                func = (p) => p.OnServerInput(args.First().As<string>(), args.Last().As<string>());
                break;

            case Event.ServerExited:
                if (
                    args.Length != 3
                    || args[0] is not string id
                    || args[1] is not int code
                    || args[0] is not DateTime time
                )
                    throw new ArgumentException("缺少参数或类型不正确", nameof(args));

                func = (p) => p.OnServerExited(id, code, time);
                break;

            case Event.SereinClosed:
                func = (p) => p.OnSereinClosed();
                break;

            case Event.SereinCrashed:
                func = (p) => p.OnSereinCrashed();
                break;

            case Event.PluginsLoaded:
                func = (p) => p.OnPluginsLoaded();
                break;

            case Event.PluginsUnloading:
                func = (p) => p.OnPluginsUnloading();
                break;

            case Event.ServerStarted:
                func = (p) => p.OnServerStarted(args.First().As<string>());
                break;

            default:
                throw new NotSupportedException();
        }

        if (func is null)
            throw new NotSupportedException();

        foreach ((var name, var plugin) in Loader.Plugins)
        {
            if (cancellationToken.IsCancellationRequested)
                break;

            try
            {
                tasks.Add(func.Invoke(plugin));
            }
            catch (Exception e)
            {
                Logger.Log(LogLevel.Error, name, $"触发事件{name}时出现异常：\n{e.GetDetailString()}");
            }
        }

        return tasks;
    }
}
