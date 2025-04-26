using System.Linq;
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
        await HttpContext.SendPacketAsync(matchesProvider.Value);
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
        matchesProvider.Value.Add(match);
        matchesProvider.SaveAsyncWithDebounce();

        await HttpContext.SendPacketAsync(match.GetHashCode());
    }

    [Route(HttpVerbs.Delete, "/matches/{id}")]
    public async Task DeleteMatch(int id)
    {
        var march = FastGetMatch(id);
        matchesProvider.Value.Remove(march);
        matchesProvider.SaveAsyncWithDebounce();
        await HttpContext.SendPacketAsync();
    }

    [Route(HttpVerbs.Put, "/matches/{id}")]
    public async Task UpdateMatch(int id)
    {
        var match = await HttpContext.ConvertRequestAs<Match>();
        var oldMatch = FastGetMatch(id);
        match.DeepCloneTo(oldMatch);
        matchesProvider.SaveAsyncWithDebounce();
        await HttpContext.SendPacketAsync(oldMatch.GetHashCode());
    }

    private Match FastGetMatch(int id)
    {
        var fisrt = matchesProvider.Value.FirstOrDefault((m) => m.GetHashCode() == id);
        return fisrt is not null ? fisrt : throw HttpException.NotFound("未找到指定的匹配");
    }
}
