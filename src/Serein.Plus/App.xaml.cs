﻿using System;
using System.Windows;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Serein.Core;
using Serein.Core.Models.Output;
using Serein.Plus.Loggers;
using Serein.Plus.Ui.Pages;
using Serein.Plus.Ui.Pages.Function;
using Serein.Plus.Ui.Pages.Server;
using Serein.Plus.Ui.Pages.Settings;

namespace Serein.Plus;

public partial class App : Application
{
    public App()
    {
        AppDomain.CurrentDomain.UnhandledException += (_, e) =>
        {
            MessageBox.Show(e.ExceptionObject.ToString());
            Shutdown();
        };

        Build();
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
        builder.Services.AddSingleton<ConnectionPage>();
        builder.Services.AddSingleton<PluginConsolePage>();
        builder.Services.AddSingleton<PluginListPage>();
        builder.Services.AddSingleton<PluginPage>();

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
}
