using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serein
{
    class Settings
    {
        public static class Server
        {
            public static string Path;
            public static bool EnableRestart;
            public static bool EnableOutputCommand;
            public static bool EnableLog;
            public static int OutputStyle;
        }
        public static class Bot
        {
            public static string Path;
            public static bool EnableLog;
            public static bool GivePermissionToAllAdmin;
            public static int[] GroupList;
            public static int[] PermissionList;
            public static int ListenPort;
            public static int SendPort;
        }   
        public static class Serein
        {
            public static bool EnableGetUpdate;
            public static bool EnableGetAnnouncement;
        }


        public static void SaveSettings()
        {
            

        }
        public static void LoadSettings()
        {

        }
    }
}
