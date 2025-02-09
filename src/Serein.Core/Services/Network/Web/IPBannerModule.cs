using EmbedIO.Security;
using Serein.Core.Services.Data;
using Serein.Core.Services.Network.Web.Apis;

namespace Serein.Core.Services.Network.Web;

internal class IpBannerModule : IPBanningModule
{
    public IpBannerModule(SettingProvider settingProvider)
        : base("/")
    {
        this.WithMaxRequestsPerSecond(settingProvider.Value.WebApi.MaxRequestsPerSecond);
        this.WithWhitelist(settingProvider.Value.WebApi.WhiteList);
        OnHttpException = async (context, exception) =>
        {
            if (exception.StatusCode == 403)
            {
                await ApiHelper.HandleHttpException(context, exception);
            }
        };
    }
}
