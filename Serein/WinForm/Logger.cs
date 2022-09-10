using Serein.Base;
using System;
using System.Windows.Forms;

namespace Serein
{
    internal static class Logger
    {
        public static int Type = 1;
        public static void Out(int Type, params object[] objects)
        {
            if (Program.Ui != null && !Program.Ui.Disposing)
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
                        Program.Ui.Debug_Append($"{DateTime.Now:T} {Line}");
                        break;
                    case 10:
                        Program.Ui.PanelConsoleWebBrowser_Invoke(Line);
                        break;
                    case 11:
                        Program.Ui.PanelConsoleWebBrowser_Invoke("<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>" + Log.EscapeLog(Line));
                        break;
                    case 20:
                        Program.Ui.BotWebBrowser_Invoke(Line);
                        break;
                    case 21:
                        Program.Ui.BotWebBrowser_Invoke("<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>" + Log.EscapeLog(Line));
                        break;
                    case 22:
                        Program.Ui.BotWebBrowser_Invoke("<span style=\"color:#239B56;font-weight: bold;\">[↓]</span>" + Log.EscapeLog(Line));
                        break;
                    case 23:
                        Program.Ui.BotWebBrowser_Invoke("<span style=\"color:#2874A6;font-weight: bold;\">[↑]</span>" + Log.EscapeLog(Line));
                        break;
                    case 24:
                        Program.Ui.BotWebBrowser_Invoke("<span style=\"color:#BA4A00;font-weight: bold;\">[×]</span>" + Log.EscapeLog(Line));
                        break;
                    case 30:
                        Program.Ui.SereinPluginsWebBrowser_Invoke(Line);
                        break;
                    case 31:
                        Program.Ui.SereinPluginsWebBrowser_Invoke("<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>" + Log.EscapeLog(Line));
                        break;
                    case 32:
                        Program.Ui.SereinPluginsWebBrowser_Invoke("<span style=\"color:#BA4A00;font-weight: bold;\">[×]</span>" + Log.EscapeLog(Line));
                        break;
                    case 33:
                        Program.Ui.SereinPluginsWebBrowser_Invoke(Log.EscapeLog(Line));
                        break;
                }
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
            DialogResult Result = MessageBox.Show(Text, Caption, (MessageBoxButtons)Buttons, (MessageBoxIcon)Icon);
            return Result == DialogResult.OK || Result == DialogResult.Yes;
        }
    }
}
