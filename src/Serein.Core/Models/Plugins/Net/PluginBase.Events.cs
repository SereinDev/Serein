using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Serein.Core.Models.Network.Connection.OneBot.Packets;
using Serein.Core.Services.Servers;
using Serein.Core.Utils.Extensions;

namespace Serein.Core.Models.Plugins.Net;

public abstract partial class PluginBase
{
    protected virtual Task<bool> OnServerStarting(ServerBase server) => Task.FromResult(true);

    protected virtual Task OnServerStarted(ServerBase server) => Task.CompletedTask;

    protected virtual Task<bool> OnServerStopping(ServerBase server) => Task.FromResult(true);

    protected virtual Task OnServerExited(ServerBase server, int exitcode, DateTime exitTime) =>
        Task.CompletedTask;

    protected virtual Task<bool> OnServerOutput(ServerBase server, string line) =>
        Task.FromResult(true);

    protected virtual Task<bool> OnServerRawOutput(ServerBase server, string line) =>
        Task.FromResult(true);

    protected virtual Task OnServerInput(ServerBase server, string line) => Task.CompletedTask;

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
                return OnServerStarted(args.First().OfType<ServerBase>());

            case Event.ServerStarting:
                return OnServerStarting(args.First().OfType<ServerBase>());

            case Event.ServerStopping:
                return OnServerStopping(args.First().OfType<ServerBase>());

            case Event.GroupMessageReceived:
                return OnGroupMessageReceived(args.First().OfType<MessagePacket>());

            case Event.PrivateMessageReceived:
                return OnPrivateMessageReceived(args.First().OfType<MessagePacket>());

            case Event.WsDataReceived:
                return OnWsDataReceived(args.First().OfType<string>());

            case Event.PacketReceived:
                return OnPacketReceived(args.First().OfType<JsonObject>());

            case Event.ServerOutput:
                if (args.Length != 2)
                {
                    ThrowArgumentException();
                }

                return OnServerOutput(
                    args.First().OfType<ServerBase>(),
                    args.Last().OfType<string>()
                );

            case Event.ServerRawOutput:
                if (args.Length != 2)
                {
                    ThrowArgumentException();
                }

                return OnServerRawOutput(
                    args.First().OfType<ServerBase>(),
                    args.Last().OfType<string>()
                );

            case Event.ServerInput:
                if (args.Length != 2)
                {
                    ThrowArgumentException();
                }

                return OnServerInput(
                    args.First().OfType<ServerBase>(),
                    args.Last().OfType<string>()
                );

            case Event.ServerExited:
                if (
                    args.Length != 3
                    || args[0] is not ServerBase server
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
