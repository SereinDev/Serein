using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using EmbedIO.WebSockets;
using Microsoft.Extensions.Logging;
using Serein.Core.Models.Network.Connection;
using Serein.Core.Models.Network.Web;
using Serein.Core.Services.Data;
using Serein.Core.Services.Network.Connection;
using Serein.Core.Utils.Extensions;
using Serein.Core.Utils.Json;

namespace Serein.Core.Services.Network.Web.WebSockets;

internal class ConnectionWebSocketModule : WebSocketModule
{
    private readonly ILogger<ConnectionWebSocketModule> _logger;
    private readonly SettingProvider _settingProvider;
    private readonly WsConnectionManager _wsConnectionManager;

    public ConnectionWebSocketModule(
        ILogger<ConnectionWebSocketModule> logger,
        SettingProvider settingProvider,
        WsConnectionManager wsConnectionManager
    )
        : base("/ws/connection", true)
    {
        _logger = logger;
        _settingProvider = settingProvider;
        _wsConnectionManager = wsConnectionManager;

        _wsConnectionManager.DataTransferred += OnDataTransferred;
    }

    protected override void Dispose(bool disposing)
    {
        _wsConnectionManager.DataTransferred -= OnDataTransferred;
        base.Dispose(disposing);
    }

    private void OnDataTransferred(object? sender, DataTranferredEventArgs e)
    {
        try
        {
            BroadcastAsync(
                    JsonSerializer.Serialize(
                        new BroadcastPacket(e.Type.ToString().ToLowerInvariant(), e.Data),
                        JsonSerializerOptionsFactory.Common
                    )
                )
                .Await();
        }
        catch (Exception ex)
        {
            _logger.LogDebug(ex, "尝试广播数据传输事件时出现异常");
        }
    }

    protected override Task OnMessageReceivedAsync(
        IWebSocketContext context,
        byte[] buffer,
        IWebSocketReceiveResult result
    ) => Task.CompletedTask;

    protected override async Task OnClientConnectedAsync(IWebSocketContext context)
    {
        var query = HttpUtility.ParseQueryString(context.RequestUri.Query);
        var auth = query.Get("token");

        if (
            _settingProvider.Value.WebApi.AccessTokens.Length != 0
            && (
                string.IsNullOrEmpty(auth)
                || !_settingProvider.Value.WebApi.AccessTokens.Contains(auth)
            )
        )
        {
            await context.WebSocket.CloseAsync();
        }
    }
}
