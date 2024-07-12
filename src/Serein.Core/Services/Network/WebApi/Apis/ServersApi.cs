using System.Net;

using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;

namespace Serein.Core.Services.Network.WebApi.Apis;

public partial class ApiMap
{
    [Route(HttpVerbs.Get, "/servers")]
    public void GetServers()
    {
        HttpContext.SendPacketAsync(_serverManager.Servers);
    }

    [Route(HttpVerbs.Get, "/servers/{id}")]
    public void GetServer(string id)
    {
        if (!_serverManager.Servers.TryGetValue(id, out var server))
            throw HttpException.NotFound("未找到指定的服务器");

        HttpContext.SendPacketAsync(server);
    }

    [Route(HttpVerbs.Get, "/servers/{id}/start")]
    public void StartServer(string id)
    {
        if (!_serverManager.Servers.TryGetValue(id, out var server))
            throw HttpException.NotFound("未找到指定的服务器");

        server.Start();
        HttpContext.SendPacketAsync<object>(null);
    }

    [Route(HttpVerbs.Get, "/servers/{id}/stop")]
    public void StopServer(string id)
    {
        if (!_serverManager.Servers.TryGetValue(id, out var server))
            throw HttpException.NotFound("未找到指定的服务器");

        server.Stop();
        HttpContext.SendPacketAsync<object>(null, HttpStatusCode.Accepted);
    }

    [Route(HttpVerbs.Get, "/servers/{id}/terminate")]
    public void TerminateServer(string id)
    {
        if (!_serverManager.Servers.TryGetValue(id, out var server))
            throw HttpException.NotFound("未找到指定的服务器");

        server.Terminate();
        HttpContext.SendPacketAsync<object>(null);
    }

    [Route(HttpVerbs.Get, "/servers/{id}/input")]
    public void InputServer(string id, [QueryField("line", true)] string[] lines)
    {
        if (!_serverManager.Servers.TryGetValue(id, out var server))
            throw HttpException.NotFound("未找到指定的服务器");

        foreach (var l in lines)
            server.Input(l);

        HttpContext.SendPacketAsync<object>(null);
    }

    [Route(HttpVerbs.Post, "/servers/{id}/input")]
    public async void InputServer(string id)
    {
        if (!_serverManager.Servers.TryGetValue(id, out var server))
            throw HttpException.NotFound("未找到指定的服务器");

        foreach (var l in await HttpContext.ConvertRequestAs<string[]>() ?? throw HttpException.BadRequest())
            server.Input(l);

        HttpContext.SendPacketAsync<object>(null);
    }
}
