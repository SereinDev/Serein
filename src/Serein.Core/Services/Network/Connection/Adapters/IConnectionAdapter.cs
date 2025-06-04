using System;
using System.Threading.Tasks;
using Serein.Core.Models.Network.Connection;
using WebSocket4Net;

namespace Serein.Core.Services.Network.Connection.Adapters;

public interface IConnectionAdapter : IDisposable
{
    AdapterType Type { get; }

    event EventHandler<MessageReceivedEventArgs>? DataReceived;

    event EventHandler? StatusChanged;

    bool IsActive { get; }

    Task SendMessageAsync(TargetType type, string target, string text);

    Task SendAsync(string payload);

    void Start();

    void Stop();
}
