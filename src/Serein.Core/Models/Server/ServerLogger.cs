using System;

namespace Serein.Core.Models.Server;

public sealed class ServerLogger
{
    public string Id { get; }

    internal ServerLogger(string id)
    {
        Id = id;
    }

    public event EventHandler<ServerOutputEventArgs>? Output;

    internal void WriteStandardOutput(string output)
    {
        Output?.Invoke(this, new(ServerOutputType.StandardOutput, output));
    }

    internal void WriteStandardInput(string input)
    {
        Output?.Invoke(this, new(ServerOutputType.StandardInput, input));
    }

    internal void WriteInternalError(string error)
    {
        Output?.Invoke(this, new(ServerOutputType.InternalError, error));
    }

    internal void WriteInternalInfo(string message)
    {
        Output?.Invoke(this, new(ServerOutputType.InternalInfo, message));
    }
}
