using EmbedIO;
using EmbedIO.Routing;

namespace Serein.Core.Services.Networks.WebApi.Apis;

public partial class ApiMap
{
    [Route(HttpVerbs.Get, "/servers")]
    public void GetServers()
    {
        HttpContext.SendPacketAsync(_serverManager.Servers);
    }

    [Route(HttpVerbs.Get, "/servers/{:id}")]
    public void GetServer(string id)
    {
        if (!_serverManager.Servers.TryGetValue(id, out var server))
            HttpException.NotFound("未找到指定的服务器");

        HttpContext.SendPacketAsync(server);
    }
}
