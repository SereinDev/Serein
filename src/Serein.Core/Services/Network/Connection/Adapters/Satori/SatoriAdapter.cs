using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Serein.ConnectionProtocols.Models;
using Serein.ConnectionProtocols.Models.Satori.V1;
using Serein.Core.Models.Abstractions;
using Serein.Core.Models.Commands;
using Serein.Core.Models.Network.Connection;
using Serein.Core.Services.Data;
using Serein.Core.Utils.Json;
using WebSocket4Net;

namespace Serein.Core.Services.Network.Connection.Adapters.Satori;

public sealed partial class SatoriAdapter : IConnectionAdapter
{
    private readonly ConnectionLoggerBase _logger;
    private readonly SettingProvider _settingProvider;
    private readonly CancellationTokenProvider _cancellationTokenProvider;
    private readonly System.Timers.Timer _timer;

    public event EventHandler<MessageReceivedEventArgs>? DataReceived;
    public event EventHandler? StatusChanged;

    [MemberNotNullWhen(true, nameof(_cancellationTokenSource))]
    public bool IsActive => !_cancellationTokenSource?.IsCancellationRequested ?? false;
    public HttpClient HttpClient { get; private set; }

    public AdapterType Type { get; } = AdapterType.Satori;

    private CancellationTokenSource? _cancellationTokenSource;

    private long? _sn;

    public SatoriAdapter(
        ConnectionLoggerBase logger,
        SettingProvider settingProvider,
        CancellationTokenProvider cancellationTokenProvider
    )
    {
        HttpClient = new();

        _logger = logger;
        _settingProvider = settingProvider;
        _cancellationTokenProvider = cancellationTokenProvider;

        _timer = new(10_000) { AutoReset = true };
        _timer.Elapsed += (_, _) => SendPing();
    }

    public void Dispose()
    {
        HttpClient?.Dispose();
        _cancellationTokenSource?.Dispose();

        GC.SuppressFinalize(this);
    }

    public Task SendAsync(string payload)
    {
        throw new NotSupportedException(
            "Use `SendRequestAsync<T>` or `HttpClient.SendAsync` instead."
        );
    }

    public async Task SendMessageAsync(
        TargetType type,
        string target,
        string content,
        CommandArguments? commandArguments = null
    )
    {
        if (!IsActive)
        {
            return;
        }

        try
        {
            var response = await SendApiRequestAsync(
                "v1/message.create",
                new SendMsgPacket { ChannelId = target, Content = content },
                commandArguments?.Self
            );
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, $"发送消息失败: {e.Message}");

            throw;
        }
    }

    public async Task<HttpResponseMessage> SendApiRequestAsync<T>(
        string path,
        T? content,
        Self? self
    )
    {
        var body = JsonSerializer.Serialize(content, JsonSerializerOptionsFactory.PacketStyle);

        var request = new HttpRequestMessage(HttpMethod.Post, path)
        {
            Content = new StringContent(body, new MediaTypeHeaderValue("application/json")),
            Headers =
            {
                Authorization = !string.IsNullOrEmpty(
                    _settingProvider.Value.Connection.Satori.AccessToken
                )
                    ? new("Bearer", _settingProvider.Value.Connection.Satori.AccessToken)
                    : null,
            },
        };

        if (
            self is null
            && !string.IsNullOrEmpty(_settingProvider.Value.Connection.Self.Platform)
            && !string.IsNullOrEmpty(_settingProvider.Value.Connection.Self.UserId)
        )
        {
            self = _settingProvider.Value.Connection.Self;
        }

        if (self is not null)
        {
            // Docs
            request.Headers.Add("Satori-Platform", self.Platform);
            request.Headers.Add("Satori-User-ID", self.UserId);

            // Third-party libs
            request.Headers.Add("X-Platform", self.Platform);
            request.Headers.Add("X-User-ID", self.UserId);
        }

        return await HttpClient.SendAsync(request);
    }

    public void Start()
    {
        if (IsActive)
        {
            throw new InvalidOperationException("连接未关闭");
        }

        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(
            _cancellationTokenProvider.Token
        );
        _sn = null;
        HttpClient = new() { BaseAddress = new(_settingProvider.Value.Connection.Satori.Uri) };

        _webSocket = CreateNew();
        _webSocket.Open();

        StatusChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Stop()
    {
        if (!IsActive)
        {
            throw new InvalidOperationException("连接已关闭");
        }

        _cancellationTokenSource.Cancel();
        _webSocket?.Close();
        HttpClient.CancelPendingRequests();
        HttpClient.Dispose();

        StatusChanged?.Invoke(this, EventArgs.Empty);
    }
}
