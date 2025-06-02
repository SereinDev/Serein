using System.Threading.Tasks;
using Xunit;

namespace Serein.Tests.Services.WebApi;

public partial class ApiTests
{
    [Fact]
    public async Task ShouldBeAbleToGetPlugins()
    {
        var response = await _client.GetAsync("/api/plugins");
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task ShouldBeAbleToGetPluginManager()
    {
        var response = await _client.GetAsync("/api/plugin-manager");
        Assert.True(response.IsSuccessStatusCode);
    }
}
