using System;
using System.Windows.Forms;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serein.Core;
using Serein.Core.Models.Output;
using Serein.Lite.Services.Loggers;
using Serein.Lite.Ui;
using Serein.Lite.Ui.Function;
using Serein.Lite.Ui.Members;
using Serein.Lite.Ui.Servers;
using Serein.Lite.Ui.Settings;
using Serein.Lite.Utils;

namespace Serein.Lite;

public static class Program
{
    private static readonly IHost Host;

    static Program()
    {
        var builder = SereinAppBuilder.CreateBuilder();

        builder
            .Services.AddSingleton<ILogger, NotificationLogger>()
            .AddSingleton<IConnectionLogger, ConnectionLogger>()
            .AddSingleton<IPluginLogger, PluginLogger>()
            .AddSingleton<ResourcesManager>()
            .AddSingleton<MainForm>()
            .AddSingleton<ServerPage>()
            .AddSingleton<MatchPage>()
            .AddSingleton<SchedulePage>()
            .AddSingleton<ConnectionPage>()
            .AddSingleton<PluginPage>()
            .AddSingleton<BindingPage>()
            .AddSingleton<PermissionGroupPage>()
            .AddSingleton<SettingPage>()
            .AddSingleton<AboutPage>()
            .AddSingleton<AppSettingPage>()
            .AddSingleton<ReactionSettingPage>()
            .AddSingleton<ConnectionSettingPage>()
            .AddSingleton<WebApiSettingPage>()
            .AddSingleton<SshSettingPage>();

        Host = builder.Build();
    }

    [STAThread]
    public static void Main()
    {
        AppDomain.CurrentDomain.UnhandledException += (_, e) =>
            DialogFactory.ShowErrorDialog((Exception)e.ExceptionObject);
        Application.ThreadException += (_, e) => DialogFactory.ShowErrorDialog(e.Exception);

        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);
        ApplicationConfiguration.Initialize();
        Application.Run(Host.Services.GetRequiredService<MainForm>());
    }
}
