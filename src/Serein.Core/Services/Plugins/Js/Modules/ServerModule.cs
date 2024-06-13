using System.Collections;
using System.Collections.Generic;

using Serein.Core.Models.Server;
using Serein.Core.Services.Servers;

namespace Serein.Core.Services.Plugins.Js.Modules;

public class ServerModule(ServerManager servers) : IEnumerable<KeyValuePair<string, Server>>
{
    private readonly ServerManager _serverManager = servers;

    public Server? this[string id] =>
        _serverManager.Servers.TryGetValue(id, out var server) ? server : null;

    public void Add(string id, Configuration configuration) =>
        _serverManager.Add(id, configuration);

    public bool Remove(string id) => _serverManager.Remove(id);

    public bool AnyRunning => _serverManager.AnyRunning;

    public IEnumerator<KeyValuePair<string, Server>> GetEnumerator() =>
        _serverManager.Servers.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _serverManager.Servers.GetEnumerator();
}
