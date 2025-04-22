using System;
using System.Text.Json.Serialization;

namespace Serein.Core.Models.Server;

public sealed class ServerOutputEventArgs(ServerOutputType type, string data) : EventArgs
{
    public ServerOutputType Type { get; } = type;

    public string Data { get; } = data;
}
