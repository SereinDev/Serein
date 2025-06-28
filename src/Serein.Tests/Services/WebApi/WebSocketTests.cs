using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serein.Core.Services.Data;
using WebSocket4Net;
using Xunit;

namespace Serein.Tests.Services.WebApi;

[Collection(nameof(Serein))]
public class WebSocketTests : IDisposable
{
    private readonly IHost _app;

    public WebSocketTests()
    {
        _app = HostFactory.BuildNew();

        var settingProvider = _app.Services.GetRequiredService<SettingProvider>();
        settingProvider.Value.WebApi.IsEnabled = true;
        settingProvider.Value.WebApi.AccessTokens = ["123456"];
        _app.Start();
    }

    public void Dispose()
    {
        _app.StopAsync();
        _app.Dispose();
    }

    [Theory]
    [InlineData("connection")]
    [InlineData("plugins")]
    public async Task ShouldNotBeClosedWithTokenParam(string path)
    {
        using var ws = new WebSocket($"ws://127.0.0.1:50000/ws/{path}?token=123456");
        ws.Open();

        await Task.Delay(500);

        Assert.Equal(WebSocketState.Open, ws.State);
    }

    [Theory]
    [InlineData("ws://127.0.0.1:50000/ws/plugins")]
    [InlineData("ws://127.0.0.1:50000/ws/connection")]
    [InlineData("ws://127.0.0.1:50000/ws/plugins?token=")]
    [InlineData("ws://127.0.0.1:50000/ws/connection?token=")]
    [InlineData("ws://127.0.0.1:50000/ws/plugins?token=1")]
    [InlineData("ws://127.0.0.1:50000/ws/connection?token=1")]
    public async Task ShouldBeClosedWithInvalidTokenParam(string url)
    {
        using var ws = new WebSocket(url);
        ws.Open();

        await Task.Delay(1000);

        Assert.NotEqual(WebSocketState.Open, ws.State);
    }

    [Fact]
    public async Task ShouldBeClosedWithoutIdParam()
    {
        using var ws = new WebSocket("ws://127.0.0.1:50000/ws/server?token=123456");
        ws.Open();

        await Task.Delay(1000);

        Assert.NotEqual(WebSocketState.Open, ws.State);
    }

    [Fact]
    public async Task ShouldNotBeClosedWithTokenAndIdParam()
    {
        using var ws = new WebSocket("ws://127.0.0.1:50000/ws/server?token=123456&id=myserver");
        ws.Open();

        await Task.Delay(500);

        Assert.Equal(WebSocketState.Open, ws.State);
    }
}
