using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Serein.Core.Models.Server;

namespace Serein.Core.Services.Servers;

public sealed class ServerLogger
{
    public string Id { get; }

    public ServerLogger(string id)
    {
        Id = id;
    }

    public event EventHandler<ServerOutputEventArgs>? Output;

    [JsonIgnore]
    public IReadOnlyCollection<ServerOutputEventArgs> History => _history;

    private readonly Queue<ServerOutputEventArgs> _history = new(100);

    public void WriteStandardOutput(string output)
    {
        var e = new ServerOutputEventArgs(ServerOutputType.StandardOutput, output);
        AddToHistory(e);
        Output?.Invoke(this, e);
    }

    public void WriteStandardInput(string input)
    {
        var e = new ServerOutputEventArgs(ServerOutputType.StandardInput, input);
        AddToHistory(e);
        Output?.Invoke(this, e);
    }

    public void WriteInternalError(string error)
    {
        var e = new ServerOutputEventArgs(ServerOutputType.InternalError, error);
        AddToHistory(e);
        Output?.Invoke(this, new(ServerOutputType.InternalError, error));
    }

    public void WriteInternalInfo(string message)
    {
        var e = new ServerOutputEventArgs(ServerOutputType.InternalInfo, message);
        AddToHistory(e);
        Output?.Invoke(this, e);
    }

    private void AddToHistory(ServerOutputEventArgs e)
    {
        lock (_history)
        {
            while (_history.Count >= 100)
            {
                _history.Dequeue();
            }

            _history.Enqueue(e);
        }
    }

    public void ClearHistory()
    {
        lock (_history)
        {
            _history.Clear();
        }
    }
}
