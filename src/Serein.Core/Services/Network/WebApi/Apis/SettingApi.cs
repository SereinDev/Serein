using System.Threading.Tasks;

using EmbedIO;
using EmbedIO.Routing;

namespace Serein.Core.Services.Network.WebApi.Apis;

public partial class ApiMap
{
    [Route(HttpVerbs.Get, "/setting")]
    public async Task GetSetting()
    {
        await HttpContext.SendPacketAsync(_settingProvider.Value);
    }
}
