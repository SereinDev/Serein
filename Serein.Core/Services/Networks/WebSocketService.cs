using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core.Models.Events;
using Serein.Core.Models.Settings;
using Serein.Core.Services.Data;
using Serein.Core.Utils;
using Serein.Core.Utils.Json;

using WatsonWebsocket;

namespace Serein.Core.Services.Networks;

public class WebSocketService : INetworkService
{
    private readonly IHost _host;
    private WatsonWsClient? _client;

    private SettingProvider SettingProvider => _host.Services.GetRequiredService<SettingProvider>();
    private AppSetting AppSetting => SettingProvider.Value;

    public event EventHandler? Opened;
    public event EventHandler? Closed;
    public event EventHandler<WsMessageReceivedEventArgs>? WsMessageReceived;

    public bool Active => _client?.Connected ?? false;
    public Statistics? Stats => _client?.Stats;

    public WebSocketService(IHost host)
    {
        _host = host;
    }

    private WatsonWsClient CreateNew()
    {
        var client = new WatsonWsClient(new(AppSetting.Bot.Uri)).ConfigureOptions(
            (options) =>
            {
                if (!string.IsNullOrEmpty(AppSetting.Bot.AccessToken))
                    options.SetRequestHeader(
                        "Authorization",
                        $"Bearer {AppSetting.Bot.AccessToken}"
                    );

                foreach (var kv in AppSetting.Bot.Headers)
                    options.SetRequestHeader(kv.Key, kv.Value);
            }
        );

        client.ServerConnected += Opened;
        client.ServerDisconnected += Closed;

        client.MessageReceived += OnMessageReceived;

        return client;
    }

    private void OnMessageReceived(object? sender, MessageReceivedEventArgs e)
    {
        var text = EncodingMap.UTF8.GetString(e.Data);
        JsonNode? jsonNode = null;

        try
        {
            jsonNode = JsonSerializer.Deserialize<JsonNode>(
                text,
                JsonSerializerOptionsFactory.SnakeCase
            );
        }
        catch { }

        WsMessageReceived?.Invoke(null, new() { JsonData = jsonNode, RawString = text });
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

    public async Task SendAsync<T>(T data)
    {
        if (_client is not null && _client.Connected)
            await _client.SendAsync(
                JsonSerializer.Serialize(data, JsonSerializerOptionsFactory.SnakeCase)
            );
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
