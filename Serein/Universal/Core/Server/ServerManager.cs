using Serein.Base;
using Serein.Base.Motd;
using Serein.Core.JSPlugin;
using Serein.Extensions;
using Serein.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using RegExp = System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;

namespace Serein.Core.Server
{
    internal static class ServerManager
    {
        /// <summary>
        /// 启动文件名称
        /// </summary>
        public static string StartFileName { get; private set; } = string.Empty;

        /// <summary>
        /// 存档名称
        /// </summary>
        public static string LevelName { get; private set; } = string.Empty;

        /// <summary>
        /// 难度
        /// </summary>
        public static string Difficulty { get; private set; } = string.Empty;

        /// <summary>
        /// 临时行储存
        /// </summary>
        private static string TempLine = string.Empty;

        /// <summary>
        /// 重启
        /// </summary>
        private static bool Restart;

        /// <summary>
        /// 由用户关闭服务器
        /// </summary>
        private static bool IsStoppedByUser;

        /// <summary>
        /// 服务器状态
        /// </summary>
        public static bool Status => ServerProcess != null && !ServerProcess.HasExited;

        /// <summary>
        /// CPU使用率
        /// </summary>
        public static double CPUUsage { get; private set; }

        public static Motd Motd { get; private set; } = new();

        private static Timer UpdateTimer;

        /// <summary>
        /// 当前CPU时间
        /// </summary>
        private static TimeSpan PrevProcessCpuTime = TimeSpan.Zero;

        /// <summary>
        /// 服务器进程
        /// </summary>
        private static Process ServerProcess;

        /// <summary>
        /// 输入流写入
        /// </summary>
        private static StreamWriter InputWriter;

        /// <summary>
        /// 命令历史记录列表下标
        /// </summary>
        public static int CommandHistoryIndex;

        /// <summary>
        /// 命令历史记录
        /// </summary>
        public static readonly List<string> CommandHistory = new();

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
        /// <returns>启动结果</returns>
        public static bool Start()
            => Start(false);

        /// <summary>
        /// 启动服务器
        /// </summary>
        /// <param name="quiet">静处理</param>
        /// <returns>启动结果</returns>
        public static bool Start(bool quiet)
        {
            if (Status)
            {
                if (!quiet)
                {
                    Logger.MsgBox("服务器已在运行中", "Serein", 0, 48);
                }
            }
            else if (string.IsNullOrEmpty(Global.Settings.Server.Path) || string.IsNullOrWhiteSpace(Global.Settings.Server.Path))
            {
                if (!quiet)
                {
                    Logger.MsgBox("启动路径为空", "Serein", 0, 48);
                }
            }
            else if (!File.Exists(Global.Settings.Server.Path))
            {
                if (!quiet)
                {
                    Logger.MsgBox($"启动文件\"{Global.Settings.Server.Path}\"未找到", "Serein", 0, 48);
                }
            }
            else
            {
#if CONSOLE
                Logger.Output(LogType.Server_Notice, "若要执行Serein指令，请使用\"serein 你的指令\"代替原输入方式\n");
#else
                Logger.Output(LogType.Server_Clear);
#endif
                Logger.Output(LogType.Server_Notice, "启动中");
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
                ServerProcess.Exited += (_, _) => WaitForExit();
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
                IsStoppedByUser = false;
                LevelName = string.Empty;
                Difficulty = string.Empty;
                TempLine = string.Empty;
                CommandHistory.Clear();
                StartFileName = Path.GetFileName(Global.Settings.Server.Path);
                PrevProcessCpuTime = TimeSpan.Zero;

                Task.Run(() =>
                {
                    EventTrigger.Trigger(EventType.ServerStart);
                    JSFunc.Trigger(EventType.ServerStart);
                    UpdateTimer = new Timer(2000) { AutoReset = true };
                    UpdateTimer.Elapsed += (_, _) => UpdateInfo();
                    UpdateTimer.Start();
                });
                return true;
            }
            return false;
        }

        /// <summary>
        /// 关闭服务器
        /// </summary>
        public static void Stop()
            => Stop(false);

        /// <summary>
        /// 关闭服务器
        /// </summary>
        /// <param name="quiet">静处理</param>
        public static void Stop(bool quiet)
        {
            if (Status)
            {
                foreach (string command in Global.Settings.Server.StopCommands)
                {
                    if (!(string.IsNullOrEmpty(command) || string.IsNullOrWhiteSpace(command)))
                    {
                        InputCommand(command);
                    }
                }
            }
            else if (Restart)
            {
                Restart = false;
            }
            else if (!quiet)
            {
                Logger.MsgBox("服务器不在运行中", "Serein", 0, 48);
            }
        }

        /// <summary>
        /// 强制结束服务器
        /// </summary>
        /// <returns>强制结束结果</returns>
        public static bool Kill() => Kill(false);

