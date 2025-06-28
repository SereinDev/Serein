using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using EmbedIO;
using EmbedIO.Routing;
using Serein.Core.Models.Network.Web;
using Serein.Core.Models.Plugins;
using Serein.Core.Services.Plugins.Js;
using Serein.Core.Services.Plugins.Net;

namespace Serein.Core.Services.Network.Web.Apis;

internal partial class ApiMap
{
    [Route(HttpVerbs.Get, "/plugin-manager")]
    public async Task GetPluginManager()
    {
        await HttpContext.SendPacketAsync(
            new PluginManagerStatus
            {
                IsLoading = pluginManager.IsLoading,
                IsReloading = pluginManager.IsReloading,
                CommandVariables = pluginManager.CommandVariables,
            }
        );
    }

    [Route(HttpVerbs.Post, "/plugin-manager/reload")]
    public async Task ReloadPlugins()
    {
        Task.Run(pluginManager.Reload);
        await HttpContext.SendPacketAsync(HttpStatusCode.Accepted);
    }

    [Route(HttpVerbs.Get, "/plugins")]
    public async Task GetPlugins()
    {
        var dictionary = new Dictionary<string, IPlugin>();
        foreach (var kv in netPluginLoader.Plugins)
        {
            dictionary.Add(kv.Key, kv.Value);
        }
        foreach (var kv in jsPluginLoader.Plugins)
        {
            dictionary.Add(kv.Key, kv.Value);
        }

        await HttpContext.SendPacketAsync(dictionary);
    }

    [Route(HttpVerbs.Get, "/plugins/{id}")]
    public async Task GetPlugin(string id)
    {
        await HttpContext.SendPacketAsync(FastGetPlugin(id));
    }

    [Route(HttpVerbs.Post, "/plugins/{id}/disable")]
    public async Task DisablePlugin(string id)
    {
        var plugin = FastGetPlugin(id);

        if (!plugin.IsEnabled)
        {
            throw HttpException.BadRequest($"插件（Id={id}）已被禁用");
        }

        Task.Run(plugin.Disable);
        await HttpContext.SendPacketAsync(HttpStatusCode.Accepted);
    }

    private IPlugin FastGetPlugin(string id)
    {
        return netPluginLoader.Plugins.TryGetValue(id, out PluginBase? netPlugin) ? netPlugin
            : jsPluginLoader.Plugins.TryGetValue(id, out JsPlugin? jsPlugin) ? jsPlugin
            : throw HttpException.NotFound($"未找到插件（Id={id}）");
    }
}
