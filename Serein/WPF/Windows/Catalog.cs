using Notification.Wpf;
using Serein.Utils;
using Serein.Windows.Pages;
using Serein.Windows.Pages.Function;
using Serein.Windows.Pages.Server;
using Serein.Windows.Pages.Settings;
using System.Collections.Generic;

namespace Serein.Windows
{
    internal static class Catalog
    {
        public static NotificationManager Notification { get; set; }
        public static MainWindow MainWindow { get; set; }
        public static Home Home { get; set; }
        public static Debug Debug { get; set; }

        public static class Server
        {
            public static Panel Panel { get; set; }
            public static Plugins Plugins { get; set; }
            public static Pages.Server.Container Container { get; set; }
            public static List<(Serein.Base.LogType, string)> Cache = new();
        }

        public static class Function
        {
            public static Regex Regex { get; set; }
            public static Schedule Schedule { get; set; }
            public static Member Member { get; set; }
            public static Pages.Function.JSPlugin JSPlugin { get; set; }
            public static Pages.Function.Bot Bot { get; set; }
            public static Pages.Function.Container Container { get; set; }
            public static List<(Serein.Base.LogType, string)> BotCache = new();
            public static List<(Serein.Base.LogType, string)> PluginCache = new();
        }

        public static class Settings
        {
            public static Event Event { get; set; }
            public static Pages.Settings.Bot Bot { get; set; }
            public static Pages.Settings.Server Server { get; set; }
            public static Pages.Settings.Serein Serein { get; set; }
            public static Pages.Settings.Container Container { get; set; }
        }
    }
}
