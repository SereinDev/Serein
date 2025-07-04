using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Serein.ConnectionProtocols.Models.Satori.V1.Signals;
using Serein.ConnectionProtocols.Models.Satori.V1.Signals.Bodies;
using Serein.Core.Utils.Json;
using WebSocket4Net;

namespace Serein.Core.Services.Network.Connection.Adapters.Satori;

public partial class SatoriAdapter
{
    private WebSocket? _webSocket;

    private string _uri = string.Empty;

    private WebSocket CreateNew()
    {
        var uri = _settingProvider.Value.Connection.Satori.Uri.StartsWith("http")
            ? "ws" + _settingProvider.Value.Connection.Satori.Uri[4..]
            : _settingProvider.Value.Connection.Satori.Uri;

        if (!uri.EndsWith('/'))
        {
            uri += "/";
        }

        uri += "v1/events";

        _uri = uri;

        var webSocket = new WebSocket(
            uri,
            customHeaderItems: !string.IsNullOrEmpty(
                _settingProvider.Value.Connection.OneBot.AccessToken
            )
                ? new()
                {
                    new(
                        "Authorization",
                        $"Bearer {_settingProvider.Value.Connection.Satori.AccessToken}"
                    ),
                }
                : null
        );

        webSocket.Opened += (_, _) => OnOpened(webSocket);
        webSocket.Closed += (_, _) =>
        {
            _timer.Stop();
            _logger.Log(LogLevel.Warning, $"WebSocket 连接已断开");
            StatusChanged?.Invoke(this, EventArgs.Empty);

            TryReconnect();
        };
        webSocket.MessageReceived += OnDataReceived;
        webSocket.Error += (_, e) => _logger.Log(LogLevel.Error, e.Exception.Message);

        return webSocket;
    }

    private async Task TryReconnect()
    {
        if (!IsActive || _webSocket?.State != WebSocketState.Closed)
        {
            return;
        }

        _logger.Log(
            LogLevel.Information,
            $"将在五秒后（{DateTime.Now.AddSeconds(5):T}）尝试重新连接"
        );

        await Task.Delay(5000, _cancellationTokenSource.Token);

        _webSocket = CreateNew();
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

        DataReceived?.Invoke(this, new(e.Message));
    }

    private void OnOpened(WebSocket webSocket)
    {
        _logger.Log(LogLevel.Information, $"成功连接到 {_uri}");

        var data = JsonSerializer.Serialize(
            new Signal<IdentifyBody>
            {
                Op = Opcode.Identify,
                Body = new()
                {
                    Token = _settingProvider.Value.Connection.Satori.AccessToken,
                    Sn = _sn,
                },
            },
            JsonSerializerOptionsFactory.PacketStyle
        );

        webSocket.Send(data);

        if (_settingProvider.Value.Connection.OutputData)
        {
            _logger.LogSentData(data);
        }

        _timer.Start();
    }

    private void SendPing()
    {
        if (_webSocket?.State != WebSocketState.Open)
        {
            return;
        }

        var data = JsonSerializer.Serialize(
            new Signal<object?> { Op = Opcode.Ping },
            JsonSerializerOptionsFactory.PacketStyle
        );

        _webSocket.Send(data);

        if (_settingProvider.Value.Connection.OutputData)
        {
            _logger.LogSentData(data);
        }
    }
}
