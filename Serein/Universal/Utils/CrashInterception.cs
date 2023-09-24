#if CONSOLE
using Serein.Utils.Output;
#else
using Ookii.Dialogs.Wpf;
#endif
using Serein.Core.Common;
using Serein.Core.Server;
using Serein.Utils.IO;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Serein.Utils
{
    internal static class CrashInterception
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            AppDomain.CurrentDomain.UnhandledException += (_, e) =>
                ShowException((Exception)e.ExceptionObject);
            TaskScheduler.UnobservedTaskException += (_, e) => ShowException(e.Exception);
        }

        /// <summary>
        /// 显示错误消息
        /// </summary>
        /// <param name="e">错误消息</param>
        public static void ShowException(Exception e)
        {
            if (Global.Settings.Server.AutoStop)
            {
                ServerManager.Stop(true);
            }
            Directory.CreateDirectory(Path.Combine("logs", "crash"));

            string exceptionMsg = MergeException(e);
            try
            {
                lock (Log.FileLock.Crash)
                {
                    File.AppendAllText(
                        Path.Combine("logs", "crash", $"{DateTime.Now:yyyy-MM-dd}.log"),
                        DateTime.Now
                            + "  |  "
                            + $"{Global.VERSION}-{Global.INTERNAL_VERSION} - {Global.TYPE}"
                            + "  |  "
                            + "NET"
                            + Environment.Version.ToString()
                            + Environment.NewLine
                            + Global.BuildInfo.ToString()
                            + Environment.NewLine
                            + exceptionMsg
                            + Environment.NewLine
                            + Environment.NewLine,
                        Encoding.UTF8
                    );
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
            EventTrigger.Trigger(Base.EventType.SereinCrash);
#if CONSOLE
            Logger.Output(
                Base.LogType.Error,
                $"唔……发生了一点小问题(っ °Д °;)っ\r\n"
                    + $"{exceptionMsg}\r\n\r\n"
                    + $"崩溃日志已保存在 {Path.Combine("logs", "crash", $"{DateTime.Now:yyyy-MM-dd}.log")}\r\n"
                    + "反馈此问题可以帮助作者更好的改进Serein"
            );
#else

            TaskDialog taskDialog =
                new()
                {
                    Buttons = { new(ButtonType.Ok) },
                    MainInstruction = "唔……发生了一点小问题(っ °Д °;)っ",
                    WindowTitle = "Serein",
                    Content =
                        ""
                        + $"版本：{Global.VERSION}-{Global.INTERNAL_VERSION} - {Global.TYPE}\n"
                        + $"时间：{DateTime.Now}\n"
                        + $"NET版本：{Environment.Version}\n"
                        + $"编译时间：{Global.BuildInfo.Time}\n\n"
                        + $"◦ 崩溃日志已保存在 {Path.Combine("logs", "crash", $"{DateTime.Now:yyyy-MM-dd}.log")}\n"
                        + $"◦ 反馈此问题可以帮助作者更好的改进Serein",
                    MainIcon = TaskDialogIcon.Error,
                    Footer =
                        $"你可以<a href=\"https://github.com/Zaitonn/Serein/issues/new?labels=%E2%9D%97+%E5%B4%A9%E6%BA%83&template=crush_report.yml&title=崩溃反馈+{e.GetType()}\">提交Issue</a>或<a href=\"https://jq.qq.com/?_wv=1027&k=XNZqPSPv\">加群</a>反馈此问题",
                    FooterIcon = TaskDialogIcon.Information,
                    EnableHyperlinks = true,
                    ExpandedInformation = exceptionMsg
                };
            taskDialog.HyperlinkClicked += (_, e) =>
                Process.Start(new ProcessStartInfo(e.Href) { UseShellExecute = true });
            taskDialog.ShowDialog();
#endif
        }

        /// <summary>
        /// 合并错误信息
        /// </summary>
        /// <param name="e">错误信息</param>
        /// <returns>错误信息</returns>
        public static string MergeException(Exception? e)
        {
            string message = string.Empty;
            while (e != null)
            {
                message = e + Environment.NewLine + message;
                e = e?.InnerException;
            }
            return message;
        }
    }
}
