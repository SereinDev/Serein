using System;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Serein.Core.Models.Abstractions;
using Serein.Core.Models.Network.Web;
using Serein.Core.Services.Data;
using Serein.Core.Utils.Extensions;
using Serein.Core.Utils.Json;

namespace Serein.Core.Services.Network.Web.WebSockets;

internal class ConnectionWebSocketModule : WebSocketModuleBase
{
    private readonly ILogger<ConnectionWebSocketModule> _logger;
    private readonly ConnectionLoggerBase _connectionLoggerBase;

    public ConnectionWebSocketModule(
        ILogger<ConnectionWebSocketModule> logger,
        SettingProvider settingProvider,
        ConnectionLoggerBase connectionLoggerBase
    )
        : base("/ws/connection", settingProvider)
    {
        _logger = logger;
        _connectionLoggerBase = connectionLoggerBase;

        _connectionLoggerBase.Logging += OnLogging;
    }

    protected override void Dispose(bool disposing)
    {
        _connectionLoggerBase.Logging -= OnLogging;
        base.Dispose(disposing);
    }

    private void OnLogging((string Type, string Data) e)
    {
        try
        {
            BroadcastAsync(
                    JsonSerializer.Serialize(
                        new BroadcastPacket(e.Data, e.Data),
                        JsonSerializerOptionsFactory.Common
                    )
                )
                .Await();
        }
        catch (Exception ex)
        {
            _logger.LogDebug(ex, "尝试广播连接输出时出现异常");
        }
    }
}
