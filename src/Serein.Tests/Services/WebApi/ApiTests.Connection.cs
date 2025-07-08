using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Serein.Tests.Services.WebApi;

public partial class ApiTests
{
    [Fact]
    public async Task CanSwitchConnectionStatus()
    {
        var response = await _client.PostAsync("/api/connection", new StringContent(string.Empty));
        Assert.True(response.IsSuccessStatusCode);

        response = await _client.DeleteAsync("/api/connection");
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task CanGetConnectionStatus()
    {
        var response = await _client.GetAsync("/api/connection");
        Assert.True(response.IsSuccessStatusCode);
    }
}
