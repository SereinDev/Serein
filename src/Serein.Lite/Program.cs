using System;
using System.Windows.Forms;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Serein.Core;
using Serein.Core.Models.Output;
using Serein.Lite.Loggers;
using Serein.Lite.Ui;
using Serein.Lite.Ui.Function;
using Serein.Lite.Ui.Members;
using Serein.Lite.Ui.Servers;
using Serein.Lite.Ui.Settings;
using Serein.Lite.Utils;

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
        builder.Services.AddSingleton<BindingPage>();

        builder.Services.AddSingleton<SettingPage>();
        builder.Services.AddSingleton<AboutPage>();
        builder.Services.AddSingleton<AppSettingPage>();
        builder.Services.AddSingleton<ReactionSettingPage>();
        builder.Services.AddSingleton<ConnectionSettingPage>();
        builder.Services.AddSingleton<WebApiSettingPage>();
        builder.Services.AddSingleton<SshSettingPage>();

        App = builder.Build();
    }

    [STAThread]
    public static void Main()
    {
        AppDomain.CurrentDomain.UnhandledException += (_, e) =>
            DialogFactory.ShowErrorDialog((Exception)e.ExceptionObject);
        Application.ThreadException += (_, e) => DialogFactory.ShowErrorDialog(e.Exception);
        
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);
        ApplicationConfiguration.Initialize();
        Application.Run(App.Services.GetRequiredService<MainForm>());
    }
}
