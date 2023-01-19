using Newtonsoft.Json;
using Serein.Base;
using Serein.Extensions;
using Serein.Items;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;

namespace Serein.JSPlugin
{
    internal static class JSPluginManager
    {
        /// <summary>
        /// 插件项列表
        /// </summary>
        public static Dictionary<string, Plugin> PluginDict = new();

        /// <summary>
        /// 定时器字典
        /// </summary>
        public static Dictionary<long, Timer> Timers = new();

        /// <summary>
        /// WS客户端列表
        /// </summary>
        public static List<JSWebSocket> WebSockets = new();

        /// <summary>
        /// 加载插件
        /// </summary>
        public static void Load()
        {
            string pluginPath = Path.Combine("plugins");
            if (!Directory.Exists(pluginPath))
            {
                Directory.CreateDirectory(pluginPath);
            }
            else
            {
                string[] files = Directory.GetFiles(pluginPath, "*.js", SearchOption.TopDirectoryOnly);
                foreach (string file in files)
                {
                    string @namespace = Path.GetFileNameWithoutExtension(file);
                    Logger.Out(LogType.Plugin_Notice, $"正在加载{Path.GetFileName(file)}");
                    try
                    {
                        PluginDict.Add(@namespace, new Plugin(@namespace)
                        {
                            File = Path.GetFileName(file)
                        });
                        PluginDict[@namespace].Engine = JSEngine.Run(File.ReadAllText(file, Encoding.UTF8), PluginDict[@namespace].Engine, out string ExceptionMessage);
                        if (!string.IsNullOrEmpty(ExceptionMessage))
                        {
                            Logger.Out(LogType.Plugin_Error, ExceptionMessage);
                            PluginDict[@namespace].Dispose();
                        }
                        else
                        {
                            PluginDict[@namespace].Available = true;
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.Out(LogType.Plugin_Error, e.Message);
                        Logger.Out(LogType.Debug, e);
                    }
                }
                List<string> failedFiles = new();
                PluginDict.Keys.ToList().ForEach((Key) =>
                {
                    if (PluginDict.TryGetValue(Key, out Plugin Value) && !Value.Available)
                    {
                        failedFiles.Add(Value.File);
                    }
                });
                Logger.Out(LogType.Plugin_Notice, $"插件加载完毕，共加载{files.Length}个插件，其中{failedFiles.Count}个加载失败");
                if (failedFiles.Count > 0)
                {
                    Logger.Out(LogType.Plugin_Notice, "以下插件加载出现问题，请咨询原作者获取更多信息：  " + string.Join(" ,", failedFiles));
                }
                System.Threading.Tasks.Task.Run(() =>
                {
                    5000.ToSleepFor();
                    Logger.Out(LogType.Debug, "插件列表\n", JsonConvert.SerializeObject(PluginDict, Formatting.Indented));
                });
#if WINFORM
                Program.Ui.LoadJSPluginPublicly();
#elif WPF
                Windows.Catalog.Function.JSPlugin?.LoadPublicly();
#endif
            }
        }

        /// <summary>
        /// 重新加载插件
        /// </summary>
        public static void Reload()
        {
            Logger.Out(LogType.Plugin_Clear);
            JSFunc.ClearAllTimers();
            JSFunc.Trigger(EventType.PluginsReload);
            500.ToSleepFor();
            PluginDict.Keys.ToList().ForEach((Key) => PluginDict[Key].Dispose());
            PluginDict.Clear();
            JSFunc.GlobalID = 0;
            Load();
        }
    }
}
