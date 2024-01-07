using System;
using System.Windows;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core;
using Serein.Core.Models;
using Serein.Core.Services.Data;
using Serein.Plus.Ui.Pages;
using Serein.Plus.Ui.Pages.Function;
using Serein.Plus.Ui.Pages.Server;

namespace Serein.Plus;

public partial class App : Application
{
    public static IHost Host { get; } = Build();

    public App()
    {
        AppDomain.CurrentDomain.UnhandledException += (_, e) =>
        {
            MessageBox.Show(e.ExceptionObject.ToString());
            Shutdown();
        };

        Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
    }

    private static IHost Build()
    {
        var builder = new SereinAppBuilder();
        builder.ConfigureService();

        builder.Services.AddSingleton<PanelPage>();
        builder.Services.AddSingleton<MatchPage>();
        builder.Services.AddSingleton<SchedulePage>();
        builder.Services.AddSingleton<PluginPage>();
        builder.Services.AddSingleton<NotImplPage>();
        builder.Services.AddSingleton<SettingPage>();

        builder.Services.AddSingleton<IOutputHandler, AppOutputHandler>(
            (services) =>
                new(services.GetRequiredService<SettingProvider>().Value.Application.LogLevel)
        );

        return builder.Build();
    }
}
