using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core.Models.Network;
using Serein.Core.Models.Network.Connection.OneBot;
using Serein.Core.Models.Network.Connection.OneBot.ActionParams;
using Serein.Core.Models.Network.Connection.OneBot.Messages;
using Serein.Core.Models.Network.Connection.OneBot.Packets;
using Serein.Core.Models.Plugins;
using Serein.Core.Models.Settings;
using Serein.Core.Services.Commands;
using Serein.Core.Services.Data;
using Serein.Core.Services.Plugins;
using Serein.Core.Utils.Json;

using WebSocket4Net;

namespace Serein.Core.Services.Network.Connection;

public class WsConnectionManager
{
    private readonly IHost _host;
    private readonly Matcher _matcher;
    private readonly EventDispatcher _eventDispatcher;

    private IServiceProvider Services => _host.Services;
    private Setting Setting => Services.GetRequiredService<SettingProvider>().Value;
    private WebSocketService WebSocketService => Services.GetRequiredService<WebSocketService>();
    private ReverseWebSocketService ReverseWebSocketService =>
        Services.GetRequiredService<ReverseWebSocketService>();
    private CancellationTokenSource? _cancellationTokenSource;

    public event EventHandler? StatusChanged;
    public event EventHandler<JsonPacketReceivedEventArgs>? JsonPacketReceived;
    public event EventHandler<MessagePacketReceivedEventArgs>? MessagePacketReceived;

    public bool Active => WebSocketService.Active || ReverseWebSocketService.Active;

    public WsConnectionManager(IHost host, Matcher matcher, EventDispatcher eventDispatcher)
    {
        _host = host;
        _matcher = matcher;
        _eventDispatcher = eventDispatcher;

        WebSocketService.MessageReceived += OnMessageReceived;
        ReverseWebSocketService.MessageReceived += OnMessageReceived;

        WebSocketService.StatusChanged += StatusChanged;
        ReverseWebSocketService.StatusChanged += StatusChanged;
    }

    private void OnMessageReceived(object? sender, MessageReceivedEventArgs e)
    {
        if (!_eventDispatcher.Dispatch(Event.WsDataReceived, e.Message))
            return;

        var text = e.Message;

        var node = JsonSerializer.Deserialize<JsonNode>(text);

        if (node is null)
            return;

        if (Setting.Connection.OutputData)
            JsonPacketReceived?.Invoke(this, new(node));

        if (!_eventDispatcher.Dispatch(Event.PacketReceived, node))
            return;

        switch (node["post_type"]?.ToString())
        {
            case "message":
            case "message_sent":
                var packet = node.ToObject<MessagePacket>(JsonSerializerOptionsFactory.SnakeCase);

                if (packet is not null)
                {
                    MessagePacketReceived?.Invoke(this, new(packet));

                    if (
                        packet.MessageType == MessageType.Group
                            && !_eventDispatcher.Dispatch(Event.GroupMessageReceived, packet)
                        || packet.MessageType == MessageType.Private
                            && !_eventDispatcher.Dispatch(Event.PrivateMessageReceived, packet)
                    )
                        return;

                    _matcher.MatchMsgAsync(packet);
                }
                break;
        }
    }

    public void Start()
    {
        if (ReverseWebSocketService.Active)
            throw new InvalidOperationException("反向WebSocket服务器未关闭");

        if (WebSocketService.Active)
            throw new InvalidOperationException("WebSocket连接未断开");

        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = new();

        if (Setting.Connection.UseReverseWebSocket)
            ReverseWebSocketService.Start(_cancellationTokenSource.Token);
        else
            WebSocketService.Start(_cancellationTokenSource.Token);
    }

    public void Stop()
    {
        if (!Active && !WebSocketService.Connecting)
            throw new InvalidOperationException("WebSocket未连接");

        _cancellationTokenSource?.Cancel();

        if (ReverseWebSocketService.Active)
            ReverseWebSocketService.Stop();

        if (WebSocketService.Active)
            WebSocketService.Stop();
    }

    public async Task SendAsync<T>(T body)
        where T : notnull
    {
        var text = JsonSerializer.Serialize(body, JsonSerializerOptionsFactory.SnakeCase);

        await SendTextAsync(text);
    }

    public async Task SendTextAsync(string text)
    {
        if (ReverseWebSocketService.Active)
            await ReverseWebSocketService.SendAsync(text);
        else if (WebSocketService.Active)
            await WebSocketService.SendAsync(text);
    }

    private async Task SendActionRequestAsync<T>(string endpoint, T @params)
        where T : notnull, IActionParams
    {
        await SendAsync(new ActionRequest<T> { Action = endpoint, Params = @params });
    }

    public async Task SendGroupMsgAsync(string target, string message)
    {
        await SendActionRequestAsync(
            "send_msg",
            new MessageParams
            {
                GroupId = target,
                Message = message,
                AutoEscape = Setting.Connection.AutoEscape
            }
        );
    }

    public async Task SendPrivateMsgAsync(string target, string message)
    {
        await SendActionRequestAsync(
            "send_msg",
            new MessageParams
            {
                UserId = target,
                Message = message,
                AutoEscape = Setting.Connection.AutoEscape
            }
        );
    }
}
