using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core.Models.Network.OneBot;
using Serein.Core.Models.Network.OneBot.ActionParams;
using Serein.Core.Models.Network.OneBot.Messages;
using Serein.Core.Models.Network.OneBot.Packets;
using Serein.Core.Models.Output;
using Serein.Core.Models.Plugins;
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
    private IOutputHandler Logger => Services.GetRequiredService<IOutputHandler>();

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

        var node = JsonSerializer.Deserialize<JsonNode>(text);

        if (node is null)
            return;

        if (Setting.Network.OutputData)
            Logger.LogBotJsonPacket(node);

        if (!_eventDispatcher.Dispatch(Event.PacketReceived, node))
            return;

        switch (node["post_type"]?.ToString())
        {
            case "message":
            case "message_sent":
                var packet = node.ToObject<MessagePacket>(JsonSerializerOptionsFactory.SnakeCase);

                if (packet is not null)
                {
                    Logger.LogBotReceivedMessage(packet);

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

        if (Setting.Network.UseReverseWebSocket)
            ReverseWebSocketService.Start();
        else
            WebSocketService.Start();
    }

    public void Stop()
    {
        if (!Active)
            throw new InvalidOperationException("WebSocket未连接");

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
                AutoEscape = Setting.Network.AutoEscape
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
                AutoEscape = Setting.Network.AutoEscape
            }
        );
    }
}
