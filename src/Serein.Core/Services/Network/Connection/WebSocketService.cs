using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serein.Core.Models.Output;
using Serein.Core.Services.Data;
using WebSocket4Net;

namespace Serein.Core.Services.Network.Connection;

public sealed class WebSocketService(IHost host, SettingProvider settingProvider)
    : IConnectionService
{
    private readonly Lazy<IConnectionLogger> _connectionLogger =
        new(host.Services.GetRequiredService<IConnectionLogger>);
    private readonly SettingProvider _settingProvider = settingProvider;

    private CancellationTokenSource? _reconnectCancellationToken;
    private bool _closedManually;
    private bool _connectedSuccessfully;
    private WebSocket? _client;
    private string _uri = string.Empty;

    public event EventHandler<MessageReceivedEventArgs>? MessageReceived;
    public event EventHandler? StatusChanged;

    public bool Connecting { get; private set; }
    public bool Active =>
        _client?.State == WebSocketState.Open
        || Connecting
        || _reconnectCancellationToken is not null
            && !_reconnectCancellationToken.IsCancellationRequested;

    private WebSocket CreateNew()
    {
        _uri = _settingProvider.Value.Connection.Uri;

        var headers = new Dictionary<string, string>(_settingProvider.Value.Connection.Headers);
        if (!string.IsNullOrEmpty(_settingProvider.Value.Connection.AccessToken))
        {
            headers["Authorization"] = $"Bearer {_settingProvider.Value.Connection.AccessToken}";
        }

        var client = new WebSocket(
            _settingProvider.Value.Connection.Uri,
            string.Join('\x20', _settingProvider.Value.Connection.SubProtocols),
            customHeaderItems: [.. headers]
        );

        client.MessageReceived += MessageReceived;
        client.Opened += StatusChanged;
        client.Opened += (_, _) =>
        {
            _connectionLogger.Value.Log(LogLevel.Information, $"成功连接到 {_uri}");
            Connecting = false;
            _connectedSuccessfully = true;
        };
        client.Closed += StatusChanged;
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

        return client;
    }

    public void Dispose()
    {
        _client?.Dispose();
        GC.SuppressFinalize(this);
    }

    public Task SendAsync(string text)
    {
        if (_client is not null && _client.State == WebSocketState.Open)
        {
            _client.Send(text);
        }
        return Task.CompletedTask;
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
            return;
        }

        _closedManually = true;
        _client?.Close();
    }

    private async Task TryReconnect()
    {
        if (_closedManually || !_connectedSuccessfully)
        {
            return;
        }

        _reconnectCancellationToken?.Dispose();
        _reconnectCancellationToken = new();
        _connectionLogger.Value.Log(
            LogLevel.Information,
            $"将在五秒后（{DateTime.Now.AddSeconds(5):T}）尝试重新连接"
        );

        await Task.Delay(5000, _reconnectCancellationToken.Token);

        Start();
        _reconnectCancellationToken.Cancel();
    }
}
