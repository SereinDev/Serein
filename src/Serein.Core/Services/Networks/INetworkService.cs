using System;
using System.Threading;
using System.Threading.Tasks;

using WatsonWebsocket;

namespace Serein.Core.Services.Networks;

public interface INetworkService : IDisposable
{
    event EventHandler<MessageReceivedEventArgs>? MessageReceived;
    event EventHandler? StatusChanged;

    bool Active { get; }

    Statistics? Stats { get; }

    Task SendAsync(string text);

    void Start(CancellationToken token);

    void Stop();
}
