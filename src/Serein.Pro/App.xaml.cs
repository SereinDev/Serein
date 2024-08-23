using System.Diagnostics;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;

using Serein.Core;
using Serein.Core.Models.Output;
using Serein.Pro.Services;
using Serein.Pro.Services.Loggers;
using Serein.Pro.ViewModels;
using Serein.Pro.Views;

namespace Serein.Pro;

public partial class App : Application
{
    public SereinApp Host { get; } = Build();

    public App()
    {
        InitializeComponent();

#if DEBUG
        UnhandledException += (_, e) => Debug.WriteLine(e);
#endif
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        var mainWindow = Host.Services.GetRequiredService<MainWindow>();

        mainWindow.Activate();
    }

    private static SereinApp Build()
    {
        var builder = new SereinAppBuilder();
        builder.ConfigureService();

        builder.Services.AddSingleton<InfoBarProvider>();
        builder.Services.AddSingleton<MainWindow>();
        builder.Services.AddSingleton<ShellPage>();

        builder.Services.AddTransient<ShellViewModel>();
        builder.Services.AddTransient<SettingViewModel>();

        builder.Services.AddSingleton<ILogger, NotificationLogger>();
        builder.Services.AddSingleton<IPluginLogger, PluginLogger>();
        builder.Services.AddSingleton<IConnectionLogger, ConnectionLogger>();
        return builder.Build();
    }
}
