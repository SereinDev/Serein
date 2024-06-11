using System.Threading.Tasks;

using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core.Services.Data;
using Serein.Core.Services.Servers;

namespace Serein.Core.Services.Networks.WebApi.Apis;

public partial class ApiMap : WebApiController
{
    private readonly IHost _host;
    private readonly ServerManager _serverManager;
    private readonly SettingProvider _settingProvider;

    public ApiMap(IHost host)
    {
        _host = host;
        var services = host.Services;
        _settingProvider = services.GetRequiredService<SettingProvider>();
        _serverManager = services.GetRequiredService<ServerManager>();
    }

    [Route(HttpVerbs.Get, "/docs")]
    public void RedirectToDocs()
    {
        HttpContext.Redirect("https://sereindev.github.io/");
    }

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
