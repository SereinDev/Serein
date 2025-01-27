using System.Threading.Tasks;
using EmbedIO;
using EmbedIO.Routing;

namespace Serein.Core.Services.Network.Web.Apis;

internal partial class ApiMap
{
    [Route(HttpVerbs.Get, "/setting")]
    public async Task GetSetting()
    {
        await HttpContext.SendPacketAsync(settingProvider.Value);
    }
}
