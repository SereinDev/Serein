using System;
using System.Text.Json;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core.Models;
using Serein.Core.Models.Events;
using Serein.Core.Models.OneBot.Packets;
using Serein.Core.Utils.Json;

namespace Serein.Core.Services.Networks;

public class WsNetwork
{
    private readonly IHost _host;
    private readonly Matcher _matcher;

    private IServiceProvider Service => _host.Services;
    private WebSocketService WebSocketService => Service.GetRequiredService<WebSocketService>();
    private ReverseWebSocketService ReverseWebSocketService =>
        Service.GetRequiredService<ReverseWebSocketService>();
    private IOutputHandler Output => Service.GetRequiredService<IOutputHandler>();

    public WsNetwork(IHost host, Matcher matcher)
    {
        _host = host;
        _matcher = matcher;
        WebSocketService.WsMessageReceived += OnWsMessageReceived;
        ReverseWebSocketService.WsMessageReceived += OnWsMessageReceived;
    }

    private void OnWsMessageReceived(object? sender, WsMessageReceivedEventArgs e)
    {
        if (e.JsonData is null || e.JsonData.GetValueKind() != JsonValueKind.Object)
            return;

        switch (e.JsonData["post_type"]?.ToString())
        {
            case "message":
            case "message_sent":
                var packet = e.JsonData.ToObject<MessagePacket>(
                    JsonSerializerOptionsFactory.SnakeCase
                );

                if (packet is not null)
                {
                    _matcher.MatchMsg(packet);
                    Output.LogBotMessage(packet);
                }
                break;
        }

        Output.LogBotJsonPacket(e.JsonData);
    }
}
