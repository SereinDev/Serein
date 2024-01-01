using System;
using System.IO;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serein.Core.Services;
using Serein.Core.Services.Data;
using Serein.Core.Services.Networks;
using Serein.Core.Services.Plugins;
using Serein.Core.Services.Plugins.Js;
using Serein.Core.Services.Server;

namespace Serein.Core;

public class SereinAppBuilder
{
    static SereinAppBuilder()
    {
        Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
    }

    public IServiceCollection Services => _hostAppBuilder.Services;

    public ILoggingBuilder LoggingBuilder => _hostAppBuilder.Logging;

    private readonly HostApplicationBuilder _hostAppBuilder;

    public SereinAppBuilder()
    {
        _hostAppBuilder = new HostApplicationBuilder();
        _hostAppBuilder.Logging.ClearProviders();
    }

    public void ConfigureService()
    {
        Services.AddSingleton<SettingProvider>();
        Services.AddSingleton<MatchesProvider>();
        Services.AddSingleton<ScheduleProvider>();

        Services.AddSingleton<SystemInfoFactory>();
        Services.AddSingleton<Matcher>();
        Services.AddSingleton<ServerManager>();
        Services.AddSingleton<CommandParser>();
        Services.AddSingleton<CommandRunner>();
        Services.AddSingleton<ScheduleRunner>();

        Services.AddSingleton<WebSocketService>();
        Services.AddSingleton<ReverseWebSocketService>();
        Services.AddSingleton<WsNetwork>();

        Services.AddSingleton<EngineFactory>();
        Services.AddSingleton<PluginHost>();
    }

    public SereinApp Build() => new(_hostAppBuilder.Build());
}
