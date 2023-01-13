using Serein.Base;
using Serein.Items;
using Serein.JSPlugin;
using Serein.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Serein.Server
{
    public static class ServerManager
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

        private static string TempLine = string.Empty;

        private static bool Restart;

        public static bool Killed { get; private set; }

        /// <summary>
        /// 服务器状态
        /// </summary>
        public static bool Status => ServerProcess != null && !ServerProcess.HasExited;

        /// <summary>
        /// CPU使用率
        /// </summary>
        public static double CPUUsage { get; private set; }

        /// <summary>
        /// 当前CPU时间
        /// </summary>
        private static TimeSpan PrevProcessCpuTime = TimeSpan.Zero;

        /// <summary>
        /// 服务器进程
        /// </summary>
        private static Process ServerProcess;

        /// <summary>
        /// 输入流写入者
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
                    Logger.MsgBox(":(\n服务器已在运行中", "Serein", 0, 48);
                }
            }
            else if (string.IsNullOrEmpty(Global.Settings.Server.Path) || string.IsNullOrWhiteSpace(Global.Settings.Server.Path))
            {
                if (!quiet)
                {
                    Logger.MsgBox(":(\n启动路径为空", "Serein", 0, 48);
                }
            }
            else if (!File.Exists(Global.Settings.Server.Path))
            {
                if (!quiet)
                {
                    Logger.MsgBox($":(\n启动文件\"{Global.Settings.Server.Path}\"未找到", "Serein", 0, 48);
                }
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
                LevelName = string.Empty;
                Difficulty = string.Empty;
                TempLine = string.Empty;
                CommandHistory.Clear();
                StartFileName = Path.GetFileName(Global.Settings.Server.Path);
                PrevProcessCpuTime = TimeSpan.Zero;
                System.Threading.Tasks.Task.Factory.StartNew(UpdateCPUUsage);
                EventTrigger.Trigger(EventType.ServerStart);
                JSFunc.Trigger(EventType.ServerStart);
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
                Logger.MsgBox(":(\n服务器不在运行中", "Serein", 0, 48);
            }
        }

        /// <summary>
        /// 强制结束服务器
        /// </summary>
        /// <returns>强制结束结果</returns>
        public static bool Kill()
            => Kill(false);

        /// <summary>
        /// 强制结束服务器
        /// </summary>
        /// <param name="quiet">静处理</param>
        /// <returns>强制结束结果</returns>
        public static bool Kill(bool quiet)
        {
            if (quiet)
            {
                if (!Status)
                {
                    return false;
                }
                try
                {
                    ServerProcess.Kill();
                    Killed = true;
                    Restart = false;
                    return true;
                }
                catch (Exception e)
                {
                    Logger.Out(LogType.Debug, e);
                }
                return false;
            }
            else if (!Status)
            {
                Logger.MsgBox(":(\n服务器不在运行中", "Serein", 0, 48);
            }
#if CONSOLE
            else
            {
                DateTime nowTime = DateTime.Now;
                if ((nowTime - LastKillTime).TotalSeconds < 5)
                {
                    LastKillTime = nowTime;
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
                    LastKillTime = nowTime;
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
        /// 输入行
        /// </summary>
        /// <param name="line">行</param>
        /// <param name="quiet">静处理</param>
        /// <param name="usingUnicode">使用Unicode</param>
        public static void InputCommand(string line, bool quiet = false, bool usingUnicode = false)
        {
            if (Status)
            {
                line = line.TrimEnd('\n').Replace("\n", "\\n").Replace("\r", "\\n");
                string line_copy = line;
                if (CommandHistory.Count > 50)
                {
                    CommandHistory.RemoveRange(0, CommandHistory.Count - 50);
                }
                if (
                    (CommandHistory.Count > 0 && CommandHistory[CommandHistory.Count - 1] != line || CommandHistory.Count == 0) &&
                    (!quiet || !(string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line))))
                {
                    CommandHistoryIndex = CommandHistory.Count + 1;
                    CommandHistory.Add(line);
                }
#if !CONSOLE
                if (Global.Settings.Server.EnableOutputCommand)
                {
                    Logger.Out(LogType.Server_Output, $">{Log.EscapeLog(line)}");
                }
#endif
                if (usingUnicode || Global.Settings.Server.EnableUnicode)
                {
                    line_copy = ConvertToUnicode(line_copy);
                }
                InputWriter.WriteLine(line_copy);
                JSFunc.Trigger(EventType.ServerSendCommand, line);
                if (Global.Settings.Server.EnableLog)
                {
                    if (!Directory.Exists(IO.GetPath("logs", "console")))
                    {
                        Directory.CreateDirectory(IO.GetPath("logs", "console"));
                    }
                    try
                    {
                        File.AppendAllText(
                            IO.GetPath("logs", "console", $"{DateTime.Now:yyyy-MM-dd}.log"),
                            ">" + Log.OutputRecognition(line) + "\n",
                            Encoding.UTF8
                        );
                    }
                    catch { }
                }
                line = null;
            }
            else if (line.Trim().ToLower() == "start")
            {
                Start(quiet);
            }
            else if (line.Trim().ToLower() == "stop")
            {
                Restart = false;
            }
        }

        /// <summary>
        /// 输出处理
        /// </summary>
        private static void SortOutputHandler(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                string line = Log.OutputRecognition(e.Data);
                if (string.IsNullOrEmpty(LevelName) && System.Text.RegularExpressions.Regex.IsMatch(line, Global.Settings.Matches.LevelName, RegexOptions.IgnoreCase))
                {
                    LevelName = System.Text.RegularExpressions.Regex.Match(line, Global.Settings.Matches.LevelName, RegexOptions.IgnoreCase).Groups[1].Value.Trim();
                }
                else if (string.IsNullOrEmpty(Difficulty) && System.Text.RegularExpressions.Regex.IsMatch(line, Global.Settings.Matches.Difficulty, RegexOptions.IgnoreCase))
                {
                    Difficulty = System.Text.RegularExpressions.Regex.Match(line, Global.Settings.Matches.Difficulty, RegexOptions.IgnoreCase).Groups[1].Value.Trim();
                }
#if CONSOLE
                Logger.Out(LogType.Server_Output, e.Data);
#else
                Logger.Out(LogType.Server_Output, Log.ColorLog(e.Data, Global.Settings.Server.OutputStyle));
#endif
                if (Global.Settings.Server.EnableLog)
                {
                    if (!Directory.Exists(IO.GetPath("logs", "console")))
                    {
                        Directory.CreateDirectory(IO.GetPath("logs", "console"));
                    }
                    try
                    {
                        File.AppendAllText(
                            IO.GetPath("logs", "console", $"{DateTime.Now:yyyy-MM-dd}.log"),
                            Log.OutputRecognition(line) + "\n",
                            Encoding.UTF8
                        );
                    }
                    catch { }
                }
                bool isMuiltLinesMode = false;
                foreach (string regex in Global.Settings.Matches.MuiltLines)
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(line, regex, RegexOptions.IgnoreCase))
                    {
                        TempLine = line.Trim('\r', '\n');
                        isMuiltLinesMode = true;
                        break;
                    }
                }
                if (!isMuiltLinesMode)
                {
                    if (!string.IsNullOrEmpty(TempLine))
                    {
                        string tempLine = TempLine + "\n" + line;
                        TempLine = string.Empty;
                        Matcher.Process(tempLine);
                    }
                    else
                    {
                        Matcher.Process(line);
                    }
                }
                JSFunc.Trigger(EventType.ServerOutput, line);
                JSFunc.Trigger(EventType.ServerOriginalOutput, e.Data);
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
                Logger.Out(LogType.Server_Notice, $"进程疑似非正常退出（返回：{ServerProcess.ExitCode}）");
                Restart = Global.Settings.Server.EnableRestart;
                EventTrigger.Trigger(EventType.ServerExitUnexpectedly);
            }
            else
            {
                Logger.Out(LogType.Server_Notice, $"进程已退出（返回：{ServerProcess.ExitCode}）");
                EventTrigger.Trigger(EventType.ServerStop);
            }
            if (Restart)
            {
                System.Threading.Tasks.Task.Factory.StartNew(RestartTimer);
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
                Restart = Status;
                Stop();
            }
        }

        /// <summary>
        /// 重启计时器
        /// </summary>
        private static void RestartTimer()
        {
            Logger.Out(LogType.Server_Notice,
                $"服务器将在5s后（{DateTime.Now.AddSeconds(5):T}）重新启动"
                );
#if CONSOLE
            Logger.Out(LogType.Server_Notice, "你可以输入\"stop\"来取消这次重启");
#else
            Logger.Out(LogType.Server_Notice, "你可以按下停止按钮来取消这次重启");
#endif
            for (int i = 0; i < 10; i++)
            {
                System.Threading.Tasks.Task.Delay(500).GetAwaiter().GetResult();
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
                Logger.Out(LogType.Server_Notice, "重启已取消");
            }
        }

        /// <summary>
        /// 获取CPU占用
        /// </summary>
        public static void UpdateCPUUsage()
        {
            while (Status)
            {
                System.Threading.Tasks.Task.Delay(2000).GetAwaiter().GetResult();
                CPUUsage = (ServerProcess.TotalProcessorTime - PrevProcessCpuTime).TotalMilliseconds / 2000 / Environment.ProcessorCount * 100;
                PrevProcessCpuTime = ServerProcess.TotalProcessorTime;
                if (CPUUsage > 100)
                {
                    CPUUsage = 100;
                }
            }
        }

        /// <summary>
        /// 获取运行时间
        /// </summary>
        /// <returns>运行时间</returns>
        public static string GetTime()
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
