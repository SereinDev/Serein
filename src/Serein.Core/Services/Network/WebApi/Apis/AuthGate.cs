using System.Linq;
using System.Threading.Tasks;
using EmbedIO;
using Serein.Core.Services.Data;

namespace Serein.Core.Services.Network.WebApi.Apis;

internal class AuthGate(SettingProvider settingProvider) : WebModuleBase("/api")
{
    private readonly SettingProvider _settingProvider = settingProvider;

    public override bool IsFinalHandler => false;

    protected override async Task OnRequestAsync(IHttpContext context)
    {
        if (_settingProvider.Value.WebApi.AccessTokens.Length == 0)
        {
            return;
        }

        var auth = context.Request.Headers.Get("Authorization");

        if (string.IsNullOrEmpty(auth))
        {
            await ApiHelper.HandleHttpException(context, HttpException.Unauthorized());
            return;
        }

        if (auth.StartsWith("Bearer "))
        {
            auth = auth[7..];
        }

        if (!_settingProvider.Value.WebApi.AccessTokens.Contains(auth))
        {
            await ApiHelper.HandleHttpException(context, HttpException.Unauthorized());
        }
    }
}
