using Serein.Items;
using Serein.Settings;
using System;
using System.Collections.Generic;
using System.Net;

namespace Serein
{
    internal class Global
    {
        public static string Path = AppDomain.CurrentDomain.BaseDirectory;
        public static string SettingPath = AppDomain.CurrentDomain.BaseDirectory + "settings";
        public static string VERSION = "v1.3.0";
        public static List<RegexItem> RegexItems = new List<RegexItem>();
        public static List<TaskItem> TaskItems = new List<TaskItem>();
        public static List<MemberItem> MemberItems = new List<MemberItem>();
        public static Ui.Ui Ui = null;
        public static Item Settings = new Item();
        public static bool Crash = false;
        public static bool MultiOpen = false;
        public static bool FirstOpen = false;
        public static IList<string> Args = null;
        public static void UpdateRegexItems(List<RegexItem> New)
        {
            lock (RegexItems)
            {
                RegexItems = New;
            }
        }
        public static void UpdateTaskItems(List<TaskItem> New)
        {
            lock (TaskItems)
            {
                TaskItems = New;
            }
        }
        public static void UpdateMemberItems(List<MemberItem> New)
        {
            lock (MemberItems)
            {
                MemberItems = New;
            }
        }
        public static void Debug(object o)
        {
            if (Ui != null && o != null)
            {
                try
                {
                    Ui.Debug_Append($"{DateTime.Now:T} {o}");
                }
                catch { }
            }
        }
        public static void Logger(int Type, params object[] objects)
        {
            if (Ui != null)
            {
                string Line = string.Empty;
                foreach (var o in objects)
                {
                    if (o != null) { Line += " " + o.ToString(); }
                }
                Line = Line.TrimEnd();
                switch (Type)
                {
                    case 999:
                        Ui.Debug_Append($"{DateTime.Now:T} {Line}");
                        break;
                    case 10:
                        Ui.PanelConsoleWebBrowser_Invoke(Line);
                        break;
                    case 11:
                        Ui.PanelConsoleWebBrowser_Invoke("<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>" + WebUtility.HtmlEncode(Line));
                        break;
                    case 20:
                        Ui.BotWebBrowser_Invoke(Line);
                        break;
                    case 21:
                        Ui.BotWebBrowser_Invoke("<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>" + WebUtility.HtmlEncode(Line));
                        break;
                    case 22:
                        Ui.BotWebBrowser_Invoke("<span style=\"color:#239B56;font-weight: bold;\">[↓]</span>" + WebUtility.HtmlEncode(Line));
                        break;
                    case 23:
                        Ui.BotWebBrowser_Invoke("<span style=\"color:#2874A6;font-weight: bold;\">[↑]</span>" + WebUtility.HtmlEncode(Line));
                        break;
                    case 24:
                        Ui.BotWebBrowser_Invoke("<span style=\"color:#BA4A00;font-weight: bold;\">[×]</span>" + WebUtility.HtmlEncode(Line));
                        break;
                    case 30:
                        Ui.SereinPluginsWebBrowser_Invoke(Line);
                        break;
                    case 31:
                        Ui.SereinPluginsWebBrowser_Invoke("<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>" + WebUtility.HtmlEncode(Line));
                        break;
                    case 32:
                        Ui.SereinPluginsWebBrowser_Invoke("<span style=\"color:#BA4A00;font-weight: bold;\">[×]</span>" + WebUtility.HtmlEncode(Line));
                        break;
                    case 33:
                        Ui.SereinPluginsWebBrowser_Invoke(WebUtility.HtmlEncode(Line));
                        break;
                }
            }
        }
    }
}