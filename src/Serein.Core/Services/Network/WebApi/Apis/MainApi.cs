using System.Threading.Tasks;

using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core.Services.Data;
using Serein.Core.Services.Servers;

namespace Serein.Core.Services.Network.WebApi.Apis;

public partial class ApiMap(ServerManager serverManager, SettingProvider settingProvider) : WebApiController
{
    private readonly ServerManager _serverManager = serverManager;
    private readonly SettingProvider _settingProvider = settingProvider;

    [Route(HttpVerbs.Get, "/")]
    public async Task Root()
    {
        await HttpContext.SendPacketAsync(":)");
    }

    [Route(HttpVerbs.Get, "/version")]
    public async Task GetVersion()
    {
        await HttpContext.SendPacketAsync(SereinApp.Version);
    }
}
