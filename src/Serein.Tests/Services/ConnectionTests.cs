using System;
using System.Text.Json;
using System.Threading.Tasks;
using Fleck;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serein.ConnectionProtocols.Models.OneBot.V11.Packets;
using Serein.Core.Models.Network.Connection;
using Serein.Core.Services.Data;
using Serein.Core.Services.Network.Connection;
using Serein.Core.Utils.Extensions;
using Serein.Core.Utils.Json;
using WebSocket4Net;
using Xunit;

namespace Serein.Tests.Services;

[Collection(nameof(Serein))]
public class ConnectionTests : IDisposable
{
    private readonly IHost _app;
    private readonly SettingProvider _settingProvider;
    private readonly ConnectionManager _connectionManager;

    public ConnectionTests()
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
    public void ShouldBeAbleToConnectToWebSocketServer()
    {
        using var server = new WebSocketServer(_settingProvider.Value.Connection.OneBot.Uri);
        server.Start(
            (socket) =>
                socket.OnOpen = () =>
                    socket.Send(
                        JsonSerializer.Serialize(
                            new MessagePacket(),
                            JsonSerializerOptionsFactory.PacketStyle
                        )
                    )
        );

        _connectionManager.Start();

        Task.Delay(2000).Await();
        Assert.True(_connectionManager.IsActive);
    }

    [Fact]
    public void ShouldStartReverseWebSocketServer()
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
