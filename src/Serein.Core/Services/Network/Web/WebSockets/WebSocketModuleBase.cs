using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Web;
using EmbedIO;
using EmbedIO.WebSockets;
using Serein.Core.Services.Data;

namespace Serein.Core.Services.Network.Web.WebSockets;

internal abstract class WebSocketModuleBase(
    string path,
    WebAuthenticationProvider webAuthenticationProvider,
    WebSocketTicketService webSocketTicketService
) : WebSocketModule(path, true)
{
    protected override Task OnMessageReceivedAsync(
        IWebSocketContext context,
        byte[] buffer,
        IWebSocketReceiveResult result
    )
    {
        return Task.CompletedTask;
    }

    protected override async Task OnClientConnectedAsync(IWebSocketContext context)
    {
        if (!TryAuthorize(context, webAuthenticationProvider, webSocketTicketService))
        {
            await context.WebSocket.CloseAsync();
        }
    }

    protected static bool TryAuthorize(
        IWebSocketContext context,
        WebAuthenticationProvider webAuthenticationProvider,
        WebSocketTicketService webSocketTicketService
    )
    {
        return webAuthenticationProvider.Value.Count == 0
            || TryAuthorize(
                context,
                HttpUtility.ParseQueryString(context.RequestUri.Query),
                webAuthenticationProvider,
                webSocketTicketService
            );
    }

    protected static bool TryAuthorize(
        IWebSocketContext context,
        NameValueCollection query,
        WebAuthenticationProvider webAuthenticationProvider,
        WebSocketTicketService webSocketTicketService
    )
    {
        var ticket = query.Get("ticket");

        if (!string.IsNullOrWhiteSpace(ticket))
        {
            return webSocketTicketService.ValidateTicket(ticket, context.RequestUri.AbsolutePath);
        }

        var authorization = ResolveAuthorization(context, query);
        return !string.IsNullOrWhiteSpace(authorization)
            && WebAuthenticationMatcher.IsAuthorized(
                authorization,
                HttpVerbs.Get,
                context.RequestUri.PathAndQuery,
                webAuthenticationProvider.Value
            );
    }

    private static string? ResolveAuthorization(
        IWebSocketContext context,
        NameValueCollection query
    )
    {
        var authorization = context.Headers.Get("Authorization");
        if (!string.IsNullOrWhiteSpace(authorization))
        {
            return authorization;
        }

        var bearer = query.Get("bearer");
        return string.IsNullOrWhiteSpace(bearer) ? null : $"Bearer {bearer}";
    }
}
