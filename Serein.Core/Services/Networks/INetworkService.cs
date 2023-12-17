using System;
using System.Threading.Tasks;

using Serein.Core.Models.Events;

using WatsonWebsocket;

namespace Serein.Core.Services.Networks;

public interface INetworkService : IDisposable
{
    event EventHandler? Opened;

    event EventHandler? Closed;

    event EventHandler<WsMessageReceivedEventArgs>? WsMessageReceived;

    bool Active { get; }

    Statistics? Stats { get; }

    Task SendAsync(string text);

    Task SendAsync<T>(T data);

    Task StartAsync();

    Task StopAsync();
}
