using System;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serein.Core.Models.Output;
using Serein.Core.Models.Settings;
using Serein.Core.Services.Data;

using WatsonWebsocket;

namespace Serein.Core.Services.Networks;

public class WebSocketService : INetworkService
{
    private readonly IHost _host;
    private WatsonWsClient? _client;
    private string _uri;
    private bool _connecting;

    private IServiceProvider Services => _host.Services;
    private IOutputHandler Logger => Services.GetRequiredService<IOutputHandler>();
    private SettingProvider SettingProvider => Services.GetRequiredService<SettingProvider>();
    private Setting Setting => SettingProvider.Value;

    public event EventHandler<MessageReceivedEventArgs>? MessageReceived;

    public bool Active => _client?.Connected ?? false;
    public Statistics? Stats => _client?.Stats;

    public WebSocketService(IHost host)
    {
        _host = host;
        _uri = string.Empty;
    }

    private WatsonWsClient CreateNew()
    {
        _uri = Setting.Network.Uri;
        var client = new WatsonWsClient(new(Setting.Network.Uri))
        {
            EnableStatistics = true,
            AcceptInvalidCertificates = true
        }.ConfigureOptions(
            (options) =>
            {
                if (!string.IsNullOrEmpty(Setting.Network.AccessToken))
                    options.SetRequestHeader(
                        "Authorization",
                        $"Bearer {Setting.Network.AccessToken}"
                    );

                foreach (var kv in Setting.Network.Headers)
                    options.SetRequestHeader(kv.Key, kv.Value);

                foreach (var subProtocol in Setting.Network.SubProtocols)
                    options.AddSubProtocol(subProtocol);
            }
        );

        client.MessageReceived += MessageReceived;
        client.ServerConnected += (_, _) =>
            Logger.LogBotConsole(LogLevel.Information, $"成功连接到{_uri}");
        client.ServerDisconnected += (_, _) => Logger.LogBotConsole(LogLevel.Warning, "连接已断开");

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

    public void Start()
    {
        if (_connecting)
            throw new InvalidOperationException("正在连接中");

        _client = CreateNew();
        _connecting = true;
        _client
            .StartWithTimeoutAsync(10)
            .ContinueWith(
                (task) =>
                {
                    _connecting = false;
                    if (!task.Result)
                    {
                        Logger.LogBotConsole(LogLevel.Error, "连接超时");
                    }
                }
            );
    }

    public void Stop()
    {
        _client?.Stop();
    }
}
