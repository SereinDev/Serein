#if !CONSOLE
using Ookii.Dialogs.Wpf;
#endif 
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
            AppDomain.CurrentDomain.UnhandledException += (sneder, e) => ShowException((Exception)e.ExceptionObject);
            TaskScheduler.UnobservedTaskException += (sender, e) => ShowException(e.Exception);
        }

        /// <summary>
        /// 显示错误消息
        /// </summary>
        /// <param name="e">错误消息</param>
        public static void ShowException(Exception e)
        {
            if (Global.Settings.Server.AutoStop)
                ServerManager.Stop(true);
            if (!Directory.Exists(IO.GetPath("logs", "crash")))
                Directory.CreateDirectory(IO.GetPath("logs", "crash"));
            string ExceptionMsg = MergeException(e);
            try
            {
                File.AppendAllText(
                    IO.GetPath("logs", "crash", $"{DateTime.Now:yyyy-MM-dd}.log"),
                    DateTime.Now + "  |  "
                    + Global.VERSION + "  |  " +
                    "NET" + Environment.Version.ToString() +
                    "\n" +
                    Global.BuildInfo.ToString() +
                    "\n" +
                    ExceptionMsg +
                    "\n\n",
                    Encoding.UTF8
                    );
            }
            catch { }
            EventTrigger.Trigger(Items.EventType.SereinCrash);
#if CONSOLE
            Logger.Out(Items.LogType.Error,
                $"唔……发生了一点小问题(っ °Д °;)っ\r\n" +
                $"{ExceptionMsg}\r\n\r\n" +
                $"崩溃日志已保存在 {IO.GetPath("logs", "crash", $"{DateTime.Now:yyyy-MM-dd}.log")}\r\n" +
                "反馈此问题可以帮助作者更好的改进Serein");
#else

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
                    $"NET版本：{Environment.Version}\n" +
                    $"编译时间：{Global.BuildInfo.Time}\n\n" +
                    $"◦ 崩溃日志已保存在 {IO.GetPath("logs", "crash", $"{DateTime.Now:yyyy-MM-dd}.log")}\n" +
                    $"◦ 反馈此问题可以帮助作者更好的改进Serein",
                MainIcon = TaskDialogIcon.Error,
                Footer = "你可以<a href=\"https://github.com/Zaitonn/Serein/issues/new\">提交Issue</a>或<a href=\"https://jq.qq.com/?_wv=1027&k=XNZqPSPv\">加群</a>反馈此问题",
                FooterIcon = TaskDialogIcon.Information,
                EnableHyperlinks = true,
                ExpandedInformation = ExceptionMsg,
            };
            TaskDialog.HyperlinkClicked += HyperlinkClicked;
            TaskDialog.ShowDialog();
#endif
        }

#if !CONSOLE
        /// <summary>
        /// 链接点击处理
        /// </summary>
        private static void HyperlinkClicked(object sender, HyperlinkClickedEventArgs e)
            => Process.Start(new ProcessStartInfo(e.Href) { UseShellExecute = true });
#endif

        /// <summary>
        /// 合并错误信息
        /// </summary>
        /// <param name="e">错误信息</param>
        /// <returns>错误信息</returns>
        private static string MergeException(Exception e)
        {
            string Message = string.Empty;
            while (e != null)
            {
                Message += e.ToString() + "\r\n";
                e = e.InnerException;
            }
            return Message;
        }
    }
}