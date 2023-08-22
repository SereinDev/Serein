using Serein.Base;
using Serein.Base.Motd;
using Serein.Core.Common;
using Serein.Core.JSPlugin;
using Serein.Extensions;
using Serein.Utils;
using Serein.Utils.IO;
using Serein.Utils.Output;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using RegExp = System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;

#if WINFORM||WPF
using System.Runtime.InteropServices;
#endif

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
        private static string _tempLine = string.Empty;

        /// <summary>
        /// 重启
        /// </summary>
        private static bool _restart;

        /// <summary>
        /// 由用户关闭服务器
        /// </summary>
        private static bool _isStoppedByUser;

        /// <summary>
        /// 服务器状态
        /// </summary>
        public static bool Status => !_serverProcess?.HasExited ?? false;

        /// <summary>
        /// CPU使用率
        /// </summary>
        public static double CPUUsage { get; private set; }

        /// <summary>
        /// Motd对象
        /// </summary>
        public static Motd? Motd { get; private set; }

        /// <summary>
        /// 更新计时器
        /// </summary>
        private static Timer? _updateTimer;

        /// <summary>
        /// 当前CPU时间
        /// </summary>
        private static TimeSpan _prevProcessCpuTime = TimeSpan.Zero;

        /// <summary>
        /// 服务器进程
        /// </summary>
        private static Process? _serverProcess;

        /// <summary>
        /// 输入流写入
        /// </summary>
        private static StreamWriter? _inputWriter;

        /// <summary>
        /// 命令历史记录列表下标
        /// </summary>
        public static int CommandHistoryIndex;

        /// <summary>
        /// 进程ID
        /// </summary>
        public static int PID => _serverProcess?.Id ?? -1;

        /// <summary>
        /// 命令历史记录
        /// </summary>
        public static readonly List<string> CommandHistory = new();

        /// <summary>
        /// 编码列表
        /// </summary>
        private static readonly Encoding[] _encodings =
        {
            new UTF8Encoding(false),
            new UTF8Encoding(true),
            new UnicodeEncoding(false, false),
            new UnicodeEncoding(true, false),
            Encoding.UTF32,
            Encoding.ASCII,
            Encoding.GetEncoding("GBK")
        };

#if CONSOLE
        /// <summary>
        /// 上一次执行强制结束时间
        /// </summary>
        private static DateTime _lastKillTime = DateTime.Now;
