using System;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core.Models.Network.Connection.OneBot;
using Serein.Core.Models.Network.Connection.OneBot.ActionParams;
using Serein.Core.Models.Output;
using Serein.Core.Models.Plugins;
using Serein.Core.Models.Settings;
using Serein.Core.Services.Data;
using Serein.Core.Services.Plugins;
using Serein.Core.Utils.Json;

using WebSocket4Net;

namespace Serein.Core.Services.Network.Connection;

public class WsConnectionManager : INotifyPropertyChanged
{
    private readonly PropertyChangedEventArgs _sentArg,
        _receivedArg,
        _activeArg;
    private readonly SettingProvider _settingProvider;
    private readonly EventDispatcher _eventDispatcher;
    private readonly ReverseWebSocketService _reverseWebSocketService;
    private readonly WebSocketService _webSocketService;
    private readonly PacketHandler _packetHandler;
    private readonly Lazy<IConnectionLogger> _connectionLogger;

    private Setting Setting => _settingProvider.Value;
    private CancellationTokenSource? _cancellationTokenSource;
    private ulong _sent;
    private ulong _received;

    public bool Active => _webSocketService.Active || _reverseWebSocketService.Active;
    public ulong Sent => _sent;
    public ulong Received => _received;
    public DateTime? ConnectedAt { get; private set; }

    public event PropertyChangedEventHandler? PropertyChanged;


    public WsConnectionManager(
        IHost host,
        SettingProvider settingProvider,
        EventDispatcher eventDispatcher,
        ReverseWebSocketService reverseWebSocketService,
        WebSocketService webSocketService,
        PacketHandler packetHandler
    )
    {
        _settingProvider = settingProvider;
        _eventDispatcher = eventDispatcher;
        _reverseWebSocketService = reverseWebSocketService;
        _webSocketService = webSocketService;
        _packetHandler = packetHandler;
        _connectionLogger = new(host.Services.GetRequiredService<IConnectionLogger>);

        _activeArg = new(nameof(Active));
        _sentArg = new(nameof(Sent));
        _receivedArg = new(nameof(Received));

        _webSocketService.MessageReceived += OnMessageReceived;
        _reverseWebSocketService.MessageReceived += OnMessageReceived;

        _webSocketService.StatusChanged += OnStatusChanged;
        _reverseWebSocketService.StatusChanged += OnStatusChanged;

        void OnStatusChanged(object? sender, EventArgs e)
        {
            PropertyChanged?.Invoke(sender, _activeArg);
            ConnectedAt = Active ? DateTime.Now : null;
        }
    }

    private void OnMessageReceived(object? sender, MessageReceivedEventArgs e)
    {
        Interlocked.Increment(ref _received);
        PropertyChanged?.Invoke(this, _receivedArg);

        if (!_eventDispatcher.Dispatch(Event.WsDataReceived, e.Message))
            return;

        if (Setting.Connection.OutputData)
            _connectionLogger.Value.LogReceivedData(e.Message);

        var node = JsonSerializer.Deserialize<JsonNode>(e.Message);

        if (node is null)
            return;

        _packetHandler.Handle(node);
    }

    public void Start()
    {
        if (_reverseWebSocketService.Active)
            throw new InvalidOperationException("反向WebSocket服务器未关闭");

        if (_webSocketService.Active)
            throw new InvalidOperationException("WebSocket连接未断开");

        _sent = _received = 0;

        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = new();

        if (Setting.Connection.UseReverseWebSocket)
            _reverseWebSocketService.Start(_cancellationTokenSource.Token);
        else
            _webSocketService.Start(_cancellationTokenSource.Token);
    }

    public void Stop()
    {
        if (!Active && !_webSocketService.Connecting)
            throw new InvalidOperationException("WebSocket未连接");

        ConnectedAt = null;
        _sent = _received = 0;
        _cancellationTokenSource?.Cancel();

        if (_reverseWebSocketService.Active)
            _reverseWebSocketService.Stop();

        if (_webSocketService.Active)
            _webSocketService.Stop();
    }

    public async Task SendAsync<T>(T body)
        where T : notnull
    {
        var text = JsonSerializer.Serialize(body, JsonSerializerOptionsFactory.SnakeCase);

        await SendDataAsync(text);
    }

    public async Task SendDataAsync(string data)
    {
        Interlocked.Increment(ref _sent);
        PropertyChanged?.Invoke(this, _sentArg);

        if (Setting.Connection.OutputData)
            _connectionLogger.Value.LogSentData(data);

        if (_reverseWebSocketService.Active)
            await _reverseWebSocketService.SendAsync(data);
        else if (_webSocketService.Active)
            await _webSocketService.SendAsync(data);
    }

    private async Task SendActionRequestAsync<T>(string endpoint, T @params)
        where T : notnull, IActionParams
    {
        await SendAsync(new ActionRequest<T> { Action = endpoint, Params = @params });
    }

    public Task SendGroupMsgAsync(string target, string message)
        => SendGroupMsgAsync(long.Parse(target), message);

    public async Task SendGroupMsgAsync(long target, string message)
    {
        await SendActionRequestAsync(
            "send_msg",
            new MessageParams
            {
                GroupId = target,
                Message = message,
                AutoEscape = Setting.Connection.AutoEscape
            }
        );
        _connectionLogger.Value.LogReceivedMessage($"[群聊({target})] {message}");
    }
    public Task SendPrivateMsgAsync(string target, string message)
        => SendPrivateMsgAsync(long.Parse(target), message);

    public async Task SendPrivateMsgAsync(long target, string message)
    {
        await SendActionRequestAsync(
            "send_msg",
            new MessageParams
            {
                UserId = target,
                Message = message,
                AutoEscape = Setting.Connection.AutoEscape
            }
        );
        _connectionLogger.Value.LogReceivedMessage($"[私聊({target})] {message}");
    }
}
