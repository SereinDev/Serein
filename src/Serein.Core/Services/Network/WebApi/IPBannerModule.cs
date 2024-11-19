using System.Threading.Tasks;

using EmbedIO;
using EmbedIO.Security;

using Serein.Core.Services.Data;
using Serein.Core.Services.Network.WebApi.Apis;

namespace Serein.Core.Services.Network.WebApi;

internal class IPBannerModule : IPBanningModule
{
    public IPBannerModule(SettingProvider settingProvider)
        : base("/")
    {
        this.WithMaxRequestsPerSecond(settingProvider.Value.WebApi.MaxRequestsPerSecond);
        this.WithWhitelist(settingProvider.Value.WebApi.WhiteList);
        OnHttpException = Handle403;
    }

    public static async Task Handle403(IHttpContext context, IHttpException exception)
    {
        if (exception.StatusCode == 403)
        {
            await ApiHelper.HandleHttpException(context, exception);
        }
    }
}
