using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

using Serein.Core.Models.Network.Connection.OneBot.Packets;
using Serein.Core.Utils.Extensions;

namespace Serein.Core.Models.Plugins.Net;

public abstract partial class PluginBase
{
    protected virtual Task<bool> OnServerStarting(string id) => Task.FromResult(true);

    protected virtual Task OnServerStarted(string id) => Task.CompletedTask;

    protected virtual Task<bool> OnServerStopping(string id) => Task.FromResult(true);

    protected virtual Task OnServerExited(string id, int exitcode, DateTime exitTime) =>
        Task.CompletedTask;

    protected virtual Task<bool> OnServerOutput(string id, string line) => Task.FromResult(true);

    protected virtual Task<bool> OnServerRawOutput(string id, string line) => Task.FromResult(true);

    protected virtual Task OnServerInput(string id, string line) => Task.CompletedTask;

    protected virtual Task OnGroupIncreased() => Task.CompletedTask;

    protected virtual Task OnGroupDecreased() => Task.CompletedTask;

    protected virtual Task OnGroupPoked() => Task.CompletedTask;

    protected virtual Task<bool> OnGroupMessageReceived(MessagePacket packet) =>
        Task.FromResult(true);

    protected virtual Task<bool> OnPrivateMessageReceived(MessagePacket packet) =>
        Task.FromResult(true);

    protected virtual Task<bool> OnWsDataReceived(string data) => Task.FromResult(true);

    protected virtual Task<bool> OnPacketReceived(JsonNode packet) => Task.FromResult(true);

    protected virtual Task OnSereinClosed() => Task.CompletedTask;

    protected virtual Task OnSereinCrashed() => Task.CompletedTask;

    protected virtual Task OnPluginsLoaded() => Task.CompletedTask;

    protected virtual Task OnPluginsUnloading() => Task.CompletedTask;

    internal Task Invoke(Event @event, params object[] args)
    {
        switch (@event)
        {
            case Event.ServerStarting:
                return OnServerStarting(args.First().As<string>());

            case Event.ServerStopping:
                return OnServerStopping(args.First().As<string>());

            case Event.GroupMessageReceived:
                return OnGroupMessageReceived(args.First().As<MessagePacket>());

            case Event.PrivateMessageReceived:
                return OnPrivateMessageReceived(args.First().As<MessagePacket>());

            case Event.WsDataReceived:
                return OnWsDataReceived(args.First().As<string>());

            case Event.PacketReceived:
                return OnPacketReceived(args.First().As<JsonNode>());

            case Event.ServerOutput:
                if (args.Length != 2)
                    ThrowArgumentException();

                return OnServerOutput(args.First().As<string>(), args.Last().As<string>());

            case Event.ServerRawOutput:
                if (args.Length != 2)
                    ThrowArgumentException();

                return OnServerRawOutput(args.First().As<string>(), args.Last().As<string>());

            case Event.ServerInput:
                if (args.Length != 2)
                    ThrowArgumentException();

                return OnServerInput(args.First().As<string>(), args.Last().As<string>());

            case Event.ServerExited:
                if (
                    args.Length != 3
                    || args[0] is not string id
                    || args[1] is not int code
                    || args[2] is not DateTime time
                )
                    ThrowArgumentException();

                return OnServerExited(id, code, time);

            case Event.SereinClosed:
                return OnSereinClosed();

            case Event.SereinCrashed:
                return OnSereinCrashed();

            case Event.PluginsLoaded:
                return OnPluginsLoaded();

            case Event.PluginsUnloading:
                return OnPluginsUnloading();

            case Event.ServerStarted:
                return OnServerStarted(args.First().As<string>());

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
