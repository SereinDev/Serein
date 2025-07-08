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
    public async Task CanGetMatches()
    {
        var response = await _client.GetAsync("/api/matches");
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task CanGetMatch()
    {
        var match = new Match();
        _app.Services.GetRequiredService<MatchProvider>().Value.Add(match);

        var response = await _client.GetAsync("/api/matches/" + match.GetHashCode());
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task CanAddMatch()
    {
        var response = await _client.PostAsync(
            "/api/matches",
            new StringContent(
                JsonSerializer.Serialize(new Match(), JsonSerializerOptionsFactory.Common),
                mediaType: new("application/json")
            )
        );
        Assert.True(response.IsSuccessStatusCode);
        Assert.NotEmpty(_app.Services.GetRequiredService<MatchProvider>().Value);
    }

    [Fact]
    public async Task CanRemoveMatch()
    {
        var match = new Match();
        _app.Services.GetRequiredService<MatchProvider>().Value.Add(match);

        var response = await _client.DeleteAsync("/api/matches/" + match.GetHashCode());

        Assert.True(response.IsSuccessStatusCode);
        Assert.Empty(_app.Services.GetRequiredService<MatchProvider>().Value);
    }
}
