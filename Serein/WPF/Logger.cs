using Serein.Base;
using Serein.Items;
using Serein.Windows;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;

namespace Serein
{
    internal static class Logger
    {
        const int MaxLines = 40;
        public static readonly int Type = 2;

        public static void Out(LogType Type, params object[] objects)
        {
            string Line = string.Empty;
            foreach (var o in objects)
            {
                if (o != null)
                    Line += o.ToString() + " ";
            }
            Line = Line.TrimEnd();
            switch (Type)
            {
                case LogType.Debug:
                    if (Global.Settings.Serein.Debug)
                    {
                        Catalog.Debug?.AppendText($"{DateTime.Now:T} {Line}");
                        if (!Directory.Exists(Global.Path + "\\logs\\debug"))
                            Directory.CreateDirectory(Global.Path + "\\logs\\debug");
                        try
                        {
                            File.AppendAllText(
                                Global.Path + $"\\logs\\debug\\{DateTime.Now:yyyy-MM-dd}.log",
                                $"{DateTime.Now:T} {Line}" + "\n",
                                Encoding.UTF8
                                );
                        }
                        catch { }
                    }
                    break;
                case LogType.Server_Output:
                    Catalog.Server.Panel?.AppendText(Line);
                    Catalog.Server.Cache.Add(Line);
                    if (Catalog.Server.Cache.Count > MaxLines)
                        Catalog.Server.Cache.RemoveRange(0, Catalog.Server.Cache.Count - MaxLines);
                    break;
                case LogType.Server_Notice:
                    Line = "<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>" + Log.EscapeLog(Line);
                    Catalog.Server.Panel?.AppendText(Line);
                    Catalog.Server.Cache.Add(Line);
                    if (Catalog.Server.Cache.Count > MaxLines)
                        Catalog.Server.Cache.RemoveRange(0, Catalog.Server.Cache.Count - MaxLines);
                    break;
                case LogType.Server_Clear:
                    Catalog.Server.Panel?.AppendText("#clear");
                    Catalog.Server.Cache.Clear();
                    break;
                case LogType.Bot_Output:
                    Catalog.Function.Bot?.AppendText(Line);
                    Catalog.Function.BotCache.Add(Line);
                    if (Catalog.Function.BotCache.Count > MaxLines)
                        Catalog.Function.BotCache.RemoveRange(0, Catalog.Function.BotCache.Count - MaxLines);
                    break;
                case LogType.Bot_Notice:
                    Line = "<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>" + Log.EscapeLog(Line);
                    Catalog.Function.Bot?.AppendText(Line);
                    Catalog.Function.BotCache.Add(Line);
                    if (Catalog.Function.BotCache.Count > MaxLines)
                        Catalog.Function.BotCache.RemoveRange(0, Catalog.Function.BotCache.Count - MaxLines);
                    break;
                case LogType.Bot_Receive:
                    Line = "<span style=\"color:#239B56;font-weight: bold;\">[↓]</span>" + Log.EscapeLog(Line);
                    Catalog.Function.Bot?.AppendText(Line);
                    Catalog.Function.BotCache.Add(Line);
                    if (Catalog.Function.BotCache.Count > MaxLines)
                        Catalog.Function.BotCache.RemoveRange(0, Catalog.Function.BotCache.Count - MaxLines);
                    break;
                case LogType.Bot_Send:
                    Line = "<span style=\"color:#2874A6;font-weight: bold;\">[↑]</span>" + Log.EscapeLog(Line);
                    Catalog.Function.Bot?.AppendText(Line);
                    Catalog.Function.BotCache.Add(Line);
                    if (Catalog.Function.BotCache.Count > MaxLines)
                        Catalog.Function.BotCache.RemoveRange(0, Catalog.Function.BotCache.Count - MaxLines);
                    break;
                case LogType.Bot_Error:
                    Line = "<span style=\"color:#BA4A00;font-weight: bold;\">[×]</span>" + Log.EscapeLog(Line);
                    Catalog.Function.Bot?.AppendText(Line);
                    Catalog.Function.BotCache.Add(Line);
                    if (Catalog.Function.BotCache.Count > MaxLines)
                        Catalog.Function.BotCache.RemoveRange(0, Catalog.Function.BotCache.Count - MaxLines);
                    break;
                case LogType.Bot_Clear:
                    Catalog.Function.Bot?.AppendText("#clear");
                    Catalog.Function.BotCache.Clear();
                    break;
                case LogType.Plugin_Notice:
                    Line = "<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>" + Log.EscapeLog(Line);
                    Catalog.Function.JSPlugin?.AppendText(Line);
                    Catalog.Function.PluginCache.Add(Line);
                    if (Catalog.Function.PluginCache.Count > MaxLines)
                        Catalog.Function.PluginCache.RemoveRange(0, Catalog.Function.PluginCache.Count - MaxLines);
                    break;
                case LogType.Plugin_Error:
                    Line = "<span style=\"color:#BA4A00;font-weight: bold;\">[×]</span>" + Log.EscapeLog(Line);
                    Catalog.Function.JSPlugin?.AppendText(Line);
                    Catalog.Function.PluginCache.Add(Line);
                    if (Catalog.Function.PluginCache.Count > MaxLines)
                        Catalog.Function.PluginCache.RemoveRange(0, Catalog.Function.PluginCache.Count - MaxLines);
                    break;
                case LogType.Plugin_Info:
                    Line = Log.EscapeLog(Line);
                    Catalog.Function.JSPlugin?.AppendText(Line);
                    Catalog.Function.PluginCache.Add(Line);
                    if (Catalog.Function.PluginCache.Count > MaxLines)
                        Catalog.Function.PluginCache.RemoveRange(0, Catalog.Function.PluginCache.Count - MaxLines);
                    break;
                case LogType.Plugin_Warn:
                    Line = "<span style=\"color:#9c8022;font-weight: bold;\">[!]</span>" + Log.EscapeLog(Line);
                    Catalog.Function.JSPlugin?.AppendText(Line);
                    Catalog.Function.PluginCache.Add(Line);
                    if (Catalog.Function.PluginCache.Count > MaxLines)
                        Catalog.Function.PluginCache.RemoveRange(0, Catalog.Function.PluginCache.Count - MaxLines);
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
            }
        }

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
            if (Buttons == 0)
            {
                Catalog.MainWindow.OpenSnackbar(
                    "执行失败",
                    Text.Replace(":(\n", string.Empty),
                    Icon == 48 ? SymbolRegular.Warning24 : SymbolRegular.Dismiss24
                    );
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
                    Topmost = true
                };
                Msg.ButtonLeftName = Buttons <= 1 ? "确定" : "是";
                Msg.ButtonRightName = Buttons <= 1 ? "取消" : "否";
                Msg.ButtonRightClick += (sender, e) => Msg.Close();
                Msg.ButtonLeftClick += (sender, e) =>
                {
                    Confirmed = true;
                    Msg.Close();
                };
                Msg.ShowDialog();
                return Confirmed;
            }
        }
    }
}
