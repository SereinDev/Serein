using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Serein.Core.Models.Commands;
using Serein.Core.Utils.Json;
using Xunit;

namespace Serein.Tests.Services.WebApi;

public partial class ApiTests
{
    [Fact]
    public async Task ShouldBeAbleToAddSchedule()
    {
        var response = await _client.PostAsync(
            "/api/schedules",
            new StringContent(
                JsonSerializer.Serialize(new Schedule(), JsonSerializerOptionsFactory.Common),
                mediaType: new("application/json")
            )
        );
        Assert.True(response.IsSuccessStatusCode);
    }
}
