using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Text;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Sentry;

using Serein.Cli.Services;
using Serein.Cli.Services.Interaction;
using Serein.Cli.Services.Loggers;
using Serein.Core;
using Serein.Core.Models.Output;
using Serein.Core.Services.Loggers;
using Serein.Core.Utils;

namespace Serein.Cli;

public static class Program
{
    public static int Main(string[] args)
    {
        Console.InputEncoding = EncodingMap.UTF8;
        Console.OutputEncoding = EncodingMap.UTF8;
        Console.ResetColor();

        return CreateParser().Invoke(args);
    }

    private static Parser CreateParser()
    {
        var rootCommnad = new RootCommand();
        rootCommnad.AddOption(new Option<bool>("--debug", "启用调试输出"));
        rootCommnad.AddOption(new Option<bool>("--log", "启用日志模式"));
        rootCommnad.SetHandler(BuildApp);

        return new CommandLineBuilder(rootCommnad)
            .UseExceptionHandler(OnException)
            .UseVersionOption()
            .UseHelp()
            .UseTypoCorrections()
            .UseParseErrorReporting()
            .RegisterWithDotnetSuggest()
            .Build();
    }

    private static void OnException(Exception e, InvocationContext? context)
    {
        SentrySdk.CaptureException(e);

        var fileName = CrashHelper.CreateLog(e);

        Console.ForegroundColor = ConsoleColor.Red;

        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("唔……崩溃了(っ °Д °;)っ");
        stringBuilder.AppendLine(e.ToString());
        stringBuilder.AppendLine();
        stringBuilder.AppendLine(
            $"完整崩溃日志已保存在 {fileName}，请先善用搜索引擎寻找解决方案。"
        );
        stringBuilder.AppendLine(
            "在确定不是自身问题（如文件语法不正确、文件缺失等）后，你可以通过以下方式反馈此问题，帮助我们更好的改进 Serein！"
        );
        stringBuilder.AppendLine($"· GitHub Issue（{UrlConstants.Issues}）");
        stringBuilder.AppendLine("  【推荐】在GitHub上反馈，方便作者定位和跟踪问题");
        stringBuilder.AppendLine($"· 交流群（{UrlConstants.Group}）");
        stringBuilder.AppendLine("  通过共同讨论分析和确定问题，但效率可能较低");
        stringBuilder.AppendLine();

        Console.WriteLine(stringBuilder);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("【注】反馈问题时你应该上传崩溃日志文件，而不是此窗口的截图");
        Console.ResetColor();

        if (!Console.IsInputRedirected)
            Console.ReadLine();

        if (e.HResult != 0 && context is not null)
            context.ExitCode = e.HResult;
    }

    private static void ShowWelcomePage(ILogger logger)
    {
        logger.LogInformation("");
        logger.LogInformation("欢迎使用Serein！！");
        logger.LogInformation("");
        logger.LogInformation(
            "如果你是第一次使用Serein，那么一定要仔细阅读以下内容，相信这些会对你有所帮助(๑•̀ㅂ•́)و✧"
        );
        logger.LogInformation("· 官网文档（{}）", UrlConstants.Docs);
        logger.LogInformation("  这里有详细完整的功能介绍和教程，推荐新手仔细阅读");
        logger.LogInformation("· GitHub仓库（{}）", UrlConstants.Repository);
        logger.LogInformation(
            "  这是储存 Serein 源代码的地方。欢迎每一个人为 Serein 的发展贡献力量"
        );
        logger.LogInformation("· 交流群（{}）", UrlConstants.Group);
        logger.LogInformation("  欢迎加群闲聊划水:)");

        logger.LogWarning("此软件与Mojang Studio、网易、Microsoft没有从属关系");
        logger.LogWarning("Serein is licensed under GPL-v3.0");
        logger.LogWarning("Copyright © 2022 Zaitonn. All Rights Reserved.");
    }

    private static void ShowWarningOfLogMode(ILogger logger)
    {
        logger.LogWarning("你开启了日志模式！");
        logger.LogWarning(
            $"在此模式下，应用程序会将完整的调试日志保存在\"{PathConstants.LogDirectory}/app\"目录下（可能很大很大很大，并对硬盘的读写速度产生一定影响）"
        );
        logger.LogWarning("除非你知道你在干什么 / 是开发者要求的，请不要在此模式下运行Serein！！");
        logger.LogWarning("");
        logger.LogWarning("当然你也不需要太担心，若要退出此模式只需要重新启动就行啦 :D");
    }

    private static void BuildApp()
    {
        var builder = SereinAppBuilder.CreateBuilder();

        builder.Logging.AddProvider(new CliLoggerProvider());
        builder
            .Services.AddHostedService<TitleUpdater>()
            .AddHostedService<InputReader>()
            .AddHostedService<CancelKeyHandlingService>()
            .AddSingleton<IConnectionLogger, ConnectionLogger>()
            .AddSingleton<IPluginLogger, PluginLogger>();

        var app = builder.Build();
        var logger = app.Services.GetRequiredService<ILogger<SereinApp>>();

        if (SereinApp.StartForTheFirstTime)
            ShowWelcomePage(logger);

        if (FileLoggerProvider.IsEnable)
            ShowWarningOfLogMode(logger);

        app.Run();
    }
}
