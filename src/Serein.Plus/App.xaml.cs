using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serein.Core;
using Serein.Core.Models.Output;
using Serein.Core.Services;
using Serein.Plus.Pages;
using Serein.Plus.Pages.Settings;
using Serein.Plus.Services;
using Serein.Plus.Services.Loggers;
using Serein.Plus.Utils;
using Serein.Plus.ViewModels;

namespace Serein.Plus;

public partial class App : Application
{
    private readonly IHost _app = Build();

    public App()
    {
        ShutdownMode = ShutdownMode.OnMainWindowClose;
        DispatcherUnhandledException += static (_, e) => DialogFactory.ShowErrorDialog(e.Exception);
        AppDomain.CurrentDomain.UnhandledException += static (_, e) =>
            DialogFactory.ShowErrorDialog((Exception)e.ExceptionObject);
    }

    private static IHost Build()
    {
        var builder = SereinAppBuilder.CreateBuilder();

        builder.Logging.SetMinimumLevel(LogLevel.Trace);

        builder
            .Services.AddSingleton<TitleUpdater>()
            .AddSingleton<InfoBarProvider>()
            .AddSingleton<BalloonTipProvider>()
            .AddSingleton<MainWindow>()
            .AddSingleton<NotImplPage>()
            .AddSingleton<ShellPage>()
            .AddSingleton<ShellViewModel>()
            .AddSingleton<ServerPage>()
            .AddSingleton<MatchPage>()
            .AddSingleton<SchedulePage>()
            .AddSingleton<ConnectionPage>()
            .AddSingleton<BindingPage>()
            .AddSingleton<PermissionGroupPage>()
            .AddSingleton<PluginPage>()
            .AddSingleton<PluginViewModel>()
            .AddSingleton<PluginConsolePage>()
            .AddSingleton<PluginConsoleViewModel>()
            .AddSingleton<PluginListPage>()
            .AddSingleton<AboutPage>()
            .AddSingleton<AppSettingPage>()
            .AddSingleton<ConnectionSettingPage>()
            .AddSingleton<WebApiSettingPage>()
            .AddSingleton<ReactionSettingPage>()
            .AddSingleton<CategoriesPage>()
            .AddSingleton<ILogger, NotificationLogger>()
            .AddSingleton<IPluginLogger, PluginLogger>()
            .AddSingleton<IConnectionLogger, ConnectionLogger>();

        var app = builder.Build();
        app.Services.GetRequiredService<SentryReporter>().Initialize();
        return app;
    }

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        MainWindow = _app.Services.GetRequiredService<MainWindow>();
        MainWindow.Show();
    }

    private void Application_Exit(object sender, ExitEventArgs e)
    {
        _app.StopAsync();
    }
}
