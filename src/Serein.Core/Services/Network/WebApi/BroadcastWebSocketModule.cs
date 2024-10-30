using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

using EmbedIO.WebSockets;

using Serein.Core.Models.Network.WebApi;
using Serein.Core.Models.Server;
using Serein.Core.Services.Data;
using Serein.Core.Services.Servers;
using Serein.Core.Utils;
using Serein.Core.Utils.Json;

namespace Serein.Core.Services.Network.WebApi;

internal class BroadcastWebSocketModule : WebSocketModule
{
    private readonly ServerManager _serverManager;
    private readonly SettingProvider _settingProvider;
    private readonly Dictionary<string, List<IWebSocketContext>> _clients = [];

    public BroadcastWebSocketModule(ServerManager serverManager, SettingProvider settingProvider)
        : base("/ws/broadcast", true)
    {
        _serverManager = serverManager;
        _settingProvider = settingProvider;
        _serverManager.ServersUpdated += OnServersUpdate;

        foreach (var server in _serverManager.Servers.Values)
        {
            server.ServerOutput += NotifyOutput;
            server.ServerStatusChanged += NotifyStatusChanged;
        }
    }

    protected override Task OnMessageReceivedAsync(
        IWebSocketContext context,
        byte[] buffer,
        IWebSocketReceiveResult result
    )
    {
        return Task.CompletedTask;
    }

    protected override async Task OnClientConnectedAsync(IWebSocketContext context)
    {
        var query = HttpUtility.ParseQueryString(context.RequestUri.Query);
        var auth = query.Get("token");

        if (
            _settingProvider.Value.WebApi.AccessTokens.Length != 0
            && (
                string.IsNullOrEmpty(auth)
                || !_settingProvider.Value.WebApi.AccessTokens.Contains(auth)
            )
        )
        {
            await context.WebSocket.CloseAsync();
            return;
        }

        var id = query.Get("id");
        if (string.IsNullOrEmpty(id) || !_serverManager.Servers.ContainsKey(id))
        {
            await context.WebSocket.CloseAsync();
            return;
        }

        if (!_clients.TryGetValue(id, out var list))
            list = _clients[id] = [];

        list.Add(context);
    }

    protected override Task OnClientDisconnectedAsync(IWebSocketContext context)
    {
        var query = HttpUtility.ParseQueryString(context.RequestUri.Query);
        var id = query.Get("id");

        if (!string.IsNullOrEmpty(id) && _clients.TryGetValue(id, out var list))
        {
            list.Remove(context);
            if (list.Count == 0)
                _clients.Remove(id);
        }

        return Task.CompletedTask;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        _serverManager.ServersUpdated -= OnServersUpdate;
        _clients.Clear();
    }

    private void OnServersUpdate(object? sender, ServersUpdatedEventArgs e)
    {
        if (e.Type == ServersUpdatedType.Added)
        {
            e.Server.ServerOutput += NotifyOutput;
            e.Server.ServerStatusChanged += NotifyStatusChanged;
        }
        else if (e.Type == ServersUpdatedType.Removed)
        {
            e.Server.ServerOutput -= NotifyOutput;
            e.Server.ServerStatusChanged += NotifyStatusChanged;

            if (_clients.TryGetValue(e.Id, out var list) && list.Count != 0)
            {
                var payload = EncodingMap.UTF8.GetBytes(
                    JsonSerializer.Serialize(
                        new WebSocketBroadcastPacket(WebSocketBroadcastType.Removed),
                        JsonSerializerOptionsFactory.CamelCase
                    )
                );

                foreach (var context in list)
                    context.WebSocket.SendAsync(payload, true);
            }
        }
    }

    private void NotifyOutput(object? sender, ServerOutputEventArgs e)
    {
        if (sender is not Server server)
            return;

        if (_clients.TryGetValue(server.Id, out var list) && list.Count != 0)
        {
            var payload = EncodingMap.UTF8.GetBytes(
                JsonSerializer.Serialize(
                    new WebSocketBroadcastPacket(
                        e.OutputType switch
                        {
                            ServerOutputType.Information => WebSocketBroadcastType.Information,
                            ServerOutputType.Raw => WebSocketBroadcastType.Output,
                            ServerOutputType.Error => WebSocketBroadcastType.Error,
                            ServerOutputType.InputCommand => WebSocketBroadcastType.Input,
                            _ => throw new NotSupportedException(),
                        },
                        e.Data
                    ),
                    JsonSerializerOptionsFactory.CamelCase
                )
            );

            foreach (var context in list)
                context.WebSocket.SendAsync(payload, true);
        }
    }

    private void NotifyStatusChanged(object? sender, EventArgs e)
    {
        if (sender is not Server server)
            return;

        if (_clients.TryGetValue(server.Id, out var list) && list.Count != 0)
        {
            var payload = EncodingMap.UTF8.GetBytes(
                JsonSerializer.Serialize(
                    new WebSocketBroadcastPacket(
                        server.Status
                            ? WebSocketBroadcastType.Started
                            : WebSocketBroadcastType.Stopped
                    ),
                    JsonSerializerOptionsFactory.CamelCase
                )
            );

            foreach (var context in list)
                context.WebSocket.SendAsync(payload, true);
        }
    }
}
