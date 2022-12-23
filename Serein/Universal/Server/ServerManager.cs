using Serein.Base;
using Serein.Items;
using Serein.JSPlugin;
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
        public static bool Status => ServerProcess != null && !ServerProcess.HasExited;
        public static bool Restart = false, Finished = false;
        private static bool Killed;
        public static double CPUPersent = 0;
        public static int CommandListIndex = 0;
        private static readonly object Lock = new object();
        private static TimeSpan PrevCpuTime = TimeSpan.Zero;

        /// <summary>
        /// 服务器进程
        /// </summary>
        private static Process ServerProcess;

        /// <summary>
        /// 输入流写入者
        /// </summary>
        private static StreamWriter InputWriter;

        /// <summary>
        /// 命令历史记录
        /// </summary>
        public static List<string> CommandHistory = new List<string>();

        /// <summary>
        /// 编码列表
        /// </summary>
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

#if CONSOLE
        private static DateTime LastKillTime = DateTime.Now;
#endif

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
#if CONSOLE
                Logger.Out(LogType.Server_Notice, "若要执行Serein指令，请使用\"serein 你的指令\"代替原输入方式\r\n");
#else
                Logger.Out(LogType.Server_Clear);
#endif
                Logger.Out(LogType.Server_Notice, "启动中");
                ServerProcess = Process.Start(new ProcessStartInfo(Global.Settings.Server.Path)
                {
                    FileName = Global.Settings.Server.Path,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    StandardOutputEncoding = EncodingList[Global.Settings.Server.OutputEncoding],
                    WorkingDirectory = Path.GetDirectoryName(Global.Settings.Server.Path)
                });
                ServerProcess.EnableRaisingEvents = true;
                ServerProcess.Exited += (sender, e) => WaitForExit();
                InputWriter = new StreamWriter(
                    ServerProcess.StandardInput.BaseStream,
                    EncodingList[Global.Settings.Server.InputEncoding]
                   )
                {
                    AutoFlush = true,
                    NewLine = string.IsNullOrEmpty(Global.Settings.Server.LineTerminator) ? Environment.NewLine : Global.Settings.Server.LineTerminator.Replace("\\n", "\n").Replace("\\r", "\r")
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
                CommandHistory.Clear();
                StartFileName = Path.GetFileName(Global.Settings.Server.Path);
                PrevCpuTime = TimeSpan.Zero;
                System.Threading.Tasks.Task.Factory.StartNew(GetCPUPercent);
                EventTrigger.Trigger(EventType.ServerStart);
                JSFunc.Trigger(EventType.ServerStart);
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
            if (Quiet)
            {
                if (!Status)
                    return false;
                try
                {
                    ServerProcess.Kill();
                    Killed = true;
                    Restart = false;
                    return true;
                }
                catch { }
                return false;
            }
            else if (!Status)
                Logger.MsgBox(":(\n服务器不在运行中", "Serein", 0, 48);
#if CONSOLE
            else
            {
                DateTime NowTime = DateTime.Now;
                if ((NowTime - LastKillTime).TotalSeconds < 5)
                {
                    LastKillTime = NowTime;
                    try
                    {
                        ServerProcess.Kill();
                        Killed = true;
                        Restart = false;
                        return true;
                    }
                    catch (Exception e)
                    {
                        Logger.Out(LogType.Warn, "强制结束失败：\r\n", e.Message);
                        Logger.Out(LogType.Debug, e);
                        return false;
                    }
                }
                else
                {
                    LastKillTime = NowTime;
                    Logger.Out(LogType.Warn, "请在5s内再次执行强制结束服务器（Ctrl+C 或输入 serein kill）以确认此操作");
                    return false;
                }
            }
#else
            else if (Logger.MsgBox("确定结束进程吗？\n此操作可能导致存档损坏等问题", "Serein", 1, 48) && (
                    !StartFileName.ToUpper().EndsWith(".BAT") || (
                    StartFileName.ToUpper().EndsWith(".BAT") &&
                    Logger.MsgBox("由于启动文件为批处理文件（*.bat），\n强制结束进程功能可能不一定有效\n是否继续？", "Serein", 1, 48))))
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
                    return false;
                }
            }
#endif
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
                string Command_Copy = Command.TrimEnd('\n');
                if (CommandHistory.Count > 50)
                    CommandHistory.RemoveRange(0, CommandHistory.Count - 50);
                if (
                    (CommandHistory.Count > 0 && CommandHistory[CommandHistory.Count - 1] != Command_Copy || CommandHistory.Count == 0) &&
                    (!Quiet || !(string.IsNullOrEmpty(Command_Copy) || string.IsNullOrWhiteSpace(Command_Copy))))
                {
                    CommandListIndex = CommandHistory.Count + 1;
                    CommandHistory.Add(Command_Copy);
                }
#if !CONSOLE
                if (Global.Settings.Server.EnableOutputCommand)
                    Logger.Out(LogType.Server_Output, $">{Log.EscapeLog(Command_Copy)}");
#endif
                if (!IsSpecifiedCommand)
                {
                    if (Unicode || Global.Settings.Server.EnableUnicode)
                        Command_Copy = ConvertToUnicode(Command_Copy);
                    InputWriter.WriteLine(Command_Copy.Replace("\\r", "\r"));
                    JSFunc.Trigger(EventType.ServerSendCommand, Command);
                }
                if (Global.Settings.Server.EnableLog)
                {
                    if (!Directory.Exists(IO.GetPath("logs", "console")))
                        Directory.CreateDirectory(IO.GetPath("logs", "console"));
                    try
                    {
                        File.AppendAllText(
                            IO.GetPath("logs", "console", $"{DateTime.Now:yyyy-MM-dd}.log"),
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
#if CONSOLE
                Logger.Out(LogType.Server_Output, outLine.Data);
#else
                Logger.Out(LogType.Server_Output, Log.ColorLog(outLine.Data, Global.Settings.Server.OutputStyle));
#endif
                if (Global.Settings.Server.EnableLog)
                {
                    if (!Directory.Exists(IO.GetPath("logs", "console")))
                        Directory.CreateDirectory(IO.GetPath("logs", "console"));
                    try
                    {
                        File.AppendAllText(
                            IO.GetPath("logs", "console", $"{DateTime.Now:yyyy-MM-dd}.log"),
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
                lock (Lock)
                {
                    System.Threading.Tasks.Task.Run(() => JSFunc.Trigger(EventType.ServerOutput, Line));
                    System.Threading.Tasks.Task.Run(() => JSFunc.Trigger(EventType.ServerOriginalOutput, outLine.Data));
                    System.Threading.Tasks.Task.Delay(75).GetAwaiter().GetResult();
                }
            }
        }

        /// <summary>
        /// 等待服务器退出
        /// </summary>
        private static void WaitForExit()
        {
            InputWriter.Close();
            InputWriter.Dispose();
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
            JSFunc.Trigger(EventType.ServerStop, ServerProcess.ExitCode);
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
#if CONSOLE
            Logger.Out(LogType.Server_Notice, "你可以输入\"stop\"来取消这次重启");
#else
            Logger.Out(LogType.Server_Notice, "你可以按下停止按钮来取消这次重启");
#endif
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