#endif

        /// <summary>
        /// 启动服务器
        /// </summary>
        /// <returns>启动结果</returns>
        public static bool Start() => Start(false);

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
                    MsgBox.Show("服务器已在运行中");
                }
            }
            else if (string.IsNullOrEmpty(Global.Settings.Server.Path) || string.IsNullOrWhiteSpace(Global.Settings.Server.Path))
            {
                if (!quiet)
                {
                    MsgBox.Show("启动路径为空");
                }
            }
            else if (!File.Exists(Global.Settings.Server.Path))
            {
                if (!quiet)
                {
                    MsgBox.Show($"启动文件\"{Global.Settings.Server.Path}\"未找到");
                }
            }
            else if (!quiet && Path.GetFileName(Global.Settings.Server.Path).Contains("Serein") && !MsgBox.Show("禁止禁止禁止套娃（）", true))
            {
                return false;
            }
            else
            {
#if CONSOLE
                Logger.Output(LogType.Server_Notice, "若要执行Serein指令，请使用\"serein <你的指令>\"代替原输入方式\n");
#else
                Logger.Output(LogType.Server_Clear);
#endif
                Logger.Output(LogType.Server_Notice, "启动中");

                try
                {
                    #region 主变量初始化
                    _serverProcess = Process.Start(new ProcessStartInfo(Global.Settings.Server.Path)
                    {
                        FileName = Global.Settings.Server.Path,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        RedirectStandardInput = true,
                        StandardOutputEncoding = _encodings[Global.Settings.Server.OutputEncoding],
                        WorkingDirectory = Path.GetDirectoryName(Global.Settings.Server.Path),
                        Arguments = Global.Settings.Server.Argument ?? string.Empty
                    });
                    _serverProcess!.EnableRaisingEvents = true;
                    _serverProcess.Exited += (_, _) => CloseAll();
                    _inputWriter = new(
                        _serverProcess.StandardInput.BaseStream,
                        _encodings[Global.Settings.Server.InputEncoding]
                       )
                    {
                        AutoFlush = true,
                        NewLine = string.IsNullOrEmpty(Global.Settings.Server.LineTerminator) ? Environment.NewLine : Global.Settings.Server.LineTerminator.Replace("\\n", "\n").Replace("\\r", "\r")
                    };
                    _serverProcess.BeginOutputReadLine();
                    _serverProcess.OutputDataReceived += SortOutputHandler;
                    #endregion

                    #region 变量初始化
                    _restart = false;
                    _isStoppedByUser = false;
                    LevelName = string.Empty;
                    Difficulty = string.Empty;
                    _tempLine = string.Empty;
                    CommandHistory.Clear();
                    StartFileName = Path.GetFileName(Global.Settings.Server.Path);
                    _prevProcessCpuTime = TimeSpan.Zero;
                    #endregion
                }
                catch (Exception e)
                {
                    Logger.Output(LogType.Debug, e);
                    MsgBox.Show("创建进程失败\n" + e.Message, false, true);
                    return false;
                }

                #region 服务器启动后相关调用
                Task.Run(() =>
                {
                    EventTrigger.Trigger(EventType.ServerStart);
                    JSFunc.Trigger(EventType.ServerStart);
                    _updateTimer = new(2000) { AutoReset = true };
                    _updateTimer.Elapsed += (_, _) => UpdateInfo();
                    _updateTimer.Start();
                });
                return true;
                #endregion
            }
            return false;
        }

        /// <summary>
        /// 关闭服务器
        /// </summary>
        public static void Stop() => Stop(false);

        /// <summary>
        /// 关闭服务器
        /// </summary>
        /// <param name="quiet">静处理</param>
        public static void Stop(bool quiet)
        {
            if (Status)
            {
#if WINFORM || WPF
                if (!quiet && Global.Settings.Server.StopCommands.Length == 0 && PID > 0 && Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    Logger.Output(LogType.Server_Notice, "当前未设置关服命令，将发送Ctrl+C作为替代");
                    AttachConsole((uint)PID);
                    GenerateConsoleCtrlEvent(CtrlTypes.CTRL_C_EVENT, (uint)PID);
                    FreeConsole();
                    return;
                }
#endif
                foreach (string command in Global.Settings.Server.StopCommands)
                {
                    if (!string.IsNullOrEmpty(command))
                    {
                        InputCommand(command);
                    }
                }
            }
            else if (_restart)
            {
                _restart = false;
            }
            else if (!quiet)
            {
                MsgBox.Show("服务器不在运行中");
            }
        }

#if WINFORM || WPF

        [DllImport("kernel32.dll")]
        private static extern bool AttachConsole(uint dwProcessId);

        [DllImport("kernel32.dll")]
        private static extern bool FreeConsole();

        private enum CtrlTypes : uint
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT,
            CTRL_CLOSE_EVENT,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT
        }

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GenerateConsoleCtrlEvent(CtrlTypes dwCtrlEvent, uint dwProcessGroupId);

#endif

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
                        _serverProcess?.Kill(true);
#else
                        _serverProcess?.Kill();
#endif
                        _isStoppedByUser = true;
                        _restart = false;
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
                MsgBox.Show("服务器不在运行中");
            }
