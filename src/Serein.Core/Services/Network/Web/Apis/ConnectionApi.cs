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

    [Route(HttpVerbs.Post, "/connection")]
    public async Task StartConnection()
    {
        wsConnectionManager.Start();
        await HttpContext.SendPacketAsync(HttpStatusCode.Accepted);
    }

    [Route(HttpVerbs.Delete, "/connection")]
    public async Task StopConnection()
    {
        wsConnectionManager.Stop();
        await HttpContext.SendPacketAsync(HttpStatusCode.NoContent);
    }
}
