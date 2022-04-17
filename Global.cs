using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Serein
{
    class Global
    {
        public static string PATH = AppDomain.CurrentDomain.BaseDirectory;
        public static string SETTINGPATH = AppDomain.CurrentDomain.BaseDirectory+"settings";
        public static string VERSION = "Testing 2022";
        public static Serein serein;
        public static WebBrowser PanelConsoleWebBrowser;
        public static WebBrowser BotWebBrowser;
        public static Settings_Server Settings_server = new Settings_Server();
        public static Settings_Bot Settings_bot = new Settings_Bot();
        public static Settings_Serein Settings_serein = new Settings_Serein();

    }
}