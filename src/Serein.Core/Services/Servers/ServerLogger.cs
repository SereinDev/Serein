using System;
using System.Collections.Generic;
using Serein.Core.Models.Server;

namespace Serein.Core.Services.Servers;

public sealed class ServerLogger
{
    public string Id { get; }

    internal ServerLogger(string id)
    {
        Id = id;
    }

    public event EventHandler<ServerOutputEventArgs>? Output;

    public IReadOnlyCollection<ServerOutputEventArgs> History => _history;

    private readonly Queue<ServerOutputEventArgs> _history = new(100);

    internal void WriteStandardOutput(string output)
    {
        var e = new ServerOutputEventArgs(ServerOutputType.StandardOutput, output);
        AddToHistory(e);
        Output?.Invoke(this, e);
    }

    internal void WriteStandardInput(string input)
    {
        var e = new ServerOutputEventArgs(ServerOutputType.StandardInput, input);
        AddToHistory(e);
        Output?.Invoke(this, e);
    }

    internal void WriteInternalError(string error)
    {
        var e = new ServerOutputEventArgs(ServerOutputType.InternalError, error);
        AddToHistory(e);
        Output?.Invoke(this, new(ServerOutputType.InternalError, error));
    }

    internal void WriteInternalInfo(string message)
    {
        var e = new ServerOutputEventArgs(ServerOutputType.InternalInfo, message);
        AddToHistory(e);
        Output?.Invoke(this, e);
    }

    private void AddToHistory(ServerOutputEventArgs e)
    {
        lock (_history)
        {
            if (_history.Count >= 100)
            {
                _history.Dequeue();
            }

            _history.Enqueue(e);
        }
    }

    internal void ClearHistory()
    {
        lock (_history)
        {
            _history.Clear();
        }
    }
}
