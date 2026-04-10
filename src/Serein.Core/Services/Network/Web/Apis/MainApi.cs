using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using Serein.Core.Models.Network.Web;
using Serein.Core.Services.Data;
using Serein.Core.Services.Network.Connection;
using Serein.Core.Services.Plugins;
using Serein.Core.Services.Plugins.Js;
using Serein.Core.Services.Plugins.Net;
using Serein.Core.Services.Servers;

namespace Serein.Core.Services.Network.Web.Apis;

internal sealed partial class ApiMap(
    SereinApp sereinApp,
    PluginManager pluginManager,
    ServerManager serverManager,
    JsPluginLoader jsPluginLoader,
    NetPluginLoader netPluginLoader,
    SettingProvider settingProvider,
    ConnectionManager connectionManager,
    HardwareInfoProvider hardwareInfoProvider,
    AutomationTaskProvider automationTaskProvider,
    WebSocketTicketService webSocketTicketService
) : WebApiController
{
    private static readonly Lazy<List<ApiEndpointRecord>> Records = new(GenerateRouteInfos);

    [Route(HttpVerbs.Get, "/")]
    public async Task GetAppInfo()
    {
        await HttpContext.SendPacketAsync(sereinApp);
    }

    [Route(HttpVerbs.Get, "/routes")]
    public async Task GetRoutes()
    {
        await HttpContext.SendPacketAsync(Records);
    }

    private static List<ApiEndpointRecord> GenerateRouteInfos()
    {
        var records = new List<ApiEndpointRecord>();
        foreach (var methodInfo in typeof(ApiMap).GetMethods())
        {
            records.AddRange(
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

        return records;
    }
}
