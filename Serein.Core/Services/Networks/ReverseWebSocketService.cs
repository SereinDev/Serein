using System;
using System.Linq;
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

public class ReverseWebSocketService : INetworkService
{
    private readonly IHost _host;
    private WatsonWsServer? _server;

    private SettingProvider SettingProvider => _host.Services.GetRequiredService<SettingProvider>();
    private AppSetting AppSetting => SettingProvider.Value;

    public event EventHandler? Opened;
    public event EventHandler? Closed;
    public event EventHandler<WsMessageReceivedEventArgs>? WsMessageReceived;

    public bool Active => _server?.IsListening ?? false;
    public Statistics? Stats => _server?.Stats;

    public ReverseWebSocketService(IHost host)
    {
        _host = host;
    }

    private WatsonWsServer CreateNew()
    {
        var server = new WatsonWsServer(new(AppSetting.Bot.Uri)) { EnableStatistics = true };

        server.ServerStopped += Closed;
        server.MessageReceived += OnMessageReceived;

        return server;
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
        _server?.Dispose();
        GC.SuppressFinalize(this);
    }

    public async Task SendAsync(string text)
    {
        if (_server is not null && _server.IsListening)
        {
            var clients = _server.ListClients();
            await Task.WhenAll(clients.Select((client) => _server.SendAsync(client.Guid, text)));
        }
    }

    public async Task SendAsync<T>(T data)
    {
        if (_server is not null && _server.IsListening)
            await SendAsync(JsonSerializer.Serialize(data, JsonSerializerOptionsFactory.SnakeCase));
    }

    public async Task StartAsync()
    {
        _server = CreateNew();
        await _server.StartAsync();
        Opened?.Invoke(null, new());
    }

    public Task StopAsync()
    {
        _server?.Stop();

        return Task.CompletedTask;
    }
}
