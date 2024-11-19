using System;

using Serein.Core.Services.Network.Connection;
using Serein.Core.Utils.Extensions;

namespace Serein.Core.Services.Plugins.Js.Properties;

public sealed class ConnectionProperty
{
    private readonly WsConnectionManager _wsConnectionManager;

    internal ConnectionProperty(WsConnectionManager wsConnectionManager)
    {
        _wsConnectionManager = wsConnectionManager;
    }

    public bool Active => _wsConnectionManager.Active;

    public void Start()
    {
        _wsConnectionManager.Start();
    }

    public void Stop()
    {
        _wsConnectionManager.Stop();
    }

    public void SendData(string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        _wsConnectionManager.SendDataAsync(text).Await();
    }

    public void SendGroupMsg(long target, string message)
    {
        ArgumentNullException.ThrowIfNull(message);

        _wsConnectionManager.SendGroupMsgAsync(target, message).Await();
    }

    public void SendPrivateMsg(long target, string message)
    {
        ArgumentNullException.ThrowIfNull(message);

        _wsConnectionManager.SendPrivateMsgAsync(target, message).Await();
    }
}
