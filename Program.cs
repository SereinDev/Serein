using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Serein
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledException);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Serein serein = new Serein();
            Global.serein = serein;
            Application.Run(serein);
        }
        private static void ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            MessageBox.Show(
                $"线程异常：\n" +
                $"{e.Exception.StackTrace}\n" +
                $"版本： {Global.VERSION}\n" +
                $"时间：{DateTime.Now}\n\n\n" +
                $"若有必要，请在GitHub提交Issue反馈此问题",
                $"Serein", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(
                $"程序异常：\n" +
                $"{e.ExceptionObject}\n" +
                $"版本： {Global.VERSION}\n" +
                $"时间：{DateTime.Now}\n\n\n" +
                $"若有必要，请在GitHub提交Issue反馈此问题",
                $"Serein", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
