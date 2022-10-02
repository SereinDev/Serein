using Serein.Base;
using Serein.Windows;
using System;
using System.IO;
using System.Text;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;

namespace Serein
{
    internal static class Logger
    {
        public static int Type = 2;
        public static void Out(int Type, params object[] objects)
        {
            string Line = string.Empty;
            foreach (var o in objects)
            {
                if (o != null) { Line += o.ToString() + " "; }
            }
            Line = Line.TrimEnd();
            switch (Type)
            {
                case 999:
                    if (Global.Settings.Serein.Debug)
                    {
                        Catalog.Debug?.AppendText($"{DateTime.Now:T} {Line}");
                        if (!Directory.Exists(Global.Path + "\\logs\\debug"))
                        {
                            Directory.CreateDirectory(Global.Path + "\\logs\\debug");
                        }
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
                case 10:
                    Catalog.Server.Panel?.AppendText(Line);
                    break;
                case 11:
                    Catalog.Server.Panel?.AppendText("<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>" + Log.EscapeLog(Line));
                    break;
                case 20:
                    Catalog.Function.Bot?.AppendText(Line);
                    break;
                case 21:
                    Catalog.Function.Bot?.AppendText("<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>" + Log.EscapeLog(Line));
                    break;
                case 22:
                    Catalog.Function.Bot?.AppendText("<span style=\"color:#239B56;font-weight: bold;\">[↓]</span>" + Log.EscapeLog(Line));
                    break;
                case 23:
                    Catalog.Function.Bot?.AppendText("<span style=\"color:#2874A6;font-weight: bold;\">[↑]</span>" + Log.EscapeLog(Line));
                    break;
                case 24:
                    Catalog.Function.Bot?.AppendText("<span style=\"color:#BA4A00;font-weight: bold;\">[×]</span>" + Log.EscapeLog(Line));
                    break;
                case 30:
                    Catalog.Function.JSPlugin?.AppendText(Line);
                    break;
                case 31:
                    Catalog.Function.JSPlugin?.AppendText("<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>" + Log.EscapeLog(Line));
                    break;
                case 32:
                    Catalog.Function.JSPlugin?.AppendText("<span style=\"color:#BA4A00;font-weight: bold;\">[×]</span>" + Log.EscapeLog(Line));
                    break;
                case 33:
                    Catalog.Function.JSPlugin?.AppendText(Log.EscapeLog(Line));
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
                    ResizeMode = System.Windows.ResizeMode.NoResize
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
