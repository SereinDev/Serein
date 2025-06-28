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
        var body = new { Id = "test", Configuration = new { FileName = "cmd" } };
        var response = await _client.PostAsync(
            "/api/servers",
            new StringContent(
                JsonSerializer.Serialize(body, JsonSerializerOptionsFactory.Common),
                mediaType: new("application/json")
            )
        );

        Assert.True(response.IsSuccessStatusCode);
        Assert.NotEmpty(await response.Content.ReadAsStringAsync());
        Assert.NotEmpty(_app.Services.GetRequiredService<ServerManager>().Servers);
    }

    [Fact]
    public async Task ShouldBeAbleToRemoveServer()
    {
        var serverManager = _app.Services.GetRequiredService<ServerManager>();
        serverManager.Add("1234", new());
        var response = await _client.DeleteAsync("/api/servers/1234");

        Assert.True(response.IsSuccessStatusCode);
        Assert.Empty(await response.Content.ReadAsStringAsync());
        Assert.Empty(serverManager.Servers);
    }
}
