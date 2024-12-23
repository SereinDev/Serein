using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fleck;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serein.Core.Models.Output;
using Serein.Core.Services.Data;
using Serein.Core.Utils.Extensions;
using WebSocket4Net;
using MsLogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Serein.Core.Services.Network.Connection;

public sealed class ReverseWebSocketService : IConnectionService
{
    private WebSocketServer? _server;
    private readonly Dictionary<string, IWebSocketConnection> _webSockets = [];
    private readonly SettingProvider _settingProvider;

    public ReverseWebSocketService(IHost host, SettingProvider settingProvider)
    {
        _settingProvider = settingProvider;
        _logger = new(host.Services.GetRequiredService<IConnectionLogger>);

        FleckLog.LogAction = (level, msg, _) =>
        {
            if (level == LogLevel.Error)
            {
                _logger.Value.Log(MsLogLevel.Error, msg);
            }
        };
    }

    private readonly Lazy<IConnectionLogger> _logger;

    public event EventHandler<MessageReceivedEventArgs>? MessageReceived;
    public event EventHandler? StatusChanged;

    public bool Active => _server is not null;

    private WebSocketServer CreateNew()
    {
        var server = new WebSocketServer(_settingProvider.Value.Connection.Uri)
        {
            RestartAfterListenError = true,
            SupportedSubProtocols = _settingProvider.Value.Connection.SubProtocols,
        };

        return server;
    }

    private void ConfigServer(IWebSocketConnection webSocket)
    {
        webSocket.OnOpen += () => _webSockets.Add(GetEndPoint(), webSocket);
        webSocket.OnOpen += () =>
            _logger.Value.Log(
                MsLogLevel.Information,
                $"[{GetEndPoint()}] 连接到反向WebSocket服务器"
            );

        webSocket.OnClose += () => _webSockets.Remove(GetEndPoint());
        webSocket.OnClose += () =>
            _logger.Value.Log(
                MsLogLevel.Information,
                $"[{GetEndPoint()}] 从反向WebSocket服务器断开"
            );

        webSocket.OnError += (e) =>
            _logger.Value.Log(
                MsLogLevel.Error,
                $"[{GetEndPoint()}] 发生错误：{Environment.NewLine}" + e.GetDetailString()
            );

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
        {
            return;
        }

        List<Task> tasks;

        lock (_webSockets)
        {
            tasks =
            [
                .. _webSockets.Values.Select(
                    (client) => client.IsAvailable ? client.Send(text) : Task.CompletedTask
                ),
            ];
        }

        await Task.WhenAll(tasks);
    }

    public void Start()
    {
        if (Active)
        {
            throw new InvalidOperationException();
        }
        _server = CreateNew();
        _server.Start(ConfigServer);
        _logger.Value.Log(
            MsLogLevel.Information,
            $"反向WebSocket服务器已在{_settingProvider.Value.Connection.Uri}开启"
        );
        StatusChanged?.Invoke(null, EventArgs.Empty);
    }

    public void Stop()
    {
        if (_server is not null)
        {
            _server.Dispose();
            _logger.Value.Log(MsLogLevel.Information, "反向WebSocket服务器已停止");
            _server = null;
            _webSockets.Clear();
            StatusChanged?.Invoke(null, EventArgs.Empty);
        }
    }
}
