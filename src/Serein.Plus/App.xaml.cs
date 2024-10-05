using System;
using System.Windows;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Serein.Core;
using Serein.Core.Models.Output;
using Serein.Core.Utils;
using Serein.Plus.Pages;
using Serein.Plus.Pages.Settings;
using Serein.Plus.Services;
using Serein.Plus.Services.Loggers;
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
            CrashHelper.CreateLog((Exception)e.ExceptionObject);
            Shutdown();
        };

        ShutdownMode = ShutdownMode.OnMainWindowClose;
    }

    private static SereinApp Build()
    {
        var builder = new SereinAppBuilder();
        builder.ConfigureService();

        builder.Services.AddSingleton<TitleUpdater>();
        builder.Services.AddSingleton<InfoBarProvider>();
        builder.Services.AddSingleton<BalloonTipProvider>();
        
        builder.Services.AddSingleton<MainWindow>();
        builder.Services.AddSingleton<NotImplPage>();

        builder.Services.AddSingleton<ShellPage>();
        builder.Services.AddSingleton<ShellViewModel>();

        builder.Services.AddSingleton<ServerPage>();
        builder.Services.AddSingleton<MatchPage>();
        builder.Services.AddSingleton<SchedulePage>();

        builder.Services.AddSingleton<ConnectionPage>();
        builder.Services.AddSingleton<ConnectionViewModel>();

        builder.Services.AddSingleton<PermissionGroupPage>();

        builder.Services.AddSingleton<PluginPage>();
        builder.Services.AddSingleton<PluginViewModel>();

        builder.Services.AddSingleton<PluginConsolePage>();
        builder.Services.AddSingleton<PluginConsoleViewModel>();

        builder.Services.AddSingleton<PluginListPage>();

        builder.Services.AddSingleton<AboutPage>();
        builder.Services.AddSingleton<AppSettingPage>();
        builder.Services.AddSingleton<ConnectionSettingPage>();
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
