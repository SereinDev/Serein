using Notification.Wpf;
using Serein.Windows.Pages;
using Serein.Windows.Pages.Function;
using Serein.Windows.Pages.Server;
using Serein.Windows.Pages.Settings;
using System.Collections.Generic;

namespace Serein.Windows
{
    internal struct Catalog
    {
        public static NotificationManager? Notification;
        public static MainWindow? MainWindow;
        public static Home? Home;
        public static Debug? Debug;

        public struct Server
        {
            public static Panel? Panel;
            public static Plugins? Plugins;
            public static Pages.Server.Container? Container;
            public static List<(Serein.Base.LogType, string)> Cache = new();
        }

        public struct Function
        {
            public static Regex? Regex;
            public static Schedule? Schedule;
            public static Member? Member;
            public static Pages.Function.JSPlugin? JSPlugin;
            public static Pages.Function.Bot? Bot;
            public static Pages.Function.Container? Container;
            public static List<(Base.LogType, string)> BotCache = new();
            public static List<(Base.LogType, string)> PluginCache = new();
        }

        public struct Settings
        {
            public static Event? Event;
            public static Pages.Settings.Bot? Bot;
            public static Pages.Settings.Server? Server;
            public static Pages.Settings.Serein? Serein;
            public static Pages.Settings.Container? Container;
        }
    }
}
