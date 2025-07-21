using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serein.ConnectionProtocols.Models;
using Serein.Core.Models.Abstractions;
using Serein.Core.Models.Commands;
using Serein.Core.Models.Network.Connection;
using Serein.Core.Services.Data;
using WebSocket4Net;

namespace Serein.Core.Services.Network.Connection.Adapters.OneBot;

public sealed class ForwardWebSocketAdapter(
    IHost host,
    SettingProvider settingProvider,
    ActionBuilder actionBuilder,
    CancellationTokenProvider cancellationTokenProvider
) : IConnectionAdapter
{
    private readonly Lazy<ConnectionLoggerBase> _connectionLogger = new(
        host.Services.GetRequiredService<ConnectionLoggerBase>
    );

    private CancellationTokenSource? _reconnectCancellationToken;
    private bool _closedManually;
    private bool _connectedSuccessfully;
    private WebSocket? _client;
    private string _uri = string.Empty;

    public event EventHandler<MessageReceivedEventArgs>? DataReceived;
    public event EventHandler? StatusChanged;

    public AdapterType Type { get; } = AdapterType.OneBot_ForwardWebSocket;
    public bool Connecting { get; private set; }
    public bool IsActive =>
        _client?.State is WebSocketState.Open or WebSocketState.Connecting
        || Connecting
        || _reconnectCancellationToken is not null
            && !_reconnectCancellationToken.IsCancellationRequested;

    private WebSocket CreateNew()
    {
        _uri = settingProvider.Value.Connection.OneBot.Uri;

        var headers = new Dictionary<string, string>(
            settingProvider.Value.Connection.OneBot.Headers
        );
        if (!string.IsNullOrEmpty(settingProvider.Value.Connection.OneBot.AccessToken))
        {
            headers["Authorization"] =
                $"Bearer {settingProvider.Value.Connection.OneBot.AccessToken}";
        }

        var client = new WebSocket(
            settingProvider.Value.Connection.OneBot.Uri,
            string.Join('\x20', settingProvider.Value.Connection.OneBot.SubProtocols),
            customHeaderItems: [.. headers]
        );

        client.MessageReceived += (_, e) => DataReceived?.Invoke(this, e);

        client.Opened += (_, _) =>
        {
            _connectionLogger.Value.Log(LogLevel.Information, $"成功连接到 {_uri}");
            Connecting = false;
            _connectedSuccessfully = true;
        };
        client.Closed += (_, _) =>
        {
            _connectionLogger.Value.Log(LogLevel.Warning, "连接已断开");
            Connecting = false;

            TryReconnect();
        };
        client.Error += (_, e) =>
            _connectionLogger.Value.Log(
                LogLevel.Error,
                $"{e.Exception.GetType().FullName}: {e.Exception.Message}"
            );

        client.Opened += StatusChanged;
        client.Closed += StatusChanged;
        client.Error += (_, _) => StatusChanged?.Invoke(this, EventArgs.Empty);

        return client;
    }

    public void Dispose()
    {
        _client?.Dispose();
        _reconnectCancellationToken?.Dispose();
    }

    public Task SendAsync(string text)
    {
        if (_client is not null && _client.State == WebSocketState.Open)
        {
            _client.Send(text);
        }

        return Task.CompletedTask;
    }

    public Task SendMessageAsync(
        TargetType type,
        string target,
        string content,
        CommandArguments? commandArguments = null,
        Self? self = null
    )
    {
        return SendAsync(actionBuilder.Build(type, target, content, commandArguments, self));
    }

    public void Start()
    {
        if (Connecting)
        {
            throw new InvalidOperationException("正在连接中");
        }

        _client = CreateNew();
        Connecting = true;
        _connectedSuccessfully = _closedManually = false;

        Task.Run(() =>
        {
            try
            {
                _client.Open();
            }
            catch (Exception e)
            {
                Connecting = false;
                _connectionLogger.Value.Log(LogLevel.Error, e.Message);
            }
        });
    }

    public void Stop()
    {
        if (
            _reconnectCancellationToken is not null
            && !_reconnectCancellationToken.IsCancellationRequested
        )
        {
            _reconnectCancellationToken.Cancel();
            _connectionLogger.Value.Log(LogLevel.Information, $"重连已取消");
            StatusChanged?.Invoke(this, EventArgs.Empty);
            return;
        }

        _closedManually = true;
        _client?.Close();
    }

    private async Task TryReconnect()
    {
        if (
            _closedManually
            || !_connectedSuccessfully
            || !settingProvider.Value.Connection.OneBot.AutoReconnect
        )
        {
            return;
        }

        _reconnectCancellationToken?.Dispose();
        _reconnectCancellationToken = CancellationTokenSource.CreateLinkedTokenSource(
            cancellationTokenProvider.Token
        );
        _connectionLogger.Value.Log(
            LogLevel.Information,
            $"将在五秒后（{DateTime.Now.AddSeconds(5):T}）尝试重新连接"
        );

        await Task.Delay(5000, _reconnectCancellationToken.Token);

        Start();
        _reconnectCancellationToken.Cancel();
    }
}
