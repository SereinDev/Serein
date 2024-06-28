using System;
using System.Windows.Forms;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Serein.Core;
using Serein.Core.Models.Output;
using Serein.Lite.Loggers;
using Serein.Lite.Ui;
using Serein.Lite.Ui.Function;
using Serein.Lite.Ui.Servers;
using Serein.Lite.Ui.Settings;

namespace Serein.Lite;

public static class Program
{
    private static readonly SereinApp App;

    static Program()
    {
        var builder = new SereinAppBuilder();
        builder.ConfigureService();

        builder.Services.AddSingleton<ILogger, NotificationLogger>();
        builder.Services.AddSingleton<IConnectionLogger, ConnectionLogger>();
        builder.Services.AddSingleton<IPluginLogger, PluginLogger>();

        builder.Services.AddSingleton<ResourcesManager>();
        builder.Services.AddSingleton<MainForm>();
        builder.Services.AddSingleton<ServerPage>();
        builder.Services.AddSingleton<MatchPage>();
        builder.Services.AddSingleton<SchedulePage>();
        builder.Services.AddSingleton<ConnectionPage>();
        builder.Services.AddSingleton<PluginPage>();
        builder.Services.AddSingleton<SettingPage>();
        builder.Services.AddSingleton<AppSettingPage>();
        builder.Services.AddSingleton<ConnectionSettingPage>();

        App = builder.Build();
    }

    [STAThread]
    public static void Main()
    {
        App.StartAsync();

        ApplicationConfiguration.Initialize();
        Application.Run(App.Services.GetRequiredService<MainForm>());
    }
}
