using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;

using Serein.Core.Models.Server;
using Serein.Core.Utils.Json;

namespace Serein.Core.Services.Network.WebApi.Apis;

public partial class ApiMap
{
    [Route(HttpVerbs.Get, "/servers")]
    public async Task GetServers()
    {
        await HttpContext.SendPacketAsync(_serverManager.Servers);
    }

    [Route(HttpVerbs.Post, "/servers/{id}")]
    public async Task AddServer(string id)
    {
        var jsonObject = await HttpContext.ConvertRequestAs<JsonObject>();
        var configuration =
            JsonSerializer.Deserialize<Configuration>(
                jsonObject?["configuration"],
                JsonSerializerOptionsFactory.CamelCase
            ) ?? throw HttpException.BadRequest();

        _serverManager.Add(id, configuration);
        await HttpContext.SendPacketAsync<object>(null);
    }

    [Route(HttpVerbs.Delete, "/servers/{id}")]
    public async Task RemoveServer(string id)
    {
        _serverManager.Remove(id);
        await HttpContext.SendPacketAsync<object>(null);
    }

    [Route(HttpVerbs.Get, "/servers/{id}")]
    public async Task GetServer(string id)
    {
        if (!_serverManager.Servers.TryGetValue(id, out var server))
            throw HttpException.NotFound("未找到指定的服务器");

        await HttpContext.SendPacketAsync(server);
    }

    [Route(HttpVerbs.Get, "/servers/{id}/start")]
    public async Task StartServer(string id)
    {
        if (!_serverManager.Servers.TryGetValue(id, out var server))
            throw HttpException.NotFound("未找到指定的服务器");

        server.Start();
        await HttpContext.SendPacketAsync<object>(null);
    }

    [Route(HttpVerbs.Get, "/servers/{id}/stop")]
    public async Task StopServer(string id)
    {
        if (!_serverManager.Servers.TryGetValue(id, out var server))
            throw HttpException.NotFound("未找到指定的服务器");

        server.Stop();
        await HttpContext.SendPacketAsync<object>(null, HttpStatusCode.Accepted);
    }

    [Route(HttpVerbs.Get, "/servers/{id}/terminate")]
    public async Task TerminateServer(string id)
    {
        if (!_serverManager.Servers.TryGetValue(id, out var server))
            throw HttpException.NotFound("未找到指定的服务器");

        server.Terminate();
        await HttpContext.SendPacketAsync<object>(null);
    }

    [Route(HttpVerbs.Get, "/servers/{id}/input")]
    public async Task InputServer(string id, [QueryField("line", true)] string[] lines)
    {
        if (!_serverManager.Servers.TryGetValue(id, out var server))
            throw HttpException.NotFound("未找到指定的服务器");

        if (server.Status != ServerStatus.Running)
            throw HttpException.Forbidden("服务器未运行");

        foreach (var l in lines)
            server.Input(l);

        await HttpContext.SendPacketAsync<object>(null);
    }

    [Route(HttpVerbs.Post, "/servers/{id}/input")]
    public async Task InputServer(string id)
    {
        if (!_serverManager.Servers.TryGetValue(id, out var server))
            throw HttpException.NotFound("未找到指定的服务器");

        if (server.Status != ServerStatus.Running)
            throw HttpException.Forbidden("服务器未运行");

        foreach (
            var l in await HttpContext.ConvertRequestAs<string[]>()
                ?? throw HttpException.BadRequest()
        )
            server.Input(l);

        await HttpContext.SendPacketAsync<object>(null);
    }
}
