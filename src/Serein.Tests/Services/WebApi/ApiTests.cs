using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using EmbedIO;
using EmbedIO.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serein.Core.Services.Data;
using Serein.Core.Services.Network.Web.Apis;
using Xunit;

namespace Serein.Tests.Services.WebApi;

[Collection(nameof(Serein))]
public partial class ApiTests : IDisposable
{
    private readonly HttpClient _client;
    private readonly IHost _app;

    public ApiTests()
    {
        _app = HostFactory.BuildNew();

        var settingProvider = _app.Services.GetRequiredService<SettingProvider>();
        settingProvider.Value.WebApi.IsEnabled = true;
        _client = new() { BaseAddress = new(settingProvider.Value.WebApi.UrlPrefixes.First()) };
        _app.Start();
    }

    public void Dispose()
    {
        _app.StopAsync();
        _app.Dispose();
        _client.Dispose();
    }

    [Theory]
    [InlineData("/api/")]
    [InlineData("/api/plugins")]
    [InlineData("/api/servers")]
    [InlineData("/api/settings")]
    [InlineData("/api/plugin-manager")]
    public async Task CanVisitSpecifiedPath(string path)
    {
        var response = await _client.GetAsync(path);

        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task CanGetHardwareInfo()
    {
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

    [Fact]
    public async Task CanGetSettings()
    {
        var response = await _client.GetAsync("/api/settings");
        Assert.True(response.IsSuccessStatusCode);
    }
}
