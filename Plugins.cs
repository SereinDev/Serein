using System.IO;
using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Serein
{
    class Plugins
    {
        public static string PluginPath="";
        public static string[] GetPluglin()
        {
            if (File.Exists(Global.Settings_server.Path))
            {
                if (Directory.Exists(Path.GetDirectoryName(Global.Settings_server.Path) + "\\plugin"))
                {
                    PluginPath= Path.GetDirectoryName(Global.Settings_server.Path) + "\\plugin";
                }
                else if (Directory.Exists(Path.GetDirectoryName(Global.Settings_server.Path) + "\\plugins"))
                {
                    PluginPath = Path.GetDirectoryName(Global.Settings_server.Path) + "\\plugins";
                }
                else
                {
                    PluginPath = "";
                }
                if (PluginPath != "")
                {
                    string[] Files = Directory.GetFiles(PluginPath, "*", SearchOption.TopDirectoryOnly);
                    
                    return Files;
                }
            }
            return null;
        }
    }
}
