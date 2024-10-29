using System.Net;
using System.Threading.Tasks;

using EmbedIO;
using EmbedIO.Routing;

namespace Serein.Core.Services.Network.WebApi.Apis;

internal partial class ApiMap
{
    [Route(HttpVerbs.Get, "/connection")]
    public async Task GetConnectionInfo()
    {
        await HttpContext.SendPacketAsync(_wsConnectionManager);
    }

    [Route(HttpVerbs.Get, "/connection/open")]
    public async Task StartConnection()
    {
        _wsConnectionManager.Start();
        await HttpContext.SendPacketAsync(HttpStatusCode.Accepted);
    }

    [Route(HttpVerbs.Get, "/connection/close")]
    public async Task StopConnection()
    {
        _wsConnectionManager.Stop();
        await HttpContext.SendPacketAsync();
    }
}