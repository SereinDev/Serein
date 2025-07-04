using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Serein.Core.Services.Servers;
using Serein.Core.Utils.Json;
using Xunit;

namespace Serein.Tests.Services.WebApi;

public partial class ApiTests
{
    [Fact]
    public async Task ShouldBeAbleToAddServer()
    {
        var body = new
        {
            Id = "test",
            Configuration = new
            {
                FileName = Environment.OSVersion.Platform == PlatformID.Win32NT ? "cmd" : "sh",
            },
        };
        var response = await _client.PostAsync(
            "/api/servers",
            new StringContent(
                JsonSerializer.Serialize(body, JsonSerializerOptionsFactory.Common),
                mediaType: new("application/json")
            )
        );

        Assert.True(response.IsSuccessStatusCode);
        Assert.NotEmpty(_app.Services.GetRequiredService<ServerManager>().Servers);
    }

    [Fact]
    public async Task ShouldBeAbleToRemoveServer()
    {
        var serverManager = _app.Services.GetRequiredService<ServerManager>();
        var response = await _client.DeleteAsync("/api/servers/myserver");

        Assert.True(response.IsSuccessStatusCode);
        Assert.Empty(serverManager.Servers);
    }
}
