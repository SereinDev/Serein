using Serein.Items;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Timers;

namespace Serein.Plugin
{
    internal class Plugins
    {
        /// <summary>
        /// 事件对象
        /// </summary>
        public static Event Event = new Event();

        /// <summary>
        /// 命令项列表
        /// </summary>
        public static List<CommandItem> CommandItems = new List<CommandItem>();

        /// <summary>
        /// 插件项列表
        /// </summary>
        public static List<Items.Plugin> PluginItems = new List<Items.Plugin>();

        /// <summary>
        /// 定时器字典
        /// </summary>
        public static Dictionary<int, Timer> Timers = new Dictionary<int, Timer>();

        /// <summary>
        /// WS客户端列表
        /// </summary>
        public static List<JSWebSocket> WebSockets = new List<JSWebSocket>();

        /// <summary>
        /// 加载插件
        /// </summary>
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
                    Logger.Out(LogType.Plugin_Notice, $"正在加载{Path.GetFileName(Filename)}");
                    Temp = PluginItems.Count;
                    try
                    {
                        Jint.Engine Engine = JSEngine.Init();
                        string ExceptionMessage = JSEngine.Run(File.ReadAllText(Filename, Encoding.UTF8));
                        bool Success = string.IsNullOrEmpty(ExceptionMessage);
                        if (!Success)
                        {
                            ErrorFiles.Add(Path.GetFileName(Filename));
                            Logger.Out(LogType.Plugin_Error, ExceptionMessage);
                        }
                        else
                        {
                            if (Temp == PluginItems.Count - 1)
                                PluginItems[PluginItems.Count - 1].Engine = Engine;
                            else
                                PluginItems.Add(
                                    new Items.Plugin()
                                    {
                                        Name = Path.GetFileNameWithoutExtension(Filename),
                                        Version = "-",
                                        Author = "-",
                                        Description = "-",
                                        Engine = Engine
                                    });
                        }
                    }
                    catch (Exception e)
                    {
                        ErrorFiles.Add(Path.GetFileName(Filename));
                        Logger.Out(LogType.Plugin_Error, e.ToString());
                    }
                }
                Logger.Out(LogType.Plugin_Notice, $"插件加载完毕，共加载{Files.Length}个插件，其中{ErrorFiles.Count}个加载失败");
                if (ErrorFiles.Count > 0)
                    Logger.Out(LogType.Plugin_Error, "以下插件加载出现问题，请咨询原作者获取更多信息：" + string.Join(" ,", ErrorFiles));
            }
        }

        /// <summary>
        /// 重新加载插件
        /// </summary>
        public static void Reload()
        {
            Logger.Out(LogType.Plugin_Clear);
            JSFunc.Trigger("onPluginsReload");
            WebSockets.ForEach((WSC) => WSC.Dispose());
            WebSockets.Clear();
            PluginItems.ForEach((x) => x.Engine = null);
            PluginItems.Clear();
            CommandItems.Clear();
            Event.Dispose();
            Event = new Event();
            JSFunc.ClearAllTimers();
            Load();
        }
    }
}
