using System;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using EmbedIO;
using Serein.Core.Models.Network.Connection;
using Serein.Core.Models.Plugins;
using Serein.Core.Services.Servers;

namespace Serein.Core.Services.Plugins.Net;

public abstract partial class PluginBase
{
    protected virtual Task<bool> OnServerStarting(Server server)
    {
        return Task.FromResult(true);
    }

    protected virtual Task OnServerStarted(Server server)
    {
        return Task.CompletedTask;
    }

    protected virtual Task<bool> OnServerStopping(Server server)
    {
        return Task.FromResult(true);
    }

    protected virtual Task OnServerExited(Server server, int exitcode, DateTime exitTime)
    {
        return Task.CompletedTask;
    }

    protected virtual Task<bool> OnServerOutput(Server server, string line)
    {
        return Task.FromResult(true);
    }

    protected virtual Task<bool> OnServerRawOutput(Server server, string line)
    {
        return Task.FromResult(true);
    }

    protected virtual Task OnServerInput(Server server, string line)
    {
        return Task.CompletedTask;
    }

    protected virtual Task<bool> OnGroupMessageReceived(Packets packets)
    {
        return Task.FromResult(true);
    }

    protected virtual Task<bool> OnPrivateMessageReceived(Packets packets)
    {
        return Task.FromResult(true);
    }

    protected virtual Task<bool> OnChannelMessageReceived(Packets packets)
    {
        return Task.FromResult(true);
    }

    protected virtual Task<bool> OnConnectionDataReceived(string data)
    {
        return Task.FromResult(true);
    }

    protected virtual Task<bool> OnPacketReceived(JsonNode packet)
    {
        return Task.FromResult(true);
    }

    protected virtual Task<bool> OnHttpRequestReceived(IHttpContext httpContext)
    {
        return Task.FromResult(true);
    }

    protected virtual Task OnSereinClosed()
    {
        return Task.CompletedTask;
    }

    protected virtual Task OnSereinCrashed()
    {
        return Task.CompletedTask;
    }

    protected virtual Task OnPluginsLoaded()
    {
        return Task.CompletedTask;
    }

    protected virtual Task OnPluginsUnloading()
    {
        return Task.CompletedTask;
    }

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
                return OnServerOutput((Server)args[0], (string)args[1]);

            case Event.ServerRawOutput:
                return OnServerRawOutput((Server)args[0], (string)args[1]);

            case Event.ServerInput:
                return OnServerInput((Server)args[0], (string)args[1]);

            case Event.ServerExited:
                return OnServerExited((Server)args[0], (int)args[1], (DateTime)args[2]);

            case Event.SereinClosed:
                return OnSereinClosed();

            case Event.SereinCrashed:
                return OnSereinCrashed();

            case Event.PluginsLoaded:
                return OnPluginsLoaded();

            case Event.PluginsUnloading:
                return OnPluginsUnloading();

            case Event.HttpRequestReceived:
                return OnHttpRequestReceived((IHttpContext)args[0]);

            default:
                throw new NotSupportedException();
        }
    }
}
