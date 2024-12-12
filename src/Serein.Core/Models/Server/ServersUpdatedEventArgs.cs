using System;
using Serein.Core.Services.Servers;

namespace Serein.Core.Models.Server;

public class ServersUpdatedEventArgs(ServersUpdatedType type, string id, ServerBase server)
    : EventArgs
{
    public string Id { get; } = id;
    public ServersUpdatedType Type { get; } = type;
    public ServerBase Server { get; } = server;
}
