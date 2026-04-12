using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using EmbedIO;
using EmbedIO.Security;
using Serein.Core.Models.Network.Web.WebAuthentication;
using Serein.Core.Services.Data;
using Serein.Core.Services.Network.Web.Apis;

namespace Serein.Core.Services.Network.Web.Authentication;

internal sealed class AuthGate(WebAuthenticationProvider provider, SettingProvider settingProvider)
    : WebModuleBase("/")
{
    private readonly ConcurrentDictionary<string, int> _authenticationTrialCounts = [];

    public override bool IsFinalHandler => false;

    protected override async Task OnRequestAsync(IHttpContext context)
    {
        var path = context.Request.Url.AbsolutePath;
        var shouldBan = false;

        lock (provider.Value)
        {
            if (provider.Value.Count == 0 || !path.StartsWith("/api"))
            {
                return;
            }

            var authorization = context.Request.Headers["Authorization"];

            if (
                WebAuthenticationMatcher.IsAuthorized(
                    authorization,
                    context.Request.HttpVerb,
                    context.Request.Url.PathAndQuery,
                    provider.Value,
                    settingProvider.Value.WebApi.ReplayProtectionLevel
                )
            )
            {
                _authenticationTrialCounts.TryRemove(
                    context.RemoteEndPoint.Address.ToString(),
                    out _
                );
                return;
            }

            if (!string.IsNullOrWhiteSpace(authorization))
            {
                lock (_authenticationTrialCounts)
                {
                    var value = _authenticationTrialCounts.AddOrUpdate(
                        context.RemoteEndPoint.Address.ToString(),
                        1,
                        (_, count) => count + 1
                    );

                    if (value > 10)
                    {
                        _authenticationTrialCounts.TryRemove(
                            context.RemoteEndPoint.Address.ToString(),
                            out _
                        );
                        shouldBan = true;
                    }
                }
            }

            if (
                !shouldBan
                && (
                    provider.Value.Any(a => a is UserAuthentication)
                    || context.Request.Url.Query.Contains("auth=digest")
                )
            )
            {
                context.Response.Headers.Add(
                    "WWW-Authenticate",
                    WebAuthenticationMatcher.CreateDigestChallengeHeader()
                );
            }
        }

        if (shouldBan)
        {
            IPBanningModule.TryBanIP(context.RemoteEndPoint.Address, TimeSpan.FromMinutes(5));

            await ApiHelper.HandleHttpException(context, HttpException.Forbidden());
        }
        else
        {
            await ApiHelper.HandleHttpException(context, HttpException.Unauthorized());
        }
    }
}
