using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Serein.Core.Models.Network.Connection;
using Serein.Core.Models.Plugins;
using Serein.Core.Services.Servers;

namespace Serein.Core.Services.Plugins.Net;

public abstract partial class PluginBase
{
    protected virtual Task<bool> OnServerStarting(Server server) => Task.FromResult(true);

    protected virtual Task OnServerStarted(Server server) => Task.CompletedTask;

    protected virtual Task<bool> OnServerStopping(Server server) => Task.FromResult(true);

    protected virtual Task OnServerExited(Server server, int exitcode, DateTime exitTime) =>
        Task.CompletedTask;

    protected virtual Task<bool> OnServerOutput(Server server, string line) =>
        Task.FromResult(true);

    protected virtual Task<bool> OnServerRawOutput(Server server, string line) =>
        Task.FromResult(true);

    protected virtual Task OnServerInput(Server server, string line) => Task.CompletedTask;

    protected virtual Task<bool> OnGroupMessageReceived(Packets packet) => Task.FromResult(true);

    protected virtual Task<bool> OnPrivateMessageReceived(Packets packet) => Task.FromResult(true);

    protected virtual Task<bool> OnChannelMessageReceived(Packets packet) => Task.FromResult(true);

    protected virtual Task<bool> OnConnectionDataReceived(string data) => Task.FromResult(true);

    protected virtual Task<bool> OnPacketReceived(JsonNode packet) => Task.FromResult(true);

    protected virtual Task OnSereinClosed() => Task.CompletedTask;

    protected virtual Task OnSereinCrashed() => Task.CompletedTask;

    protected virtual Task OnPluginsLoaded() => Task.CompletedTask;

    protected virtual Task OnPluginsUnloading() => Task.CompletedTask;

    internal Task Invoke(Event @event, params object[] args)
    {
        switch (@event)
        {
            case Event.ServerStarted:
                return OnServerStarted((Server)args[0]);

            case Event.ServerStarting:
                return OnServerStarting((Server)args[0]);

            case Event.ServerStopping:
                return OnServerStopping((Server)args[0]);

            case Event.GroupMessageReceived:
                return OnGroupMessageReceived((Packets)args[0]);

            case Event.PrivateMessageReceived:
                return OnPrivateMessageReceived((Packets)args[0]);

            case Event.ChannelMessageReceived:
                return OnChannelMessageReceived((Packets)args[0]);

            case Event.ConnectionDataReceived:
                return OnConnectionDataReceived((string)args[0]);

            case Event.PacketReceived:
                return OnPacketReceived((JsonNode)args[0]);

            case Event.ServerOutput:
                if (args.Length != 2)
                {
                    ThrowArgumentException();
                }

                return OnServerOutput((Server)args[0], (string)args[1]);

            case Event.ServerRawOutput:
                if (args.Length != 2)
                {
                    ThrowArgumentException();
                }

                return OnServerRawOutput((Server)args[0], (string)args[1]);

            case Event.ServerInput:
                if (args.Length != 2)
                {
                    ThrowArgumentException();
                }

                return OnServerInput((Server)args[0], (string)args[1]);

            case Event.ServerExited:
                if (
                    args.Length != 3
                    || args[0] is not Server server
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
