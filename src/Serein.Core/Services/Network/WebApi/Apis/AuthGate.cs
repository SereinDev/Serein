using System.Linq;
using System.Threading.Tasks;

using EmbedIO;

using Serein.Core.Services.Data;

namespace Serein.Core.Services.Network.WebApi.Apis;

public class AuthGate(SettingProvider settingProvider) : WebModuleBase("/api")
{
    private readonly SettingProvider _settingProvider = settingProvider;

    public override bool IsFinalHandler => false;

    protected override Task OnRequestAsync(IHttpContext context)
    {
        var auth = context.Request.Headers.Get("Authorization");

        if (string.IsNullOrEmpty(auth))
            HttpException.Unauthorized();

        if (auth!.StartsWith("Bearer "))
            auth = auth[7..];

        if (!_settingProvider.Value.WebApi.AccessTokens.Contains(auth))
            HttpException.Unauthorized();

        return Task.CompletedTask;
    }
}
