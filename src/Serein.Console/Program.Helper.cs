using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Serein.Console.Utils;
using Serein.Core.Utils;
using Spectre.Console;
using SysConsole = System.Console;

namespace Serein.Console;

public static partial class Program
{
    private static void HandleException(Exception e)
    {
        var fileName = CrashHelper.CreateLog(e);

        SimpleConsole.WriteLine();
        SimpleConsole.Log(LogLevel.Critical, "唔……崩溃了(っ °Д °;)っ");
        SimpleConsole.WriteException(e);
        SimpleConsole.WriteLine();
        SimpleConsole.Log(
            LogLevel.Information,
            new Markup(
                $"完整崩溃日志已保存在 [underline]{fileName.EscapeMarkup()}[/]，请先善用搜索引擎寻找解决方案。"
            )
        );
        SimpleConsole.Log(
            LogLevel.Information,
            "在确定不是自身问题（如文件语法不正确、文件缺失等）后，你可以通过以下方式反馈此问题，帮助我们更好的改进 Serein！"
        );
        SimpleConsole.Log(
            LogLevel.Information,
            new Columns(
                new Panel(
                    new Rows(
                        new Text("在GitHub上反馈，方便作者定位和跟踪问题"),
                        new Markup($"→ [link CornflowerBlue underline]{UrlConstants.Issues}[/]")
                    )
                )
                    .RoundedBorder()
                    .Header("[LightGoldenrod2_1]推荐[/]·GitHub Issue"),
                new Panel(
                    new Rows(
                        new Text("通过共同讨论分析和确定问题，但效率可能较低"),
                        new Markup($"→ [link CornflowerBlue underline]{UrlConstants.Group}[/]")
                    )
                )
                    .RoundedBorder()
                    .Header("交流群")
            ).Collapse()
        );
        SimpleConsole.WriteLine(
            LogLevel.Warning,
            "反馈问题时你应该上传崩溃日志文件，而不是此窗口的截图"
        );
        SimpleConsole.WriteLine();
        SimpleConsole.WriteLine();
        SimpleConsole.WriteLine();

        if (!SysConsole.IsInputRedirected)
        {
            AnsiConsole.Write(
                new Rule("[silver]按回车键退出[/]") { Style = new Style(Color.Gray) }.Centered()
            );
            SysConsole.ReadLine();
        }
    }

    private static void ShowWelcomePage()
    {
        SimpleConsole.Log(
            LogLevel.Information,
            new Rows(
                new Markup(
                    "[bold white]欢迎使用 [DarkSeaGreen3]Serein.Console[/]！[/]\n"
                ).Centered(),
                new Text(
                    "如果你是第一次使用Serein，那么一定要仔细阅读以下内容，相信这些会对你有所帮助(๑•̀ㅂ•́)و✧"
                ).Centered(),
                new Table()
                    .AddColumn("_", (c) => c.Centered())
                    .AddColumn("_")
                    .HideHeaders()
                    .AddRow(
                        new Markup("官网文档", Color.White),
                        new Rows(
                            new Markup("这里有详细完整的功能介绍和教程，推荐新手仔细阅读"),
                            new Markup($"→ [link CornflowerBlue underline]{UrlConstants.Docs}[/]")
                        )
                    )
                    .AddRow(
                        new Markup("GitHub仓库", Color.White),
                        new Rows(
                            new Markup(
                                "这是储存 Serein 源代码的地方。欢迎每一个人为 Serein 的发展贡献力量"
                            ),
                            new Markup(
                                $"→ [link CornflowerBlue underline]{UrlConstants.Repository}[/]"
                            )
                        )
                    )
                    .AddRow(
                        new Markup("交流群", Color.White),
                        new Rows(
                            new Markup("欢迎加群闲聊划水:)"),
                            new Markup($"→ [link CornflowerBlue underline]{UrlConstants.Group}[/]")
                        )
                    )
                    .RoundedBorder()
                    .ShowRowSeparators()
                    .Caption(
                        "此软件与Mojang Studio、网易、Microsoft没有从属关系\n"
                            + "Serein is licensed under GPL-v3.0",
                        new(Color.Gray, decoration: Decoration.Italic)
                    )
                    .Expand()
            )
        );

        Task.Delay(2000).Wait();
    }

    private static void ShowWarningOfLogMode()
    {
        SimpleConsole.Log(
            LogLevel.Warning,
            new Panel(
                $"在此模式下，Serein会将完整的调试日志保存在\"[underline]{PathConstants.LogDirectory}/app[/]\"目录下（可能很大很大很大，并对硬盘的读写速度产生一定影响）\n\n"
                    + "除非你知道你在干什么/是开发者要求的，请不要在此模式下运行Serein！！"
            )
                .RoundedBorder()
                .BorderColor(Color.Yellow)
                .Header("[yellow bold] 你开启了日志模式！ [/]")
                .HeaderAlignment(Justify.Center)
        );

        SimpleConsole.Log(
            LogLevel.Information,
            "当然你也不需要太担心，若要退出此模式只需要重新启动就行啦"
        );
    }

    private static void CheckConflict()
    {
        var processes = BaseChecker.CheckConflictProcesses();
        if (processes.Count == 0)
        {
            return;
        }

        var table = new Table().AddColumn("PID", c => c.Centered()).AddColumn("进程名称").Expand();
        foreach (var process in processes)
        {
            table.AddRow(process.Id.ToString(), process.ProcessName);
        }

        SimpleConsole.Log(
            LogLevel.Warning,
            new Panel(table)
                .RoundedBorder()
                .BorderColor(Color.Yellow)
                .Header("[yellow bold] 检测到冲突进程！ [/]")
                .HeaderAlignment(Justify.Center)
        );

        SimpleConsole.Log(
            LogLevel.Warning,
            "在同一文件夹内同时运行多个Serein实例可能导致文件读写冲突或无法正确保存"
        );
    }
}
