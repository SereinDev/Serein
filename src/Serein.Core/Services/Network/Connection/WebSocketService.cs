using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serein.Core.Models.Output;
using Serein.Core.Models.Settings;
using Serein.Core.Services.Data;

using WebSocket4Net;

namespace Serein.Core.Services.Network.Connection;

public class WebSocketService(IHost host, SettingProvider settingProvider) : IConnectionService
{
    private IConnectionLogger ConnectionLogger => host.Services.GetRequiredService<IConnectionLogger>();
    private readonly SettingProvider _settingProvider = settingProvider;
    private WebSocket? _client;
    private string _uri = string.Empty;

    private Setting Setting => _settingProvider.Value;

    public event EventHandler<MessageReceivedEventArgs>? MessageReceived;
    public event EventHandler? StatusChanged;

    public bool Active => _client?.State == WebSocketState.Open;
    public bool Connecting { get; private set; }

    private WebSocket CreateNew()
    {
        _uri = Setting.Connection.Uri;

        var headers = new Dictionary<string, string>(Setting.Connection.Headers);
        if (!string.IsNullOrEmpty(Setting.Connection.AccessToken))
            headers["Authorization"] = $"Bearer {Setting.Connection.AccessToken}";

        var client = new WebSocket(
            Setting.Connection.Uri,
            string.Join('\x20', Setting.Connection.SubProtocols),
            customHeaderItems: [.. headers]
        );

        client.MessageReceived += MessageReceived;
        client.Opened += (_, _) => ConnectionLogger.Log(LogLevel.Information, $"成功连接到{_uri}");
        client.Opened += StatusChanged;
        client.Closed += StatusChanged;
        client.Closed += (_, _) => ConnectionLogger.Log(LogLevel.Warning, "连接已断开");
        client.Error += (_, e) =>
            ConnectionLogger.Log(LogLevel.Error, $"{e.Exception.GetType().FullName}: {e.Exception.Message}");

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
                    ConnectionLogger.Log(LogLevel.Error, e.Message);
                }
                finally
                {
                    Connecting = false;
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
