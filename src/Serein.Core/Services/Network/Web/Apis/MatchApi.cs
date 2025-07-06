using System.Linq;
using System.Net;
using System.Threading.Tasks;
using EmbedIO;
using EmbedIO.Routing;
using Force.DeepCloner;
using Serein.Core.Models.Commands;

namespace Serein.Core.Services.Network.Web.Apis;

internal partial class ApiMap
{
    [Route(HttpVerbs.Get, "/matches")]
    public async Task GetMatches()
    {
        await HttpContext.SendPacketAsync(matchProvider.Value);
    }

    [Route(HttpVerbs.Get, "/matches/{id}")]
    public async Task GetMatch(int id)
    {
        await HttpContext.SendPacketAsync(FastGetMatch(id));
    }

    [Route(HttpVerbs.Post, "/matches")]
    public async Task AddMatch()
    {
        var match = await HttpContext.ConvertRequestAs<Match>();
        matchProvider.Value.Add(match);
        matchProvider.SaveAsyncWithDebounce();

        await HttpContext.SendPacketAsync(match, HttpStatusCode.Created);
    }

    [Route(HttpVerbs.Delete, "/matches/{id}")]
    public async Task DeleteMatch(int id)
    {
        var match = FastGetMatch(id);
        matchProvider.Value.Remove(match);
        matchProvider.SaveAsyncWithDebounce();
        await HttpContext.SendPacketWithEmptyDataAsync(HttpStatusCode.NoContent);
    }

    [Route(HttpVerbs.Put, "/matches/{id}")]
    public async Task UpdateMatch(int id)
    {
        var match = await HttpContext.ConvertRequestAs<Match>();
        var oldMatch = FastGetMatch(id);
        match.DeepCloneTo(oldMatch);
        oldMatch.ForceUpdate();
        matchProvider.SaveAsyncWithDebounce();
        await HttpContext.SendPacketAsync(oldMatch);
    }

    private Match FastGetMatch(int id)
    {
        var fisrt = matchProvider.Value.FirstOrDefault((m) => m.GetHashCode() == id);
        return fisrt is not null ? fisrt : throw HttpException.NotFound("未找到指定的匹配");
    }
}
