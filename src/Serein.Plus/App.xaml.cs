using System;
using System.Windows;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Ookii.Dialogs.Wpf;

using Serein.Core;
using Serein.Core.Models.Output;
using Serein.Core.Services;
using Serein.Core.Utils;
using Serein.Core.Utils.Extensions;
using Serein.Plus.Pages;
using Serein.Plus.Pages.Settings;
using Serein.Plus.Services;
using Serein.Plus.Services.Loggers;
using Serein.Plus.ViewModels;

namespace Serein.Plus;

public partial class App : Application
{
    private readonly IHost _app = Build();

    public App()
    {
        ShutdownMode = ShutdownMode.OnMainWindowClose;
        DispatcherUnhandledException += static (_, e) => ShowErrorDialog(e.Exception);
        AppDomain.CurrentDomain.UnhandledException += static (_, e) =>
            ShowErrorDialog((Exception)e.ExceptionObject);
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

    private static void ShowErrorDialog(Exception e)
    {
        try
        {
            var fileName = CrashHelper.CreateLog(e);
            var button1 = new TaskDialogButton("GitHub Issue")
            {
                CommandLinkNote = "【推荐】在GitHub上反馈，方便作者定位和跟踪问题",
            };
            var button2 = new TaskDialogButton("交流群")
            {
                CommandLinkNote = "通过共同讨论分析和确定问题，但效率可能较低",
            };

            var dialog = new TaskDialog
            {
                Buttons = { new(ButtonType.Ok), button1, button2 },
                CenterParent = true,
                Content =
                    $"{e.GetType().FullName}: {e.Message} \r\n\r\n"
                    + $"完整崩溃日志已保存在 {fileName}，请先善用搜索引擎寻找解决方案。\r\n"
                    + "在确定不是自身问题（如文件语法不正确、文件缺失等）后，你可以通过以下方式反馈此问题，帮助我们更好的改进 Serein！",
                EnableHyperlinks = true,
                Footer = "反馈问题时你应该上传崩溃日志文件，而不是此窗口的截图",
                FooterIcon = TaskDialogIcon.Warning,
                ExpandedInformation = e.StackTrace,
                MainIcon = TaskDialogIcon.Error,
                MainInstruction = "唔……崩溃了(っ °Д °;)っ",
                WindowTitle = "Serein.Plus",
                ButtonStyle = TaskDialogButtonStyle.CommandLinks,
            };

            var btn = dialog.ShowDialog();

            if (btn == button1)
            {
                UrlConstants.Issues.OpenInBrowser();
            }
            else if (btn == button2)
            {
                UrlConstants.Group.OpenInBrowser();
            }
        }
        catch { }
    }
}
