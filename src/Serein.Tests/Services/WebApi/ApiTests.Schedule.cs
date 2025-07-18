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
    public async Task CanGetSchedules()
    {
        var response = await _client.GetAsync("/api/schedules");
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task CanGetSchedule()
    {
        var match = new Schedule();
        _app.Services.GetRequiredService<ScheduleProvider>().Value.Add(match);

        var response = await _client.GetAsync("/api/schedules/" + match.GetHashCode());
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task CanAddSchedule()
    {
        var response = await _client.PostAsync(
            "/api/schedules",
            new StringContent(
                JsonSerializer.Serialize(new Schedule(), JsonSerializerOptionsFactory.Common),
                mediaType: new("application/json")
            )
        );
        Assert.True(response.IsSuccessStatusCode);
        Assert.NotEmpty(_app.Services.GetRequiredService<ScheduleProvider>().Value);
    }

    [Fact]
    public async Task CanRemoveSchedule()
    {
        var match = new Schedule();
        _app.Services.GetRequiredService<ScheduleProvider>().Value.Add(match);

        var response = await _client.DeleteAsync("/api/schedules/" + match.GetHashCode());

        Assert.True(response.IsSuccessStatusCode);
        Assert.Empty(_app.Services.GetRequiredService<ScheduleProvider>().Value);
        Assert.Empty(await response.Content.ReadAsStringAsync());
    }
}
