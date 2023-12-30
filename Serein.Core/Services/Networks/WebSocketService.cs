using System;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core.Models.Settings;
using Serein.Core.Services.Data;

using WatsonWebsocket;

namespace Serein.Core.Services.Networks;

public class WebSocketService : INetworkService
{
    private readonly IHost _host;
    private WatsonWsClient? _client;

    private SettingProvider SettingProvider => _host.Services.GetRequiredService<SettingProvider>();
    private Setting Setting => SettingProvider.Value;

    public event EventHandler? Opened;
    public event EventHandler? Closed;
    public event EventHandler<MessageReceivedEventArgs>? MessageReceived;

    public bool Active => _client?.Connected ?? false;
    public Statistics? Stats => _client?.Stats;

    public WebSocketService(IHost host)
    {
        _host = host;
    }

    private WatsonWsClient CreateNew()
    {
        var client = new WatsonWsClient(new(Setting.Connection.Uri)).ConfigureOptions(
            (options) =>
            {
                if (!string.IsNullOrEmpty(Setting.Connection.AccessToken))
                    options.SetRequestHeader(
                        "Authorization",
                        $"Bearer {Setting.Connection.AccessToken}"
                    );

                foreach (var kv in Setting.Connection.Headers)
                    options.SetRequestHeader(kv.Key, kv.Value);
            }
        );

        client.ServerConnected += Opened;
        client.ServerDisconnected += Closed;

        client.MessageReceived += MessageReceived;

        return client;
    }

    public void Dispose()
    {
        _client?.Dispose();
        GC.SuppressFinalize(this);
    }

    public async Task SendAsync(string text)
    {
        if (_client is not null && _client.Connected)
            await _client.SendAsync(text);
    }

    public async Task StartAsync()
    {
        _client = CreateNew();
        await _client.StartWithTimeoutAsync();
    }

    public async Task StopAsync()
    {
        if (_client is not null)
            await _client.StopAsync();
    }
}
