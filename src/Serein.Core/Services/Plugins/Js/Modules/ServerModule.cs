using Serein.Core.Models.Server;
using Serein.Core.Services.Servers;

namespace Serein.Core.Services.Plugins.Js.Modules;

public class ServerModule(ServerManager servers)
{
    private readonly ServerManager _serverManager = servers;

    public Server this[string id] => _serverManager.Servers[id];

    public void Add(string id, Configuration configuration) =>
        _serverManager.Add(id, configuration);

    public bool Remove(string id) => _serverManager.Remove(id);

    public bool AnyRunning => _serverManager.AnyRunning;
}
