using System;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

using Serein.Core.Models.OneBot.Packets;

namespace Serein.Core.Models.Plugins.CSharp;

public abstract partial class PluginBase
{
    public virtual Task<bool> OnServerStarting() => Task.FromResult(true);

    public virtual Task OnServerStarted() => Task.CompletedTask;

    public virtual Task<bool> OnServerStopping() => Task.FromResult(true);

    public virtual Task OnServerExited(int exitcode, DateTime exitTime) => Task.CompletedTask;

    public virtual Task<bool> OnServerOutput(string line) => Task.FromResult(true);

    public virtual Task<bool> OnServerRawOutput(string line) => Task.FromResult(true);

    public virtual Task OnServerInput(string line) => Task.CompletedTask;

    public virtual Task OnGroupIncreased() => Task.CompletedTask;

    public virtual Task OnGroupDecreased() => Task.CompletedTask;

    public virtual Task OnGroupPoked() => Task.CompletedTask;

    public virtual Task<bool> OnGroupMessageReceived(MessagePacket packet) => Task.FromResult(true);

    public virtual Task<bool> OnPrivateMessageReceived(MessagePacket packet) =>
        Task.FromResult(true);

    public virtual Task<bool> OnWsDataReceived(byte[] data) => Task.FromResult(true);

    public virtual Task<bool> OnPacketReceived(JsonNode packet) => Task.FromResult(true);

    public virtual Task OnSereinClosed() => Task.CompletedTask;

    public virtual Task OnSereinCrashed() => Task.CompletedTask;

    public virtual Task OnPluginsLoaded() => Task.CompletedTask;

    public virtual Task OnPluginsUnloading() => Task.CompletedTask;
}
