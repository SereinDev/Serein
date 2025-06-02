using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serein.Core.Services.Data;
using Xunit;

namespace Serein.Tests.Services.WebApi;

[Collection(nameof(Serein))]
public class AuthorizationTests : IDisposable
{
    private readonly HttpClient _client;
    private readonly IHost _app;

    public AuthorizationTests()
    {
        _app = HostFactory.BuildNew();

        var settingProvider = _app.Services.GetRequiredService<SettingProvider>();
        settingProvider.Value.WebApi.IsEnabled = true;
        settingProvider.Value.WebApi.AccessTokens = ["123456"];
        _client = new() { BaseAddress = new(settingProvider.Value.WebApi.UrlPrefixes.First()) };
        _app.Start();
    }

    public void Dispose()
    {
        _app.StopAsync();
        _app.Dispose();
    }

    [Theory]
    [InlineData("/api/")]
    [InlineData("/api/metadata")]
    [InlineData("/api/connection")]
    [InlineData("/api/servers")]
    [InlineData("/api/settings")]
    public async Task ShouldGet403WithoutAuthorization(string path)
    {
        var response = await _client.GetAsync(path);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

        _client.DefaultRequestHeaders.Authorization = new("Bearer", "wrongtoken");
        response = await _client.GetAsync(path);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task ShouldNotGet403WhenRequestingRoot()
    {
        var response = await _client.GetAsync("/");
        Assert.True(response.IsSuccessStatusCode);
    }

    [Theory]
    [InlineData("/api/")]
    [InlineData("/api/metadata")]
    [InlineData("/api/connection")]
    [InlineData("/api/servers")]
    [InlineData("/api/settings")]
    public async Task ShouldNotGet403WithAuthorization(string path)
    {
        _client.DefaultRequestHeaders.Authorization = new("Bearer", "123456");

        var response = await _client.GetAsync(path);
        Assert.True(response.IsSuccessStatusCode);
    }
}
