using System;
using System.CommandLine;
using System.CommandLine.Parsing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serein.Cli.Services;
using Serein.Cli.Services.Interaction;
using Serein.Cli.Services.Interaction.Handlers;
using Serein.Cli.Services.Loggers;
using Serein.Cli.Utils;
using Serein.Core;
using Serein.Core.Models.Abstractions;
using Serein.Core.Services;
using Serein.Core.Services.Loggers;
using Serein.Core.Services.Network;
using Serein.Core.Utils;

namespace Serein.Cli;

public static class Program
{
    public static int Main(string[] args)
    {
        Console.InputEncoding = EncodingMap.UTF8;
        Console.OutputEncoding = EncodingMap.UTF8;
        Console.ResetColor();

        return CommandLineParserBuilder.Build(BuildApp).Invoke(args);
    }

    private static void ShowWelcomePage(ILogger logger)
    {
        logger.LogInformation("");
        logger.LogInformation("欢迎使用Serein！！");
        logger.LogInformation("");
        logger.LogInformation(
            "如果你是第一次使用Serein，那么一定要仔细阅读以下内容，相信这些会对你有所帮助(๑•̀ㅂ•́)و✧"
        );
        logger.LogInformation("");
        logger.LogInformation("▫ 官网文档（{}）", UrlConstants.Docs);
        logger.LogInformation("  这里有详细完整的功能介绍和教程，推荐新手仔细阅读");
        logger.LogInformation("");
        logger.LogInformation("▫ GitHub仓库（{}）", UrlConstants.Repository);
        logger.LogInformation(
            "  这是储存 Serein 源代码的地方。欢迎每一个人为 Serein 的发展贡献力量"
        );
        logger.LogInformation("");
        logger.LogInformation("▫ 交流群（{}）", UrlConstants.Group);
        logger.LogInformation("  欢迎加群闲聊划水:)");
        logger.LogInformation("");

        logger.LogWarning("此软件与Mojang Studio、网易、Microsoft没有从属关系");
        logger.LogWarning("Serein is licensed under GPL-v3.0");
        logger.LogWarning("Copyright © 2022 Zaitonn. All Rights Reserved.");
    }

    private static void ShowWarningOfLogMode(ILogger logger)
    {
        logger.LogWarning("你开启了日志模式！");
        logger.LogWarning(
            "在此模式下，应用程序会将完整的调试日志保存在\"{}/app\"目录下（可能很大很大很大，并对硬盘的读写速度产生一定影响）",
            PathConstants.LogDirectory
        );
        logger.LogWarning("除非你知道你在干什么 / 是开发者要求的，请不要在此模式下运行Serein！！");
        logger.LogWarning("");
        logger.LogWarning("当然你也不需要太担心，若要退出此模式只需要重新启动就行啦 :D");
    }

    private static void CheckConflict(ILogger logger)
    {
        var processes = BaseChecker.CheckConflictProcesses();
        if (processes.Count > 0)
        {
            logger.LogWarning("检测到冲突进程：");
            foreach (var process in processes)
            {
                logger.LogWarning(" · {}, Id={}", process.ProcessName, process.Id);
            }

            logger.LogWarning(
                "在同一文件夹内同时运行多个Serein实例可能导致文件读写冲突或无法正确保存"
            );
        }
    }

    private static void BuildApp()
    {
        var builder = SereinAppBuilder.CreateBuilder();

        builder.Logging.AddProvider(new CliLoggerProvider());
        builder
            .Services.AddHostedService<TitleUpdater>()
            .AddHostedService<CancelKeyHandlingService>()
            .AddHostedService<InputLoopService>()
            .AddSingleton<ServerSwitcher>()
            .AddSingleton<InputHandler>()
            .AddSingleton<PluginHandler>()
            .AddSingleton<ServerHandler>()
            .AddSingleton<ConnectionHandler>()
            .AddSingleton<ClearScreenHandler>()
            .AddSingleton<VersionHandler>()
            .AddSingleton<ExitHandler>()
            .AddSingleton<HelpHandler>()
            .AddSingleton<CommandProvider>()
            .AddSingleton<CommandPromptCallbacks>()
            .AddSingleton<IConnectionLogger, ConnectionLogger>()
            .AddSingleton<IPluginLogger, PluginLogger>();

        var app = builder.Build();
        app.Services.GetRequiredService<SentryReporter>().Initialize();
        var logger = app.Services.GetRequiredService<ILogger<SereinApp>>();
        var updateChecker = app.Services.GetRequiredService<UpdateChecker>();
        var serverSwitcher = app.Services.GetRequiredService<ServerSwitcher>();

        if (SereinAppBuilder.StartForTheFirstTime)
        {
            ShowWelcomePage(logger);
        }

        if (FileLoggerProvider.IsEnabled)
        {
            ShowWarningOfLogMode(logger);
        }

        CheckConflict(logger);

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

        updateChecker.Prepared += (_, _) =>
            logger.LogInformation("新的版本下载完成，并将在退出时自动替换");

        app.WaitForShutdown();
    }
}
