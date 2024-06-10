using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Fleck;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core.Models.Output;
using Serein.Core.Models.Settings;
using Serein.Core.Services.Data;

using WebSocket4Net;

using MsLogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Serein.Core.Services.Networks;

public class ReverseWebSocketService : INetworkService
{
    private readonly IHost _host;
    private WebSocketServer? _server;
    private readonly Dictionary<string, IWebSocketConnection> _webSockets;

    private IServiceProvider Services => _host.Services;
    private ISereinLogger Logger => Services.GetRequiredService<ISereinLogger>();
    private SettingProvider SettingProvider => Services.GetRequiredService<SettingProvider>();
    private Setting Setting => SettingProvider.Value;

    public event EventHandler<MessageReceivedEventArgs>? MessageReceived;
    public event EventHandler? StatusChanged;

    public bool Active => _server is not null;

    public ReverseWebSocketService(IHost host)
    {
        _host = host;
        _webSockets = new();
    }

    private WebSocketServer CreateNew()
    {
        var server = new WebSocketServer(Setting.Network.Uri)
        {
            RestartAfterListenError = true,
            SupportedSubProtocols = Setting.Network.SubProtocols
        };

        return server;
    }

    private void ConfigServer(IWebSocketConnection webSocket)
    {
        webSocket.OnOpen += () => _webSockets.Add(GetEndPoint(), webSocket);
        webSocket.OnOpen += () =>
            Logger.LogBotConsole(MsLogLevel.Information, $"{GetEndPoint()}连接到反向WebSocket服务器");

        webSocket.OnClose += () => _webSockets.Remove(GetEndPoint());
        webSocket.OnClose += () =>
            Logger.LogBotConsole(MsLogLevel.Information, $"{GetEndPoint()}从反向WebSocket服务器断开");

        webSocket.OnError += (e) =>
            Logger.LogBotConsole(MsLogLevel.Error, $"{GetEndPoint()}发生错误：{e}");

        webSocket.OnMessage += (msg) => MessageReceived?.Invoke(webSocket, new(msg));

        string GetEndPoint() =>
            $"{webSocket.ConnectionInfo.ClientIpAddress}:{webSocket.ConnectionInfo.ClientPort}";
    }

    public void Dispose()
    {
        _server?.Dispose();
        GC.SuppressFinalize(this);
    }

    public async Task SendAsync(string text)
    {
        if (_server is null)
            return;

        List<Task> tasks;

        lock (_webSockets)
            tasks = new(
                _webSockets.Values.Select(
                    (client) => client.IsAvailable ? client.Send(text) : Task.CompletedTask
                )
            );

        await Task.WhenAll(tasks);
    }

    public void Start(CancellationToken token)
    {
        if (Active)
            throw new InvalidOperationException();

        _server = CreateNew();
        _server.Start(ConfigServer);
        Logger.LogBotConsole(MsLogLevel.Information, $"反向WebSocket服务器已在{Setting.Network.Uri}开启");
        StatusChanged?.Invoke(null, EventArgs.Empty);
    }

    public void Stop()
    {
        if (_server is not null)
        {
            _server.Dispose();
            StatusChanged?.Invoke(null, EventArgs.Empty);
            Logger.LogBotConsole(MsLogLevel.Information, "反向WebSocket服务器已停止");
            _server = null;
            _webSockets.Clear();
        }
    }
}
