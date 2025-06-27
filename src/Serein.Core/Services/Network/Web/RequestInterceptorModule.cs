using System.Threading.Tasks;
using EmbedIO;
using EmbedIO.Security;
using Serein.Core.Models.Plugins;
using Serein.Core.Services.Data;
using Serein.Core.Services.Network.Web.Apis;
using Serein.Core.Services.Plugins;

namespace Serein.Core.Services.Network.Web;

internal class RequestInterceptorModule : IPBanningModule
{
    private readonly EventDispatcher _eventDispatcher;

    public RequestInterceptorModule(
        EventDispatcher eventDispatcher,
        SettingProvider settingProvider
    )
        : base("/")
    {
        _eventDispatcher = eventDispatcher;

        this.WithMaxRequestsPerSecond(settingProvider.Value.WebApi.MaxRequestsPerSecond);
        this.WithWhitelist(settingProvider.Value.WebApi.WhiteList);

        OnHttpException = async (context, exception) =>
        {
            if (context.Request.Url.PathAndQuery.StartsWith("/api/"))
            {
                await ApiHelper.HandleHttpException(context, exception);
            }
        };
    }

    protected override Task OnRequestAsync(IHttpContext context)
    {
        base.OnRequestAsync(context);

        if (!_eventDispatcher.Dispatch(Event.HttpRequestReceived, context))
        {
            context.SetHandled();
        }

        return Task.CompletedTask;
    }
}
