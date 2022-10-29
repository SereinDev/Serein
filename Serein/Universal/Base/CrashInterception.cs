using Ookii.Dialogs.Wpf;
using Serein.Server;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Serein.Base
{
    internal static class CrashInterception
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            AppDomain.CurrentDomain.UnhandledException += (sneder, e) => ShowException(e.ExceptionObject.ToString());
            TaskScheduler.UnobservedTaskException += (sender, e) => ShowException(e.Exception.ToString());
        }

        /// <summary>
        /// 显示错误消息
        /// </summary>
        /// <param name="ExceptionMsg">错误消息</param>
        public static void ShowException(string ExceptionMsg)
        {
            if (ServerManager.Status && Global.Settings.Server.AutoStop)
            {
                foreach (string Command in Global.Settings.Server.StopCommands)
                {
                    ServerManager.InputCommand(Command);
                }
            }
            if (!Directory.Exists(Global.Path + "\\logs\\crash"))
                Directory.CreateDirectory(Global.Path + "\\logs\\crash");
            try
            {
                File.AppendAllText(
                    Global.Path + $"\\logs\\crash\\{DateTime.Now:yyyy-MM-dd}.log",
                    DateTime.Now + "  |  "
                    + Global.VERSION + "  |  " +
                    "NET" + Environment.Version.ToString() +
                    "\n" +
                    ExceptionMsg +
                    "\n===============================================\n",
                    Encoding.UTF8
                    );
            }
            catch { }
            EventTrigger.Trigger("Serein_Crash");
            if (Logger.Type == 0)
                Logger.Out(Items.LogType.Error, $"唔……发生了一点小问题(っ °Д °;)っ\r\n" +
                    $"{ExceptionMsg}\r\n\r\n" +
                    $"崩溃日志已保存在{Global.Path + $"logs\\crash\\{DateTime.Now:yyyy-MM-dd}.log"}\r\n" +
                    "反馈此问题可以帮助作者更好的改进Serein");
            else
            {
                TaskDialog TaskDialog = new TaskDialog
                {
                    Buttons = {
                        new TaskDialogButton(ButtonType.Ok)
                    },
                    MainInstruction = "唔……发生了一点小问题(っ °Д °;)っ",
                    WindowTitle = "Serein",
                    Content = "" +
                $"版本： {Global.VERSION}\n" +
                $"时间：{DateTime.Now}\n" +
                $"NET版本：{Environment.Version}\n\n" +
                $"◦ 崩溃日志已保存在{Global.Path + $"logs\\crash\\{DateTime.Now:yyyy-MM-dd}.log"}\n" +
                $"◦ 反馈此问题可以帮助作者更好的改进Serein",
                    MainIcon = TaskDialogIcon.Error,
                    Footer = "你可以<a href=\"https://github.com/Zaitonn/Serein/issues/new\">提交Issue</a>或<a href=\"https://jq.qq.com/?_wv=1027&k=XNZqPSPv\">加群</a>反馈此问题",
                    FooterIcon = TaskDialogIcon.Information,
                    EnableHyperlinks = true,
                    ExpandedInformation = ExceptionMsg,
                };
                TaskDialog.HyperlinkClicked += (sender, e) => Process.Start(new ProcessStartInfo(e.Href) { UseShellExecute = true });
                TaskDialog.ShowDialog();
            }
        }
    }
}