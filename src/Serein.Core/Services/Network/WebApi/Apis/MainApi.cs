using System.Collections.Generic;
using System.Threading.Tasks;

using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;

using Serein.Core.Services.Commands;

using Serein.Core.Services.Data;
using Serein.Core.Services.Network.Connection;
using Serein.Core.Services.Servers;

namespace Serein.Core.Services.Network.WebApi.Apis;

public partial class ApiMap(
    HardwareInfoProvider hardwareInfoProvider,
    ServerManager serverManager,
    SettingProvider settingProvider,
    WsConnectionManager wsConnectionManager
) : WebApiController
{
    private readonly HardwareInfoProvider _hardwareInfoProvider = hardwareInfoProvider;
    private readonly ServerManager _serverManager = serverManager;
    private readonly SettingProvider _settingProvider = settingProvider;
    private readonly WsConnectionManager _wsConnectionManager = wsConnectionManager;

    [Route(HttpVerbs.Get, "/")]
    public async Task Root()
    {
        await HttpContext.SendPacketAsync(
            new Dictionary<string, string?>()
            {
                ["version"] = SereinApp.Version,
                ["fullVersion"] = SereinApp.FullVersion,
                ["type"] = SereinApp.Type.ToString(),
            }
        );
    }
}
