using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Serein.Core.Models.Commands;
using Serein.Core.Services.Data;
using Serein.Core.Utils.Json;
using Xunit;

namespace Serein.Tests.Services.WebApi;

public partial class ApiTests
{
    [Fact]
    public async Task ShouldBeAbleToGetMatches()
    {
        var response = await _client.GetAsync("/api/matches");
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task ShouldBeAbleToGetMatch()
    {
        var match = new Match();
        _app.Services.GetRequiredService<MatchProvider>().Value.Add(match);

        var response = await _client.GetAsync("/api/matches/" + match.GetHashCode());
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task ShouldBeAbleToAddMatch()
    {
        var response = await _client.PostAsync(
            "/api/matches",
            new StringContent(
                JsonSerializer.Serialize(new Match(), JsonSerializerOptionsFactory.Common),
                mediaType: new("application/json")
            )
        );
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task ShouldBeAbleToRemoveMatch()
    {
        var match = new Match();
        _app.Services.GetRequiredService<MatchProvider>().Value.Add(match);

        var response = await _client.DeleteAsync("/api/matches/" + match.GetHashCode());
        Assert.True(response.IsSuccessStatusCode);
        Assert.Empty(_app.Services.GetRequiredService<MatchProvider>().Value);
    }
}
