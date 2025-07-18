using System.Collections.Generic;
using System.Threading.Tasks;
using EmbedIO;
using EmbedIO.Routing;
using Force.DeepCloner;
using Serein.Core.Models.Commands;
using Serein.Core.Models.Settings;

namespace Serein.Core.Services.Network.Web.Apis;

internal partial class ApiMap
{
    [Route(HttpVerbs.Get, "/settings")]
    public async Task GetSetting()
    {
        await HttpContext.SendPacketAsync(settingProvider.Value);
    }

    [Route(HttpVerbs.Put, "/settings/connection")]
    public async Task UpdateConnectionSetting()
    {
        var connectionSetting = await HttpContext.ConvertRequestAs<ConnectionSetting>();
        connectionSetting.DeepCloneTo(settingProvider.Value.Connection);
        settingProvider.SaveAsyncWithDebounce();

        await HttpContext.SendPacketWithEmptyDataAsync();
    }

    [Route(HttpVerbs.Put, "/settings/web-api")]
    public async Task UpdateWebApiSetting()
    {
        var webApiSetting = await HttpContext.ConvertRequestAs<WebApiSetting>();
        webApiSetting.DeepCloneTo(settingProvider.Value.WebApi);
        settingProvider.SaveAsyncWithDebounce();

        await HttpContext.SendPacketWithEmptyDataAsync();
    }

    [Route(HttpVerbs.Put, "/settings/application")]
    public async Task UpdateApplicationSetting()
    {
        var applicationSetting = await HttpContext.ConvertRequestAs<ApplicationSetting>();
        applicationSetting.DeepCloneTo(settingProvider.Value.Application);
        settingProvider.SaveAsyncWithDebounce();

        await HttpContext.SendPacketWithEmptyDataAsync();
    }

    [Route(HttpVerbs.Put, "/settings/reactions")]
    public async Task UpdateReactionSetting()
    {
        var dict = await HttpContext.ConvertRequestAs<Dictionary<ReactionType, string[]>>();
        lock (settingProvider.Value.Reactions)
        {
            settingProvider.Value.Reactions.Clear();

            foreach (var (key, value) in Setting.DefaultReactions)
            {
                settingProvider.Value.Reactions[key] = dict.TryGetValue(key, out var v) ? v : value;
            }
        }
        settingProvider.SaveAsyncWithDebounce();

        await HttpContext.SendPacketWithEmptyDataAsync();
    }
}
