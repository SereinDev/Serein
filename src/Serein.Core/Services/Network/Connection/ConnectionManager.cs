using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serein.Core.Models.Abstractions;
using Serein.Core.Models.Network.Connection;
using Serein.Core.Models.Plugins;
using Serein.Core.Services.Data;
using Serein.Core.Services.Network.Connection.Adapters;
using Serein.Core.Services.Network.Connection.Adapters.OneBot;
using Serein.Core.Services.Network.Connection.Adapters.Satori;
using Serein.Core.Services.Plugins;
using Serein.Core.Utils;
using Serein.Core.Utils.Json;
using WebSocket4Net;

namespace Serein.Core.Services.Network.Connection;

public sealed class ConnectionManager : INotifyPropertyChanged
{
    private readonly PropertyChangedEventArgs _sentArg,
        _receivedArg,
        _isActiveArg;
    private readonly LogWriter _logWriter;
    private readonly PacketHandler _packetHandler;
    private readonly SettingProvider _settingProvider;
    private readonly EventDispatcher _eventDispatcher;
    private readonly Lazy<ForwardWebSocketAdapter> _forwardWebSocketAdapter;
    private readonly Lazy<ReverseWebSocketAdapter> _reverseWebSocketAdapter;
    private readonly Lazy<SatoriAdapter> _satoriAdapter;
    private readonly Lazy<IConnectionLogger> _connectionLogger;

    private IConnectionAdapter? _activeAdapter;

    private ulong _sent;
    private ulong _received;

    [MemberNotNullWhen(true, nameof(_activeAdapter))]
    public bool IsActive => _activeAdapter is not null;
    public ulong Sent => _sent;
    public ulong Received => _received;
    public DateTime? ConnectedAt { get; private set; }

    public event PropertyChangedEventHandler? PropertyChanged;
    public event EventHandler<DataTranferredEventArgs>? DataTransferred;

    public ConnectionManager(
        IHost host,
        ILogger<LogWriter> logger,
        SettingProvider settingProvider,
        EventDispatcher eventDispatcher,
        PacketHandler packetHandler,
        CancellationTokenProvider cancellationTokenProvider
    )
    {
        _isActiveArg = new(nameof(IsActive));
        _sentArg = new(nameof(Sent));
        _receivedArg = new(nameof(Received));

        _logWriter = new(logger, PathConstants.ConnectionLogDirectory);
        _settingProvider = settingProvider;
        _eventDispatcher = eventDispatcher;
        _packetHandler = packetHandler;

        _satoriAdapter = new(GetConfiguredAdapter<SatoriAdapter>);
        _reverseWebSocketAdapter = new(GetConfiguredAdapter<ReverseWebSocketAdapter>);
        _forwardWebSocketAdapter = new(GetConfiguredAdapter<ForwardWebSocketAdapter>);
        _connectionLogger = new(host.Services.GetRequiredService<IConnectionLogger>);

        cancellationTokenProvider.Token.Register(() =>
        {
            if (!IsActive)
            {
                return;
            }

            try
            {
                Stop();
            }
            catch { }
        });

        T GetConfiguredAdapter<T>()
            where T : IConnectionAdapter
        {
            var adapter = host.Services.GetRequiredService<T>();
            adapter.DataReceived += OnMessageReceived;
            adapter.StatusChanged += OnStatusChanged;
            return adapter;
        }

        void OnStatusChanged(object? sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, _isActiveArg);
            ConnectedAt = IsActive ? DateTime.Now : null;
        }
    }

    private void OnMessageReceived(object? sender, MessageReceivedEventArgs e)
    {
        if (sender is not IConnectionAdapter adapter)
        {
            return;
        }

        Interlocked.Increment(ref _received);
        PropertyChanged?.Invoke(this, _receivedArg);
        DataTransferred?.Invoke(
            this,
            new() { Data = e.Message, Type = DataTranferredType.Received }
        );

        if (_settingProvider.Value.Connection.SaveLog)
        {
            _logWriter.WriteAsync($"{DateTime.Now:t} [Received] {e.Message}");
        }

        if (_settingProvider.Value.Connection.OutputData)
        {
            _connectionLogger.Value.LogReceivedData(e.Message);
        }

        if (!_eventDispatcher.Dispatch(Event.ConnectionDataReceived, e.Message))
        {
            return;
        }

        try
        {
            var jsonNode = JsonSerializer.Deserialize<JsonNode>(e.Message);

            if (jsonNode is null)
            {
                return;
            }

            _packetHandler.Handle(adapter.Type, jsonNode);
        }
        catch (Exception ex)
        {
            _connectionLogger.Value.Log(LogLevel.Error, ex.Message);

            if (_settingProvider.Value.Connection.OutputData)
            {
                _logWriter.WriteAsync($"{DateTime.Now:t} [Error] {ex}");
            }
        }
    }

    public void Start()
    {
        if (IsActive)
        {
            throw new InvalidOperationException("已连接");
        }

        _sent = _received = 0;

        switch (_settingProvider.Value.Connection.Adapter)
        {
            case AdapterType.OneBot_ForwardWebSocket:
                _forwardWebSocketAdapter.Value.Start();
                _activeAdapter = _forwardWebSocketAdapter.Value;
                break;

            case AdapterType.OneBot_ReverseWebSocket:
                _reverseWebSocketAdapter.Value.Start();
                _activeAdapter = _reverseWebSocketAdapter.Value;
                break;

            case AdapterType.Satori:
                _satoriAdapter.Value.Start();
                _activeAdapter = _satoriAdapter.Value;
                break;

            default:
                throw new NotSupportedException("不支持的适配器类型");
        }
    }

    public void Stop()
    {
        if (!IsActive)
        {
            throw new InvalidOperationException("未连接");
        }

        ConnectedAt = null;
        _sent = _received = 0;

        PropertyChanged?.Invoke(this, _sentArg);
        PropertyChanged?.Invoke(this, _receivedArg);

        _activeAdapter.Stop();
    }

    public async Task SendAsync<T>(T body)
        where T : notnull
    {
        var text = JsonSerializer.Serialize(body, JsonSerializerOptionsFactory.PacketStyle);

        await SendDataAsync(text);
    }

    public async Task SendDataAsync(string data)
    {
        if (!IsActive)
        {
            return;
        }

        Interlocked.Increment(ref _sent);
        PropertyChanged?.Invoke(this, _sentArg);
        DataTransferred?.Invoke(this, new() { Data = data, Type = DataTranferredType.Sent });

        if (_settingProvider.Value.Connection.SaveLog)
        {
            _logWriter.WriteAsync($"{DateTime.Now:t} [Sent] {data}");
        }
        if (_settingProvider.Value.Connection.OutputData)
        {
            _connectionLogger.Value.LogSentData(data);
        }

        await _activeAdapter.SendAsync(data);
    }

    public async Task SendMessageAsync(TargetType type, string target, string text)
    {
        if (IsActive)
        {
            await _activeAdapter.SendMessageAsync(type, target, text);
        }
    }
}
