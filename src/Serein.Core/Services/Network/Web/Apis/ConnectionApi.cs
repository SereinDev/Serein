using System.Net;
using System.Threading.Tasks;
using EmbedIO;
using EmbedIO.Routing;

namespace Serein.Core.Services.Network.Web.Apis;

internal partial class ApiMap
{
    [Route(HttpVerbs.Get, "/connection")]
    public async Task GetConnectionInfo()
    {
        await HttpContext.SendPacketAsync(wsConnectionManager);
    }

    [Route(HttpVerbs.Get, "/connection/open")]
    public async Task StartConnection()
    {
        wsConnectionManager.Start();
        await HttpContext.SendPacketAsync(HttpStatusCode.Accepted);
    }

    [Route(HttpVerbs.Get, "/connection/close")]
    public async Task StopConnection()
    {
        wsConnectionManager.Stop();
        await HttpContext.SendPacketAsync();
    }
}
