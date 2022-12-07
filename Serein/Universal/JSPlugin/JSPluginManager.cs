using Serein.Items;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;

namespace Serein.JSPlugin
{
    internal class JSPluginManager
    {
        /// <summary>
        /// 最后加载的文件
        /// </summary>
        public static string LatestFile { get; private set; } = string.Empty;

        /// <summary>
        /// 插件项列表
        /// </summary>
        public static Dictionary<string, Plugin> PluginDict = new Dictionary<string, Plugin>();

        /// <summary>
        /// 定时器字典
        /// </summary>
        public static Dictionary<long, Timer> Timers = new Dictionary<long, Timer>();

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
                string[] Files = Directory.GetFiles(PluginPath, "*.js", SearchOption.TopDirectoryOnly);
                foreach (string Filename in Files)
                {
                    LatestFile = Path.GetFileName(Filename);
                    Logger.Out(LogType.Plugin_Notice, $"正在加载{Path.GetFileName(Filename)}");
                    try
                    {
                        PluginDict.Add(LatestFile, new Plugin
                        {
                            Engine = JSEngine.Init(Name: LatestFile),
                            Name = Path.GetFileNameWithoutExtension(Filename),
                            File = LatestFile
                        });
                        string ExceptionMessage = JSEngine.Run(File.ReadAllText(Filename, Encoding.UTF8), PluginDict[LatestFile].Engine);
                        if (!string.IsNullOrEmpty(ExceptionMessage))
                            Logger.Out(LogType.Plugin_Error, ExceptionMessage);
                        else
                            PluginDict[LatestFile].LoadedSuccessfully = false;
                    }
                    catch (Exception e)
                    {
                        Logger.Out(LogType.Plugin_Error, e.Message);
                        Logger.Out(LogType.Debug, e);
                    }
                }
                List<string> ErrorFiles = new List<string>();
                PluginDict.Keys.ToList().ForEach((Key) =>
                {
                    if (PluginDict.TryGetValue(Key, out Plugin Value) && Value.LoadedSuccessfully)
                        ErrorFiles.Add(Value.File);
                });
                Logger.Out(LogType.Plugin_Notice, $"插件加载完毕，共加载{Files.Length}个插件，其中{ErrorFiles.Count}个加载失败");
                if (ErrorFiles.Count > 0)
                    Logger.Out(LogType.Plugin_Error, "以下插件加载出现问题，请咨询原作者获取更多信息：" + string.Join(" ,", ErrorFiles));
                LatestFile = null;
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
            PluginDict.Keys.ToList().ForEach((Key) =>
            {
                if (PluginDict.ContainsKey(Key))
                {
                    PluginDict[Key].Engine.Dispose();
                    PluginDict[Key].Engine = null;
                }
            });
            PluginDict.Clear();
            JSFunc.ClearAllTimers();
            JSFunc.ID = 0;
            Load();
        }
    }
}
