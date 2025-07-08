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
using Xunit;

namespace Serein.Tests.Services.Connection;

[Collection(nameof(Serein))]
public class OneBotForwardAdapterTests : IDisposable
{
    private readonly IHost _app;
    private readonly SettingProvider _settingProvider;
    private readonly ConnectionManager _connectionManager;

    public OneBotForwardAdapterTests()
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
    public void CanConnectToOneBotWebSocketServer()
    {
        _settingProvider.Value.Connection.Adapter = AdapterType.OneBot_ForwardWebSocket;
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
}
