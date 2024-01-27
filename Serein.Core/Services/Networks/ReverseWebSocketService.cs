using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serein.Core.Models.Output;
using Serein.Core.Models.Settings;
using Serein.Core.Services.Data;

using WatsonWebsocket;

namespace Serein.Core.Services.Networks;

public class ReverseWebSocketService : INetworkService
{
    private readonly IHost _host;
    private WatsonWsServer? _server;

    private IServiceProvider Services => _host.Services;
    private IOutputHandler Logger => Services.GetRequiredService<IOutputHandler>();
    private SettingProvider SettingProvider => Services.GetRequiredService<SettingProvider>();
    private Setting Setting => SettingProvider.Value;

    public event EventHandler<MessageReceivedEventArgs>? MessageReceived;

    public bool Active => _server?.IsListening ?? false;
    public Statistics? Stats => _server?.Stats;

    public ReverseWebSocketService(IHost host)
    {
        _host = host;
    }

    private WatsonWsServer CreateNew()
    {
        var server = new WatsonWsServer(new(Setting.Network.Uri)) { EnableStatistics = true };

        server.ClientConnected += (_, e) =>
            Logger.LogBotConsole(LogLevel.Information, $"{e.Client.IpPort}连接到反向WebSocket服务器");

        server.ClientDisconnected += (_, e) =>
            Logger.LogBotConsole(LogLevel.Information, $"{e.Client.IpPort}从反向WebSocket服务器断开");

        server.ServerStopped += (_, _) =>
            Logger.LogBotConsole(LogLevel.Information, "反向WebSocket服务器已停止");

        server.MessageReceived += MessageReceived;

        return server;
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

    public void Start()
    {
        if (Active)
            throw new InvalidOperationException();

        _server = CreateNew();
        _server.StartAsync();
        Logger.LogBotConsole(LogLevel.Information, $"反向WebSocket服务器已在{Setting.Network.Uri}开启");
    }

    public void Stop()
    {
        _server?.Stop();
    }
}
