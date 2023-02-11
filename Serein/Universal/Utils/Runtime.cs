using System.IO;
using System;
using System.Collections.Generic;
using Serein.Core;
using Serein.Extensions;
using Serein.Core.JSPlugin;
using Serein.Core.Server;
using System.Threading.Tasks;
#if !CONSOLE
using Ookii.Dialogs.Wpf;
using System.Diagnostics;
#endif

namespace Serein.Utils
{
    internal static class Runtime
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            CrashInterception.Init();
            Directory.SetCurrentDirectory(Global.PATH);
#if WINFORM
            ResourcesManager.InitConsole();
#endif
            IO.ReadAll();
            Task.Run(SystemInfo.Init);
#if !CONSOLE
            AppDomain.CurrentDomain.ProcessExit += (_, _) => IO.Timer.Stop();
#endif
            AppDomain.CurrentDomain.ProcessExit += (_, _) => IO.LazyTimer.Stop();
            AppDomain.CurrentDomain.ProcessExit += (_, _) => IO.Update();
        }

        /// <summary>
        /// 开始核心功能调用
        /// </summary>
        public static void Start()
        {
            TaskRunner.Start();
            Net.Init();
            IO.StartSaving();
            Task.Run(JSPluginManager.Load);
            if (Global.FirstOpen) { ShowWelcomePage(); }
            if (File.Exists("Updater.exe")) { File.Delete("Updater.exe"); }
            IList<string> args = Environment.GetCommandLineArgs();
            Task.Run(() =>
            {
                (Global.Settings.Serein.AutoRun.Delay > 0 ? Global.Settings.Serein.AutoRun.Delay : 0).ToSleep();
                if (Global.Settings.Serein.AutoRun.ConnectWS || args.Contains("auto_connect"))
                {
                    Task.Run(Websocket.Open);
                }
                if (Global.Settings.Serein.AutoRun.StartServer || args.Contains("auto_start"))
                {
                    Task.Run(ServerManager.Start);
                }
            });
        }

        /// <summary>
        /// 显示欢迎页面
        /// </summary>
        public static void ShowWelcomePage()
        {
#if !CONSOLE
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
            taskDialog.HyperlinkClicked += (_, e) => Process.Start(new ProcessStartInfo(e.Href) { UseShellExecute = true });
            taskDialog.ShowDialog();
#else       
            Logger.Output(Base.LogType.Info,
                "欢迎使用Serein！！\n" +
                "如果你是第一次使用Serein，那么一定要仔细阅读以下内容，相信这些会对你有所帮助OwO\n" +
                "◦ 官网文档：\x1b[4m\x1b[36mhttps://serein.cc\x1b[0m\n" +
                "◦ GitHub仓库：\x1b[4m\x1b[36mhttps://github.com/Zaitonn/Serein\x1b[0m\n" +
                "◦ 交流群：\x1b[4m\x1b[36m954829203\x1b[0m\n" +
                "（控制台不支持超链接，你可以复制后到浏览器中打开）");
#endif
        }
    }
}