using System;
using System.Threading.Tasks;
using System.Web;
using EmbedIO;
using EmbedIO.Routing;

namespace Serein.Core.Services.Network.Web.Apis;

internal partial class ApiMap
{
    [Route(HttpVerbs.Get, "/ws/ticket")]
    public async Task CreateWebSocketTicket()
    {
        var query = HttpUtility.ParseQueryString(HttpContext.Request.Url.Query);
        var path = query.Get("path");

        if (string.IsNullOrWhiteSpace(path))
        {
            throw HttpException.BadRequest();
        }

        if (!path.StartsWith("/ws/", StringComparison.OrdinalIgnoreCase))
        {
            throw HttpException.BadRequest();
        }

        await HttpContext.SendPacketAsync(
            new
            {
                Ticket = webSocketTicketService.IssueTicket(path),
                ExpiresInSeconds = WebSocketTicketService.DefaultTtlSeconds,
                Path = path,
            }
        );
    }
}
