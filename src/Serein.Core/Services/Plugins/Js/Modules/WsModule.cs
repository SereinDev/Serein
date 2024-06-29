using System;

using Serein.Core.Services.Network.Connection;
using Serein.Core.Utils.Extensions;

namespace Serein.Core.Services.Plugins.Js.Modules;

public class WsModule(WsConnectionManager wsNetwork)
{
    private readonly WsConnectionManager _wsNetwork = wsNetwork;

    public bool Active => _wsNetwork.Active;

    public void Start()
    {
        _wsNetwork.Start();
    }

    public void Stop()
    {
        _wsNetwork.Stop();
    }

    public void SendData(string text)
    {
        ArgumentNullException.ThrowIfNull(text, nameof(text));

        _wsNetwork.SendTextAsync(text).Await();
    }

    public void SendGroupMsg(string target, string message)
    {
        ArgumentNullException.ThrowIfNull(target, nameof(target));
        ArgumentNullException.ThrowIfNull(message, nameof(message));

        _wsNetwork.SendGroupMsgAsync(target, message).Await();
    }

    public void SendPrivateMsg(string target, string message)
    {
        ArgumentNullException.ThrowIfNull(target, nameof(target));
        ArgumentNullException.ThrowIfNull(message, nameof(message));

        _wsNetwork.SendPrivateMsgAsync(target, message).Await();
    }
}
