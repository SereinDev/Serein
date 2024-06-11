using System;
using System.Threading;
using System.Threading.Tasks;

using WebSocket4Net;

namespace Serein.Core.Services.Networks.Connection;

public interface IConnectionService : IDisposable
{
    event EventHandler<MessageReceivedEventArgs>? MessageReceived;
    event EventHandler? StatusChanged;

    bool Active { get; }

    Task SendAsync(string text);

    void Start(CancellationToken token);

    void Stop();
}
