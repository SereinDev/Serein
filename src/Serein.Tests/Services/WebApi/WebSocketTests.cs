using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serein.Core.Models.Network.Web.WebAuthentication;
using Serein.Core.Services.Data;
using Serein.Core.Utils.Json;
using WebSocket4Net;
using Xunit;

namespace Serein.Tests.Services.WebApi;

[Collection(nameof(Serein))]
public class WebSocketTests : IDisposable
{
    private readonly IHost _app;
    private readonly HttpClient _client;

    public WebSocketTests()
    {
        _app = HostFactory.BuildNew();

        var settingProvider = _app.Services.GetRequiredService<SettingProvider>();
        var webAuthenticationProvider = _app.Services.GetRequiredService<WebAuthenticationProvider>();
        settingProvider.Value.WebApi.StartWhenSettingUp = true;
        webAuthenticationProvider.Value.Clear();
        webAuthenticationProvider.Value.Add(new TokenAuthentication { Token = "123456" });
        _client = new() { BaseAddress = new(settingProvider.Value.WebApi.UrlPrefixes.First()) };
        _app.Start();
    }

    public void Dispose()
    {
        _client.Dispose();
        _app.StopAsync();
        _app.Dispose();
    }

    [Theory]
    [InlineData("connection")]
    [InlineData("plugins")]
    public async Task ShouldNotBeClosedWithBearerHeader(string path)
    {
        using var ws = new WebSocket(
            $"ws://127.0.0.1:50000/ws/{path}",
            customHeaderItems: [new KeyValuePair<string, string>("Authorization", "Bearer 123456")]
        );
        ws.Open();

        await Task.Delay(500);

        Assert.Equal(WebSocketState.Open, ws.State);
    }

    [Theory]
    [InlineData("ws://127.0.0.1:50000/ws/plugins")]
    [InlineData("ws://127.0.0.1:50000/ws/connection")]
    [InlineData("ws://127.0.0.1:50000/ws/plugins?token=123456")]
    [InlineData("ws://127.0.0.1:50000/ws/connection?token=123456")]
    public async Task ShouldBeClosedWithoutValidCredentials(string url)
    {
        using var ws = new WebSocket(url);
        ws.Open();

        await Task.Delay(1000);

        Assert.NotEqual(WebSocketState.Open, ws.State);
    }

    [Fact]
    public async Task ShouldNotBeClosedWithTicketInQueryString()
    {
        var ticket = await CreateTicketAsync("/ws/plugins");
        using var ws = new WebSocket($"ws://127.0.0.1:50000/ws/plugins?ticket={Uri.EscapeDataString(ticket)}");
        ws.Open();

        await Task.Delay(500);

        Assert.Equal(WebSocketState.Open, ws.State);
    }

    [Fact]
    public async Task ShouldBeClosedWithoutIdParam()
    {
        using var ws = new WebSocket(
            "ws://127.0.0.1:50000/ws/server",
            customHeaderItems: [new KeyValuePair<string, string>("Authorization", "Bearer 123456")]
        );
        ws.Open();

        await Task.Delay(1000);

        Assert.NotEqual(WebSocketState.Open, ws.State);
    }

    [Fact]
    public async Task ShouldNotBeClosedWithTicketAndIdParam()
    {
        var ticket = await CreateTicketAsync("/ws/server");
        using var ws = new WebSocket(
            $"ws://127.0.0.1:50000/ws/server?ticket={Uri.EscapeDataString(ticket)}&id=myserver"
        );
        ws.Open();

        await Task.Delay(500);

        Assert.Equal(WebSocketState.Open, ws.State);
    }

    private async Task<string> CreateTicketAsync(string path)
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"/api/ws/ticket?path={Uri.EscapeDataString(path)}"
        );
        request.Headers.Authorization = new("Bearer", "123456");

        using var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json, new() { AllowTrailingCommas = true });

        if (
            doc.RootElement.TryGetProperty("data", out var data)
            && data.TryGetProperty("ticket", out var ticket)
            && ticket.ValueKind == JsonValueKind.String
            && !string.IsNullOrWhiteSpace(ticket.GetString())
        )
        {
            return ticket.GetString()!;
        }

        throw new InvalidOperationException("未获取到ws ticket");
    }
}
