using System;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

using Serein.Core.Models.Network.Connection.OneBot.Packets;

namespace Serein.Core.Models.Plugins.Net;

public abstract partial class PluginBase
{
    public virtual Task<bool> OnServerStarting(string id) => Task.FromResult(true);

    public virtual Task OnServerStarted(string id) => Task.CompletedTask;

    public virtual Task<bool> OnServerStopping(string id) => Task.FromResult(true);

    public virtual Task OnServerExited(string id, int exitcode, DateTime exitTime) => Task.CompletedTask;

    public virtual Task<bool> OnServerOutput(string id, string line) => Task.FromResult(true);

    public virtual Task<bool> OnServerRawOutput(string id, string line) => Task.FromResult(true);

    public virtual Task OnServerInput(string id, string line) => Task.CompletedTask;

    public virtual Task OnGroupIncreased() => Task.CompletedTask;

    public virtual Task OnGroupDecreased() => Task.CompletedTask;

    public virtual Task OnGroupPoked() => Task.CompletedTask;

    public virtual Task<bool> OnGroupMessageReceived(MessagePacket packet) => Task.FromResult(true);

    public virtual Task<bool> OnPrivateMessageReceived(MessagePacket packet) =>
        Task.FromResult(true);

    public virtual Task<bool> OnWsDataReceived(string data) => Task.FromResult(true);

    public virtual Task<bool> OnPacketReceived(JsonNode packet) => Task.FromResult(true);

    public virtual Task OnSereinClosed() => Task.CompletedTask;

    public virtual Task OnSereinCrashed() => Task.CompletedTask;

    public virtual Task OnPluginsLoaded() => Task.CompletedTask;

    public virtual Task OnPluginsUnloading() => Task.CompletedTask;
}
