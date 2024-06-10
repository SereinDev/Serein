using System;

namespace Serein.Core.Services.Servers;

public class ServerOutputEventArgs(ServerOutputType outputType, string? data = null) : EventArgs
{
    public ServerOutputType OutputType { get; init; } = outputType;
    public string? Data { get; init; } = data;
}
