using System;
using System.Windows;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Serein.Core;
using Serein.Core.Models.Output;
using Serein.Plus.Loggers;
using Serein.Plus.Pages;
using Serein.Plus.Pages.Settings;
using Serein.Plus.Services;
using Serein.Plus.ViewModels;

namespace Serein.Plus;

public partial class App : Application
{
    private readonly SereinApp _app = Build();

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

        builder.Services.AddSingleton<InfoBarProvider>();
        builder.Services.AddSingleton<BalloonTipProvider>();
        
        builder.Services.AddSingleton<MainWindow>();
        builder.Services.AddTransient<NotImplPage>();

        builder.Services.AddSingleton<ShellPage>();
        builder.Services.AddTransient<ShellViewModel>();

        builder.Services.AddSingleton<HomePage>();
        builder.Services.AddSingleton<ServerPage>();
        builder.Services.AddSingleton<MatchPage>();
        builder.Services.AddSingleton<SchedulePage>();

        builder.Services.AddSingleton<ConnectionPage>();
        builder.Services.AddTransient<ConnectionViewModel>();

        builder.Services.AddSingleton<PluginPage>();
        builder.Services.AddTransient<PluginViewModel>();

        builder.Services.AddSingleton<PluginConsolePage>();
        builder.Services.AddSingleton<PluginListPage>();

        builder.Services.AddSingleton<AboutPage>();
        builder.Services.AddSingleton<AppSettingPage>();
        builder.Services.AddSingleton<ConnectionSettingPage>();
        builder.Services.AddSingleton<PageSettingPage>();
        builder.Services.AddSingleton<ReactionSettingPage>();
        builder.Services.AddSingleton<CategoriesPage>();

        builder.Services.AddSingleton<ILogger, NotificationLogger>();
        builder.Services.AddSingleton<IPluginLogger, PluginLogger>();
        builder.Services.AddSingleton<IConnectionLogger, ConnectionLogger>();
        return builder.Build();
    }

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        _app.Services.GetRequiredService<MainWindow>().Show();
    }

    private void Application_Exit(object sender, ExitEventArgs e)
    {
        _app.StopAsync();
    }
}
