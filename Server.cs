using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

namespace Serein
{
     public class Server
     {
        public static bool Restart = false;
        public static string Path = @"";
        public static bool Status = false;
        static ProcessStartInfo ServerProcessInfo;
        static Process ServerProcess;
        static object[] objects = new object[1];
        static Thread WaitForExitThread, RestartTimerThread;
        static bool Killed;

        public static void Start() 
        {
            if (string.IsNullOrEmpty(Path) || string.IsNullOrWhiteSpace(Path))
            {
                MessageBox.Show(":(\n启动路径为空.");
            }
            else if (!File.Exists(Path))
            {
                MessageBox.Show($":(\n启动文件\"{Path}\"未找到.");
            }
            else if (Status)
            {
                MessageBox.Show(":(\n服务器已在运行中.");
            }
            else
            {
                Global.serein.PanelConsoleWebBrowser_Invoke("#clear");
                Global.serein.PanelConsoleWebBrowser_Invoke("<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>启动中");
                ServerProcessInfo = new ProcessStartInfo(Path);
                ServerProcessInfo.FileName = Path;
                ServerProcessInfo.UseShellExecute = false;
                ServerProcessInfo.CreateNoWindow = true;
                ServerProcessInfo.RedirectStandardOutput = true;
                ServerProcessInfo.RedirectStandardInput = true;
                ServerProcessInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(Path);
                ServerProcess = Process.Start(ServerProcessInfo);
                ServerProcess.BeginOutputReadLine();
                ServerProcess.OutputDataReceived += new DataReceivedEventHandler(SortOutputHandler);
                Restart = Settings.Server.EnableRestart;
                Status = true;
                Killed = false;
                WaitForExitThread = new Thread(WaitForExit);
                WaitForExitThread.IsBackground = true;
                WaitForExitThread.Start();
                Global.serein.PanelConsoleWebBrowser_Invoke("<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>监听进程已启动");
            }
        }
        public static void Stop()
        {
            if (Status)
            {
                InputCommand("stop");
            }
            else if (!Status && Restart)
            {
                Restart = false;
            }
            else
            {
                MessageBox.Show(":(\n服务器不在运行中.");
            }
        }
        public static void Kill()
        {
            if (
                Status 
                &&
                (int)MessageBox.Show(
                    "确定结束进程吗？\n此操作可能导致存档损坏等问题",
                    "Serein",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning
                    )==1
                )
            {
                ServerProcess.Kill();
                Killed = true;
            }
            else
            {
                MessageBox.Show(":(\n服务器不在运行中.");
            }
        }
        public static void InputCommand(string Command)
        {
            if (Status)
            {
                if (Settings.Server.EnableOutputCommand)
                {
                    Global.serein.PanelConsoleWebBrowser_Invoke($">{Command}");
                }
                ServerProcess.StandardInput.WriteLine(Command);
            }
        }
        private static void SortOutputHandler(object sendingProcess,DataReceivedEventArgs outLine)
        { 
            if (!string.IsNullOrEmpty(outLine.Data))
            {
                Global.serein.PanelConsoleWebBrowser_Invoke(Log.ColorLog(outLine.Data, 2));
            }
        }
        public static void WaitForExit()
        {
            ServerProcess.WaitForExit();
            Status = false;
            Global.serein.PanelConsoleWebBrowser_Invoke(
                $"<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>进程已退出（返回：{ServerProcess.ExitCode}）"
                );
            if (Killed && ServerProcess.ExitCode != 0)
            {
                Global.serein.PanelConsoleWebBrowser_Invoke(
                $"<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>进程疑似非正常退出"
                );
                if(Settings.Server.EnableRestart)
                {
                    Restart = true;
                } 
            }
            if (Restart)
            {
                RestartTimerThread = new Thread(RestartTimer);
                RestartTimerThread.IsBackground = true;
                RestartTimerThread.Start();
            }
            WaitForExitThread.Abort();
        }
        public static void RestartRequest()
        {
            if (Status)
            {
                InputCommand("stop");
                Restart = true;
            }
            else
            {
                MessageBox.Show(":(\n服务器不在运行中.");
            }
        }
        public static void WaitForExitThreadStart()
        {
            WaitForExitThread.IsBackground = true;
            //WaitForExitThread.Start();
        }
        private static void RestartTimer()
        {
            Global.serein.PanelConsoleWebBrowser_Invoke(
                $"<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>服务器将在5s后重新启动."
                );
            Global.serein.PanelConsoleWebBrowser_Invoke(
                $"<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>你可以按下停止按钮来取消这次重启."
                );
            for (int i = 0; i < 10; i++)
            {
                if (! Restart)
                {
                    break;
                }
                RestartTimerThread.Join(500);
            }
            if (Restart)
            {
                Start();
            }
            else
            {
                Global.serein.PanelConsoleWebBrowser_Invoke(
                $"<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>重启已取消."
                );
            }
            RestartTimerThread.Abort();
        }
     }
}
