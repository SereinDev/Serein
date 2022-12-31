using Serein.Items;
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
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
#endif

namespace Serein.Base
{
    internal static class Logger
    {
        /// <summary>
        /// 输出
        /// </summary>
        /// <param name="Type">输出类型</param>
        /// <param name="objects">内容</param>
        public static void Out(LogType Type, params object[] objects)
        {
#if WINFORM
            if (Program.Ui == null || Program.Ui.Disposing)
            {
                return;
            }
#endif
            StringBuilder bld = new StringBuilder();
            foreach (var o in objects)
            {
                if (o != null)
                {
                    if (o is Exception e)
                    {
                        bld.Append("\r\n" + CrashInterception.MergeException(e));
                    }
                    else
                    {
                        bld.Append(o.ToString() + " ");
                    }
                }
            }
            string Line = bld.ToString().TrimEnd();
            switch (Type)
            {
#if CONSOLE
                case LogType.Info:
                case LogType.Server_Notice:
                case LogType.Bot_Notice:
                case LogType.Plugin_Notice:
                    WriteLine(1, Line, true);
                    break;
                case LogType.Warn:
                    WriteLine(2, Line, true);
                    break;
                case LogType.Plugin_Warn:
                    WriteLine(2, Line);
                    break;
                case LogType.Error:
                case LogType.Bot_Error:
                    WriteLine(3, Line, true);
                    break;
                case LogType.Plugin_Error:
                    WriteLine(3, Line);
                    break;
                case LogType.Null:
                case LogType.Server_Output:
                    WriteLine(0, Line);
                    break;
                case LogType.Bot_Receive:
                    WriteLine(1, $"\x1b[92m[↓]\x1b[0m {Line}");
                    break;
                case LogType.Bot_Send:
                    WriteLine(1, $"\x1b[36m[↑]\x1b[0m {Line}");
                    break;
                case LogType.Plugin_Info:
                    WriteLine(1, Line);
                    break;
                case LogType.Version_New:
                    WriteLine(1, $"当前版本：{Global.VERSION} （发现新版本:{Line}，你可以打开" +
                        $"\x1b[4m\x1b[36mhttps://github.com/Zaitonn/Serein/releases/latest\x1b[0m获取最新版）", true);
                    break;
                case LogType.Version_Latest:
                    WriteLine(1, "获取更新成功，当前已是最新版:)", true);
                    break;
                case LogType.Version_Failure:
                    WriteLine(3, "更新获取异常：\n" + Line, true);
                    break;
#elif WINFORM
                case LogType.Server_Output:
                    Program.Ui.PanelConsoleWebBrowser_Invoke(Line);
                    break;
                case LogType.Server_Notice:
                    Program.Ui.PanelConsoleWebBrowser_Invoke("<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>" + Log.EscapeLog(Line));
                    break;
                case LogType.Server_Clear:
                    Program.Ui.PanelConsoleWebBrowser_Invoke("#clear");
                    break;
                case LogType.Bot_Notice:
                    Program.Ui.BotWebBrowser_Invoke("<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>" + Log.EscapeLog(Line));
                    break;
                case LogType.Bot_Receive:
                    Program.Ui.BotWebBrowser_Invoke("<span style=\"color:#239B56;font-weight: bold;\">[↓]</span>" + Log.EscapeLog(Line));
                    break;
                case LogType.Bot_Send:
                    Program.Ui.BotWebBrowser_Invoke("<span style=\"color:#2874A6;font-weight: bold;\">[↑]</span>" + Log.EscapeLog(Line));
                    break;
                case LogType.Bot_Error:
                    Program.Ui.BotWebBrowser_Invoke("<span style=\"color:#BA4A00;font-weight: bold;\">[×]</span>" + Log.EscapeLog(Line));
                    break;
                case LogType.Bot_Clear:
                    Program.Ui.BotWebBrowser_Invoke("#clear");
                    break;
                case LogType.Plugin_Info:
                    Program.Ui.SereinPluginsWebBrowser_Invoke(Log.EscapeLog(Line));
                    break;
                case LogType.Plugin_Notice:
                    Program.Ui.SereinPluginsWebBrowser_Invoke("<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>" + Log.EscapeLog(Line));
                    break;
                case LogType.Plugin_Error:
                    Program.Ui.SereinPluginsWebBrowser_Invoke("<span style=\"color:#BA4A00;font-weight: bold;\">[×]</span>" + Log.EscapeLog(Line));
                    break;
                case LogType.Plugin_Warn:
                    Program.Ui.SereinPluginsWebBrowser_Invoke("<span style=\"color:#9c8022;font-weight: bold;\">[!]</span>" + Log.EscapeLog(Line));
                    break;
                case LogType.Plugin_Clear:
                    Program.Ui.SereinPluginsWebBrowser_Invoke("#clear");
                    break;
                case LogType.Version_New:
                    Program.Ui.ShowBalloonTip("发现新版本:\n" + Line);
                    Program.Ui.SettingSereinVersion_Update($"当前版本：{Global.VERSION} （发现新版本:{Line}，你可以点击下方链接获取最新版）");
                    break;
                case LogType.Version_Latest:
                    Program.Ui.ShowBalloonTip(
                        "获取更新成功\n" +
                        "当前已是最新版:)");
                    Program.Ui.SettingSereinVersion_Update($"当前版本：{Global.VERSION} （已是最新版qwq）");
                    break;
                case LogType.Version_Failure:
                    Program.Ui.ShowBalloonTip("更新获取异常：\n" + Line);
                    break;
#elif WPF
                case LogType.Server_Output:
                    Catalog.Server.Panel?.AppendText(Line);
                    Catalog.Server.Cache.Add(Line);
                    if (Catalog.Server.Cache.Count > Global.Settings.Serein.MaxCacheLines)
                    {
                        Catalog.Server.Cache.RemoveRange(0, Catalog.Server.Cache.Count - Global.Settings.Serein.MaxCacheLines);
                    }
                    break;
                case LogType.Server_Notice:
                    Line = "<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>" + Log.EscapeLog(Line);
                    Catalog.Server.Panel?.AppendText(Line);
                    Catalog.Server.Cache.Add(Line);
                    if (Catalog.Server.Cache.Count > Global.Settings.Serein.MaxCacheLines)
                    {
                        Catalog.Server.Cache.RemoveRange(0, Catalog.Server.Cache.Count - Global.Settings.Serein.MaxCacheLines);
                    }
                    break;
                case LogType.Server_Clear:
                    Catalog.Server.Panel?.AppendText("#clear");
                    Catalog.Server.Cache.Clear();
                    break;
                case LogType.Bot_Notice:
                    Line = "<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>" + Log.EscapeLog(Line);
                    Catalog.Function.Bot?.AppendText(Line);
                    Catalog.Function.BotCache.Add(Line);
                    if (Catalog.Function.BotCache.Count > Global.Settings.Serein.MaxCacheLines)
                    {
                        Catalog.Function.BotCache.RemoveRange(0, Catalog.Function.BotCache.Count - Global.Settings.Serein.MaxCacheLines);
                    }
                    break;
                case LogType.Bot_Receive:
                    Line = "<span style=\"color:#239B56;font-weight: bold;\">[↓]</span>" + Log.EscapeLog(Line);
                    Catalog.Function.Bot?.AppendText(Line);
                    Catalog.Function.BotCache.Add(Line);
                    if (Catalog.Function.BotCache.Count > Global.Settings.Serein.MaxCacheLines)
                    {
                        Catalog.Function.BotCache.RemoveRange(0, Catalog.Function.BotCache.Count - Global.Settings.Serein.MaxCacheLines);
                    }
                    break;
                case LogType.Bot_Send:
                    Line = "<span style=\"color:#2874A6;font-weight: bold;\">[↑]</span>" + Log.EscapeLog(Line);
                    Catalog.Function.Bot?.AppendText(Line);
                    Catalog.Function.BotCache.Add(Line);
                    if (Catalog.Function.BotCache.Count > Global.Settings.Serein.MaxCacheLines)
                    {
                        Catalog.Function.BotCache.RemoveRange(0, Catalog.Function.BotCache.Count - Global.Settings.Serein.MaxCacheLines);
                    }
                    break;
                case LogType.Bot_Error:
                    Line = "<span style=\"color:#BA4A00;font-weight: bold;\">[×]</span>" + Log.EscapeLog(Line);
                    Catalog.Function.Bot?.AppendText(Line);
                    Catalog.Function.BotCache.Add(Line);
                    if (Catalog.Function.BotCache.Count > Global.Settings.Serein.MaxCacheLines)
                    {
                        Catalog.Function.BotCache.RemoveRange(0, Catalog.Function.BotCache.Count - Global.Settings.Serein.MaxCacheLines);
                    }
                    break;
                case LogType.Bot_Clear:
                    Catalog.Function.Bot?.AppendText("#clear");
                    Catalog.Function.BotCache.Clear();
                    break;
                case LogType.Plugin_Notice:
                    Line = "<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>" + Log.EscapeLog(Line);
                    Catalog.Function.JSPlugin?.AppendText(Line);
                    Catalog.Function.PluginCache.Add(Line);
                    if (Catalog.Function.PluginCache.Count > Global.Settings.Serein.MaxCacheLines)
                    {
                        Catalog.Function.PluginCache.RemoveRange(0, Catalog.Function.PluginCache.Count - Global.Settings.Serein.MaxCacheLines);
                    }
                    break;
                case LogType.Plugin_Error:
                    Line = "<span style=\"color:#BA4A00;font-weight: bold;\">[×]</span>" + Log.EscapeLog(Line);
                    Catalog.Function.JSPlugin?.AppendText(Line);
                    Catalog.Function.PluginCache.Add(Line);
                    if (Catalog.Function.PluginCache.Count > Global.Settings.Serein.MaxCacheLines)
                    {
                        Catalog.Function.PluginCache.RemoveRange(0, Catalog.Function.PluginCache.Count - Global.Settings.Serein.MaxCacheLines);
                    }
                    break;
                case LogType.Plugin_Info:
                    Line = Log.EscapeLog(Line);
                    Catalog.Function.JSPlugin?.AppendText(Line);
                    Catalog.Function.PluginCache.Add(Line);
                    if (Catalog.Function.PluginCache.Count > Global.Settings.Serein.MaxCacheLines)
                    {
                        Catalog.Function.PluginCache.RemoveRange(0, Catalog.Function.PluginCache.Count - Global.Settings.Serein.MaxCacheLines);
                    }
                    break;
                case LogType.Plugin_Warn:
                    Line = "<span style=\"color:#9c8022;font-weight: bold;\">[!]</span>" + Log.EscapeLog(Line);
                    Catalog.Function.JSPlugin?.AppendText(Line);
                    Catalog.Function.PluginCache.Add(Line);
                    if (Catalog.Function.PluginCache.Count > Global.Settings.Serein.MaxCacheLines)
                    {
                        Catalog.Function.PluginCache.RemoveRange(0, Catalog.Function.PluginCache.Count - Global.Settings.Serein.MaxCacheLines);
                    }
                    break;
                case LogType.Plugin_Clear:
                    Catalog.Function.JSPlugin?.AppendText("#clear");
                    Catalog.Function.PluginCache.Clear();
                    break;
                case LogType.Version_New:
                    Catalog.Notification?.Show(
                        "Serein",
                        "发现新版本:\n" + Line + "\n点击此处打开下载页面",
                        onClick: () => Process.Start(
                            new ProcessStartInfo("https://github.com/Zaitonn/Serein/releases/latest") { UseShellExecute = true }
                            ),
                        expirationTime: new TimeSpan(100)
                        );
                    Catalog.Settings.Serein?.UpdateVersion($"（发现新版本:{Line}，你可以点击下方链接获取最新版）");
                    break;
                case LogType.Version_Latest:
                    Catalog.Notification?.Show(
                        "Serein",
                        "获取更新成功\n" +
                        "当前已是最新版:)");
                    Catalog.Settings.Serein?.UpdateVersion("（已是最新版）");
                    break;
                case LogType.Version_Failure:
                    Catalog.Notification?.Show("Serein", "更新获取异常：\n" + Line);
                    break;
#endif
                case LogType.Debug:
                case LogType.DetailDebug:
                default:
                    if (!Global.Settings.Serein.DevelopmentTool.EnableDebug ||
                        Global.Settings.Serein.DevelopmentTool.DetailDebug ^ Type == LogType.DetailDebug)
                    {
                        return;
                    }
                    StackTrace st = new StackTrace(true);
                    Line =
                        $"[{st.GetFrame(1).GetMethod().DeclaringType}" +
                        $"{(Global.Settings.Serein.DevelopmentTool.DetailDebug ? " " + st.GetFrame(1).GetMethod() : "." + st.GetFrame(1).GetMethod().Name)}] " +
                        $"{Line}";
#if CONSOLE
                    WriteLine(4, Line);
#elif WINFORM
                    Program.Ui.Debug_Append($"{DateTime.Now:T} {Line}");
#elif WPF
                    Catalog.Debug?.AppendText($"{DateTime.Now:T} {Line}");
#endif
                    if (!Directory.Exists(IO.GetPath("logs", "debug")))
                    {
                        Directory.CreateDirectory(IO.GetPath("logs", "debug"));
                    }
                    try
                    {
                        File.AppendAllText(
                            IO.GetPath("logs", "debug", $"{DateTime.Now:yyyy-MM-dd}.log"),
                            $"{DateTime.Now:T} {Line}\n",
                            Encoding.UTF8
                            );
                    }
                    catch { }
                    break;
            }
        }

#if CONSOLE
        /// <summary>
        /// 控制台输出互斥锁
        /// </summary>
        private static readonly object Lock = new object();

