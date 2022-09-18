using Serein.Items;
using Serein.Settings;
using System;
using System.Collections.Generic;

namespace Serein
{
    partial class Global
    {
        public static string Path = AppDomain.CurrentDomain.BaseDirectory;
        public static string SettingPath = AppDomain.CurrentDomain.BaseDirectory + "settings";
        public static string VERSION = "v1.3.2";
        public static List<Regex> RegexItems = new List<Regex>();
        public static List<Task> TaskItems = new List<Task>();
        public static Dictionary<long, Member> MemberItems = new Dictionary<long, Member>();
        public static Item Settings = new Item();
        public static bool Crash = false;
        public static bool MultiOpen = false;
        public static bool FirstOpen = false;
        public static IList<string> Args = null;
        public static BuildInfo BuildInfo = new BuildInfo();

        public static void UpdateRegexItems(List<Regex> New)
        {
            lock (RegexItems)
            {
                RegexItems = New;
            }
        }

        public static void UpdateTaskItems(List<Task> New)
        {
            lock (TaskItems)
            {
                TaskItems = New;
            }
        }

        public static void UpdateMemberItems(Dictionary<long, Member> New)
        {
            lock (MemberItems)
            {
                MemberItems = New;
            }
        }
    }
}