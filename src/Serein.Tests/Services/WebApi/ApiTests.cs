using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using EmbedIO;
using EmbedIO.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serein.Core.Services.Data;
using Serein.Core.Services.Network.Web;
using Serein.Core.Services.Network.Web.Apis;
using Xunit;

namespace Serein.Tests.Services.WebApi;

[Collection(nameof(Serein))]
public class ApiTests : IDisposable
{
    private readonly HttpClient _client;
    private readonly IHost _app;
    private readonly Core.Services.Network.Web.WebServer _httpServer;

    public ApiTests()
    {
        _app = HostFactory.BuildNew();

        var settingProvider = _app.Services.GetRequiredService<SettingProvider>();
        settingProvider.Value.WebApi.IsEnabled = true;
        _httpServer = _app.Services.GetRequiredService<Core.Services.Network.Web.WebServer>();
        _client = new() { BaseAddress = new(settingProvider.Value.WebApi.UrlPrefixes.First()) };
    }

    public void Dispose()
    {
        if (_httpServer.State != WebServerState.Stopped)
        {
            _httpServer.Stop();
        }

        _app.StopAsync();
    }

    [Theory]
    [InlineData("/api/")]
    [InlineData("/api/metadata")]
    [InlineData("/api/connection")]
    [InlineData("/api/servers")]
    [InlineData("/api/setting")]
    public async Task ShouldBeAbleToVisitSpecifiedPath(string path)
    {
        _app.Start();
        var response = await _client.GetAsync(path);

        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task ShouldBeAbleToGetHardwareInfo()
    {
        _app.Start();
        foreach (var methodInfo in typeof(ApiMap).GetMethods())
        {
            var attribute = methodInfo
                .GetCustomAttributes(typeof(RouteAttribute), true)
                .OfType<RouteAttribute>()
                .FirstOrDefault();

            if (
                attribute is null
                || attribute.Verb != HttpVerbs.Get
                || !attribute.Route.StartsWith("/hardware")
            )
            {
                continue;
            }

            var response = await _client.GetAsync("/api" + attribute.Route);
            Assert.True(response.IsSuccessStatusCode);
        }
    }
}
