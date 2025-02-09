using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using Serein.Core.Models.Network.Web;
using Serein.Core.Services.Commands;
using Serein.Core.Services.Data;
using Serein.Core.Services.Network.Connection;
using Serein.Core.Services.Plugins;
using Serein.Core.Services.Plugins.Js;
using Serein.Core.Services.Plugins.Net;
using Serein.Core.Services.Servers;

namespace Serein.Core.Services.Network.Web.Apis;

internal partial class ApiMap(
    SereinApp sereinApp,
    PluginManager pluginManager,
    ServerManager serverManager,
    JsPluginLoader jsPluginLoader,
    NetPluginLoader netPluginLoader,
    SettingProvider settingProvider,
    MatchesProvider matchesProvider,
    ScheduleProvider scheduleProvider,
    WsConnectionManager wsConnectionManager,
    HardwareInfoProvider hardwareInfoProvider
) : WebApiController
{
    private List<ApiEndpointRecord>? _records;

    [Route(HttpVerbs.Get, "/metadata")]
    public async Task GetMetaData()
    {
        await HttpContext.SendPacketAsync(sereinApp);
    }

    [Route(HttpVerbs.Get, "/")]
    public async Task GetUrlInfo()
    {
        if (_records is null)
        {
            GenerateUrlInfos();
        }

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
