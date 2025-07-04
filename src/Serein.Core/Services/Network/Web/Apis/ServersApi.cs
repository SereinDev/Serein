using System;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using Force.DeepCloner;
using Serein.Core.Models.Network.Web;
using Serein.Core.Models.Server;
using Serein.Core.Services.Servers;
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

        await HttpContext.SendPacketAsync(
            serverManager.Add(id, configuration),
            HttpStatusCode.Created
        );
    }

    [Route(HttpVerbs.Put, "/servers/{id}")]
    public async Task UpdateServer(string id)
    {
        var server = FastGetServer(id);

        var jsonObject = await HttpContext.ConvertRequestAs<JsonObject>();
        var configuration =
            JsonSerializer.Deserialize<Configuration>(
                jsonObject?["configuration"],
                JsonSerializerOptionsFactory.Common
            ) ?? throw HttpException.BadRequest("请求中未包含有效的服务器配置");

        configuration.DeepCloneTo(server.Configuration);

        await HttpContext.SendPacketAsync(server);
    }

    [Route(HttpVerbs.Delete, "/servers/{id}")]
    public async Task RemoveServer(string id)
    {
        if (serverManager.Remove(id))
        {
            await HttpContext.SendPacketWithEmptyDataAsync(HttpStatusCode.NoContent);
        }
        else
        {
            throw HttpException.NotFound("未找到指定的服务器");
        }
    }

    [Route(HttpVerbs.Get, "/servers/{id}")]
    public async Task GetServer(string id)
    {
        var server = FastGetServer(id);

        await HttpContext.SendPacketAsync(server);
    }

    [Route(HttpVerbs.Get, "/servers/{id}/history")]
    public async Task GetServerConsoleHistory(string id)
    {
        var server = FastGetServer(id);

        await HttpContext.SendPacketAsync(
            server.Logger.History.Select(
                (line) =>
                    new BroadcastPacket(
                        line.Type switch
                        {
                            ServerOutputType.StandardOutput => BroadcastTypes.Output,
                            ServerOutputType.StandardInput => BroadcastTypes.Input,
                            ServerOutputType.InternalError => BroadcastTypes.Error,
                            ServerOutputType.InternalInfo => BroadcastTypes.Info,
                            _ => throw new NotSupportedException(),
                        },
                        line.Data
                    )
            )
        );
    }

    [Route(HttpVerbs.Post, "/servers/{id}/start")]
    public async Task StartServer(string id)
    {
        var server = FastGetServer(id);

        server.Start();

        if (!server.Configuration.Pty.IsEnabled)
        {
            await HttpContext.SendPacketWithEmptyDataAsync(HttpStatusCode.OK);
        }
        else
        {
            await HttpContext.SendPacketWithEmptyDataAsync(HttpStatusCode.Accepted);
        }
    }

    [Route(HttpVerbs.Post, "/servers/{id}/stop")]
    public async Task StopServer(string id)
    {
        var server = FastGetServer(id);

        server.Stop();
        await HttpContext.SendPacketWithEmptyDataAsync(HttpStatusCode.Accepted);
    }

    [Route(HttpVerbs.Post, "/servers/{id}/terminate")]
    public async Task TerminateServer(string id)
    {
        var server = FastGetServer(id);

        server.Terminate();
        await HttpContext.SendPacketWithEmptyDataAsync(HttpStatusCode.Accepted);
    }

    [Route(HttpVerbs.Post, "/servers/{id}/input")]
    public async Task InputServer(string id)
    {
        var server = FastGetServer(id);

        if (!server.Status)
        {
            throw HttpException.Forbidden("服务器未运行");
        }

        foreach (
            var l in await HttpContext.ConvertRequestAs<string[]>()
                ?? throw HttpException.BadRequest()
        )
        {
            server.Input(l);
        }

        HttpContext.SendPacketWithEmptyDataAsync(HttpStatusCode.Accepted);
    }

    private Server FastGetServer(string id)
    {
        return serverManager.Servers.TryGetValue(id, out var server)
            ? server
            : throw HttpException.NotFound("未找到指定的服务器");
    }
}
