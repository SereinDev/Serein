﻿using Serein.Windows.Pages;
using Serein.Windows.Pages.Function;
using Serein.Windows.Pages.Server;
using Serein.Windows.Pages.Settings;
using Notification.Wpf;

namespace Serein.Windows
{
    internal static class Window
    {
        public static NotificationManager Notification = new NotificationManager();

        public static MainWindow MainWindow { get; set; }
        public static Home Home { get; set; }
        public static Debug Debug { get; set; }
        public static class Server
        {
            public static Panel Panel { get; set; }
            public static Plugins Plugins { get; set; }
        }
        public static class Function
        {
            public static Regex Regex { get; set; }
            public static Task Task { get; set; }
            public static Member Member { get; set; }
            public static JSPlugin JSPlugin { get; set; }
            public static Pages.Function.Bot Bot { get; set; }
        }
        public static class Settings
        {
            public static Event Event { get; set; }
            public static Pages.Settings.Bot Bot { get; set; }
            public static Pages.Settings.Server Server { get; set; }
            public static Pages.Settings.Serein Serein { get; set; }
        }
    }
}
