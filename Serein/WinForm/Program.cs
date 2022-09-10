using Serein.Base;
using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
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
        public static Ui.Ui Ui = null;

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        /// <param name="args">启动参数</param>
        [STAThread]
        private static void Main(string[] args)
        {
            CrashInterception.Init();
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (sender, e) => CrashInterception.ShowException(e.Exception.ToString());
            Loader.ReadAll();
            Global.Args = args;
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
            Ui = new Ui.Ui();
            Application.Run(Ui);
        }

        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
    }
}
