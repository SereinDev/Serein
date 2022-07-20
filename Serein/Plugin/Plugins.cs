using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Serein.Plugin
{
    internal class Plugins
    {
        public static List<PluginItem> PluginItems = new List<PluginItem>();
        public static List<string> PluginNames
        {
            get
            {
                List<string> list = new List<string>();
                foreach (PluginItem Item in PluginItems)
                {
                    list.Add(Item.Name);
                }
                return list;
            }
        }
        public static void Load()
        {
            string PluginPath = Global.Path + "\\plugins";
            if (!Directory.Exists(PluginPath))
                Directory.CreateDirectory(PluginPath);
            else
            {
                List<string> ErrorFiles = new List<string>();
                string[] Files = Directory.GetFiles(PluginPath, "*.js", SearchOption.TopDirectoryOnly);
                foreach (string Filename in Files)
                {
                    StreamReader reader = new StreamReader(Filename, Encoding.UTF8);
                    if (!JSEngine.Run(reader.ReadToEnd()))
                        ErrorFiles.Add(Path.GetFileName(Filename));
                }
            }
        }
    }
}
