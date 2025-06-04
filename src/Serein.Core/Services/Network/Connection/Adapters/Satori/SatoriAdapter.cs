using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Timers;
using Serein.ConnectionProtocols.Models.Satori.V1;
using Serein.ConnectionProtocols.Models.Satori.V1.Signals;
using Serein.ConnectionProtocols.Models.Satori.V1.Signals.Bodies;
using Serein.Core.Models.Abstractions;
using Serein.Core.Models.Network.Connection;
using Serein.Core.Services.Data;
using Serein.Core.Utils.Json;
using WebSocket4Net;

namespace Serein.Core.Services.Network.Connection.Adapters.Satori;

public partial class SatoriAdapter : IConnectionAdapter
{
    private readonly IConnectionLogger _logger;
    private readonly SettingProvider _settingProvider;
    private readonly Timer _timer;

    public event EventHandler<MessageReceivedEventArgs>? DataReceived;
    public event EventHandler? StatusChanged;

    [MemberNotNullWhen(true, nameof(HttpClient))]
    public bool IsActive { get; private set; }
    public HttpClient? HttpClient { get; private set; }

    public AdapterType Type { get; } = AdapterType.Satori;

    private WebSocket? _webSocket;
    private long? _sn;

    public SatoriAdapter(IConnectionLogger logger, SettingProvider settingProvider)
    {
        HttpClient = new();
        _logger = logger;
        _settingProvider = settingProvider;
        _timer = new(10_000) { AutoReset = true };
        _timer.Elapsed += (_, _) => SendPing();
    }

    public void Dispose()
    {
        HttpClient?.Dispose();

        GC.SuppressFinalize(this);
    }

    public Task SendAsync(string payload)
    {
        throw new NotSupportedException("Use HttpClient.SendAsync instead.");
    }

    public async Task SendMessageAsync(TargetType type, string target, string text)
    {
        if (!IsActive)
        {
            return;
        }

        var content = JsonSerializer.Serialize(
            new SendMsgPacket { ChannelId = target, Content = text },
            JsonSerializerOptionsFactory.PacketStyle
        );

        var response = await HttpClient.SendAsync(
            new(HttpMethod.Post, "message.create")
            {
                Content = new StringContent(content, new MediaTypeHeaderValue("application/json")),
                Headers =
                {
                    Authorization = !string.IsNullOrEmpty(
                        _settingProvider.Value.Connection.Satori.ApiAccessToken
                    )
                        ? new("Bearer", _settingProvider.Value.Connection.Satori.ApiAccessToken)
                        : null,
                },
            }
        );

        response.EnsureSuccessStatusCode();
    }

    public void Start()
    {
        if (IsActive)
        {
            throw new InvalidOperationException();
        }

        _sn = null;
        HttpClient = new() { BaseAddress = new(_settingProvider.Value.Connection.Satori.ApiUrl) };
        _webSocket = CreateNew();
    }

    public void Stop()
    {
        if (!IsActive)
        {
            throw new InvalidOperationException();
        }

        _webSocket?.Close();
        HttpClient.CancelPendingRequests();
    }
}
