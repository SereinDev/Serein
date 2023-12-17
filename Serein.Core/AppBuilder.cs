using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serein.Core.Services;
using Serein.Core.Services.Data;
using Serein.Core.Services.Networks;
using Serein.Core.Services.Server;

namespace Serein.Core;

public class AppBuilder
{
    private IServiceCollection Services => _hostAppBuilder.Services;

    private readonly HostApplicationBuilder _hostAppBuilder;

    public AppBuilder()
    {
        _hostAppBuilder = new HostApplicationBuilder();
        _hostAppBuilder.Logging.ClearProviders();
    }

    public void ConfigueService()
    {
        Services.AddSingleton<SettingProvider>();
        Services.AddSingleton<MatchesProvider>();

        Services.AddSingleton<SystemInfoFactory>();
        Services.AddSingleton<Matcher>();
        Services.AddSingleton<ServerManager>();
        Services.AddSingleton<CommandParser>();
        Services.AddSingleton<CommandRunner>();

        Services.AddSingleton<WebSocketService>();
        Services.AddSingleton<ReverseWebSocketService>();
        Services.AddSingleton<WsNetwork>();
    }

    public App Build() => new(_hostAppBuilder.Build());
}
