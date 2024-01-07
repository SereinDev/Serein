using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core.Models;
using Serein.Core.Models.Plugins;
using Serein.Core.Models.OneBot;
using Serein.Core.Models.OneBot.ActionParams;
using Serein.Core.Models.OneBot.Messages;
using Serein.Core.Models.OneBot.Packets;
using Serein.Core.Models.Settings;
using Serein.Core.Services.Data;
using Serein.Core.Services.Plugins;
using Serein.Core.Utils;
using Serein.Core.Utils.Json;

using WatsonWebsocket;

namespace Serein.Core.Services.Networks;

public class WsNetwork
{
    private readonly IHost _host;
    private readonly Matcher _matcher;
    private readonly EventDispatcher _eventDispatcher;

    private IServiceProvider Services => _host.Services;
    private Setting Setting => Services.GetRequiredService<SettingProvider>().Value;
    private WebSocketService WebSocketService => Services.GetRequiredService<WebSocketService>();
    private ReverseWebSocketService ReverseWebSocketService =>
        Services.GetRequiredService<ReverseWebSocketService>();
    private IOutputHandler Output => Services.GetRequiredService<IOutputHandler>();

    private bool _useReverseWebSocket;

    public bool Active => WebSocketService.Active || ReverseWebSocketService.Active;

    public WsNetwork(IHost host, Matcher matcher, EventDispatcher eventDispatcher)
    {
        _host = host;
        _matcher = matcher;
        _eventDispatcher = eventDispatcher;
        WebSocketService.MessageReceived += OnMessageReceived;
        ReverseWebSocketService.MessageReceived += OnMessageReceived;
    }

    private void OnMessageReceived(object? sender, MessageReceivedEventArgs e)
    {
        if (!_eventDispatcher.Dispatch(Event.WsDataReceived, e.Data))
            return;

        var text = EncodingMap.UTF8.GetString(e.Data);

        var node = JsonSerializer.Deserialize<JsonNode>(
            text,
            JsonSerializerOptionsFactory.SnakeCase
        );

        if (node is null || node.GetValueKind() != JsonValueKind.Object)
            return;

        if (!_eventDispatcher.Dispatch(Event.PacketReceived, node))
            return;

        switch (node["post_type"]?.ToString())
        {
            case "message":
            case "message_sent":
                var packet = node.ToObject<MessagePacket>(JsonSerializerOptionsFactory.SnakeCase);

                if (packet is not null)
                {
                    Output.LogBotMessage(packet);

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

        Output.LogBotJsonPacket(node);
    }

    public async Task StartAsync()
    {
        if (Active)
            throw new InvalidOperationException();

        _useReverseWebSocket = Setting.Connection.UseReverseWebSocket;

        if (_useReverseWebSocket)
            await ReverseWebSocketService.StartAsync();
        else
            await WebSocketService.StartAsync();
    }

    public async Task StopAsync()
    {
        _useReverseWebSocket = Setting.Connection.UseReverseWebSocket;

        if (ReverseWebSocketService.Active)
            await ReverseWebSocketService.StopAsync();

        if (WebSocketService.Active)
            await WebSocketService.StopAsync();
    }

    public async Task SendAsync<T>(T body)
        where T : notnull
    {
        var text = JsonSerializer.Serialize(body, JsonSerializerOptionsFactory.SnakeCase);

        await SendTextAsync(text);
    }

    public async Task SendTextAsync(string text)
    {
        if (_useReverseWebSocket)
            await ReverseWebSocketService.SendAsync(text);
        else
            await WebSocketService.SendAsync(text);
    }

    private async Task SendActionRequestAsync<T>(string endpoint, T @params)
        where T : notnull
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
