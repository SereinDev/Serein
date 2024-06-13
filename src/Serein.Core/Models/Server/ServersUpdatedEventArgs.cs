using System;

namespace Serein.Core.Models.Server;

public class ServersUpdatedEventArgs(string id, ServersUpdatedType type) : EventArgs
{
    public string Id { get; } = id;
    public ServersUpdatedType Type { get; } = type;
}
