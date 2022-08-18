using Serein.Base;
using Serein.Server;
using System;
using System.IO;
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
        /// <param name="args">启动参数</param>
        [STAThread]
        private static void Main(string[] args)
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += new ThreadExceptionEventHandler(ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledException);
            Console.Base.Init();
            Console.Base.Load(args);
            Console.Base.Start();
        }
        private static void ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Abort(e.Exception);
        }

        private static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Abort(e.ExceptionObject);
        }

        /// <summary>
        /// Serein错误处理
        /// </summary>
        /// <param name="obj">Exception</param>
        private static void Abort(object obj)
        {
            Global.Crash = true;
            if (ServerManager.Status && Global.Settings.Server.AutoStop)
            {
                foreach (string Command in Global.Settings.Server.StopCommand.Split(';'))
                {
                    ServerManager.InputCommand(Command);
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
                    "\n===============================================\n"
                    );
                LogWriter.Flush();
                LogWriter.Close();
            }
            catch { }
            EventTrigger.Trigger("Serein_Crash");
            Global.Logger(3, $"{obj}\r\n\r\n" +
                $"崩溃日志已保存在{Global.Path + $"logs\\crash\\{DateTime.Now:yyyy-MM-dd}.log"}\r\n" +
                "反馈此问题可以帮助作者更好的改进Serein");
            Global.Crash = false;
        }
    }
}
