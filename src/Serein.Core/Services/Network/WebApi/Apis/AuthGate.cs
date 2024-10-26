using System.Linq;
using System.Threading.Tasks;

using EmbedIO;

using Serein.Core.Services.Data;

namespace Serein.Core.Services.Network.WebApi.Apis;

public class AuthGate(SettingProvider settingProvider) : WebModuleBase("/")
{
    private readonly SettingProvider _settingProvider = settingProvider;

    public override bool IsFinalHandler => false;

    protected override Task OnRequestAsync(IHttpContext context)
    {
        if (context.RequestedPath == "/broadcast")
            return Task.CompletedTask;

        var auth = context.Request.Headers.Get("Authorization");

        if (_settingProvider.Value.WebApi.AccessTokens.Length == 0)
            return Task.CompletedTask;

        if (string.IsNullOrEmpty(auth))
            throw HttpException.Unauthorized();

        if (auth.StartsWith("Bearer "))
            auth = auth[7..];

        return !_settingProvider.Value.WebApi.AccessTokens.Contains(auth)
            ? throw HttpException.Unauthorized()
            : Task.CompletedTask;
    }
}
