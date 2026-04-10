using System;
using System.Threading.Tasks;
using EmbedIO;
using Serein.Core.Services.Data;
using Serein.Core.Services.Network.Web.Apis;
using Serein.Core.Utils;

namespace Serein.Core.Services.Network.Web;

internal sealed class AuthGate(WebAuthenticationProvider provider) : WebModuleBase("/")
{
    public override bool IsFinalHandler => false;

    protected override async Task OnRequestAsync(IHttpContext context)
    {
        var path = context.Request.Url.AbsolutePath;

        if (provider.Value.Count == 0 || !path.StartsWith("/api") && !path.StartsWith("/_auth/"))
        {
            return;
        }

        var authorization = context.Request.Headers["Authorization"];

        lock (provider.Value)
        {
            if (
                !string.IsNullOrEmpty(authorization)
                && WebAuthenticationMatcher.IsAuthorized(
                    authorization,
                    context.Request.HttpVerb,
                    context.Request.Url.PathAndQuery,
                    provider.Value
                )
            )
            {
                return;
            }
        }

        if (path.StartsWith("/api"))
        {
            await ApiHelper.HandleHttpException(context, HttpException.Unauthorized());
        }
        else if (path == "/_auth/digest")
        {
            context.Response.StatusCode = 401;
            context.Response.Headers.Add(
                "WWW-Authenticate",
                WebAuthenticationMatcher.CreateDigestChallengeHeader()
            );

            await context.SendStringAsync("Unauthorized", "text/html", EncodingMap.UTF8);

            context.SetHandled();
        }
    }
}
