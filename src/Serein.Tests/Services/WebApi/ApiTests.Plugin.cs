using System.Threading.Tasks;
using Xunit;

namespace Serein.Tests.Services.WebApi;

public partial class ApiTests
{
    [Fact]
    public async Task CanGetPlugins()
    {
        var response = await _client.GetAsync("/api/plugins");
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task CanGetPluginManager()
    {
        var response = await _client.GetAsync("/api/plugin-manager");
        Assert.True(response.IsSuccessStatusCode);
    }
}
