using Serein.Core.Services.Servers;
using Serein.Core.Utils;

namespace Serein.Core.Models.Plugins.Js.Modules;

public class ServerModule(ServerDictionary servers)
{
    private readonly ServerDictionary _servers = servers;

    public Services.Servers.Server? this[string id] =>
        _servers.TryGetValue(id, out var server) ? server : null;
}
