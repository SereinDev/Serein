using System;

namespace Serein.Core.Models.Server;

public sealed class ServerOutputEventArgs(ServerOutputType outputType, string data) : EventArgs
{
    public ServerOutputType OutputType { get; } = outputType;

    public string Data { get; } = data;
}
