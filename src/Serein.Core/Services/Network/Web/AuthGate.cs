using System.Linq;
using System.Threading.Tasks;
using EmbedIO;
using Serein.Core.Models.Network.Web.WebAuthentication;
using Serein.Core.Services.Data;
using Serein.Core.Services.Network.Web.Apis;

namespace Serein.Core.Services.Network.Web;

internal sealed class AuthGate(WebAuthenticationProvider provider) : WebModuleBase("/")
{
    public override bool IsFinalHandler => false;

    protected override async Task OnRequestAsync(IHttpContext context)
    {
        var path = context.Request.Url.AbsolutePath;

        lock (provider.Value)
        {
            if (provider.Value.Count == 0 || !path.StartsWith("/api"))
            {
                return;
            }

            var authorization = context.Request.Headers["Authorization"];

            if (
                !string.IsNullOrWhiteSpace(authorization)
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

            if (
                provider.Value.Any(a => a is UserAuthentication)
                || context.Request.Url.Query.Contains("auth=digest")
            )
            {
                context.Response.Headers.Add(
                    "WWW-Authenticate",
                    WebAuthenticationMatcher.CreateDigestChallengeHeader()
                );
            }
        }

        await ApiHelper.HandleHttpException(context, HttpException.Unauthorized());
    }
}
