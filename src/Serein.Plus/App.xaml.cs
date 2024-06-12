using System;
using System.Windows;

using Microsoft.Extensions.DependencyInjection;

using Serein.Core;
using Serein.Core.Models.Output;
using Serein.Plus.Ui.Pages;
using Serein.Plus.Ui.Pages.Function;
using Serein.Plus.Ui.Pages.Server;
using Serein.Plus.Ui.Pages.Settings;

namespace Serein.Plus;

public partial class App : Application
{
    public static SereinApp Host { get; } = Build();

    public App()
    {
        AppDomain.CurrentDomain.UnhandledException += (_, e) =>
        {
            MessageBox.Show(e.ExceptionObject.ToString());
            Shutdown();
        };

        ShutdownMode = ShutdownMode.OnMainWindowClose;
    }

    private static SereinApp Build()
    {
        var builder = new SereinAppBuilder();
        builder.ConfigureService();

        builder.Services.AddSingleton<NotImplPage>();

        builder.Services.AddSingleton<ServerPage>();
        builder.Services.AddSingleton<MatchPage>();
        builder.Services.AddSingleton<SchedulePage>();
        builder.Services.AddSingleton<NetworkPage>();
        builder.Services.AddSingleton<PluginConsolePage>();
        builder.Services.AddSingleton<PluginListPage>();
        builder.Services.AddSingleton<PluginPage>();

        builder.Services.AddSingleton<AboutPage>();
        builder.Services.AddSingleton<AppSettingPage>();
        builder.Services.AddSingleton<NetworkSettingPage>();
        builder.Services.AddSingleton<PageSettingPage>();
        builder.Services.AddSingleton<ReactionSettingPage>();
        builder.Services.AddSingleton<CategoriesPage>();

        builder.Services.AddSingleton<ISereinLogger, WpfOutputHandler>(
            (services) => new(services)
        );

        return builder.Build();
    }
}
