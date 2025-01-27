using System;
using Serein.Core.Services.Servers;

namespace Serein.Core.Models.Server;

public class ServersUpdatedEventArgs(
    ServersUpdatedType type,
    string id,
    Services.Servers.Server server
) : EventArgs
{
    public string Id { get; } = id;
    public ServersUpdatedType Type { get; } = type;
    public Services.Servers.Server Server { get; } = server;
}
