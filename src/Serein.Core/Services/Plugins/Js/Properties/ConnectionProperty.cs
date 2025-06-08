using System;
using System.Threading.Tasks;
using Serein.Core.Models.Network.Connection;
using Serein.Core.Services.Network.Connection;

namespace Serein.Core.Services.Plugins.Js.Properties;

public sealed class ConnectionProperty
{
    private readonly ConnectionManager _connectionManager;

    internal ConnectionProperty(ConnectionManager connectionManager)
    {
        _connectionManager = connectionManager;
    }

    public bool IsActive => _connectionManager.IsActive;

    public void Start()
    {
        _connectionManager.Start();
    }

    public void Stop()
    {
        _connectionManager.Stop();
    }

    public async Task SendDataAsync(string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        await _connectionManager.SendDataAsync(text);
    }

    public async Task SendMsgAsync(TargetType targetType, string target, string message)
    {
        ArgumentNullException.ThrowIfNull(target);
        ArgumentNullException.ThrowIfNull(message);

        await _connectionManager.SendMessageAsync(targetType, target, message);
    }
}
