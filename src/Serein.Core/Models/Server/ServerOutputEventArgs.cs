using System;

namespace Serein.Core.Models.Server;

public sealed class ServerOutputEventArgs(ServerOutputType outputType, string data) : EventArgs
{
    public ServerOutputType OutputType { get; init; } = outputType;

    public string Data { get; init; } = data;
}
