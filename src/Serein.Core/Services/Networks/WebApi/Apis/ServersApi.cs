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

    [Route(HttpVerbs.Get, "/servers/{:id}/start")]
    public void StartServer(string id)
    {
        if (!_serverManager.Servers.TryGetValue(id, out var server))
            HttpException.NotFound("未找到指定的服务器");

        server!.Start();
    }

    [Route(HttpVerbs.Get, "/servers/{:id}/stop")]
    public void StopServer(string id)
    {
        if (!_serverManager.Servers.TryGetValue(id, out var server))
            HttpException.NotFound("未找到指定的服务器");

        server!.Stop();
    }

    [Route(HttpVerbs.Get, "/servers/{:id}/terminate")]
    public void TerminateServer(string id)
    {
        if (!_serverManager.Servers.TryGetValue(id, out var server))
            HttpException.NotFound("未找到指定的服务器");

        server!.Terminate();
    }
}
