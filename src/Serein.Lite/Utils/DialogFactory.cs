using Ookii.Dialogs.WinForms;

using Serein.Core.Utils.Extensions;

namespace Serein.Lite.Utils;

public static class DialogFactory
{
    public static void ShowWelcomeDialog()
    {
        var dialog = new TaskDialog
        {
            Buttons = { new(ButtonType.Ok) },
            CenterParent = true,
            Content =
                "如果你是第一次使用Serein，那么一定要仔细阅读以下内容，相信这些会对你有所帮助(๑•̀ㅂ•́)و✧\n"
                + "◦ 官网文档：<a href=\"https://sereindev.github.io\">https://sereindev.github.io</a>\n"
                + "◦ GitHub仓库：<a href=\"https://github.com/SereinDev/Serein\">https://github.com/SereinDev/Serein</a>\n",
            EnableHyperlinks = true,
            Footer = "使用此软件即视为你已阅读并同意了<a href=\"https://sereindev.github.io/docs/more/agreement\">用户协议</a>",
            ExpandedInformation =
                "此软件与Mojang Studio、网易、Microsoft没有从属关系\n"
                + "Serein is licensed under <a href=\"https://github.com/SereinDev/Serein/blob/main/LICENSE\">GPL-v3.0</a>\n"
                + "Copyright © 2022 <a href=\"https://github.com/Zaitonn\">Zaitonn</a>. All Rights Reserved.",
            FooterIcon = TaskDialogIcon.Information,
            MainInstruction = "欢迎使用Serein！！",
            WindowTitle = "Serein.Lite",
        };

        dialog.HyperlinkClicked += (_, e) => e.Href.OpenInBrowser();

        dialog.ShowDialog();
    }
}
