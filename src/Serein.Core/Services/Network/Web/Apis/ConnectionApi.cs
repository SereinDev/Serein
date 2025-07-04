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
        await HttpContext.SendPacketAsync(connectionManager);
    }

    [Route(HttpVerbs.Post, "/connection")]
    public async Task StartConnection()
    {
        connectionManager.Start();
        await HttpContext.SendPacketWithEmptyDataAsync(HttpStatusCode.Accepted);
    }

    [Route(HttpVerbs.Delete, "/connection")]
    public async Task StopConnection()
    {
        connectionManager.Stop();
        await HttpContext.SendPacketWithEmptyDataAsync(HttpStatusCode.NoContent);
    }
}
