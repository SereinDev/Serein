using Serein.Items;
using Serein.Settings;
using System;
using System.Collections.Generic;

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
    }
}