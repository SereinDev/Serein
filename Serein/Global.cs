using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Serein
{
    internal class Global
    {
        public static string Path = AppDomain.CurrentDomain.BaseDirectory;
        public static string SettingPath = AppDomain.CurrentDomain.BaseDirectory + "settings";
        public static string VERSION = "v1.2.7";
        public static List<RegexItem> RegexItems = new List<RegexItem>();
        public static List<TaskItem> TaskItems = new List<TaskItem>();
        public static Ui Ui = null;
        public static Settings_Server Settings_Server = new Settings_Server();
        public static Settings_Matches Settings_Matches = new Settings_Matches();
        public static Settings_Bot Settings_Bot = new Settings_Bot();
        public static Settings_Serein Settings_Serein = new Settings_Serein();
        public static bool Crash = false;
        public static bool MultiOpen = false;
        public static bool FirstOpen = false;
        public static void Debug(object o)
        {
            if (Ui != null &&o!=null)
            {
                try
                {
                    Ui.Debug_Append($"{DateTime.Now} {o}");
                }
                catch { }
            }
        }
    }
}