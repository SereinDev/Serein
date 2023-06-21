using Serein.Core.Generic;
using Serein.Core.JSPlugin;
using Serein.Core.Server;
using Serein.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

#if !CONSOLE
using Ookii.Dialogs.Wpf;
#endif

namespace Serein.Utils
{
    internal static class Runtime
    {
        /// <summary>
        /// 命令行参数
        /// </summary>
        private static IList<string> _args => Environment.GetCommandLineArgs();

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            CrashInterception.Init();
            Debug.WriteLine(Global.LOGO);
            Directory.SetCurrentDirectory(Global.PATH);
#if WINFORM
            ResourcesManager.InitConsole();
#endif
            IO.ReadAll();
            Task.Run(SystemInfo.Init);
#if !CONSOLE
            AppDomain.CurrentDomain.ProcessExit += (_, _) => IO.Timer.Stop();
#endif
            if (_args.Contains("debug"))
            {
                Global.Settings.Serein.DevelopmentTool.EnableDebug = true;
            }
            AppDomain.CurrentDomain.ProcessExit += (_, _) => IO.LazyTimer.Stop();
        }

        /// <summary>
        /// 开始核心功能调用
        /// </summary>
        public static void Start()
        {
            TaskRunner.Start();
            Heartbeat.Start();
            Update.Init();
            IO.StartSaving();
            if (Global.FirstOpen)
            {
                ShowWelcomePage();
            }
            Task.Run(JSPluginManager.Load);
            Task.Run(() =>
            {
                (Global.Settings.Serein.AutoRun.Delay > 0 ? Global.Settings.Serein.AutoRun.Delay : 0).ToSleep();
                if (Global.Settings.Serein.AutoRun.ConnectWS || _args.Contains("auto_connect"))
                {
                    Task.Run(Websocket.Open);
                }
                if (Global.Settings.Serein.AutoRun.StartServer || _args.Contains("auto_start"))
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
                        new(ButtonType.Ok)
                    },
                MainInstruction = "欢迎使用Serein！！",
                WindowTitle = "Serein",
                Content = "" +
                    "如果你是第一次使用Serein，那么一定要仔细阅读以下内容，相信这些会对你有所帮助(๑•̀ㅂ•́)و✧\n" +
                    "◦ 官网文档：<a href=\"https://serein.cc\">https://serein.cc</a>\n" +
                    "◦ GitHub仓库：<a href=\"https://github.com/Zaitonn/Serein\">https://github.com/Zaitonn/Serein</a>\n" +
                    "◦ 交流群：<a href=\"https://jq.qq.com/?_wv=1027&k=XNZqPSPv\">954829203</a>",
                Footer = "使用此软件即视为你已阅读并同意了<a href=\"https://serein.cc/docs/more/agreement\">用户协议</a>",
                FooterIcon = TaskDialogIcon.Information,
                EnableHyperlinks = true,
                ExpandedInformation = "此软件与Mojang Studio、网易、Microsoft没有从属关系\n" +
                     "Serein is licensed under <a href=\"https://github.com/Zaitonn/Serein/blob/main/LICENSE\">GPL-v3.0</a>\n" +
                     "Copyright © 2022 <a href=\"https://github.com/Zaitonn\">Zaitonn</a>. All Rights Reserved.",
            };
            taskDialog.HyperlinkClicked += (_, e) => Process.Start(new ProcessStartInfo(e.Href) { UseShellExecute = true });
            taskDialog.ShowDialog();
#else       
            Logger.Output(Base.LogType.Info,
                "欢迎使用Serein！！\n" +
                "如果你是第一次使用Serein，那么一定要仔细阅读以下内容，相信这些会对你有所帮助OwO\n" +
                "◦ 官网文档：https://serein.cc\n" +
                "◦ GitHub仓库：https://github.com/Zaitonn/Serein\n" +
                "◦ 交流群：\x1b[4m\x1b[36m954829203\x1b[0m\n" +
                "◦ 使用此软件即视为你已阅读并同意了用户协议（https://serein.cc/docs/more/agreement）" +
                "（控制台不支持超链接，你可以复制后到浏览器中打开）");
#endif
        }
    }
}