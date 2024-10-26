using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;

using Serein.Core.Models.Network.WebApi;
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
    private List<ApiEndpointRecord>? _records;

    [Route(HttpVerbs.Get, "/metadata")]
    public async Task GetMetaData()
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

    [Route(HttpVerbs.Get, "/")]
    public async Task GetUrlInfo()
    {
        if (_records is null)
            GenerateUrlInfos();

        await HttpContext.SendPacketAsync(_records);
    }

    [MemberNotNull(nameof(_records))]
    private void GenerateUrlInfos()
    {
        _records = [];
        foreach (var methodInfo in typeof(ApiMap).GetMethods())
        {
            _records.AddRange(
                methodInfo
                    .GetCustomAttributes(typeof(RouteAttribute), true)
                    .OfType<RouteAttribute>()
                    .Select(
                        (attribute) =>
                            new ApiEndpointRecord(
                                attribute.Route,
                                attribute.Verb.ToString().ToUpperInvariant()
                            )
                    )
            );
        }
    }
}
