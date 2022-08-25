using Serein.Items;
using Serein.Properties;
using Serein.Settings;
using System;
using System.Collections.Generic;

namespace Serein
{
    partial class Global
    {
        public static string Path = AppDomain.CurrentDomain.BaseDirectory;
        public static string SettingPath = AppDomain.CurrentDomain.BaseDirectory + "settings";
        public static string VERSION = "v1.3.1 " + Resources.BuildID.TrimEnd(' ', '\n', '\r').Substring(0, 6);
        public static List<RegexItem> RegexItems = new List<RegexItem>();
        public static List<TaskItem> TaskItems = new List<TaskItem>();
        public static List<MemberItem> MemberItems = new List<MemberItem>();
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
    }
}