using Jint.Native;
using Newtonsoft.Json;
using Serein.Base;
using Serein.Extensions;
using Serein.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Serein.Core.JSPlugin
{
    internal static class JSPluginManager
    {
        /// <summary>
        /// 加载插件ing
        /// </summary>
        private static bool IsLoading;

        /// <summary>
        /// 插件文件夹路径
        /// </summary>
        public const string PluginPath = "plugins";

        /// <summary>
        /// 插件项列表
        /// </summary>
        public static readonly Dictionary<string, Plugin> PluginDict = new();

        /// <summary>
        /// 定时器字典
        /// </summary>
        public static readonly Dictionary<long, Timer> Timers = new();

        /// <summary>
        /// 变量字典
        /// </summary>
        public static readonly Dictionary<string, string> CommandVariablesDict = new();

        /// <summary>
        /// 变量字典
        /// </summary>
        public static readonly Dictionary<string, JsValue> VariablesExportedDict = new();

        /// <summary>
        /// 加载插件
        /// </summary>
        public static void Load()
        {
            IsLoading = true;
            if (!Directory.Exists(PluginPath))
            {
                Directory.CreateDirectory(Path.Combine(PluginPath, "modules"));
                IsLoading = false;
                return;
            }
            string[] files = Directory.GetFiles(PluginPath, "*.js", SearchOption.TopDirectoryOnly);
            foreach (string file in files)
            {
                if (file.ToLowerInvariant().EndsWith(".module.js")) { continue; }
                string @namespace = Path.GetFileNameWithoutExtension(file);
                Logger.Output(LogType.Plugin_Notice, $"正在加载{Path.GetFileName(file)}");
                try
                {
                    PreLoadConfig config = null;
                    if (Directory.Exists(Path.Combine(PluginPath, @namespace)))
                    {
                        string configPath = Path.Combine(PluginPath, @namespace, "PreLoadConfig.json");
                        if (File.Exists(configPath))
                        {
                            config = JsonConvert.DeserializeObject<PreLoadConfig>(File.ReadAllText(configPath));
                            File.WriteAllText(configPath, config.ToJson(Formatting.Indented));
                        }
                        else
                        {
                            File.WriteAllText(configPath, new PreLoadConfig().ToJson(Formatting.Indented));
                        }
                    }
                    PluginDict.Add(@namespace, new Plugin(@namespace, config)
                    {
                        File = file
                    });
                    PluginDict[@namespace].Engine = PluginDict[@namespace].Engine.Run(File.ReadAllText(file), out string exceptionMessage);
                    if (!string.IsNullOrEmpty(exceptionMessage))
                    {
                        Logger.Output(LogType.Plugin_Error, exceptionMessage);
                        PluginDict[@namespace].Dispose();
                    }
                    else
                    {
                        PluginDict[@namespace].Available = true;
                    }
                }
                catch (Exception e)
                {
                    Logger.Output(LogType.Plugin_Error, e.Message);
                    Logger.Output(LogType.Debug, e);
                }
            }
            List<string> failedFiles = new();
            PluginDict.Keys.ToList().ForEach((key) =>
            {
                if (PluginDict.TryGetValue(key, out Plugin plugin) && !plugin.Available)
                {
                    failedFiles.Add(Path.GetFileName(plugin.File));
                }
            });
            if (files.Length != 0)
            {
                Logger.Output(LogType.Plugin_Notice, $"插件加载完毕，共加载{files.Length}个插件，其中{failedFiles.Count}个加载失败");
                if (failedFiles.Count > 0)
                {
                    Logger.Output(LogType.Plugin_Notice, "以下插件加载出现问题，请咨询原作者获取更多信息：" + string.Join("，", failedFiles));
                }
            }
            else
            {
                Logger.Output(LogType.Plugin_Notice, $"插件加载完毕，但是并没有插件被加载。扩展市场：https://serein.cc/Extension/");
            }
            Task.Run(() =>
            {
                5000.ToSleep();
                Logger.Output(LogType.Debug, "插件列表\n", JsonConvert.SerializeObject(PluginDict, Formatting.Indented));
            });
#if WINFORM
            Program.Ui?.LoadJSPluginPublicly();
#elif WPF
            Windows.Catalog.Function.JSPlugin?.LoadPublicly();
#endif
            IO.CreateDirectory(Path.Combine("plugins", "modules"));
            IsLoading = false;
        }

        /// <summary>
        /// 重新加载插件
        /// </summary>
        public static void Reload()
        {
            if (!IsLoading)
            {
                Task.Run(() =>
                {
                    Logger.Output(LogType.Plugin_Clear);
                    JSFunc.ClearAllTimers();
                    JSFunc.Trigger(EventType.PluginsReload);
                    CommandVariablesDict.Clear();
                    VariablesExportedDict.Clear();
                    PluginDict.Keys.ToList().ForEach((Key) => PluginDict[Key].Dispose());
                    PluginDict.Clear();
                    JSFunc.CurrentID = 0;
                    Load();
                });
            }
        }
    }
}
