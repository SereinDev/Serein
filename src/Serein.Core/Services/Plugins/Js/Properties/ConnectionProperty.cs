using System;
using Serein.Core.Services.Network.Connection;
using Serein.Core.Utils.Extensions;

namespace Serein.Core.Services.Plugins.Js.Properties;

public sealed class ConnectionProperty
{
    private readonly ConnectionManager _connectionManager;

    internal ConnectionProperty(ConnectionManager connectionManager)
    {
        _connectionManager = connectionManager;
    }

    public bool Active => _connectionManager.IsActive;

    public void Start()
    {
        _connectionManager.Start();
    }

    public void Stop()
    {
        _connectionManager.Stop();
    }

    public void SendData(string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        _connectionManager.SendDataAsync(text).Await();
    }

    public void SendGroupMsg(long target, string message)
    {
        ArgumentNullException.ThrowIfNull(message);

        _connectionManager.SendGroupMsgAsync(target, message).Await();
    }

    public void SendPrivateMsg(long target, string message)
    {
        ArgumentNullException.ThrowIfNull(message);

        _connectionManager.SendPrivateMsgAsync(target, message).Await();
    }
}