        /// <summary>
        /// 处理输出消息
        /// </summary>
        /// <param name="Level">输出等级</param>
        /// <param name="Line">输出行</param>
        private static void WriteLine(int Level, string Line, bool SereinTitle = false)
        {
            if (Line == "#clear" || string.IsNullOrEmpty(Line) || string.IsNullOrWhiteSpace(Line))
            {
                return;
            }
            if (Line.Contains("\n"))
            {
                Line.Split('\n').ToList().ForEach((i) => WriteLine(Level, i.Replace("\r", string.Empty), SereinTitle));
                return;
            }
            string Prefix = $"{DateTime.Now:T} ";
            lock (Lock)
            {
                switch (Level)
                {
                    case 1:
                        Prefix += "\x1b[97m[Info]\x1b[0m ";
                        break;
                    case 2:
                        Prefix += "\x1b[1m\x1b[93m[Warn]\x1b[0m\x1b[93m ";
                        break;
                    case 3:
                        Prefix += "\x1b[1m\x1b[91m[Error]\x1b[0m\x1b[91m";
                        break;
                    case 4:
                        Prefix += "\x1b[95m[Debug]\x1b[0m";
                        break;
                    default:
                        Prefix += "\x1b[97m";
                        break;
                }
                if (SereinTitle)
                {
                    if (Level == 1)
                    {
                        Prefix += "\x1b[96m[Serein]\x1b[0m ";
                    }
                    else if (Level <= 3 && Level > 1)
                    {
                        Prefix += "[Serein] ";
                    }
                }
                if (Level >= 1)
                {
                    Line = Prefix + Line;
                }
                if (!Global.Settings.Serein.ColorfulLog)
                {
                    System.Console.WriteLine(System.Text.RegularExpressions.Regex.Replace(Line, @"\[.*?m", string.Empty));
                }
                else
                {
                    System.Console.WriteLine(Line + "\x1b[0m");
                }
            }
        }
#endif

