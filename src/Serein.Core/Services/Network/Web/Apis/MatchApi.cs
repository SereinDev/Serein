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
        var fisrt = matchesProvider.Value.FirstOrDefault((m) => m.GetHashCode() == id);
        if (fisrt is not null)
        {
            matchesProvider.Value.Remove(fisrt);
            matchesProvider.SaveAsyncWithDebounce();
            await HttpContext.SendPacketAsync();
        }
        else
        {
            throw HttpException.NotFound("未找到指定的匹配");
        }
    }

    [Route(HttpVerbs.Put, "/matches/{id}")]
    public async Task UpdateMatch(int id)
    {
        var match = await HttpContext.ConvertRequestAs<Match>();
        var fisrt = matchesProvider.Value.FirstOrDefault((m) => m.GetHashCode() == id);
        if (fisrt is not null)
        {
            match.DeepCloneTo(fisrt);

            matchesProvider.SaveAsyncWithDebounce();
            await HttpContext.SendPacketAsync(fisrt.GetHashCode());
        }
        else
        {
            throw HttpException.NotFound("未找到指定的匹配");
        }
    }
}
