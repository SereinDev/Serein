using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using System.Windows.Forms;

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
                long Temp;
                foreach (string Filename in Files)
                {
                    Global.Ui.SereinPluginsWebBrowser_Invoke(
                        $"<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>正在加载{Path.GetFileName(Filename)}"
                        );
                    Temp = PluginItems.Count;
                    try
                    {
                        StreamReader reader = new StreamReader(Filename, Encoding.UTF8);
                        bool Success = JSEngine.Run(reader.ReadToEnd());
                        if (!Success)
                            ErrorFiles.Add(Path.GetFileName(Filename));
                        reader.Close();
                        if (Success) 
                        {
                            if(Temp == PluginItems.Count+1) 
                            {
                                PluginItems[PluginItems.Count - 1].Path = Filename;
                            }
                            else if (Temp == PluginItems.Count&& PluginNames.Contains(Path.GetFileNameWithoutExtension(Filename)))
                            {
                                PluginItems.Add(
                                new PluginItem()
                                {
                                    Name = Path.GetFileNameWithoutExtension(Filename),
                                    Version = "-",
                                    Author = "-",
                                    Description = "-",
                                    Path = Filename
                                });
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        ErrorFiles.Add(Path.GetFileName(Filename));
                        Global.Debug("[Plugins:Load()] " + e.Message);
                    }
                }
                Global.Ui.SereinPluginsWebBrowser_Invoke(
                    $"<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>插件加载完毕，共加载{Files.Length}个插件，其中{ErrorFiles.Count}个加载失败"
                    );
                if (ErrorFiles.Count > 0)
                {
                    Global.Ui.SereinPluginsWebBrowser_Invoke(
                        "<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>" +
                        $"以下插件加载出现问题，请咨询原作者获取更多信息<br>"+
                        string.Join("\n", ErrorFiles)
                        );
                }
            }
        }
        public static void Reload()
        {
            JSEngine.Init();
            Load();
        }
    }
}
