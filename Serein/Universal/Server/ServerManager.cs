using Serein.Base;
using Serein.Items;
using Serein.Plugin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Serein.Server
{
    public static class ServerManager
    {
        public static string StartFileName = string.Empty, Version = string.Empty, LevelName = string.Empty, Difficulty = string.Empty;
        private static string TempLine = string.Empty;
        public static bool Status
        {
            get
            {
                return ServerProcess != null && !ServerProcess.HasExited;
            }
        }
        public static bool Restart = false, Finished = false;
        private static bool Killed;
        public static double CPUPersent = 0;
        public static int CommandListIndex = 0;
        private static TimeSpan PrevCpuTime = TimeSpan.Zero;
        private static ProcessStartInfo ServerProcessInfo;
        private static Process ServerProcess;
        private static StreamWriter CommandWriter;
        public static List<string> CommandList = new List<string>();
        public static readonly Encoding[] EncodingList =
        {
            new UTF8Encoding(false),
            new UTF8Encoding(true),
            new UnicodeEncoding(false, false),
            new UnicodeEncoding(true, false),
            Encoding.UTF32,
            Encoding.ASCII,
            Encoding.GetEncoding("ISO-8859-1")
        };

        /// <summary>
        /// 启动服务器
        /// </summary>
        /// <param name="Quiet">静处理</param>
        /// <returns>启动结果</returns>
        public static bool Start(bool Quiet = false)
        {
            if (Status)
            {
                if (!Quiet)
                    Logger.MsgBox(":(\n服务器已在运行中", "Serein", 0, 48);
            }
            else if (string.IsNullOrEmpty(Global.Settings.Server.Path) || string.IsNullOrWhiteSpace(Global.Settings.Server.Path))
            {
                if (!Quiet)
                    Logger.MsgBox(":(\n启动路径为空", "Serein", 0, 48);
            }
            else if (!File.Exists(Global.Settings.Server.Path))
            {
                if (!Quiet)
                    Logger.MsgBox($":(\n启动文件\"{Global.Settings.Server.Path}\"未找到", "Serein", 0, 48);
            }
            else
            {
                if (Logger.Type == 0)
                    Logger.Out(LogType.Server_Notice, "若要执行Serein指令，请使用\"serein 你的指令\"代替原输入方式\r\n");
                else
                    Logger.Out(LogType.Server_Clear);
                Logger.Out(LogType.Server_Notice, "启动中");
                ServerProcessInfo = new ProcessStartInfo(Global.Settings.Server.Path)
                {
                    FileName = Global.Settings.Server.Path,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    StandardOutputEncoding = EncodingList[Global.Settings.Server.OutputEncoding],
                    WorkingDirectory = Path.GetDirectoryName(Global.Settings.Server.Path)
                };
                ServerProcess = Process.Start(ServerProcessInfo);
                ServerProcess.EnableRaisingEvents = true;
                ServerProcess.Exited += (sender, e) => WaitForExit();
                CommandWriter = new StreamWriter(
                    ServerProcess.StandardInput.BaseStream,
                    EncodingList[Global.Settings.Server.InputEncoding]
                   )
                {
                    AutoFlush = true,
                    NewLine = "\n"
                };
                ServerProcess.BeginOutputReadLine();
                ServerProcess.OutputDataReceived += SortOutputHandler;
                Restart = false;
                Killed = false;
                Finished = false;
                Version = "-";
                LevelName = "-";
                Difficulty = "-";
                TempLine = string.Empty;
                CommandList.Clear();
                StartFileName = Path.GetFileName(Global.Settings.Server.Path);
                PrevCpuTime = TimeSpan.Zero;
                System.Threading.Tasks.Task.Factory.StartNew(GetCPUPercent);
                EventTrigger.Trigger(EventType.ServerStart);
                JSFunc.Trigger("onServerStart");
                return true;
            }
            return false;
        }

        /// <summary>
        /// 关闭服务器
        /// </summary>
        /// <param name="Quiet">静处理</param>
        public static void Stop(bool Quiet = false)
        {
            if (Status)
            {
                foreach (string Command in Global.Settings.Server.StopCommands)
                {
                    if (!(string.IsNullOrEmpty(Command) || string.IsNullOrWhiteSpace(Command)))
                        InputCommand(Command);
                }
            }
            else if (!Status && Restart)
                Restart = false;
            else if (!Quiet)
                Logger.MsgBox(":(\n服务器不在运行中", "Serein", 0, 48);
        }

        /// <summary>
        /// 强制结束服务器
        /// </summary>
        /// <param name="Quiet">静处理</param>
        /// <returns>强制结束结果</returns>
        public static bool Kill(bool Quiet = false)
        {
            if (Status
                &&
                (Logger.Type == 0 ||
                !Quiet
                &&
                Logger.MsgBox(
                    "确定结束进程吗？\n此操作可能导致存档损坏等问题",
                    "Serein",
                    1,
                    48
                    )
                && (
                    !ServerProcessInfo.FileName.ToUpper().EndsWith(".BAT") || (
                    ServerProcessInfo.FileName.ToUpper().EndsWith(".BAT") &&
                    Logger.MsgBox(
                    "由于启动文件为批处理文件（*.bat），\n强制结束进程功能可能不一定有效\n是否继续？",
                    "Serein",
                    1,
                    48
                    )
                )))
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
                    Logger.MsgBox(":(\n强制结束失败\n" + e.Message, "Serein", 0, 16);
                }
            }
            else if (Quiet)
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
            else if (!Status && !Quiet)
                Logger.MsgBox(":(\n服务器不在运行中", "Serein", 0, 48);
            if (ServerProcess != null && ServerProcess.HasExited)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 输入命令
        /// </summary>
        /// <param name="Command">命令</param>
        /// <param name="Quiet">静处理</param>
        /// <param name="Unicode">使用Unicode</param>
        public static void InputCommand(string Command, bool Quiet = false, bool Unicode = false)
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
                if (CommandList.Count > 50)
                    CommandList.RemoveRange(0, CommandList.Count - 50);
                if (
                    (CommandList.Count > 0 && CommandList[CommandList.Count - 1] != Command_Copy || CommandList.Count == 0) &&
                    (!Quiet || !(string.IsNullOrEmpty(Command_Copy) || string.IsNullOrWhiteSpace(Command_Copy))))
                {
                    CommandListIndex = CommandList.Count + 1;
                    CommandList.Add(Command_Copy);
                }
                if (Global.Settings.Server.EnableOutputCommand && Logger.Type != 0)
                    Logger.Out(LogType.Server_Output, $">{Log.EscapeLog(Command_Copy)}");
                if (!IsSpecifiedCommand)
                {
                    if (Unicode || Global.Settings.Server.EnableUnicode)
                        Command_Copy = ConvertToUnicode(Command_Copy);
                    CommandWriter.WriteLine(Command_Copy);
                    JSFunc.Trigger("onServerSendCommand", Command);
                }
                if (Global.Settings.Server.EnableLog)
                {
                    if (!Directory.Exists(Global.Path + "\\logs\\console"))
                        Directory.CreateDirectory(Global.Path + "\\logs\\console");
                    try
                    {
                        File.AppendAllText(
                            Global.Path + $"\\logs\\console\\{DateTime.Now:yyyy-MM-dd}.log",
                            ">" + Log.OutputRecognition(Command_Copy) + "\n",
                            Encoding.UTF8
                        );
                    }
                    catch { }
                }
            }
            else if (Command.Trim().ToLower() == "start")
                Start(Quiet);
            else if (Command.Trim().ToLower() == "stop")
                Restart = false;
        }

        /// <summary>
        /// 输出处理
        /// </summary>
        private static void SortOutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            if (!string.IsNullOrEmpty(outLine.Data))
            {
                string Line = Log.OutputRecognition(outLine.Data);
                if (!Finished)
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(Line, Global.Settings.Matches.Finished, RegexOptions.IgnoreCase))
                        Finished = true;
                    else if (System.Text.RegularExpressions.Regex.IsMatch(Line, Global.Settings.Matches.Version, RegexOptions.IgnoreCase))
                        Version = System.Text.RegularExpressions.Regex.Match(Line, Global.Settings.Matches.Version, RegexOptions.IgnoreCase).Groups[1].Value.Trim();
                    else if (System.Text.RegularExpressions.Regex.IsMatch(Line, Global.Settings.Matches.LevelName, RegexOptions.IgnoreCase))
                        LevelName = System.Text.RegularExpressions.Regex.Match(Line, Global.Settings.Matches.LevelName, RegexOptions.IgnoreCase).Groups[1].Value.Trim();
                    else if (System.Text.RegularExpressions.Regex.IsMatch(Line, Global.Settings.Matches.Difficulty, RegexOptions.IgnoreCase))
                        Difficulty = System.Text.RegularExpressions.Regex.Match(Line, Global.Settings.Matches.Difficulty, RegexOptions.IgnoreCase).Groups[1].Value.Trim();
                }
                Logger.Out(
                    LogType.Server_Output,
                    Logger.Type == 0 ?
                    outLine.Data : Log.ColorLog(outLine.Data, Global.Settings.Server.OutputStyle)
                    );
                if (Global.Settings.Server.EnableLog)
                {
                    if (!Directory.Exists(Global.Path + "\\logs\\console"))
                        Directory.CreateDirectory(Global.Path + "\\logs\\console");
                    try
                    {
                        File.AppendAllText(
                            Global.Path + $"\\logs\\console\\{DateTime.Now:yyyy-MM-dd}.log",
                            Log.OutputRecognition(Line) + "\n",
                            Encoding.UTF8
                        );
                    }
                    catch { }
                }
                bool MuiltLines = false;
                foreach (string RegExp in Global.Settings.Matches.MuiltLines)
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(Line, RegExp, RegexOptions.IgnoreCase))
                    {
                        TempLine = Line.Trim('\r', '\n');
                        MuiltLines = true;
                        break;
                    }
                }
                if (!MuiltLines)
                {
                    if (!string.IsNullOrEmpty(TempLine))
                    {
                        string TempLine_ = TempLine + "\n" + Line;
                        TempLine = string.Empty;
                        Matcher.Process(TempLine_);
                    }
                    else
                        Matcher.Process(Line);
                }
                JSFunc.Trigger("onServerOutput", Line);
                JSFunc.Trigger("onServerOriginalOutput", outLine.Data);
            }
        }

        /// <summary>
        /// 等待服务器退出
        /// </summary>
        private static void WaitForExit()
        {
            CommandWriter.Close();
            CommandWriter.Dispose();
            Logger.Out(LogType.Server_Output, "");
            if (!Killed && ServerProcess.ExitCode != 0)
            {
                Logger.Out(LogType.Server_Notice,
                    $"进程疑似非正常退出（返回：{ServerProcess.ExitCode}）"
                );
                Restart = Global.Settings.Server.EnableRestart;
                EventTrigger.Trigger(EventType.ServerExitUnexpectedly);
            }
            else
            {
                Logger.Out(LogType.Server_Notice,
                    $"进程已退出（返回：{ServerProcess.ExitCode}）"
                );
                EventTrigger.Trigger(EventType.ServerStop);
            }
            if (Restart)
                System.Threading.Tasks.Task.Factory.StartNew(RestartTimer);
            Version = "-";
            LevelName = "-";
            Difficulty = "-";
            JSFunc.Trigger("onServerStop", ServerProcess.ExitCode);
        }

        /// <summary>
        /// 重启请求
        /// </summary>
        public static void RestartRequest()
        {
            if (!Restart)
            {
                Restart = Status;
                Stop();
            }
        }

        /// <summary>
        /// 重启计时器
        /// </summary>
        private static async void RestartTimer()
        {
            Logger.Out(LogType.Server_Notice,
                "服务器将在5s后重新启动"
                );
            Logger.Out(LogType.Server_Notice,
                Logger.Type > 0 ? "你可以按下停止按钮来取消这次重启" : "你可以输入\"stop\"来取消这次重启"
                );
            for (int i = 0; i < 10; i++)
            {
                await System.Threading.Tasks.Task.Delay(500);
                if (!Restart)
                    break;
            }
            if (Restart)
                Start(true);
            else
                Logger.Out(LogType.Server_Notice, "重启已取消");
        }

        /// <summary>
        /// 获取CPU占用
        /// </summary>
        public static void GetCPUPercent()
        {
            while (Status)
            {
                Thread.CurrentThread.Join(2000);
                CPUPersent = (ServerProcess.TotalProcessorTime - PrevCpuTime).TotalMilliseconds / 2000 / Environment.ProcessorCount * 100;
                PrevCpuTime = ServerProcess.TotalProcessorTime;
            }
        }

        /// <summary>
        /// 获取运行时间
        /// </summary>
        /// <returns>运行时间</returns>
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

        /// <summary>
        /// Unicode转换
        /// </summary>
        /// <param name="Text">输入</param>
        /// <returns>输出字符串</returns>
        private static string ConvertToUnicode(string Text)
        {
            string Result = "";
            for (int i = 0; i < Text.Length; i++)
            {
                if (Text[i] < 127)
                    Result += Text[i].ToString();
                else
                    Result += string.Format("\\u{0:x4}", (int)Text[i]);
            }
            return Result;
        }
    }
}
