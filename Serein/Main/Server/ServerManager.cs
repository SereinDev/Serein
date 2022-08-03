using Serein.Base;
using Serein.Plugin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Serein.Server
{
    public class ServerManager
    {
        public static string StartFileName = string.Empty, Version, LevelName, Difficulty;
        public static bool Status = false;
        public static bool Restart = false;
        public static List<string> CommandList = new List<string>();
        public static double CPUPersent = 0.0;
        public static int CommandListIndex = 0, Port = 0;
        private static bool Finished = false;
        private static ProcessStartInfo ServerProcessInfo;
        public static Process ServerProcess;
        private static bool Killed;
        public static StreamWriter CommandWriter;
        private static TimeSpan PrevCpuTime = TimeSpan.Zero;
        private static string TempLine = string.Empty;
        public static Encoding[] EncodingList =
        {
            new UTF8Encoding(false),
            new UTF8Encoding(true),
            Encoding.Unicode,
            Encoding.BigEndianUnicode,
            Encoding.UTF32,
            Encoding.ASCII,
            Encoding.GetEncoding("ISO-8859-1")
        };
        public static string Online;

        public static bool Start(bool NoMsgBox = false)
        {
            if (Status)
            {
                if (!NoMsgBox)
                {
                    MessageBox.Show(Global.Ui, ":(\n服务器已在运行中.", "Serein", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else if (string.IsNullOrEmpty(Global.Settings.Server.Path) || string.IsNullOrWhiteSpace(Global.Settings.Server.Path))
            {
                if (!NoMsgBox)
                {
                    MessageBox.Show(Global.Ui, ":(\n启动路径为空.", "Serein", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else if (!File.Exists(Global.Settings.Server.Path))
            {
                if (!NoMsgBox)
                {
                    MessageBox.Show(Global.Ui, $":(\n启动文件\"{Global.Settings.Server.Path}\"未找到.", "Serein", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                Global.Logger(10, "#clear");
                Global.Logger(11, "启动中");
                ServerProcessInfo = new ProcessStartInfo(Global.Settings.Server.Path)
                {
                    FileName = Global.Settings.Server.Path,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    StandardOutputEncoding = Encoding.UTF8,
                    WorkingDirectory = Path.GetDirectoryName(Global.Settings.Server.Path)
                };
                ServerProcess = Process.Start(ServerProcessInfo);
                CommandWriter = new StreamWriter(
                    ServerProcess.StandardInput.BaseStream,
                    EncodingList[Global.Settings.Server.EncodingIndex]
                   )
                {
                    AutoFlush = true,
                    NewLine = "\n"
                };
                ServerProcess.BeginOutputReadLine();
                ServerProcess.OutputDataReceived += new DataReceivedEventHandler(SortOutputHandler);
                Restart = false;
                Status = true;
                Killed = false;
                Finished = false;
                Version = "-";
                LevelName = "-";
                Difficulty = "-";
                TempLine = string.Empty;
                Port = 0;
                CommandList.Clear();
                StartFileName = Path.GetFileName(Global.Settings.Server.Path);
                PrevCpuTime = TimeSpan.Zero;
                Task.Run(GetCPUPercent);
                Task.Run(WaitForExit);
                EventTrigger.Trigger("Server_Start");
                JSFunc.Trigger("onServerStart");
                return true;
            }
            return false;
        }
        public static void Stop()
        {
            if (Status)
            {
                foreach (string Command in Global.Settings.Server.StopCommand.Split(';'))
                {
                    if (!(string.IsNullOrEmpty(Command) || string.IsNullOrWhiteSpace(Command)))
                    {
                        InputCommand(Command);
                    }
                }
            }
            else if (!Status && Restart)
            {
                Restart = false;
            }
            else
            {
                MessageBox.Show(Global.Ui, ":(\n服务器不在运行中.", "Serein", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        public static bool Kill(bool NoMsgBox = false)
        {
            if (
                !NoMsgBox
                &&
                Status
                &&
                (int)MessageBox.Show(
                    "确定结束进程吗？\n此操作可能导致存档损坏等问题",
                    "Serein",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning
                    ) == 1
                && (
                    !ServerProcessInfo.FileName.ToUpper().EndsWith(".BAT") || (
                    ServerProcessInfo.FileName.ToUpper().EndsWith(".BAT") &&
                    (int)MessageBox.Show(
                    "由于启动文件为批处理文件（*.bat），\n强制结束进程功能可能不一定有效\n是否继续？",
                    "Serein",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning
                    ) == 1
                ))
                )
            {
                try
                {
                    ServerProcess.Kill();
                    Killed = true;
                    Restart = false;
                    return true;
                }
                catch (Exception e)
                {
                    MessageBox.Show(
                        "强制结束失败\n" + e.Message,
                        "Serein",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                        );
                }
            }
            else if (NoMsgBox)
            {
                try
                {
                    ServerProcess.Kill();
                    Killed = true;
                    Restart = false;
                    return true;
                }
                catch { }
            }
            else if (!Status && !NoMsgBox)
            {
                MessageBox.Show(Global.Ui, ":(\n服务器不在运行中.", "Serein", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return false;
        }
        public static void InputCommand(string Command, bool NoMsgBox = false, bool Unicode = false)
        {
            if (Status)
            {
                bool IsSpecifiedCommand = false;
                string Command_Copy = Command.TrimEnd('\r', '\n');
                string Command_Copy_Prefix = Command_Copy.Split(' ')[0];
                foreach (CommandItem Item in Plugins.CommandItems)
                {
                    if (Command_Copy_Prefix == Item.Command)
                    {
                        IsSpecifiedCommand = true;
                        JSFunc.Trigger("onServerSendSpecifiedCommand", Command_Copy, Item.Function);
                    }
                }
                while (CommandList.Count >= 50)
                {
                    CommandList.RemoveAt(0);
                }
                if (
                    (CommandList.Count > 0 && CommandList[CommandList.Count - 1] != Command_Copy || CommandList.Count == 0) &&
                    (!NoMsgBox || !(string.IsNullOrEmpty(Command_Copy) || string.IsNullOrWhiteSpace(Command_Copy))))
                {
                    CommandListIndex = CommandList.Count + 1;
                    CommandList.Add(Command_Copy);
                }
                if (Global.Settings.Server.EnableOutputCommand)
                {
                    Global.Logger(10, $">{Command_Copy}");
                }
                if (!IsSpecifiedCommand)
                {
                    if (Unicode || Global.Settings.Server.EnableUnicode)
                    {
                        Command_Copy = ConvertToUnicode(Command_Copy);
                    }
                    CommandWriter.WriteLine(Command_Copy);
                    JSFunc.Trigger("onServerSendCommand", Command);
                }
                if (Global.Settings.Server.EnableLog)
                {
                    if (!Directory.Exists(Global.Path + "\\logs\\console"))
                    {
                        Directory.CreateDirectory(Global.Path + "\\logs\\console");
                    }
                    try
                    {
                        File.AppendAllText(
                            Global.Path + $"\\logs\\console\\{DateTime.Now:yyyy-MM-dd}.log",
                            ">" + Log.OutputRecognition(Command_Copy),
                            Encoding.UTF8
                        );
                    }
                    catch { }
                }
            }
            else if (Command.Trim().ToUpper() == "START")
            {
                Start(NoMsgBox);
            }
        }
        private static void SortOutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            if (!string.IsNullOrEmpty(outLine.Data))
            {
                string Line = Log.OutputRecognition(outLine.Data);
                if (!Finished)
                {
                    if (Regex.IsMatch(Line, Global.Settings.Matches.Finished, RegexOptions.IgnoreCase))
                    {
                        Finished = true;
                        Global.Ui.UpdateServerInfo(LevelName, Version, Difficulty);
                    }
                    else if (Regex.IsMatch(Line, Global.Settings.Matches.Version, RegexOptions.IgnoreCase))
                    {
                        Version = Regex.Match(Line, Global.Settings.Matches.Version, RegexOptions.IgnoreCase).Groups[1].Value.Trim();
                    }
                    else if (Regex.IsMatch(Line, Global.Settings.Matches.LevelName, RegexOptions.IgnoreCase))
                    {
                        LevelName = Regex.Match(Line, Global.Settings.Matches.LevelName, RegexOptions.IgnoreCase).Groups[1].Value.Trim();
                    }
                    else if (Regex.IsMatch(Line, Global.Settings.Matches.Difficulty, RegexOptions.IgnoreCase))
                    {
                        Difficulty = Regex.Match(Line, Global.Settings.Matches.Difficulty, RegexOptions.IgnoreCase).Groups[1].Value.Trim();
                    }
                }
                Global.Logger(
                    10,
                    Log.ColorLog(outLine.Data, Global.Settings.Server.OutputStyle)
                    );
                //Global.Debug(Log.ColorLog(outLine.Data, Global.Settings.Server.OutputStyle));
                if (Global.Settings.Server.EnableLog)
                {
                    if (!Directory.Exists(Global.Path + "\\logs\\console"))
                    {
                        Directory.CreateDirectory(Global.Path + "\\logs\\console");
                    }
                    try
                    {
                        File.AppendAllText(
                            Global.Path + $"\\logs\\console\\{DateTime.Now:yyyy-MM-dd}.log",
                            Log.OutputRecognition(Line),
                            Encoding.UTF8
                        );
                    }
                    catch { }
                }
                if (Regex.IsMatch(Line, Global.Settings.Matches.PlayerList, RegexOptions.IgnoreCase))
                {
                    TempLine = Line.Trim('\r', '\n');
                }
                else
                {
                    if (!string.IsNullOrEmpty(TempLine))
                    {
                        string TempLine_ = TempLine + "\n" + Line;
                        TempLine = string.Empty;
                        Base.Message.ProcessMsgFromConsole(TempLine_);
                    }
                    else
                    {
                        Base.Message.ProcessMsgFromConsole(Line);
                    }
                }
                JSFunc.Trigger("onServerOutput", Line);
            }
        }
        private static void WaitForExit()
        {
            ServerProcess.WaitForExit();
            Status = false;
            CommandWriter.Close();
            Global.Logger(10, "");
            if (!Killed && ServerProcess.ExitCode != 0)
            {
                Global.Logger(11,
                    $"进程疑似非正常退出（返回：{ServerProcess.ExitCode}）"
                );
                Restart = Global.Settings.Server.EnableRestart;
                EventTrigger.Trigger("Server_Error");
            }
            else
            {
                Global.Logger(11,
                    $"进程已退出（返回：{ServerProcess.ExitCode}）"
                );
                EventTrigger.Trigger("Server_Stop");
            }
            if (Restart)
                Task.Run(RestartTimer);
            Version = "-";
            LevelName = "-";
            Difficulty = "-";
            JSFunc.Trigger("onServerStop");
        }
        public static void RestartRequest()
        {
            if (!Restart)
            {
                Restart = Status;
                Stop();
            }
        }
        private static void RestartTimer()
        {
            Global.Logger(11,
                "服务器将在5s后重新启动."
                );
            Global.Logger(11,
                "你可以按下停止按钮来取消这次重启."
                );
            for (int i = 0; i < 10; i++)
            {
                Thread.CurrentThread.Join(500);
                if (!Restart)
                {
                    break;
                }
            }
            if (Restart)
            {
                Start();
            }
            else
            {
                Global.Logger(11,
                "重启已取消."
                );
            }
        }
        public static void GetCPUPercent()
        {
            while (Status)
            {
                Thread.CurrentThread.Join(2000);
                CPUPersent = (ServerProcess.TotalProcessorTime - PrevCpuTime).TotalMilliseconds / 2000 / Environment.ProcessorCount * 100;
                PrevCpuTime = ServerProcess.TotalProcessorTime;
            }
        }
        public static string GetTime()
        {
            string Time = "-";
            if (Status)
            {
                TimeSpan t = DateTime.Now - ServerProcess.StartTime;
                Time = t.TotalSeconds < 3600
                    ? $"{t.TotalSeconds / 60:N1}m"
                    : t.TotalHours < 120
                    ? $"{t.TotalMinutes / 60:N1}h"
                    : $"{t.TotalHours / 24:N2}d";
            }
            return Time;
        }
        private static string ConvertToUnicode(string Text)
        {
            string result = "";
            for (int i = 0; i < Text.Length; i++)
            {
                if (Text[i] < 127)
                {
                    result += Text[i].ToString();
                }
                else
                    result += string.Format("\\u{0:x4}", (int)Text[i]);
            }
            return result;
        }
    }
}
