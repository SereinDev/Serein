using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serein.Core.Services;
using Serein.Core.Services.Bindings;
using Serein.Core.Services.Commands;
using Serein.Core.Services.Data;
using Serein.Core.Services.Loggers;
using Serein.Core.Services.Network;
using Serein.Core.Services.Network.Connection;
using Serein.Core.Services.Network.WebApi;
using Serein.Core.Services.Network.WebApi.Apis;
using Serein.Core.Services.Permissions;
using Serein.Core.Services.Plugins;
using Serein.Core.Services.Plugins.Js;
using Serein.Core.Services.Plugins.Net;
using Serein.Core.Services.Plugins.Storages;
using Serein.Core.Services.Servers;
using Serein.Core.Utils;

namespace Serein.Core;

public static class SereinAppBuilder
{
    public static readonly bool StartForTheFirstTime = !File.Exists(PathConstants.SettingFile);

    static SereinAppBuilder()
    {
        Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
        Directory.CreateDirectory(PathConstants.Root);
    }

    public static HostApplicationBuilder CreateBuilder()
    {
        var hostAppBuilder = Host.CreateEmptyApplicationBuilder(null);
        hostAppBuilder.Logging.SetMinimumLevel(LogLevel.Trace);
        hostAppBuilder.Logging.ClearProviders();
        hostAppBuilder.Logging.AddDebug();

        if (FileLoggerProvider.IsEnabled)
        {
            hostAppBuilder.Logging.AddProvider(new FileLoggerProvider());
        }

        hostAppBuilder
            .Services.AddSingleton<SentryReporter>()
            .AddSingleton<SettingProvider>()
            .AddSingleton<MatchesProvider>()
            .AddSingleton<ScheduleProvider>()
            .AddSingleton<PermissionGroupProvider>()
            .AddSingleton<GroupManager>()
            .AddSingleton<PermissionManager>()
            .AddSingleton<HardwareInfoProvider>()
            .AddSingleton<ReactionTrigger>()
            .AddSingleton<Matcher>()
            .AddSingleton<ServerManager>()
            .AddSingleton<CommandParser>()
            .AddSingleton<CommandRunner>()
            .AddSingleton<UpdateChecker>()
            .AddSingleton<WebSocketService>()
            .AddSingleton<ReverseWebSocketService>()
            .AddSingleton<WsConnectionManager>()
            .AddSingleton<PacketHandler>()
            .AddSingleton<HttpServer>()
            .AddTransient<ApiMap>()
            .AddTransient<IPBannerModule>()
            .AddTransient<BroadcastWebSocketModule>()
            .AddSingleton<PluginManager>()
            .AddSingleton<EventDispatcher>()
            .AddSingleton<JsEngineFactory>()
            .AddSingleton<JsPluginLoader>()
            .AddSingleton<NetPluginLoader>()
            .AddSingleton<LocalStorage>()
            .AddSingleton<SessionStorage>()
            .AddDbContext<BindingRecordDbContext>(ServiceLifetime.Transient)
            .AddSingleton<BindingManager>()
            .AddHostedService<PluginService>()
            .AddHostedService<ScheduleRunner>()
            .AddHostedService<CoreService>();

        return hostAppBuilder;
    }
}