#if CONSOLE
            else
            {
                DateTime nowTime = DateTime.Now;
                if ((nowTime - _lastKillTime).TotalSeconds < 2)
                {
                    _lastKillTime = nowTime;
                    try
                    {
#if NET6_0
                        _serverProcess?.Kill(true);
#else
                        _serverProcess?.Kill();
#endif
                        _isStoppedByUser = true;
                        _restart = false;
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
                    _lastKillTime = nowTime;
                    Logger.Output(LogType.Warn, "请在2s内再次执行强制结束服务器（Ctrl+C 或输入“serein s k”）以确认此操作");
                }
            }
#else
            else if (MsgBox.Show("确定结束进程吗？\n此操作可能导致存档损坏等问题", true)
#if !NET6_0
                 && (
                    !StartFileName.ToLowerInvariant().EndsWith(".bat") || (
                    StartFileName.ToLowerInvariant().EndsWith(".bat") &&
                    MsgBox.Show("由于启动文件为批处理文件（*.bat），\n强制结束进程功能可能不一定有效\n是否继续？", true)))
#endif
                )
            {
                try
                {
#if NET6_0
                    _serverProcess?.Kill(true);
#else
                    _serverProcess?.Kill();
#endif
                    _isStoppedByUser = true;
                    _restart = false;
                    return true;
                }
                catch (Exception e)
                {
                    MsgBox.Show("强制结束失败\n" + e.Message, false, true);
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
        public static void InputCommand(string command) => InputCommand(command, false, false);

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
                        !string.IsNullOrEmpty(command)) // 为空不计入
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
                _inputWriter?.WriteLine(line_copy);
                Log.Console(">" + command);
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
                    _restart = false;
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
                    if (exp1.TryParse(RegExp.RegexOptions.IgnoreCase, out RegExp.Regex? regex) && regex!.IsMatch(lineFiltered))
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
                        if (exp2.TryParse(RegExp.RegexOptions.IgnoreCase, out RegExp.Regex? regex) && regex!.IsMatch(lineFiltered))
                        {
                            _tempLine = lineFiltered.Trim('\r', '\n');
                            isMuiltLinesMode = true;
                            break;
                        }
                    }
                    if (!isMuiltLinesMode)
                    {
                        if (!string.IsNullOrEmpty(_tempLine))
                        {
                            string tempLine = _tempLine + "\n" + lineFiltered;
                            _tempLine = string.Empty;
                            Matcher.Process(tempLine);
                        }
                        else
                        {
                            Matcher.Process(lineFiltered);
                        }
                    }
                }
                Log.Console(lineFiltered);
            }
        }

        /// <summary>
        /// 关闭相关服务
        /// </summary>
        private static void CloseAll()
        {
            if (_inputWriter is null || _serverProcess is null || _updateTimer is null)
            {
                return;
            }
            _inputWriter.Close();
            _inputWriter.Dispose();
            Logger.Output(LogType.Server_Output, "");
            _updateTimer.Stop();
            if (!_isStoppedByUser && _serverProcess.ExitCode != 0)
            {
                Logger.Output(LogType.Server_Notice, $"进程疑似非正常退出（返回：{_serverProcess.ExitCode}）");
                _restart = Global.Settings.Server.EnableRestart;
                EventTrigger.Trigger(EventType.ServerExitUnexpectedly);
            }
            else
            {
                Logger.Output(LogType.Server_Notice, $"进程已退出（返回：{_serverProcess.ExitCode}）");
                EventTrigger.Trigger(EventType.ServerStop);
            }
            if (_restart)
            {
                Task.Factory.StartNew(RestartTimer);
            }
            LevelName = string.Empty;
            Difficulty = string.Empty;
            JSFunc.Trigger(EventType.ServerStop, _serverProcess.ExitCode);
        }

        /// <summary>
        /// 重启请求
        /// </summary>
        public static void RequestRestart()
        {
            Stop();
            _restart = true;
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
                if (!_restart)
                {
                    break;
                }
            }
            if (_restart)
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
            if (Status && _serverProcess is not null)
            {
                CPUUsage = (_serverProcess.TotalProcessorTime - _prevProcessCpuTime).TotalMilliseconds / 2000 / Environment.ProcessorCount * 100;
                _prevProcessCpuTime = _serverProcess.TotalProcessorTime;
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
            else
            {
                Motd = null;
            }
        }

        /// <summary>
        /// 获取运行时间
        /// </summary>
        /// <returns>运行时间</returns>
        public static string Time => Status && _serverProcess is not null ? (DateTime.Now - _serverProcess.StartTime).ToCustomString() : string.Empty;

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