        /// <summary>
        /// 强制结束服务器
        /// </summary>
        /// <param name="quiet">静处理</param>
        /// <returns>强制结束结果</returns>
        public static bool Kill(bool quiet)
        {
            if (quiet)
            {
                if (Status)
                {
                    try
                    {
#if NET6_0
                        ServerProcess.Kill(true);
#else
                        ServerProcess.Kill();
#endif
                        IsStoppedByUser = true;
                        Restart = false;
                        return true;
                    }
                    catch (Exception e)
                    {
                        Logger.Output(LogType.Debug, e);
                    }
                }
            }
            else if (!Status)
            {
                Logger.MsgBox("服务器不在运行中", "Serein", 0, 48);
            }
#if CONSOLE
            else
            {
                DateTime nowTime = DateTime.Now;
                if ((nowTime - LastKillTime).TotalSeconds < 2)
                {
                    LastKillTime = nowTime;
                    try
                    {
#if NET6_0
                        ServerProcess.Kill(true);
#else
                        ServerProcess.Kill();
#endif
                        IsStoppedByUser = true;
                        Restart = false;
                        return true;
                    }
                    catch (Exception e)
                    {
                        Logger.Output(LogType.Warn, "强制结束失败：\n", e.Message);
                        Logger.Output(LogType.Debug, e);
                    }
                }
                else
                {
                    LastKillTime = nowTime;
                    Logger.Output(LogType.Warn, "请在2s内再次执行强制结束服务器（Ctrl+C 或输入“serein s k”）以确认此操作");
                }
            }
#else
            else if (Logger.MsgBox("确定结束进程吗？\n此操作可能导致存档损坏等问题", "Serein", 1, 48)
#if !NET6_0
                 && (
                    !StartFileName.ToLowerInvariant().EndsWith(".bat") || (
                    StartFileName.ToLowerInvariant().EndsWith(".bat") &&
                    Logger.MsgBox("由于启动文件为批处理文件（*.bat），\n强制结束进程功能可能不一定有效\n是否继续？", "Serein", 1, 48)))
#endif
                )
            {
                try
                {
#if NET6_0
                    ServerProcess.Kill(true);
#else
                    ServerProcess.Kill();
#endif
                    IsStoppedByUser = true;
                    Restart = false;
                    return true;
                }
                catch (Exception e)
                {
                    Logger.MsgBox("强制结束失败\n" + e.Message, "Serein", 0, 16);
                    return false;
                }
            }
#endif
            return false;
        }

        /// <summary>
        /// 输入行
        /// </summary>
        /// <param name="line">行</param>
        public static void InputCommand(string command)
            => InputCommand(command, false, false);

        /// <summary>
        /// 输入行
        /// </summary>
        /// <param name="command">行</param>
        /// <param name="usingUnicode">使用Unicode</param>
        /// <param name="isFromCommand">来自命令</param>
        public static void InputCommand(string command, bool usingUnicode, bool isFromCommand)
        {
            if (Status)
            {
                command = command.TrimEnd('\n').Replace("\n", "\\n").Replace("\r", "\\n");
                string line_copy = command;
                if (CommandHistory.Count > 50)
                {
                    CommandHistory.RemoveRange(0, CommandHistory.Count - 50);
                }
                if (
                    (
                        CommandHistory.Count > 0 &&
                        CommandHistory[CommandHistory.Count - 1] != command || // 与最后一项重复
                        CommandHistory.Count == 0) &&
                        !isFromCommand && // 通过Serein命令执行的不计入
                        !(string.IsNullOrEmpty(command) || string.IsNullOrWhiteSpace(command))) // 为空不计入
                {
                    CommandHistory.Add(command);
                }
                CommandHistoryIndex = CommandHistory.Count;
#if !CONSOLE
                if (Global.Settings.Server.EnableOutputCommand)
                {
                    Logger.Output(LogType.Server_Input, $">{command}");
                }
#endif
                if (usingUnicode || Global.Settings.Server.EnableUnicode)
                {
                    line_copy = ConvertToUnicode(line_copy);
                }
                InputWriter.WriteLine(line_copy);
                IO.ConsoleLog(">" + command);
                Task.Run(() => JSFunc.Trigger(EventType.ServerSendCommand, command));
            }
            else if (isFromCommand)
            {
                if (command.Trim().ToLowerInvariant() == "start")
                {
                    Start(false);
                }
                else if (command.Trim().ToLowerInvariant() == "stop")
                {
                    Restart = false;
                }
            }
        }