        /// <summary>
        /// 显示具有指定文本、标题、按钮和图标的消息框。
        /// </summary>
        /// <param name="Text">要在消息框中显示的文本。</param>
        /// <param name="Caption">要在消息框的标题栏中显示的文本。</param>
        /// <param name="Buttons">MessageBoxButtons 值之一，可指定在消息框中显示哪些按钮。</param>
        /// <param name="Icon">MessageBoxIcon 值之一，它指定在消息框中显示哪个图标。</param>
        /// <returns>按下的按钮为OK或Yes</returns>
        public static bool MsgBox(string Text, string Caption, int Buttons, int Icon)
        {
#if CONSOLE
            Text = Text.Replace(":(", string.Empty).Trim('\r', '\n');
            switch (Icon)
            {
                case 48:
                    WriteLine(2, Text, true);
                    break;
                case 16:
                    WriteLine(3, Text, true);
                    break;
            }
            return true;
#elif WINFORM
            DialogResult Result = MessageBox.Show(Text, Caption, (MessageBoxButtons)Buttons, (MessageBoxIcon)Icon);
            return Result == DialogResult.OK || Result == DialogResult.Yes;
#elif WPF
            if (Buttons == 0)
            {
                Text = Text.Replace(":(\n", string.Empty);
                if (Text.Contains("\n"))
                {
                    Catalog.MainWindow.OpenSnackbar(
                        Text.Split('\n')[0],
                        Text.Substring(Text.IndexOf('\n')).TrimStart('\n'),
                        Icon == 48 ? SymbolRegular.Warning24 : SymbolRegular.Dismiss24
                        );
                }
                else
                {
                    Catalog.MainWindow.OpenSnackbar(
                        "执行失败",
                        Text,
                        Icon == 48 ? SymbolRegular.Warning24 : SymbolRegular.Dismiss24
                        );
                }
                return true;
            }
            else
            {
                bool Confirmed = false;
                MessageBox Msg = new MessageBox()
                {
                    Title = Caption,
                    Content = Text,
                    ShowInTaskbar = false,
                    ResizeMode = System.Windows.ResizeMode.NoResize,
                    Topmost = true,
                    ButtonLeftName = Buttons <= 1 ? "确定" : "是",
                    ButtonRightName = Buttons <= 1 ? "取消" : "否"
                };
                Msg.ButtonRightClick += (sender, e) => Msg.Close();
                Msg.ButtonLeftClick += (sender, e) =>
                {
                    Confirmed = true;
                    Msg.Close();
                };
                Msg.ShowDialog();
                return Confirmed;
            }
#endif
        }
    }
}
