using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using EmbedIO;
using EmbedIO.Routing;
using Serein.Core.Models.Network.Web;
using Serein.Core.Models.Plugins;
using Serein.Core.Models.Plugins.Js;
using Serein.Core.Models.Plugins.Net;

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
        if (netPluginLoader.Plugins.TryGetValue(id, out PluginBase? netPlugin))
        {
            await HttpContext.SendPacketAsync(netPlugin);
        }
        else if (jsPluginLoader.Plugins.TryGetValue(id, out JsPlugin? jsPlugin))
        {
            await HttpContext.SendPacketAsync(jsPlugin);
        }
        else
        {
            throw HttpException.NotFound($"未找到插件（Id={id}）");
        }
    }

    [Route(HttpVerbs.Post, "/plugins/{id}/disable")]
    public async Task DisablePlugin(string id)
    {
        if (netPluginLoader.Plugins.TryGetValue(id, out PluginBase? netPlugin))
        {
            netPlugin.Disable();
            await HttpContext.SendPacketAsync(HttpStatusCode.Accepted);
        }
        else if (jsPluginLoader.Plugins.TryGetValue(id, out JsPlugin? jsPlugin))
        {
            jsPlugin.Disable();
            await HttpContext.SendPacketAsync(HttpStatusCode.Accepted);
        }
        else
        {
            throw HttpException.NotFound($"未找到插件（Id={id}）");
        }
    }
}
