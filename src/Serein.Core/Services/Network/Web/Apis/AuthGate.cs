using System.Linq;
using System.Threading.Tasks;
using EmbedIO;
using Serein.Core.Services.Data;

namespace Serein.Core.Services.Network.Web.Apis;

internal sealed class AuthGate(SettingProvider settingProvider) : WebModuleBase("/api")
{
    public override bool IsFinalHandler => false;

    protected override async Task OnRequestAsync(IHttpContext context)
    {
        if (settingProvider.Value.WebApi.AccessTokens.Length == 0)
        {
            return;
        }

        var token = context.Request.Headers.Get("Authorization");

        if (string.IsNullOrEmpty(token))
        {
            await ApiHelper.HandleHttpException(context, HttpException.Unauthorized());
            return;
        }

        if (token.StartsWith("Bearer "))
        {
            token = token[7..];
        }

        if (!settingProvider.Value.WebApi.AccessTokens.Contains(token))
        {
            await ApiHelper.HandleHttpException(context, HttpException.Unauthorized());
        }
    }
}
