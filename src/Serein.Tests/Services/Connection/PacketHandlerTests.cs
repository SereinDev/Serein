using System;
using System.Text.Json;
using System.Text.Json.Nodes;
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

namespace Serein.Tests.Services.Connection;

[Collection(nameof(Serein))]
public class PacketHandlerTests : IDisposable
{
    private readonly IHost _app;
    private readonly SettingProvider _settingProvider;
    private readonly PacketHandler _packetHandler;

    public PacketHandlerTests()
    {
        _app = HostFactory.BuildNew();
        _settingProvider = _app.Services.GetRequiredService<SettingProvider>();
        _packetHandler = _app.Services.GetRequiredService<PacketHandler>();
        _app.Start();

        _settingProvider.Value.Connection.Adapter = AdapterType.Satori;
        _settingProvider.Value.Connection.OneBot.Uri = "http://127.0.0.1:8080";
    }

    public void Dispose()
    {
        _app.StopAsync();
        _app.Dispose();
    }

    [Fact]
    public void ShouldThrowWhenUsingEmptyPluginHandler()
    {
        Assert.Throws<NotSupportedException>(
            () => _packetHandler.Handle(AdapterType.Plugin, new JsonObject())
        );
    }

    [Fact]
    public void ShouldBeAbleToCaculateCorrectTargets()
    {
        Assert.False(_packetHandler.IsListenedId(TargetType.Auto, ""));
        Assert.False(_packetHandler.IsListenedId(TargetType.Group, null));

        Assert.True(_packetHandler.IsListenedId(TargetType.Private, "123456"));

        _settingProvider.Value.Connection.ListenedIds = ["123456"];
        Assert.True(_packetHandler.IsListenedId(TargetType.Group, "123456"));
        Assert.True(_packetHandler.IsListenedId(TargetType.Channel, "123456"));
        Assert.True(_packetHandler.IsListenedId(TargetType.Guild, "123456"));
        Assert.False(_packetHandler.IsListenedId(TargetType.Group, "abcdef"));
        Assert.False(_packetHandler.IsListenedId(TargetType.Group, "1234567890"));

        _settingProvider.Value.Connection.ListenedIds = ["g:123456"];
        Assert.True(_packetHandler.IsListenedId(TargetType.Group, "123456"));
        Assert.False(_packetHandler.IsListenedId(TargetType.Channel, "123456"));
        Assert.False(_packetHandler.IsListenedId(TargetType.Guild, "123456"));

        _settingProvider.Value.Connection.ListenedIds = ["g:*"];
        Assert.True(_packetHandler.IsListenedId(TargetType.Group, "123456"));
        Assert.True(_packetHandler.IsListenedId(TargetType.Group, "abcdef"));
        Assert.False(_packetHandler.IsListenedId(TargetType.Channel, "123456"));
        Assert.False(_packetHandler.IsListenedId(TargetType.Guild, "123456"));

        _settingProvider.Value.Connection.ListenedIds = ["group:*"];
        Assert.True(_packetHandler.IsListenedId(TargetType.Group, "123456"));
        Assert.True(_packetHandler.IsListenedId(TargetType.Group, "abcdef"));
        Assert.False(_packetHandler.IsListenedId(TargetType.Channel, "123456"));
        Assert.False(_packetHandler.IsListenedId(TargetType.Guild, "123456"));
    }
}
