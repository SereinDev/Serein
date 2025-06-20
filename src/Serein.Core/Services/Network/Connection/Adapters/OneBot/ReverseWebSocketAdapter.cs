using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fleck;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serein.Core.Models.Abstractions;
using Serein.Core.Models.Commands;
using Serein.Core.Models.Network.Connection;
using Serein.Core.Services.Data;
using Serein.Core.Utils.Extensions;
using WebSocket4Net;
using MsLogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Serein.Core.Services.Network.Connection.Adapters.OneBot;

public sealed class ReverseWebSocketAdapter : IConnectionAdapter
{
    private WebSocketServer? _server;
    private readonly Dictionary<string, IWebSocketConnection> _webSockets = [];
    private readonly SettingProvider _settingProvider;
    private readonly ActionBuilder _actionBuilder;

    public ReverseWebSocketAdapter(
        IHost host,
        SettingProvider settingProvider,
        ActionBuilder actionBuilder
    )
    {
        _settingProvider = settingProvider;
        _actionBuilder = actionBuilder;
        _logger = new(host.Services.GetRequiredService<ConnectionLoggerBase>);

        FleckLog.LogAction = (level, msg, _) =>
        {
            if (level == LogLevel.Error)
            {
                _logger.Value.Log(MsLogLevel.Error, msg);
            }
        };
    }

    public AdapterType Type { get; } = AdapterType.OneBot_ReverseWebSocket;
    private readonly Lazy<ConnectionLoggerBase> _logger;

    public event EventHandler<MessageReceivedEventArgs>? DataReceived;
    public event EventHandler? StatusChanged;

    public bool IsActive => _server is not null;

    private WebSocketServer CreateNew()
    {
        var server = new WebSocketServer(_settingProvider.Value.Connection.OneBot.Uri)
        {
            RestartAfterListenError = true,
            SupportedSubProtocols = _settingProvider.Value.Connection.OneBot.SubProtocols,
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

        webSocket.OnMessage += (msg) => DataReceived?.Invoke(this, new(msg));

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
        if (IsActive)
        {
            throw new InvalidOperationException();
        }

        _server = CreateNew();
        _server.Start(ConfigServer);

        _logger.Value.Log(
            MsLogLevel.Information,
            $"反向WebSocket服务器已在{_settingProvider.Value.Connection.OneBot.Uri}开启"
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
        else
        {
            throw new InvalidOperationException();
        }
    }

    public Task SendMessageAsync(
        TargetType type,
        string target,
        string content,
        CommandArguments? commandArguments = null
    )
    {
        return SendAsync(_actionBuilder.Build(type, target, content, commandArguments));
    }
}