        /// <summary>
        /// 输出处理
        /// /// </summary>
        private static void SortOutputHandler(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                string lineFiltered = LogPreProcessing.Filter(e.Data);
                if (string.IsNullOrEmpty(LevelName) && RegExp.Regex.IsMatch(lineFiltered, Global.Settings.Matches.LevelName, RegExp.RegexOptions.IgnoreCase))
                {
                    LevelName = RegExp.Regex.Match(lineFiltered, Global.Settings.Matches.LevelName, RegExp.RegexOptions.IgnoreCase).Groups[1].Value.Trim();
                }
                else if (string.IsNullOrEmpty(Difficulty) && RegExp.Regex.IsMatch(lineFiltered, Global.Settings.Matches.Difficulty, RegExp.RegexOptions.IgnoreCase))
                {
                    Difficulty = RegExp.Regex.Match(lineFiltered, Global.Settings.Matches.Difficulty, RegExp.RegexOptions.IgnoreCase).Groups[1].Value.Trim();
                }
                bool excluded = false;
                foreach (string exp1 in Global.Settings.Server.ExcludedOutputs)
                {
                    if (exp1.TryParse(RegExp.RegexOptions.IgnoreCase, out RegExp.Regex regex) && regex.IsMatch(lineFiltered))
                    {
                        excluded = true;
                        break;
                    }
                }
                bool interdicted = false;
                interdicted = JSFunc.Trigger(EventType.ServerOutput, lineFiltered) || interdicted;
                interdicted = JSFunc.Trigger(EventType.ServerOriginalOutput, e.Data) || interdicted;
                if (!excluded && !interdicted)
                {
                    Logger.Output(LogType.Server_Output, e.Data);
                    bool isMuiltLinesMode = false;
                    foreach (string exp2 in Global.Settings.Matches.MuiltLines)
                    {
                        if (exp2.TryParse(RegExp.RegexOptions.IgnoreCase, out RegExp.Regex regex) && regex.IsMatch(lineFiltered))
                        {
                            TempLine = lineFiltered.Trim('\r', '\n');
                            isMuiltLinesMode = true;
                            break;
                        }
                    }
                    if (!isMuiltLinesMode)
                    {
                        if (!string.IsNullOrEmpty(TempLine))
                        {
                            string tempLine = TempLine + "\n" + lineFiltered;
                            TempLine = string.Empty;
                            Matcher.Process(tempLine);
                        }
                        else
                        {
                            Matcher.Process(lineFiltered);
                        }
                    }
                }
                IO.ConsoleLog(lineFiltered);
            }
        }

        /// <summary>
        /// 等待服务器退出
        /// </summary>
        private static void WaitForExit()
        {
            InputWriter.Close();
            InputWriter.Dispose();
            Logger.Output(LogType.Server_Output, "");
            UpdateTimer?.Stop();
            if (!IsStoppedByUser && ServerProcess.ExitCode != 0)
            {
                Logger.Output(LogType.Server_Notice, $"进程疑似非正常退出（返回：{ServerProcess.ExitCode}）");
                Restart = Global.Settings.Server.EnableRestart;
                EventTrigger.Trigger(EventType.ServerExitUnexpectedly);
            }
            else
            {
                Logger.Output(LogType.Server_Notice, $"进程已退出（返回：{ServerProcess.ExitCode}）");
                EventTrigger.Trigger(EventType.ServerStop);
            }
            if (Restart)
            {
                Task.Factory.StartNew(RestartTimer);
            }
            LevelName = string.Empty;
            Difficulty = string.Empty;
            JSFunc.Trigger(EventType.ServerStop, ServerProcess.ExitCode);
        }

        /// <summary>
        /// 重启请求
        /// </summary>
        public static void RestartRequest()
        {
            if (!Restart)
            {
                Restart = false;
                Stop();
            }
        }

        /// <summary>
        /// 重启计时器
        /// </summary>
        private static void RestartTimer()
        {
            Logger.Output(LogType.Server_Notice,
                $"服务器将在5s后（{DateTime.Now.AddSeconds(5):T}）重新启动"
                );
#if CONSOLE
            Logger.Output(LogType.Server_Notice, "你可以输入\"stop\"来取消这次重启");
#else
            Logger.Output(LogType.Server_Notice, "你可以按下停止按钮来取消这次重启");
#endif
            for (int i = 0; i < 10; i++)
            {
                500.ToSleep();
                if (!Restart)
                {
                    break;
                }
            }
            if (Restart)
            {
                Start(true);
            }
            else
            {
                Logger.Output(LogType.Server_Notice, "重启已取消");
            }
        }

        /// <summary>
        /// 获取CPU占用
        /// </summary>
        public static void UpdateInfo()
        {
            if (Status)
            {
                CPUUsage = (ServerProcess.TotalProcessorTime - PrevProcessCpuTime).TotalMilliseconds / 2000 / Environment.ProcessorCount * 100;
                PrevProcessCpuTime = ServerProcess.TotalProcessorTime;
                if (CPUUsage > 100)
                {
                    CPUUsage = 100;
                }
                if (Global.Settings.Server.Type == 1)
                {
                    Motd = new Motdpe(Global.Settings.Server.Port);
                }
                else if (Global.Settings.Server.Type == 2)
                {
                    Motd = new Motdje(Global.Settings.Server.Port);
                }
            }
        }

        /// <summary>
        /// 获取运行时间
        /// </summary>
        /// <returns>运行时间</returns>
        public static string Time
            => Status ? (DateTime.Now - ServerProcess.StartTime).ToCustomString() : "-";

        /// <summary>
        /// Unicode转换
        /// </summary>
        /// <param name="text">输入</param>
        /// <returns>输出字符串</returns>
        private static string ConvertToUnicode(string text)
        {
            StringBuilder stringBuilder = new();
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] < 127)
                {
                    stringBuilder.Append(text[i].ToString());
                }
                else
                {
                    stringBuilder.Append(string.Format("\\u{0:x4}", (int)text[i]));
                }
            }
            return stringBuilder.ToString();
        }
    }
}
