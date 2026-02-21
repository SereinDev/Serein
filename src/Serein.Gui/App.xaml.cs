using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serein.Core;
using Serein.Core.Models.Abstractions;
using Serein.Core.Services;
using Serein.Gui.Pages;
using Serein.Gui.Pages.Settings;
using Serein.Gui.Services;
using Serein.Gui.Services.Loggers;
using Serein.Gui.Utils;
using Serein.Gui.ViewModels;

namespace Serein.Gui;

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

        var provider = new ConsoleLoggerProvider();
        builder.Logging.AddProvider(provider);

        builder
            .Services.AddSingleton(provider)
            .AddSingleton<TitleUpdater>()
            .AddSingleton<InfoBarProvider>()
            .AddSingleton<MainWindow>()
            .AddSingleton<MainWindowViewModel>()
            .AddSingleton<NotImplPage>()
            .AddSingleton<ServerPage>()
            .AddSingleton<ServerPageViewModel>()
            .AddSingleton<AutomationPage>()
            .AddSingleton<AutomationPageViewModel>()
            .AddSingleton<ConnectionPage>()
            .AddSingleton<ConnectionPageViewModel>()
            .AddSingleton<BindingPage>()
            .AddSingleton<BindingPageViewModel>()
            .AddSingleton<PermissionGroupPage>()
            .AddSingleton<PermissionGroupPageViewModel>()
            .AddSingleton<PluginPage>()
            .AddSingleton<PluginPageViewModel>()
            .AddSingleton<PluginConsolePage>()
            .AddSingleton<PluginConsoleViewModel>()
            .AddSingleton<PluginListPage>()
            .AddSingleton<PluginListPageViewModel>()
            .AddSingleton<AboutPage>()
            .AddSingleton<AppSettingPage>()
            .AddSingleton<ConnectionSettingPage>()
            .AddSingleton<WebApiSettingPage>()
            .AddSingleton<CategoriesPage>()
            .AddSingleton<PluginLoggerBase, PluginLogger>()
            .AddSingleton<ConnectionLoggerBase, ConnectionLogger>();

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
