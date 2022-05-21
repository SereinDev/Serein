using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Serein
{
    public class Server
    {
        public static string StartFileName = "", Version, LevelName, Difficulty;
        public static bool Status = false;
        public static bool Restart = false;
        static bool Finished = false;
        static ProcessStartInfo ServerProcessInfo;
        public static Process ServerProcess;
        static Thread WaitForExitThread, RestartTimerThread;
        static bool Killed;
        static StreamWriter CommandWriter, LogWriter;
        static TimeSpan PrevCpuTime = TimeSpan.Zero, CurTime = TimeSpan.Zero;

        public static void Start(bool StartedByCommand=false)
        {
            if (string.IsNullOrEmpty(Global.Settings_Server.Path) || string.IsNullOrWhiteSpace(Global.Settings_Server.Path))
            {
                if (!StartedByCommand)
                {
                    MessageBox.Show(":(\n启动路径为空.", "Serein", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else if (!File.Exists(Global.Settings_Server.Path))
            {
                if (!StartedByCommand)
                {
                    MessageBox.Show($":(\n启动文件\"{Global.Settings_Server.Path}\"未找到.", "Serein", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else if (Status)
            {
                if (!StartedByCommand)
                {
                    MessageBox.Show(":(\n服务器已在运行中.", "Serein", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                Global.Ui.PanelConsoleWebBrowser_Invoke("#clear");
                Global.Ui.PanelConsoleWebBrowser_Invoke("<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>启动中");
                ServerProcessInfo = new ProcessStartInfo(Global.Settings_Server.Path);
                ServerProcessInfo.FileName = Global.Settings_Server.Path;
                ServerProcessInfo.UseShellExecute = false;
                ServerProcessInfo.CreateNoWindow = true;
                ServerProcessInfo.RedirectStandardOutput = true;
                ServerProcessInfo.RedirectStandardInput = true;
                ServerProcessInfo.StandardOutputEncoding = Encoding.UTF8;
                ServerProcessInfo.WorkingDirectory = Path.GetDirectoryName(Global.Settings_Server.Path);
                ServerProcess = Process.Start(ServerProcessInfo);
                CommandWriter = new StreamWriter(ServerProcess.StandardInput.BaseStream);
                CommandWriter.AutoFlush = true;
                CommandWriter.NewLine = "\n";
                ServerProcess.BeginOutputReadLine();
                ServerProcess.OutputDataReceived += new DataReceivedEventHandler(SortOutputHandler);
                Restart = false;
                Status = true;
                Killed = false;
                Finished = false;
                Version = "-";
                LevelName = "-";
                Difficulty = "-";
                StartFileName = Path.GetFileName(Global.Settings_Server.Path);
                PrevCpuTime = TimeSpan.Zero;
                CurTime = TimeSpan.Zero;
                WaitForExitThread = new Thread(WaitForExit);
                WaitForExitThread.IsBackground = true;
                WaitForExitThread.Start();
            }
        }
        public static void Stop()
        {
            if (Status)
            {
                foreach(string Command in Global.Settings_Server.StopCommand.Split(';'))
                {
                    InputCommand(Command);
                }
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
                    ) == 1
                )
            {
                if (ServerProcessInfo.FileName.ToUpper().EndsWith(".BAT"))
                {
                    if ((int)MessageBox.Show(
                    "由于启动文件为批处理文件（*.bat），\n强制结束进程功能可能不一定有效\n是否继续？",
                    "Serein",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning
                    ) == 1)
                    {
                        ServerProcess.Kill();
                        Killed = true;
                        Restart = false;
                    }
                }
                else
                {
                    ServerProcess.Kill();
                    Killed = true;
                    Restart = false;
                }
            }
            else if (!Status)
            {
                MessageBox.Show(":(\n服务器不在运行中.", "Serein", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        public static void InputCommand(string Command, bool StartedByCommand=false)
        {
            if (Status)
            {
                Command = Regex.Replace(Command, @"[\r\n]+?$", "");
                Command = Regex.Replace(Command, @"^[\r\n\s]+?", "");
                if (Global.Settings_Server.EnableOutputCommand)
                {
                    Global.Ui.PanelConsoleWebBrowser_Invoke($">{Command}");
                }
                CommandWriter.WriteLine(Command.TrimEnd('\r', '\n'));
                if (Global.Settings_Server.EnableLog)
                {
                    if (!Directory.Exists(Global.Path + "\\logs\\console"))
                    {
                        Directory.CreateDirectory(Global.Path + "\\logs\\console");
                    }
                    try
                    {
                        LogWriter = new StreamWriter(
                        Global.Path + $"\\logs\\console\\{DateTime.Now:yyyy-MM-dd}.log",
                        true,
                        Encoding.UTF8
                        );
                        LogWriter.WriteLine(">" + Log.OutputRecognition(Command));
                        LogWriter.Flush();
                        LogWriter.Close();
                    }
                    catch { }
                }
            }
            else if (Command.Trim().ToUpper() == "START")
            {
                Start(StartedByCommand);
            }
        }
        private static void SortOutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            if (!string.IsNullOrEmpty(outLine.Data))
            {
                if (!Finished)
                {
                    if (Regex.IsMatch(outLine.Data, Global.Settings_Matches.Finished, RegexOptions.IgnoreCase))
                    {
                        Finished = true;
                        Global.Ui.UpdateServerInfo(LevelName, Version, Difficulty);
                    }
                    else if (Regex.IsMatch(outLine.Data, Global.Settings_Matches.Version, RegexOptions.IgnoreCase))
                    {
                        Version = Regex.Match(outLine.Data, Global.Settings_Matches.Version, RegexOptions.IgnoreCase).Groups[1].Value.Trim();
                    }
                    else if (Regex.IsMatch(outLine.Data, Global.Settings_Matches.LevelName, RegexOptions.IgnoreCase))
                    {
                        LevelName = Regex.Match(outLine.Data, Global.Settings_Matches.LevelName, RegexOptions.IgnoreCase).Groups[1].Value.Trim();
                    }
                    else if (Regex.IsMatch(outLine.Data, Global.Settings_Matches.Difficulty, RegexOptions.IgnoreCase))
                    {
                        Difficulty = Regex.Match(outLine.Data, Global.Settings_Matches.Difficulty, RegexOptions.IgnoreCase).Groups[1].Value.Trim();
                    }
                }
                Global.Ui.PanelConsoleWebBrowser_Invoke(
                    Log.ColorLog(outLine.Data, Global.Settings_Server.OutputStyle));
                if (Global.Settings_Server.EnableLog)
                {
                    if (!Directory.Exists(Global.Path + "\\logs\\console"))
                    {
                        Directory.CreateDirectory(Global.Path + "\\logs\\console");
                    }
                    try
                    {
                        LogWriter = new StreamWriter(
                            Global.Path + $"\\logs\\console\\{DateTime.Now:yyyy-MM-dd}.log",
                            true,
                            Encoding.UTF8
                            );
                        LogWriter.WriteLine(Log.OutputRecognition(outLine.Data));
                        LogWriter.Flush();
                        LogWriter.Close();
                    }
                    catch { }
                }
                Task MsgTask = new Task(() =>
                  {
                      Message.ProcessMsgFromConsole(Log.OutputRecognition(outLine.Data));
                  });
                MsgTask.Start();
            }
        }
        private static void WaitForExit()
        {
            ServerProcess.WaitForExit();
            Status = false;
            CommandWriter.Close();
            if (!Killed && ServerProcess.ExitCode != 0)
            {
                Global.Ui.PanelConsoleWebBrowser_Invoke(
                $"<br><span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>进程疑似非正常退出（返回：{ServerProcess.ExitCode}）"
                );
                if (Global.Settings_Server.EnableRestart)
                {
                    Restart = true;
                }
            }
            else
            {
                Global.Ui.PanelConsoleWebBrowser_Invoke(
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
        private static void RestartTimer()
        {
            Global.Ui.PanelConsoleWebBrowser_Invoke(
                $"<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>服务器将在5s后重新启动."
                );
            Global.Ui.PanelConsoleWebBrowser_Invoke(
                $"<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>你可以按下停止按钮来取消这次重启."
                );
            for (int i = 0; i < 10; i++)
            {
                if (!Restart)
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
                Global.Ui.PanelConsoleWebBrowser_Invoke(
                $"<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>重启已取消."
                );
            }
            RestartTimerThread.Abort();
        }
        public static double GetCPUPersent()
        {
            CurTime = ServerProcess.TotalProcessorTime;
            double value = (CurTime - PrevCpuTime).TotalMilliseconds / 2000 / Environment.ProcessorCount * 100;
            PrevCpuTime = CurTime;
            return value;
        }
    }
}
