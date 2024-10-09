using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Text;

using Sentry;

using Serein.Core.Utils;

namespace Serein.Cli.Utils;

public static class CommandLineParserBuilder
{
    public static Parser Build(Action mainMethod)
    {
        var rootCommnad = new RootCommand();
        rootCommnad.SetHandler(mainMethod);

        rootCommnad.AddOption(new Option<bool>("--debug", "启用调试输出"));
        rootCommnad.AddOption(new Option<bool>("--log", "启用日志模式"));
        rootCommnad.AddOption(new Option<bool>("--no-color", "禁用控制台彩色输出（服务器输出不受影响）"));

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
}