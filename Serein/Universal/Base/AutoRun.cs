#if !CONSOLE
using Ookii.Dialogs.Wpf;
using System.Diagnostics;
#endif
using Serein.Server;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Serein.Base
{
    internal static class AutoRun
    {
        /// <summary>
        /// 检查启动参数
        /// </summary>
        public static void Check()
        {
#if !CONSOLE
            if (Global.FirstOpen)
            {
                ShowWelcomePage();
            }
#endif
            IList<string> args = Environment.GetCommandLineArgs();
            if (Global.Settings.Serein.AutoRun.ConnectWS || args.Contains("auto_connect"))
            {
                Task.Run(() => Websocket.Connect(true));
            }
            if (Global.Settings.Serein.AutoRun.StartServer || args.Contains("auto_start"))
            {
                Task.Run(() => ServerManager.Start(false));
            }
        }

#if !CONSOLE
        /// <summary>
        /// 显示欢迎页面
        /// </summary>
        public static void ShowWelcomePage()
        {
            TaskDialog taskDialog = new()
            {
                Buttons = {
                        new TaskDialogButton(ButtonType.Ok)
                    },
                MainInstruction = "欢迎使用Serein！！",
                WindowTitle = "Serein",
                Content = "" +
                    "如果你是第一次使用Serein，那么一定要仔细阅读以下内容，相信这些会对你有所帮助(๑•̀ㅂ•́)و✧\n" +
                    "◦ 官网文档：<a href=\"https://serein.cc\">https://serein.cc</a>\n" +
                    "◦ GitHub仓库：<a href=\"https://github.com/Zaitonn/Serein\">https://github.com/Zaitonn/Serein</a>\n" +
                    "◦ 交流群：<a href=\"https://jq.qq.com/?_wv=1027&k=XNZqPSPv\">954829203</a>",
                Footer = "此面板已发布在<a href=\"https://www.minebbs.com/resources/serein.4169/\">Minebbs</a>上，欢迎支持~",
                FooterIcon = TaskDialogIcon.Information,
                EnableHyperlinks = true,
                ExpandedInformation = "此软件与Mojang Studio、网易、Microsoft没有从属关系.\n" +
                     "Serein is licensed under <a href=\"https://github.com/Zaitonn/Serein/blob/main/LICENSE\">GPL-v3.0</a>\n" +
                     "Copyright © 2022 <a href=\"https://github.com/Zaitonn\">Zaitonn</a>. All Rights Reserved.",
            };
            taskDialog.HyperlinkClicked += (sneder, e) => Process.Start(new ProcessStartInfo(e.Href) { UseShellExecute = true });
            taskDialog.ShowDialog();
        }
#endif
    }
}