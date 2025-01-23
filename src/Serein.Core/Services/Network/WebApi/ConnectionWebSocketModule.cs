using System.Linq;
using System.Threading.Tasks;
using System.Web;
using EmbedIO.WebSockets;
using Serein.Core.Services.Data;

namespace Serein.Core.Services.Network.WebApi;

internal class ConnectionWebSocketModule(SettingProvider settingProvider)
    : WebSocketModule("/ws/connection", true)
{
    private readonly SettingProvider _settingProvider = settingProvider;

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
