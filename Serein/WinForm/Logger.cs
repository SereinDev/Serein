using System;
using Serein.Base;

namespace Serein
{
    internal static class Logger
    {
        public static void Out(int Type, params object[] objects)
        {
            if (Global.Ui != null && !Global.Ui.Disposing)
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
                        Global.Ui.Debug_Append($"{DateTime.Now:T} {Line}");
                        break;
                    case 10:
                        Global.Ui.PanelConsoleWebBrowser_Invoke(Line);
                        break;
                    case 11:
                        Global.Ui.PanelConsoleWebBrowser_Invoke("<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>" + Log.EscapeLog(Line));
                        break;
                    case 20:
                        Global.Ui.BotWebBrowser_Invoke(Line);
                        break;
                    case 21:
                        Global.Ui.BotWebBrowser_Invoke("<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>" + Log.EscapeLog(Line));
                        break;
                    case 22:
                        Global.Ui.BotWebBrowser_Invoke("<span style=\"color:#239B56;font-weight: bold;\">[↓]</span>" + Log.EscapeLog(Line));
                        break;
                    case 23:
                        Global.Ui.BotWebBrowser_Invoke("<span style=\"color:#2874A6;font-weight: bold;\">[↑]</span>" + Log.EscapeLog(Line));
                        break;
                    case 24:
                        Global.Ui.BotWebBrowser_Invoke("<span style=\"color:#BA4A00;font-weight: bold;\">[×]</span>" + Log.EscapeLog(Line));
                        break;
                    case 30:
                        Global.Ui.SereinPluginsWebBrowser_Invoke(Line);
                        break;
                    case 31:
                        Global.Ui.SereinPluginsWebBrowser_Invoke("<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>" + Log.EscapeLog(Line));
                        break;
                    case 32:
                        Global.Ui.SereinPluginsWebBrowser_Invoke("<span style=\"color:#BA4A00;font-weight: bold;\">[×]</span>" + Log.EscapeLog(Line));
                        break;
                    case 33:
                        Global.Ui.SereinPluginsWebBrowser_Invoke(Log.EscapeLog(Line));
                        break;
                }
            }
        }
    }
}
