using Ookii.Dialogs.Wpf;
using Serein.Base;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Serein
{

    /*
     *  ____ 
     * /\  _`\                        __            
     * \ \,\L\_\     __   _ __    __ /\_\    ___    
     *  \/_\__ \   /'__`\/\`'__\/'__`\/\ \ /' _ `\  
     *    /\ \L\ \/\  __/\ \ \//\  __/\ \ \/\ \/\ \ 
     *    \ `\____\ \____\\ \_\\ \____\\ \_\ \_\ \_\
     *     \/_____/\/____/ \/_/ \/____/ \/_/\/_/\/_/
     *     
     *     https://github.com/Zaitonn/Serein
     *  Copyright © 2022 Zaitonn. All Rights Reserved.
     *     
     */

    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += new ThreadExceptionEventHandler(ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledException);
            Settings.Base.ReadSettings();
            if (((IList)args).Contains("debug"))
            {
                Global.Settings.Serein.Debug = true;
            }
            if (Global.Settings.Serein.DPIAware && Environment.OSVersion.Version.Major >= 6)
            {
                SetProcessDPIAware();
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (!File.Exists(Global.Path + "console\\console.html"))
            {
                ResourcesManager.InitConsole();
                Global.FirstOpen = true;
            }
            Ui.Ui ui = new Ui.Ui();
            Global.Ui = ui;
            Application.Run(ui);
        }

        private static void ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Abort(e.Exception);
        }

        private static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Abort(e.ExceptionObject);
        }

        private static void Abort(object obj)
        {
            Global.Crash = true;
            if (Base.Server.Status && Global.Settings.Server.AutoStop)
            {
                foreach (string Command in Global.Settings.Server.StopCommand.Split(';'))
                {
                    Base.Server.InputCommand(Command);
                }
            }
            if (!Directory.Exists(Global.Path + "\\logs\\crash"))
            {
                Directory.CreateDirectory(Global.Path + "\\logs\\crash");
            }
            try
            {
                StreamWriter LogWriter = new StreamWriter(
                    Global.Path + $"\\logs\\crash\\{DateTime.Now:yyyy-MM-dd}.log",
                    true,
                    Encoding.UTF8
                    );
                LogWriter.WriteLine(
                    DateTime.Now + "  |  "
                    + Global.VERSION + "  |  " +
                    "NET" + Environment.Version.ToString() +
                    "\n" +
                    obj.ToString() +
                    "\n==============================================="
                    );
                LogWriter.Flush();
                LogWriter.Close();
            }
            catch { }
            EventTrigger.Trigger("Serein_Crash");
            Ookii.Dialogs.Wpf.TaskDialog TaskDialog = new Ookii.Dialogs.Wpf.TaskDialog
            {
                Buttons = {
                        new Ookii.Dialogs.Wpf.TaskDialogButton(ButtonType.Ok)
                    },
                MainInstruction = "唔……发生了一点小问题(っ °Д °;)っ",
                WindowTitle = "Serein",
                Content = "" +
                $"版本： {Global.VERSION}\n" +
                $"时间：{DateTime.Now}\n" +
                $"NET版本：{Environment.Version}\n\n" +
                $"◦ 崩溃日志已保存在{Global.Path + $"logs\\crash\\{DateTime.Now:yyyy-MM-dd}.log"}\n" +
                $"◦ 反馈此问题可以帮助作者更好的改进Serein",
                MainIcon = Ookii.Dialogs.Wpf.TaskDialogIcon.Error,
                Footer = "你可以<a href=\"https://github.com/Zaitonn/Serein/issues/new\">提交Issue</a>或<a href=\"https://jq.qq.com/?_wv=1027&k=XNZqPSPv\">加群</a>反馈此问题",
                FooterIcon = Ookii.Dialogs.Wpf.TaskDialogIcon.Information,
                EnableHyperlinks = true,
                ExpandedInformation = obj.ToString(),
            };
            TaskDialog.HyperlinkClicked += new EventHandler<HyperlinkClickedEventArgs>(TaskDialog_HyperLinkClicked);
            TaskDialog.ShowDialog();
            Global.Crash = false;
        }
        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        private static void TaskDialog_HyperLinkClicked(object sender, HyperlinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Href) { UseShellExecute = true });
        }
    }
}
