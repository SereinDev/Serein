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

    [Fact]
    public async Task ShouldNotBeClosedWithTokenParam()
    {
        using var ws = new WebSocket("ws://127.0.0.1:50000/ws/connection?token=123456");
        ws.Open();

        await Task.Delay(500);

        Assert.Equal(WebSocketState.Open, ws.State);
        ws.Dispose();
    }

    [Fact]
    public async Task ShouldBeClosedWithoutTokenParam()
    {
        using var ws = new WebSocket("ws://127.0.0.1:50000/ws/connection");
        ws.Open();

        await Task.Delay(1000);

        Assert.NotEqual(WebSocketState.Open, ws.State);
    }
}
