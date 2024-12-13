using System.Linq;
using Serein.Core.Models.Server;
using Serein.Core.Services.Servers;

namespace Serein.Core.Services.Plugins.Js.Properties;

public sealed class ServerProperty
{
    private readonly ServerManager _serverManager;

    internal ServerProperty(ServerManager servers)
    {
        _serverManager = servers;
    }

    public ServerBase this[string id] => _serverManager.Servers[id];

    public string[] Ids => _serverManager.Servers.Keys.ToArray();

    public ServerBase Add(string id, Configuration configuration) =>
        _serverManager.Add(id, configuration);

    public bool Remove(string id) => _serverManager.Remove(id);

    public bool AnyRunning => _serverManager.AnyRunning;
}
