using Serein.baseFunction;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Serein
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        private static void Main()
        {
            if (!File.Exists(Global.Path + "console\\console.html"))
            {
                ResourcesManager.InitConsole();
                Global.FirstOpen = true;
            }
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += new ThreadExceptionEventHandler(ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledException);
            Application.EnableVisualStyles();
            _ = Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.SetCompatibleTextRenderingDefault(false);
            Ui ui = new Ui();
            Global.Ui = ui;
            Application.Run(ui);
        }

        private static void ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Abort(e.Exception.StackTrace);
        }

        private static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Abort(e.ExceptionObject.ToString());
        }

        private static void Abort(string Text)
        {
            Global.Crash = true;
            if (Server.Status && Global.Settings_Server.AutoStop)
            {
                foreach (string Command in Global.Settings_Server.StopCommand.Split(';'))
                {
                    Server.InputCommand(Command);
                }
            }
            if (!Directory.Exists(Global.Path + "\\logs\\crash"))
            {
                _ = Directory.CreateDirectory(Global.Path + "\\logs\\crash");
            }
            try
            {
                StreamWriter LogWriter = new(
                    Global.Path + $"\\logs\\crash\\{DateTime.Now:yyyy-MM-dd}.log",
                    true,
                    Encoding.UTF8
                    );
                LogWriter.WriteLine(
                    DateTime.Now + "  |  " + Global.VERSION + "\n" +
                    Text +
                    "\n==============================================="
                    );
                LogWriter.Flush();
                LogWriter.Close();
            }
            catch { }
            _ = MessageBox.Show(
                "崩溃啦:(\n\n" +
                $"{Text}\n" +
                $"版本： {Global.VERSION}\n" +
                $"时间：{DateTime.Now}\n\n\n" +
                $"崩溃日志已保存在{Global.Path + $"logs\\crash\\{DateTime.Now:yyyy-MM-dd}.log"}\n" +
                "若有必要，请在GitHub提交Issue反馈此问题",
                "Serein", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Global.Crash = false;
        }
    }
}
