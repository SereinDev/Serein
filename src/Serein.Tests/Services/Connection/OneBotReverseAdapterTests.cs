using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serein.Core.Models.Network.Connection;
using Serein.Core.Services.Data;
using Serein.Core.Services.Network.Connection;
using Serein.Core.Utils.Extensions;
using WebSocket4Net;
using Xunit;

namespace Serein.Tests.Services.Connection;

[Collection(nameof(Serein))]
public class OneBotReverseAdapterTests : IDisposable
{
    private readonly IHost _app;
    private readonly SettingProvider _settingProvider;
    private readonly ConnectionManager _connectionManager;

    public OneBotReverseAdapterTests()
    {
        _app = HostFactory.BuildNew();
        _settingProvider = _app.Services.GetRequiredService<SettingProvider>();
        _connectionManager = _app.Services.GetRequiredService<ConnectionManager>();
        _app.Start();
    }

    public void Dispose()
    {
        _app.StopAsync();
        _app.Dispose();
    }

    [Fact]
    public void ShouldStartOneBotReverseWebSocketAdapter()
    {
        _settingProvider.Value.Connection.Adapter = AdapterType.OneBot_ReverseWebSocket;
        _settingProvider.Value.Connection.OneBot.Uri = "http://127.0.0.1:8080";
        _connectionManager.Start();

        Task.Delay(2000).Await();
        Assert.True(_connectionManager.IsActive);

        using var ws = new WebSocket("ws://127.0.0.1:8080");
        ws.Open();

        Task.Delay(2000).Await();
        Assert.Equal(WebSocketState.Open, ws.State);
    }
}
