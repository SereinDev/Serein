using System;
using System.Collections.Generic;
using System.Linq;
//using System.Windows;
using System.Threading.Tasks;
using Serein.Base;
using Serein.Windows;
using Wpf.Ui.Controls;

namespace Serein
{
    internal static class Logger
    {
        public static int Type = 2;
        public static void Out(int Type, params object[] objects)
        {
            if (Window.Server.Panel != null && Window.Function.Bot != null && Window.Function.Bot != null)
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
                        //Program.Ui.Debug_Append($"{DateTime.Now:T} {Line}");
                        break;
                    case 10:
                        Window.Server.Panel.AppendText(Line);
                        break;
                    case 11:
                        Window.Server.Panel.AppendText("<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>" + Log.EscapeLog(Line));
                        break;
                    case 20:
                        Window.Function.Bot.AppendText(Line);
                        break;
                    case 21:
                        Window.Function.Bot.AppendText("<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>" + Log.EscapeLog(Line));
                        break;
                    case 22:
                        Window.Function.Bot.AppendText("<span style=\"color:#239B56;font-weight: bold;\">[↓]</span>" + Log.EscapeLog(Line));
                        break;
                    case 23:
                        Window.Function.Bot.AppendText("<span style=\"color:#2874A6;font-weight: bold;\">[↑]</span>" + Log.EscapeLog(Line));
                        break;
                    case 24:
                        Window.Function.Bot.AppendText("<span style=\"color:#BA4A00;font-weight: bold;\">[×]</span>" + Log.EscapeLog(Line));
                        break;
                    case 30:
                        Window.Function.JSPlugin.AppendText(Line);
                        break;
                    case 31:
                        Window.Function.JSPlugin.AppendText("<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>" + Log.EscapeLog(Line));
                        break;
                    case 32:
                        Window.Function.JSPlugin.AppendText("<span style=\"color:#BA4A00;font-weight: bold;\">[×]</span>" + Log.EscapeLog(Line));
                        break;
                    case 33:
                        Window.Function.JSPlugin.AppendText(Log.EscapeLog(Line));
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
            return true;
        }
    }
}
