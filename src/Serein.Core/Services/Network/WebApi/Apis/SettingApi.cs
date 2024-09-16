using EmbedIO;
using EmbedIO.Routing;

namespace Serein.Core.Services.Network.WebApi.Apis;

public partial class ApiMap
{
    [Route(HttpVerbs.Get, "/setting")]
    public void GetSetting()
    {
        HttpContext.SendPacketAsync(_settingProvider.Value);
    }
}
