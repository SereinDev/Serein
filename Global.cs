using Serein.baseFunction;
using Serein.setting;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Serein
{
    internal class Global
    {
        public static string Path = AppDomain.CurrentDomain.BaseDirectory;
        public static string SettingPath = AppDomain.CurrentDomain.BaseDirectory + "settings";
        public static string VERSION = "v1.2.3";
        public static List<RegexItem> RegexItems = new();
        public static List<TaskItem> TaskItems = new();
        public static Ui Ui = null;
        public static WebBrowser PanelConsoleWebBrowser;
        public static WebBrowser BotWebBrowser;
        public static Settings_Server Settings_Server = new();
        public static Settings_Matches Settings_Matches = new();
        public static Settings_Bot Settings_Bot = new();
        public static Settings_Serein Settings_Serein = new();
        public static bool Crash = false;
        public static bool MultiOpen = false;
        public static bool FirstOpen = false;
    }
}