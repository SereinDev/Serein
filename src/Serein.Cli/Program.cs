using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serein.Cli.Interaction;
using Serein.Cli.Loggers;
using Serein.Core;
using Serein.Core.Models.Output;
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
        var fileName = CrashHelper.CreateLog(e);

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("唔……崩溃了(っ °Д °;)っ");
        Console.WriteLine(e.ToString());
        Console.WriteLine();
        Console.WriteLine($"完整崩溃日志已保存在 {fileName}，请先善用搜索引擎寻找解决方案。");
        Console.WriteLine("在确定不是自身问题（如文件语法不正确、文件缺失等）后，你可以通过以下方式反馈此问题，帮助我们更好的改进 Serein！");
        Console.WriteLine($"· GitHub Issue（{UrlConstants.Issues}）");
        Console.WriteLine("  【推荐】在GitHub上反馈，方便作者定位和跟踪问题");
        Console.WriteLine($"· 交流群（{UrlConstants.Group}）");
        Console.WriteLine("  通过共同讨论分析和确定问题");
        Console.WriteLine();
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
        logger.LogInformation("如果你是第一次使用Serein，那么一定要仔细阅读以下内容，相信这些会对你有所帮助(๑•̀ㅂ•́)و✧");
        logger.LogInformation("· 官网文档（{}）", UrlConstants.Docs);
        logger.LogInformation("  这里有详细完整的功能介绍和教程，推荐新手仔细阅读");
        logger.LogInformation("· GitHub仓库（{}）", UrlConstants.Repository);
        logger.LogInformation("  这是储存 Serein 源代码的地方。欢迎每一个人为 Serein 的发展贡献力量");
        logger.LogInformation("· 交流群（{}）", UrlConstants.Group);
        logger.LogInformation("  欢迎加群闲聊划水:)");

        logger.LogWarning("此软件与Mojang Studio、网易、Microsoft没有从属关系");
        logger.LogWarning("Serein is licensed under GPL-v3.0");
        logger.LogWarning("Copyright © 2022 Zaitonn. All Rights Reserved.");
    }

    private static void BuildApp()
    {
        var builder = new SereinAppBuilder();
        builder.ConfigureService();

        builder.Services.AddHostedService<TitleUpdater>();
        builder.Services.AddHostedService<InputReader>();

        builder.Services.AddSingleton<IConnectionLogger, ConnectionLogger>();
        builder.Services.AddSingleton<IPluginLogger, PluginLogger>();
        builder.Services.AddSingleton<ILogger, CliLogger>();

        var app = builder.Build();

        if (SereinApp.StartForTheFirstTime)
            ShowWelcomePage(app.Services.GetRequiredService<ILogger>());

        app.Run();
    }
}
