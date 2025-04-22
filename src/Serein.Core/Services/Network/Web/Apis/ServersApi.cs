using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using Force.DeepCloner;
using Serein.Core.Models.Server;
using Serein.Core.Utils.Json;

namespace Serein.Core.Services.Network.Web.Apis;

internal partial class ApiMap
{
    [Route(HttpVerbs.Get, "/servers")]
    public async Task GetServers()
    {
        await HttpContext.SendPacketAsync(serverManager.Servers);
    }

    [Route(HttpVerbs.Post, "/servers")]
    public async Task AddServer()
    {
        var jsonObject = await HttpContext.ConvertRequestAs<JsonObject>();

        var id = jsonObject?["id"]?.GetValue<string>();
        if (string.IsNullOrEmpty(id))
        {
            throw HttpException.BadRequest("请求中未包含有效的服务器 Id");
        }

        var configuration =
            JsonSerializer.Deserialize<Configuration>(
                jsonObject?["configuration"],
                JsonSerializerOptionsFactory.Common
            ) ?? throw HttpException.BadRequest("请求中未包含有效的服务器配置");

        serverManager.Add(id, configuration);

        await HttpContext.SendPacketAsync(HttpStatusCode.Created);
    }

    [Route(HttpVerbs.Put, "/servers/{id}")]
    public async Task UpdateServer(string id)
    {
        var jsonObject = await HttpContext.ConvertRequestAs<JsonObject>();
        var configuration =
            JsonSerializer.Deserialize<Configuration>(
                jsonObject?["configuration"],
                JsonSerializerOptionsFactory.Common
            ) ?? throw HttpException.BadRequest("请求中未包含有效的服务器配置");

        if (!serverManager.Servers.TryGetValue(id, out var server))
        {
            throw HttpException.NotFound("未找到指定的服务器");
        }

        configuration.DeepCloneTo(server.Configuration);

        await HttpContext.SendPacketAsync();
    }

    [Route(HttpVerbs.Delete, "/servers/{id}")]
    public async Task RemoveServer(string id)
    {
        serverManager.Remove(id);
        await HttpContext.SendPacketAsync(HttpStatusCode.NoContent);
    }

    [Route(HttpVerbs.Get, "/servers/{id}")]
    public async Task GetServer(string id)
    {
        if (!serverManager.Servers.TryGetValue(id, out var server))
        {
            throw HttpException.NotFound("未找到指定的服务器");
        }

        await HttpContext.SendPacketAsync(server);
    }

    [Route(HttpVerbs.Get, "/servers/{id}/history")]
    public async Task GetServerOutputHistory(string id)
    {
        if (!serverManager.Servers.TryGetValue(id, out var server))
        {
            throw HttpException.NotFound("未找到指定的服务器");
        }

        await HttpContext.SendPacketAsync(server.Logger.History);
    }

    [Route(HttpVerbs.Post, "/servers/{id}/start")]
    public async Task StartServer(string id)
    {
        if (!serverManager.Servers.TryGetValue(id, out var server))
        {
            throw HttpException.NotFound("未找到指定的服务器");
        }

        server.Start();
        await HttpContext.SendPacketAsync(HttpStatusCode.Accepted);
    }

    [Route(HttpVerbs.Post, "/servers/{id}/stop")]
    public async Task StopServer(string id)
    {
        if (!serverManager.Servers.TryGetValue(id, out var server))
        {
            throw HttpException.NotFound("未找到指定的服务器");
        }

        server.Stop();
        await HttpContext.SendPacketAsync(HttpStatusCode.Accepted);
    }

    [Route(HttpVerbs.Post, "/servers/{id}/terminate")]
    public async Task TerminateServer(string id)
    {
        if (!serverManager.Servers.TryGetValue(id, out var server))
        {
            throw HttpException.NotFound("未找到指定的服务器");
        }

        server.Terminate();
        await HttpContext.SendPacketAsync(HttpStatusCode.Accepted);
    }

    [Route(HttpVerbs.Post, "/servers/{id}/input")]
    public async Task InputServer(string id, [QueryField("line")] string[] lines)
    {
        if (!serverManager.Servers.TryGetValue(id, out var server))
        {
            throw HttpException.NotFound("未找到指定的服务器");
        }

        if (!server.Status)
        {
            throw HttpException.Forbidden("服务器未运行");
        }

        if (lines.Length == 0)
        {
            foreach (
                var l in await HttpContext.ConvertRequestAs<string[]>()
                    ?? throw HttpException.BadRequest()
            )
            {
                server.Input(l);
            }
        }
        else
        {
            foreach (var l in lines)
            {
                server.Input(l);
            }
        }

        await HttpContext.SendPacketAsync(HttpStatusCode.NoContent);
    }
}
