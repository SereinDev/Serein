using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public static bool Status = false;
        static ProcessStartInfo ServerProcessInfo;
        static Process ServerProcess;
        static Thread WaitForExitThread, RestartTimerThread;
        static bool Killed;
        static StreamWriter CommandWriter,LogWriter;

        public static void Start() 
        {
            if (string.IsNullOrEmpty(Global.Settings_server.Path) || string.IsNullOrWhiteSpace(Global.Settings_server.Path))
            {
                MessageBox.Show(":(\n启动路径为空.", "Serein", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (!File.Exists(Global.Settings_server.Path))
            {
                MessageBox.Show($":(\n启动文件\"{Global.Settings_server.Path}\"未找到.", "Serein", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (Status)
            {
                MessageBox.Show(":(\n服务器已在运行中.", "Serein", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                Global.serein.PanelConsoleWebBrowser_Invoke("#clear");
                Global.serein.PanelConsoleWebBrowser_Invoke("<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>启动中");
                ServerProcessInfo = new ProcessStartInfo(Global.Settings_server.Path);
                ServerProcessInfo.FileName = Global.Settings_server.Path;
                ServerProcessInfo.UseShellExecute = false;
                ServerProcessInfo.CreateNoWindow = true;
                ServerProcessInfo.RedirectStandardOutput = true;
                ServerProcessInfo.RedirectStandardInput = true;
                ServerProcessInfo.StandardOutputEncoding =Encoding.UTF8;
                ServerProcessInfo.WorkingDirectory = Path.GetDirectoryName(Global.Settings_server.Path);
                ServerProcess = Process.Start(ServerProcessInfo);
                CommandWriter = new StreamWriter(ServerProcess.StandardInput.BaseStream, Encoding.UTF8);
                CommandWriter.AutoFlush = true;
                CommandWriter.NewLine = "\n";
                ServerProcess.BeginOutputReadLine();
                ServerProcess.OutputDataReceived += new DataReceivedEventHandler(SortOutputHandler);
                Restart = Global.Settings_server.EnableRestart;
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
                MessageBox.Show(":(\n服务器不在运行中.", "Serein", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            else if (!Status)
            {
                MessageBox.Show(":(\n服务器不在运行中.","Serein",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }
        public static void InputCommand(string Command)
        {
            if (Status)
            {
                Command = Regex.Replace(Command, @"[\r\n]+?$", "");
                Command = Regex.Replace(Command, @"^[\r\n\s]+?", "");
                if (Global.Settings_server.EnableOutputCommand)
                {
                    Global.serein.PanelConsoleWebBrowser_Invoke($">{Command}");
                }
                CommandWriter.WriteLine(Command.TrimEnd('\r','\n'));
                if (Global.Settings_server.EnableLog)
                {
                    if (!Directory.Exists(Global.PATH + "\\logs\\console"))
                    {
                        Directory.CreateDirectory(Global.PATH + "\\logs\\console");
                    }
                    LogWriter = new StreamWriter(
                        Global.PATH + $"\\logs\\console\\{DateTime.Now:yyyy-MM-dd}.log",
                        true,
                        Encoding.UTF8
                        );
                    LogWriter.WriteLine(">"+Log.OutputRecognition(Command));
                    LogWriter.Flush();
                    LogWriter.Close();
                }
            }
        }
        private static void SortOutputHandler(object sendingProcess,DataReceivedEventArgs outLine)
        { 
            if (!string.IsNullOrEmpty(outLine.Data))
            {
                Global.serein.PanelConsoleWebBrowser_Invoke(
                    Log.ColorLog(outLine.Data,Global.Settings_server.OutputStyle));
                if (Global.Settings_server.EnableLog)
                {
                    if (!Directory.Exists(Global.PATH + "\\logs\\console"))
                    {
                        Directory.CreateDirectory(Global.PATH + "\\logs\\console");
                    }
                    LogWriter = new StreamWriter(
                        Global.PATH + $"\\logs\\console\\{DateTime.Now:yyyy-MM-dd}.log",
                        true,
                        Encoding.UTF8
                        );
                    LogWriter.WriteLine(Log.OutputRecognition(outLine.Data));
                    LogWriter.Flush();
                    LogWriter.Close();
                }
            }
        }
        public static void WaitForExit()
        {
            ServerProcess.WaitForExit();
            Status = false;
            CommandWriter.Close();
            if (! Killed && ServerProcess.ExitCode != 0)
            {
                Global.serein.PanelConsoleWebBrowser_Invoke(
                $"<br><span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>进程疑似非正常退出（返回：{ServerProcess.ExitCode}）"
                );
                if(Global.Settings_server.EnableRestart)
                {
                    Restart = true;
                }
            }
            else
            {
                Global.serein.PanelConsoleWebBrowser_Invoke(
                $"<br><span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>进程已退出（返回：{ServerProcess.ExitCode}）"
                );
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
                MessageBox.Show(":(\n服务器不在运行中.", "Serein", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
