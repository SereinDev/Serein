using Serein.Base;
using Serein.Extensions;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

#if CONSOLE
using System.Linq;
#elif WINFORM
using System.Windows.Forms;
#elif WPF
using Serein.Windows;
using System.Windows.Documents;
using System.Windows.Controls;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
#endif

namespace Serein.Utils
{
    internal static class Logger
    {
        /// <summary>
        /// 输出
        /// </summary>
        /// <param name="type">输出类型</param>
        /// <param name="objects">内容</param>
        public static void Output(LogType type, params object[] objects)
        {
#if WINFORM
            if (Program.Ui == null || Program.Ui.Disposing)
            {
                return;
            }
#endif
            StringBuilder bld = new();
            foreach (var o in objects)
            {
                if (o != null)
                {
                    if (o is Exception e)
                    {
                        bld.Append(Environment.NewLine + CrashInterception.MergeException(e));
                    }
                    else
                    {
                        bld.Append(o + " ");
                    }
                }
                else if (type == LogType.Debug || type == LogType.DetailDebug)
                {
                    bld.Append("null ");
                }
            }
            string line = bld.ToString();
            if (type != LogType.Server_Output)
            {
                line = line.TrimEnd(' ');
            }
            switch (type)
            {
#if CONSOLE
                case LogType.Info:
                case LogType.Server_Notice:
                case LogType.Bot_Notice:
                case LogType.Plugin_Notice:
                    WriteLine(1, line, true);
                    break;
                case LogType.Warn:
                    WriteLine(2, line, true);
                    break;
                case LogType.Plugin_Warn:
                    WriteLine(2, line);
                    break;
                case LogType.Error:
                case LogType.Bot_Error:
                    WriteLine(3, line, true);
                    break;
                case LogType.Plugin_Error:
                    WriteLine(3, line);
                    break;
                case LogType.Server_Output:
                    WriteLine(0, line);
                    break;
                case LogType.Bot_Receive:
                    WriteLine(1, $"\x1b[92m[↓]\x1b[0m {line}");
                    break;
                case LogType.Bot_Send:
                    WriteLine(1, $"\x1b[36m[↑]\x1b[0m {line}");
                    break;
                case LogType.Plugin_Info:
                    WriteLine(1, line);
                    break;
                case LogType.Version_New:
                    WriteLine(1, $"当前版本：{Global.VERSION} （发现新版本:{line}{(Global.Settings.Serein.AutoUpdate ? "，下载任务已在后台开始" : string.Empty)}）", true);
                    break;
                case LogType.Version_Latest:
                    WriteLine(1, "获取更新成功，当前已是最新版:)", true);
                    break;
                case LogType.Version_DownloadFailed:
                case LogType.Version_Failure:
                    WriteLine(3, $"更新{(type == LogType.Version_Failure ? "获取" : "下载")}异常：" + line, true);
                    break;
                case LogType.Version_Downloading:
                    WriteLine(1, $"正在从[{line}]下载新版本", true);
                    break;
                case LogType.Version_Ready:
                    WriteLine(1, line.Replace("\n", "，"), true);
                    break;
#elif WINFORM
                case LogType.Server_Input:
                    Program.Ui.ServerPanelConsoleWebBrowser_Invoke(LogPreProcessing.EscapeLog(line));
                    break;
                case LogType.Server_Output:
                    Program.Ui.ServerPanelConsoleWebBrowser_Invoke(LogPreProcessing.ColorLine(line, Global.Settings.Server.OutputStyle));
                    break;
                case LogType.Server_Notice:
                    Program.Ui.ServerPanelConsoleWebBrowser_Invoke("<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>" + LogPreProcessing.EscapeLog(line));
                    break;
                case LogType.Server_Clear:
                    Program.Ui.ServerPanelConsoleWebBrowser_Invoke("#clear");
                    break;
                case LogType.Bot_Notice:
                    Program.Ui.BotWebBrowser_Invoke("<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>" + LogPreProcessing.EscapeLog(line));
                    break;
                case LogType.Bot_Receive:
                    Program.Ui.BotWebBrowser_Invoke("<span style=\"color:#239B56;font-weight: bold;\">[↓]</span>" + LogPreProcessing.EscapeLog(line));
                    break;
                case LogType.Bot_Send:
                    Program.Ui.BotWebBrowser_Invoke("<span style=\"color:#2874A6;font-weight: bold;\">[↑]</span>" + LogPreProcessing.EscapeLog(line));
                    break;
                case LogType.Bot_Error:
                    Program.Ui.BotWebBrowser_Invoke("<span style=\"color:#BA4A00;font-weight: bold;\">[×]</span>" + LogPreProcessing.EscapeLog(line));
                    break;
                case LogType.Bot_Clear:
                    Program.Ui.BotWebBrowser_Invoke("#clear");
                    break;
                case LogType.Plugin_Info:
                    Program.Ui.JSPluginWebBrowser_Invoke(LogPreProcessing.EscapeLog(line));
                    break;
                case LogType.Plugin_Notice:
                    Program.Ui.JSPluginWebBrowser_Invoke("<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>" + LogPreProcessing.EscapeLog(line));
                    break;
                case LogType.Plugin_Error:
                    Program.Ui.JSPluginWebBrowser_Invoke("<span style=\"color:#BA4A00;font-weight: bold;\">[×]</span>" + LogPreProcessing.EscapeLog(line));
                    break;
                case LogType.Plugin_Warn:
                    Program.Ui.JSPluginWebBrowser_Invoke("<span style=\"color:#9c8022;font-weight: bold;\">[!]</span>" + LogPreProcessing.EscapeLog(line));
                    break;
                case LogType.Plugin_Clear:
                    Program.Ui.JSPluginWebBrowser_Invoke("#clear");
                    break;
                case LogType.Version_New:
                    Program.Ui.ShowBalloonTip("发现新版本:\n" + line);
                    Program.Ui.SettingSereinVersion_Update($"当前版本：{Global.VERSION} （发现新版本:{line}，你可以{(Global.Settings.Serein.AutoUpdate ? "等待后台自动下载或手动" : string.Empty)}点击下方链接获取最新版）");
                    break;
                case LogType.Version_Latest:
                    Program.Ui.ShowBalloonTip(
                        "获取更新成功\n" +
                        "当前已是最新版:)");
                    Program.Ui.SettingSereinVersion_Update($"当前版本：{Global.VERSION} （已是最新版）");
                    break;
                case LogType.Version_Failure:
                case LogType.Version_DownloadFailed:
                    Program.Ui.ShowBalloonTip($"更新{(type == LogType.Version_Failure ? "获取" : "下载")}异常：\n" + line);
                    break;
                case LogType.Version_Ready:
                    Program.Ui.ShowBalloonTip(line);
                    break;
#elif WPF
                case LogType.Server_Input:
                case LogType.Server_Output:
                case LogType.Server_Notice:
                    Catalog.Server.Panel?.Dispatcher.Invoke(() => Catalog.Server.Panel?.Append(LogPreProcessing.Color(type, line)));
                    Catalog.Server.Cache.Add(new(type, line));
                    if (Catalog.Server.Cache.Count > Global.Settings.Serein.MaxCacheLines)
                    {
                        Catalog.Server.Cache.RemoveRange(0, Catalog.Server.Cache.Count - Global.Settings.Serein.MaxCacheLines);
                    }
                    break;
                case LogType.Server_Clear:
                    Catalog.Server.Panel?.Dispatcher.Invoke(() => Catalog.Server.Panel?.PanelRichTextBox.Document.Blocks.Clear());
                    Catalog.Server.Cache.Clear();
                    break;
                case LogType.Bot_Notice:
                case LogType.Bot_Receive:
                case LogType.Bot_Send:
                case LogType.Bot_Error:
                    Catalog.Function.Bot?.Dispatcher.Invoke(() => Catalog.Function.Bot?.Append(LogPreProcessing.Color(type, line)));
                    Catalog.Function.BotCache.Add(new(type, line));
                    if (Catalog.Function.BotCache.Count > Global.Settings.Serein.MaxCacheLines)
                    {
                        Catalog.Function.BotCache.RemoveRange(0, Catalog.Function.BotCache.Count - Global.Settings.Serein.MaxCacheLines);
                    }
                    break;
                case LogType.Bot_Clear:
                    Catalog.Function.Bot?.Dispatcher.Invoke(() => Catalog.Function.Bot?.BotRichTextBox.Document.Blocks.Clear());
                    Catalog.Function.BotCache.Clear();
                    break;
                case LogType.Plugin_Notice:
                case LogType.Plugin_Error:
                case LogType.Plugin_Info:
                case LogType.Plugin_Warn:
                    Catalog.Function.JSPlugin?.Dispatcher.Invoke(() => Catalog.Function.JSPlugin?.Append(LogPreProcessing.Color(type, line)));
                    Catalog.Function.PluginCache.Add(new(type, line));
                    if (Catalog.Function.PluginCache.Count > Global.Settings.Serein.MaxCacheLines)
                    {
                        Catalog.Function.PluginCache.RemoveRange(0, Catalog.Function.PluginCache.Count - Global.Settings.Serein.MaxCacheLines);
                    }
                    break;
                case LogType.Plugin_Clear:
                    Catalog.Function.JSPlugin?.Dispatcher.Invoke(() => Catalog.Function.JSPlugin?.PluginRichTextBox.Document.Blocks.Clear());
                    Catalog.Function.PluginCache.Clear();
                    break;
                case LogType.Version_New:
                    Catalog.Notification?.Show(
                        "Serein",
                        "发现新版本：" + line + "\n" + (Global.Settings.Serein.AutoUpdate ? "你可以等待后台自动下载或手动点击本提示打开下载页面" : "点击本提示可打开下载页面"),
                        onClick: () => Process.Start(
                            new ProcessStartInfo("https://github.com/Zaitonn/Serein/releases/latest") { UseShellExecute = true }
                            ),
                        expirationTime: TimeSpan.FromSeconds(10)
                        );
                    Catalog.Settings.Serein?.UpdateVersion($"\n（发现新版本:{line}，你可以{(Global.Settings.Serein.AutoUpdate ? "等待后台自动下载或手动" : string.Empty)}点击下方链接获取最新版）");
                    break;
                case LogType.Version_Latest:
                    Catalog.Notification?.Show(
                        "Serein",
                        "获取更新成功\n" +
                        "当前已是最新版:)");
                    Catalog.Settings.Serein?.UpdateVersion("（已是最新版）");
                    break;
                case LogType.Version_Failure:
                case LogType.Version_DownloadFailed:
                    Catalog.Notification?.Show("Serein", $"更新{(type == LogType.Version_Failure ? "获取" : "下载")}异常：\n" + line);
                    break;
                case LogType.Version_Ready:
                    Catalog.Notification?.Show("Serein", line, expirationTime: TimeSpan.FromSeconds(10));
                    break;
#endif
                default:
                    if (!Global.Settings.Serein.DevelopmentTool.EnableDebug ||
                        Global.Settings.Serein.DevelopmentTool.DetailDebug ^ type == LogType.DetailDebug)
                    {
                        return;
                    }
                    StackTrace stackTrace = new(true);
                    line =
                        $"[{stackTrace.GetFrame(1).GetMethod().DeclaringType}" +
                        $"{(Global.Settings.Serein.DevelopmentTool.DetailDebug ? " " + stackTrace.GetFrame(1).GetMethod() : "." + stackTrace.GetFrame(1).GetMethod().Name)}] " +
                        $"{line}";
#if CONSOLE
                    WriteLine(4, line);
#elif WINFORM
                    Program.Ui.Debug_Append($"{DateTime.Now:T} {line}");
#elif WPF
                    Catalog.Debug?.AppendText($"{DateTime.Now:T} {line}");
#endif
                    Debug.WriteLine(line);
                    IO.CreateDirectory(Path.Combine("logs", "debug"));
                    try
                    {
                        lock (IO.FileLock.Debug)
                        {
                            File.AppendAllText(
                               Path.Combine("logs", "debug", $"{DateTime.Now:yyyy-MM-dd}.log"),
                               $"{DateTime.Now:T} {line}\n",
                               Encoding.UTF8
                               );
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                    }
                    break;
            }
        }

#if CONSOLE
        /// <summary>
        /// 控制台输出互斥锁
        /// </summary>
        private static readonly object Lock = new();

        /// <summary>
        /// 处理输出消息
        /// </summary>
        /// <param name="level">输出等级</param>
        /// <param name="line">输出行</param>
        private static void WriteLine(int level, string line, bool usingTitle = false)
        {
            if (line == "#clear")
            {
                return;
            }
            if (line.Contains("\n"))
            {
                line.Split('\n').ToList().ForEach((i) => WriteLine(level, i.Replace("\r", string.Empty), usingTitle));
                return;
            }
            string prefix = $"{DateTime.Now:T} ";
            lock (Lock)
            {
                prefix += level switch
                {
                    1 => "\x1b[97m[Info]\x1b[0m ",
                    2 => "\x1b[1m\x1b[93m[Warn]\x1b[0m\x1b[93m ",
                    3 => "\x1b[1m\x1b[91m[Error]\x1b[0m\x1b[91m",
                    4 => "\x1b[95m[Debug]\x1b[0m",
                    _ => "\x1b[97m",
                };
                if (usingTitle)
                {
                    if (level == 1)
                    {
                        prefix += "\x1b[96m[Serein]\x1b[0m ";
                    }
                    else if (level <= 3 && level > 1)
                    {
                        prefix += "[Serein] ";
                    }
                }
                if (level >= 1)
                {
                    line = prefix + line;
                }
                if (!Global.Settings.Serein.ColorfulLog)
                {
                    System.Console.WriteLine(System.Text.RegularExpressions.Regex.Replace(line, @"\[.*?m", string.Empty));
                    return;
                }
                if (usingTitle && line.Contains("https"))
                {
                    line = System.Text.RegularExpressions.Regex.Replace(
                        line,
                        @"(https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()!@:%_\+.~#?&\/\/=]*))",
                        "\x1b[4m\x1b[36m$1\x1b[0m");
                }
                System.Console.WriteLine(line + "\x1b[0m");
            }
        }
#endif

        /// <summary>
        /// 显示具有指定文本、标题、按钮和图标的消息框。
        /// </summary>
        /// <param name="text">要在消息框中显示的文本。</param>
        /// <param name="caption">要在消息框的标题栏中显示的文本。</param>
        /// <param name="buttons">MessageBoxButtons 值之一，可指定在消息框中显示哪些按钮。</param>
        /// <param name="icon">MessageBoxIcon 值之一，它指定在消息框中显示哪个图标。</param>
        /// <returns>按下的按钮为OK或Yes</returns>
        public static bool MsgBox(string text, string caption, int buttons, int icon)
        {
            Logger.Output(LogType.DetailDebug, new object[] { text, caption, buttons, icon }.ToJson());
#if CONSOLE
            text = text.Trim('\r', '\n');
            switch (icon)
            {
                case 48:
                    WriteLine(2, text, true);
                    break;
                case 16:
                    WriteLine(3, text, true);
                    break;
            }
            return true;
#elif WINFORM
            if (buttons == 0 && icon == 48)
            {
                text = ":(\n" + text;
            }
            DialogResult Result = MessageBox.Show(text, caption, (MessageBoxButtons)buttons, (MessageBoxIcon)icon);
            return Result == DialogResult.OK || Result == DialogResult.Yes;
#elif WPF
            if (buttons == 0)
            {
                if (text.Contains("\n"))
                {
                    Catalog.MainWindow.OpenSnackbar(
                        text.Split('\n')[0],
                        text.Substring(text.IndexOf('\n')).TrimStart('\n'),
                        icon == 48 ? SymbolRegular.Warning24 : SymbolRegular.Dismiss24
                        );
                }
                else
                {
                    Catalog.MainWindow.OpenSnackbar(
                        "执行失败",
                        text,
                        icon == 48 ? SymbolRegular.Warning24 : SymbolRegular.Dismiss24
                        );
                }
                return true;
            }
            bool confirmed = false;
            MessageBox messageBox = new()
            {
                Title = caption,
                Content = new TextBlock()
                {
                    Text = text,
                    TextWrapping = System.Windows.TextWrapping.WrapWithOverflow
                },
                ShowInTaskbar = false,
                ResizeMode = System.Windows.ResizeMode.NoResize,
                Topmost = true,
                Width = 350,
                ButtonLeftName = buttons <= 1 ? "确定" : "是",
                ButtonRightName = buttons <= 1 ? "取消" : "否"
            };
            messageBox.ButtonRightClick += (_, _) => messageBox.Close();
            messageBox.ButtonLeftClick += (_, _) =>
            {
                confirmed = true;
                messageBox.Close();
            };
            messageBox.ShowDialog();
            return confirmed;
#endif
        }
    }
}
