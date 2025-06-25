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
using Serein.Core.Services.Network.Connection.Adapters.OneBot;
using Serein.Core.Services.Network.Connection.Adapters.Satori;
using Serein.Core.Services.Network.Web;
using Serein.Core.Services.Network.Web.Apis;
using Serein.Core.Services.Network.Web.WebSockets;
using Serein.Core.Services.Permissions;
using Serein.Core.Services.Plugins;
using Serein.Core.Services.Plugins.Js;
using Serein.Core.Services.Plugins.Js.Properties;
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
        return CreateBuilder(false);
    }

    internal static HostApplicationBuilder CreateBuilder(bool enableFileLogger)
    {
        var hostAppBuilder = Host.CreateEmptyApplicationBuilder(
#if DEBUG
            new() { EnvironmentName = "Development" }
#else
            null
#endif
        );

        hostAppBuilder.Logging.SetMinimumLevel(LogLevel.Trace);
        hostAppBuilder.Logging.ClearProviders();
        hostAppBuilder.Logging.AddDebug();

        var cancellationTokenProvider = new CancellationTokenProvider();

        if (FileLoggerProvider.IsEnabled || enableFileLogger)
        {
            hostAppBuilder.Logging.AddProvider(new FileLoggerProvider(cancellationTokenProvider));
        }

        hostAppBuilder
            .Services.AddSingleton(cancellationTokenProvider)
            .AddSingleton<SentryReporter>()
            .AddSingleton<SettingProvider>()
            .AddSingleton<MatchProvider>()
            .AddSingleton<ScheduleProvider>()
            .AddSingleton<PermissionGroupProvider>()
            .AddSingleton<ImportHandler>()
            .AddSingleton<GroupManager>()
            .AddSingleton<PermissionManager>()
            .AddSingleton<HardwareInfoProvider>()
            .AddSingleton<ReactionTrigger>()
            .AddSingleton<Matcher>()
            .AddSingleton<ServerManager>()
            .AddSingleton<CommandParser>()
            .AddSingleton<CommandRunner>()
            .AddSingleton<UpdateChecker>()
            .AddSingleton<ConnectionManager>()
            .AddSingleton<ForwardWebSocketAdapter>()
            .AddSingleton<ReverseWebSocketAdapter>()
            .AddSingleton<SatoriAdapter>()
            .AddSingleton<ActionBuilder>()
            .AddSingleton<PacketHandler>()
            .AddSingleton<WebServer>()
            .AddTransient<ApiMap>()
            .AddTransient<IpBannerModule>()
            .AddTransient<ServerWebSocketModule>()
            .AddTransient<PluginWebSocketModule>()
            .AddTransient<ConnectionWebSocketModule>()
            .AddSingleton<PageExtractor>()
            .AddSingleton<PluginManager>()
            .AddSingleton<EventDispatcher>()
            .AddSingleton<JsEngineFactory>()
            .AddSingleton<JsPluginLoader>()
            .AddSingleton<PropertyFactory>()
            .AddSingleton<NetPluginLoader>()
            .AddSingleton<LocalStorage>()
            .AddSingleton<SessionStorage>()
            .AddDbContext<BindingRecordDbContext>(ServiceLifetime.Transient)
            .AddSingleton<BindingManager>()
            .AddHostedService<PluginService>()
            .AddHostedService<ScheduleRunner>()
            .AddHostedService<CoreService>()
            .AddSingleton<SereinApp>();

        return hostAppBuilder;
    }
}
