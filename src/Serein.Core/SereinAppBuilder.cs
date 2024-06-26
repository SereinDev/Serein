using System;
using System.IO;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serein.Core.Services;
using Serein.Core.Services.Data;
using Serein.Core.Services.Network;
using Serein.Core.Services.Network.Connection;
using Serein.Core.Services.Network.WebApi;
using Serein.Core.Services.Plugins;
using Serein.Core.Services.Plugins.Js;
using Serein.Core.Services.Plugins.Net;
using Serein.Core.Services.Servers;
using Serein.Core.Services.Network.Ssh;

namespace Serein.Core;

public sealed class SereinAppBuilder
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

        Services.AddSingleton<HardwareInfoProvider>();
        Services.AddSingleton<ReactionManager>();
        Services.AddSingleton<Matcher>();
        Services.AddSingleton<ServerManager>();
        Services.AddSingleton<CommandParser>();
        Services.AddSingleton<CommandRunner>();
        Services.AddSingleton<ScheduleRunner>();

        Services.AddSingleton<UpdateChecker>();
        Services.AddSingleton<WebSocketService>();
        Services.AddSingleton<ReverseWebSocketService>();
        Services.AddSingleton<WsConnectionManager>();
        Services.AddSingleton<HttpServer>();
        Services.AddSingleton<SshServiceProvider>();

        Services.AddSingleton<PluginHost>();
        Services.AddSingleton<EventDispatcher>();
        Services.AddSingleton<JsEngineFactory>();
        Services.AddSingleton<JsManager>();
        Services.AddSingleton<NetManager>();
    }

    public SereinApp Build() => new(_hostAppBuilder.Build());
}
