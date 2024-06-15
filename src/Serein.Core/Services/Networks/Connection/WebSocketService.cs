using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serein.Core.Models.Output;
using Serein.Core.Models.Settings;
using Serein.Core.Services.Data;

using WebSocket4Net;

namespace Serein.Core.Services.Networks.Connection;

public class WebSocketService(IHost host) : IConnectionService
{
    private readonly IHost _host = host;
    private WebSocket? _client;
    private string _uri = string.Empty;

    private IServiceProvider Services => _host.Services;
    private IConnectionLogger Logger => Services.GetRequiredService<IConnectionLogger>();
    private SettingProvider SettingProvider => Services.GetRequiredService<SettingProvider>();
    private Setting Setting => SettingProvider.Value;

    public event EventHandler<MessageReceivedEventArgs>? MessageReceived;
    public event EventHandler? StatusChanged;

    public bool Active => _client?.State == WebSocketState.Open;
    public bool Connecting { get; private set; }

    private WebSocket CreateNew()
    {
        _uri = Setting.Connection.Uri;
        var client = new WebSocket(
            Setting.Connection.Uri,
            string.Join('\x20', Setting.Connection.SubProtocols),
            customHeaderItems: string.IsNullOrEmpty(Setting.Connection.AccessToken)
                ? null
                : new() { new("Authorization", $"Bearer {Setting.Connection.AccessToken}") }
        );

        client.MessageReceived += MessageReceived;
        client.Opened += (_, _) => Logger.Log(LogLevel.Information, $"成功连接到{_uri}");
        client.Opened += StatusChanged;
        client.Closed += StatusChanged;
        client.Closed += (_, _) => Logger.Log(LogLevel.Warning, "连接已断开");

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
            _client.Send(text);

        return Task.CompletedTask;
    }

    public void Start(CancellationToken token)
    {
        if (Connecting)
            throw new InvalidOperationException("正在连接中");

        _client = CreateNew();
        Connecting = true;

        Task.Run(
            () =>
            {
                try
                {
                    _client.Open();
                }
                catch (Exception e)
                {
                    Logger.Log(LogLevel.Information, e.Message);
                }
            },
            token
        );
    }

    public void Stop()
    {
        _client?.Close();
    }
}
