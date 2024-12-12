using System;
using Serein.Core;
using Serein.Core.Utils;
using Serein.Core.Utils.Extensions;
#if LITE
using Ookii.Dialogs.WinForms;

namespace Serein.Lite.Utils;

#elif PLUS
using Ookii.Dialogs.Wpf;

namespace Serein.Plus.Utils;

#endif

public static class DialogFactory
{
    public static void ShowWelcomeDialog()
    {
        var button1 = new TaskDialogButton("官网文档")
        {
            CommandLinkNote = "这里有详细完整的功能介绍和教程，推荐新手仔细阅读",
        };
        var button2 = new TaskDialogButton("GitHub仓库")
        {
            CommandLinkNote = "这是储存 Serein 源代码的地方。欢迎每一个人为 Serein 的发展贡献力量",
        };

        var dialog = new TaskDialog
        {
            Buttons = { new(ButtonType.Ok), button1, button2 },
            CenterParent = true,
            Content =
                "如果你是第一次使用Serein，那么一定要仔细阅读以下内容，相信这些会对你有所帮助(๑•̀ㅂ•́)و✧",
            EnableHyperlinks = true,
            Footer =
                $"使用此软件即视为你已阅读并同意了<a href=\"{UrlConstants.DocsArgument}\">用户协议</a>",
            ExpandedInformation =
                "此软件与Mojang Studio、网易、Microsoft没有从属关系\n"
                + $"Serein is licensed under <a href=\"{UrlConstants.License}\">GPL-v3.0</a>\n"
                + $"Copyright © 2022 <a href=\"{UrlConstants.Author}\">Zaitonn</a>. All Rights Reserved.",
            FooterIcon = TaskDialogIcon.Information,
            MainInstruction = "欢迎使用Serein！！",
            WindowTitle = "Serein." + SereinApp.Type,
            ButtonStyle = TaskDialogButtonStyle.CommandLinks,
        };

        dialog.HyperlinkClicked += (_, e) => e.Href.OpenInBrowser();

        var btn = dialog.ShowDialog();

        if (btn == button1)
        {
            UrlConstants.Docs.OpenInBrowser();
        }
        else if (btn == button2)
        {
            UrlConstants.Repository.OpenInBrowser();
        }
    }

    public static void ShowErrorDialog(Exception e)
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
                WindowTitle = "Serein." + SereinApp.Type,
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

#if LITE
    public static void ShowWarningDialogOfLogMode()
    {
        var dialog = new TaskDialog
        {
            Buttons = { new(ButtonType.Ok) },
            CenterParent = true,
            Content =
                $"在此模式下，应用程序会将完整的调试日志保存在\"{PathConstants.LogDirectory}/app\"目录下（可能很大很大很大，并对硬盘的读写速度产生一定影响）\r\n"
                + "除非你知道你在干什么 / 是开发者要求的，请不要在此模式下运行Serein！！\r\n\r\n"
                + "当然你也不需要太担心，若要退出此模式只需要重新启动就行啦 :D",
            MainIcon = TaskDialogIcon.Warning,
            MainInstruction = "嘿！你开启了日志模式",
            WindowTitle = "Serein.Lite",
        };

        dialog.ShowDialog();
    }
#endif
}
