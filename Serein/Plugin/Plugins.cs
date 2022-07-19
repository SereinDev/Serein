using System.Collections.Generic;

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
    }
}
