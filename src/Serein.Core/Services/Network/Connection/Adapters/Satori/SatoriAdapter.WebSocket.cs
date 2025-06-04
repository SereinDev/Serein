using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging;
using Serein.ConnectionProtocols.Models.Satori.V1.Signals;
using Serein.ConnectionProtocols.Models.Satori.V1.Signals.Bodies;
using Serein.Core.Utils.Json;
using WebSocket4Net;

namespace Serein.Core.Services.Network.Connection.Adapters.Satori;

public partial class SatoriAdapter
{
    private WebSocket CreateNew()
    {
        var webSocket = new WebSocket(
            _settingProvider.Value.Connection.Satori.EventUrl,
            customHeaderItems: !string.IsNullOrEmpty(
                _settingProvider.Value.Connection.OneBot.AccessToken
            )
                ? new()
                {
                    new KeyValuePair<string, string>(
                        "Authorization",
                        $"Bearer {_settingProvider.Value.Connection.Satori.ApiAccessToken}"
                    ),
                }
                : null
        );

        webSocket.Opened += (_, _) => OnOpened(webSocket);
        webSocket.Closed += StatusChanged;
        webSocket.Closed += (_, _) => _timer.Stop();
        webSocket.MessageReceived += DataReceived;
        webSocket.MessageReceived += OnDataReceived;
        webSocket.Error += (_, e) => _logger.Log(LogLevel.Error, e.Exception.Message);

        return webSocket;
    }

    private void OnDataReceived(object? sender, MessageReceivedEventArgs e)
    {
        if (e.Message is null)
        {
            return;
        }

        var signal = JsonSerializer.Deserialize<Signal<JsonNode?>>(
            e.Message,
            JsonSerializerOptionsFactory.PacketStyle
        );

        if (signal?.Op == Opcode.Event && signal.Body is not null)
        {
            var eventBody = JsonSerializer.Deserialize<EventBody>(
                signal.Body,
                JsonSerializerOptionsFactory.PacketStyle
            );

            if (eventBody is not null)
            {
                _sn = eventBody.Sn;
            }
        }
    }

    private void OnOpened(WebSocket webSocket)
    {
        webSocket.Send(
            JsonSerializer.Serialize(
                new Signal<IdentifyBody>
                {
                    Op = Opcode.Identify,
                    Body = new()
                    {
                        Token = _settingProvider.Value.Connection.Satori.EventAccessToken,
                        Sn = _sn,
                    },
                },
                JsonSerializerOptionsFactory.PacketStyle
            )
        );

        _timer.Start();
    }

    private void SendPing()
    {
        if (_webSocket?.State != WebSocketState.Open)
        {
            return;
        }

        _webSocket.Send(
            JsonSerializer.Serialize(
                new Signal<object?> { Op = Opcode.Ping },
                JsonSerializerOptionsFactory.PacketStyle
            )
        );
    }
}
