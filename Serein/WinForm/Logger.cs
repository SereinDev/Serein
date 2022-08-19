using System;
using Serein.Base;

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

        public static bool MsgBox(string Text, string Caption, int Buttons, int Icon)
        {
            return true;
        }
    }
}
