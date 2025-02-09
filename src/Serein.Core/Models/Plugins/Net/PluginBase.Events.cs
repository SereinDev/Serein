using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Serein.Core.Models.Network.Connection.OneBot.Packets;

namespace Serein.Core.Models.Plugins.Net;

public abstract partial class PluginBase
{
    protected virtual Task<bool> OnServerStarting(Services.Servers.Server server) =>
        Task.FromResult(true);

    protected virtual Task OnServerStarted(Services.Servers.Server server) => Task.CompletedTask;

    protected virtual Task<bool> OnServerStopping(Services.Servers.Server server) =>
        Task.FromResult(true);

    protected virtual Task OnServerExited(
        Services.Servers.Server server,
        int exitcode,
        DateTime exitTime
    ) => Task.CompletedTask;

    protected virtual Task<bool> OnServerOutput(Services.Servers.Server server, string line) =>
        Task.FromResult(true);

    protected virtual Task<bool> OnServerRawOutput(Services.Servers.Server server, string line) =>
        Task.FromResult(true);

    protected virtual Task OnServerInput(Services.Servers.Server server, string line) =>
        Task.CompletedTask;

    protected virtual Task<bool> OnGroupMessageReceived(MessagePacket packet) =>
        Task.FromResult(true);

    protected virtual Task<bool> OnPrivateMessageReceived(MessagePacket packet) =>
        Task.FromResult(true);

    protected virtual Task<bool> OnWsDataReceived(string data) => Task.FromResult(true);

    protected virtual Task<bool> OnPacketReceived(JsonObject packet) => Task.FromResult(true);

    protected virtual Task OnSereinClosed() => Task.CompletedTask;

    protected virtual Task OnSereinCrashed() => Task.CompletedTask;

    protected virtual Task OnPluginsLoaded() => Task.CompletedTask;

    protected virtual Task OnPluginsUnloading() => Task.CompletedTask;

    internal Task Invoke(Event @event, params object[] args)
    {
        switch (@event)
        {
            case Event.ServerStarted:
                return OnServerStarted((Services.Servers.Server)args.First());

            case Event.ServerStarting:
                return OnServerStarting((Services.Servers.Server)args.First());

            case Event.ServerStopping:
                return OnServerStopping((Services.Servers.Server)args.First());

            case Event.GroupMessageReceived:
                return OnGroupMessageReceived((MessagePacket)args.First());

            case Event.PrivateMessageReceived:
                return OnPrivateMessageReceived((MessagePacket)args.First());

            case Event.WsDataReceived:
                return OnWsDataReceived((string)args.First());

            case Event.PacketReceived:
                return OnPacketReceived((JsonObject)args.First());

            case Event.ServerOutput:
                if (args.Length != 2)
                {
                    ThrowArgumentException();
                }

                return OnServerOutput((Services.Servers.Server)args.First(), (string)args.Last());

            case Event.ServerRawOutput:
                if (args.Length != 2)
                {
                    ThrowArgumentException();
                }

                return OnServerRawOutput(
                    (Services.Servers.Server)args.First(),
                    (string)args.Last()
                );

            case Event.ServerInput:
                if (args.Length != 2)
                {
                    ThrowArgumentException();
                }

                return OnServerInput((Services.Servers.Server)args.First(), (string)args.Last());

            case Event.ServerExited:
                if (
                    args.Length != 3
                    || args[0] is not Services.Servers.Server server
                    || args[1] is not int code
                    || args[2] is not DateTime time
                )
                {
                    ThrowArgumentException();
                }

                return OnServerExited(server, code, time);

            case Event.SereinClosed:
                return OnSereinClosed();

            case Event.SereinCrashed:
                return OnSereinCrashed();

            case Event.PluginsLoaded:
                return OnPluginsLoaded();

            case Event.PluginsUnloading:
                return OnPluginsUnloading();

            default:
                throw new NotSupportedException();
        }

        [DoesNotReturn]
        static void ThrowArgumentException()
        {
            throw new ArgumentException("缺少参数或类型不正确", nameof(args));
        }
    }
}
