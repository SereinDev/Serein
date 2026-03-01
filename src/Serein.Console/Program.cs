using System;
using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serein.Console.Services;
using Serein.Console.Services.Interaction;
using Serein.Console.Services.Interaction.Handlers;
using Serein.Console.Services.Loggers;
using Serein.Core;
using Serein.Core.Models.Abstractions;
using Serein.Core.Services;
using Serein.Core.Services.Loggers;
using Serein.Core.Services.Network;
using Serein.Core.Utils;
using SysConsole = System.Console;

namespace Serein.Console;

public static partial class Program
{
    public static int Main(string[] args)
    {
        SysConsole.InputEncoding = EncodingMap.UTF8;
        SysConsole.OutputEncoding = EncodingMap.UTF8;
        SysConsole.ResetColor();

        try
        {
            var rootCommnad = new RootCommand();
            rootCommnad.SetAction((_) => BuildApp());
            rootCommnad.Options.Add(new Option<bool>("--debug", "启用调试输出"));
            rootCommnad.Options.Add(new Option<bool>("--log", "启用日志模式"));

            return rootCommnad.Parse(args).Invoke(new() { EnableDefaultExceptionHandler = false });
        }
        catch (Exception e)
        {
            HandleException(e);
            return e.HResult switch
            {
                0 => 1,
                _ => e.HResult,
            };
        }
    }

    private static void BuildApp()
    {
        var builder = SereinAppBuilder.CreateBuilder();

        builder.Logging.AddProvider(new ConsoleLoggerProvider());
        builder
            .Services.AddHostedService<TitleUpdater>()
            .AddHostedService<InputLoopService>()
            .AddSingleton<ServerSwitcher>()
            .AddSingleton<InputHandler>()
            .AddSingleton<PluginHandler>()
            .AddSingleton<ServerHandler>()
            .AddSingleton<ConnectionHandler>()
            .AddSingleton<ClearScreenHandler>()
            .AddSingleton<WebServerHandler>()
            .AddSingleton<VersionHandler>()
            .AddSingleton<ExitHandler>()
            .AddSingleton<HelpHandler>()
            .AddSingleton<CommandProvider>()
            .AddSingleton<CommandPromptCallbacks>()
            .AddSingleton<ConnectionLoggerBase, ConnectionLogger>()
            .AddSingleton<PluginLoggerBase, PluginLogger>();

        var app = builder.Build();
        app.Services.GetRequiredService<SentryReporter>().Initialize();
        var logger = app.Services.GetRequiredService<ILogger<SereinApp>>();
        var updateChecker = app.Services.GetRequiredService<UpdateChecker>();
        var serverSwitcher = app.Services.GetRequiredService<ServerSwitcher>();

        if (SereinAppBuilder.StartForTheFirstTime)
        {
            ShowWelcomePage();
        }

        if (FileLoggerProvider.IsEnabled)
        {
            ShowWarningOfLogMode();
        }

        CheckConflict();

        app.Start();
        serverSwitcher.Initialize();

        updateChecker.Updated += (_, _) =>
        {
            if (updateChecker.Latest is not null)
            {
                logger.LogInformation(
                    "发现新版本：{}{}发布地址：{}",
                    updateChecker.Latest.TagName,
                    Environment.NewLine,
                    updateChecker.Latest.Url
                );
            }
        };

        updateChecker.ReadyToReplace += (_, _) =>
            logger.LogInformation("新的版本下载完成，并将在退出时自动替换");

        app.WaitForShutdown();
    }
}
