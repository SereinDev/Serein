using Newtonsoft.Json;
using Serein.Base;
using Serein.Items;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Serein.JSPlugin
{
    internal static class JSPluginManager
    {
        /// <summary>
        /// CancellationToken
        /// </summary>
        public static CancellationTokenSource TokenSource = new CancellationTokenSource();

        /// <summary>
        /// 插件项列表
        /// </summary>
        public static Dictionary<string, Plugin> PluginDict = new Dictionary<string, Plugin>();

        /// <summary>
        /// 定时器字典
        /// </summary>
        public static Dictionary<long, System.Timers.Timer> Timers = new Dictionary<long, System.Timers.Timer>();

        /// <summary>
        /// WS客户端列表
        /// </summary>
        public static List<JSWebSocket> WebSockets = new List<JSWebSocket>();

        /// <summary>
        /// 加载插件
        /// </summary>
        public static void Load()
        {
            string PluginPath = IO.GetPath("plugins");
            if (!Directory.Exists(PluginPath))
                Directory.CreateDirectory(PluginPath);
            else
            {
                string[] Files = Directory.GetFiles(PluginPath, "*.js", SearchOption.TopDirectoryOnly);
                TokenSource = new CancellationTokenSource();
                foreach (string Filename in Files)
                {
                    string Namespace = Path.GetFileNameWithoutExtension(Filename);
                    Logger.Out(LogType.Plugin_Notice, $"正在加载{Path.GetFileName(Filename)}");
                    try
                    {
                        PluginDict.Add(Namespace, new Plugin
                        {
                            Engine = JSEngine.Init(Namespace: Namespace),
                            Name = Namespace,
                            File = Path.GetFileName(Filename),
                            Namespace = Namespace
                        });
                        PluginDict[Namespace].Event.Namespace = Namespace;
                        PluginDict[Namespace].Engine = JSEngine.Run(File.ReadAllText(Filename, Encoding.UTF8), PluginDict[Namespace].Engine, out string ExceptionMessage);
                        if (!string.IsNullOrEmpty(ExceptionMessage))
                            Logger.Out(LogType.Plugin_Error, ExceptionMessage);
                        else
                            PluginDict[Namespace].LoadedSuccessfully = true;
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
                    if (PluginDict.TryGetValue(Key, out Plugin Value) && !Value.LoadedSuccessfully)
                        ErrorFiles.Add(Value.File);
                });
                Logger.Out(LogType.Plugin_Notice, $"插件加载完毕，共加载{Files.Length}个插件，其中{ErrorFiles.Count}个加载失败");
                if (ErrorFiles.Count > 0)
                    Logger.Out(LogType.Plugin_Error, "以下插件加载出现问题，请咨询原作者获取更多信息：" + string.Join(" ,", ErrorFiles));
                System.Threading.Tasks.Task.Run(async () =>
                {
                    await System.Threading.Tasks.Task.Delay(5000);
                    Logger.Out(LogType.Debug, "插件列表\n", JsonConvert.SerializeObject(PluginDict, Formatting.Indented));
                });
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
            Thread.Sleep(500);
            TokenSource.Cancel();
            PluginDict.Keys.ToList().ForEach((Key) =>
            {
                if (PluginDict.ContainsKey(Key))
                {
                    PluginDict[Key].Engine.Dispose();
                    PluginDict[Key].Engine = null;
                    PluginDict[Key].WebSockets.ForEach((ws) => ws.Dispose());
                }
            });
            PluginDict.Clear();
            JSFunc.ID = 0;
            Load();
        }
    }
}
