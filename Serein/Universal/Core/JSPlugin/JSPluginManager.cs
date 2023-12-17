using Jint.Native;
using Newtonsoft.Json;
using Serein.Base;
using Serein.Core.JSPlugin.Native;
using Serein.Extensions;
using Serein.Utils.IO;
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
        private static bool _loading;

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
            _loading = true;
            if (!Directory.Exists(PluginPath))
            {
                Directory.CreateDirectory(Path.Combine(PluginPath, "modules"));
                _loading = false;
                return;
            }
            ReadAll();
            SummaryAll();
            Task.Run(() =>
            {
                5000.ToSleep();
                Utils.Output.Logger.Output(
                    LogType.Debug,
                    "插件列表\n",
                    JsonConvert.SerializeObject(PluginDict, Formatting.Indented)
                );
            });
#if WINFORM
            Program.Ui?.LoadJSPluginPublicly();
#elif WPF
            Windows.Catalog.Function.JSPlugin?.LoadPublicly();
#endif
            Directory.CreateDirectory(Path.Combine("plugins", "modules"));
            _loading = false;
        }

        /// <summary>
        /// 输出加载结果
        /// </summary>
        private static void SummaryAll()
        {
            List<string> failedFiles = new();
            PluginDict.Keys
                .ToList()
                .ForEach(
                    (key) =>
                    {
                        if (PluginDict.TryGetValue(key, out Plugin? plugin) && !plugin.Available)
                        {
                            failedFiles.Add(Path.GetFileName(plugin.File));
                        }
                    }
                );
            if (PluginDict.Count > 0)
            {
                Utils.Output.Logger.Output(
                    LogType.Plugin_Notice,
                    $"插件加载完毕，共加载{PluginDict.Count}个插件，其中{failedFiles.Count}个加载失败"
                );
                if (failedFiles.Count > 0)
                {
                    Utils.Output.Logger.Output(
                        LogType.Plugin_Notice,
                        "以下插件加载出现问题，请咨询原作者获取更多信息："
                            + string.Join("，", (IEnumerable<string>)failedFiles)
                    );
                }
                JSFunc.Trigger(EventType.PluginsLoaded);
            }
            else
            {
                Utils.Output.Logger.Output(
                    LogType.Plugin_Notice,
                    $"插件加载完毕，但是并没有插件被加载。扩展市场：https://market.serein.cc/"
                );
            }
        }

        /// <summary>
        /// 读取所有插件
        /// </summary>
        private static void ReadAll()
        {
            string[] files = Directory.GetFiles(PluginPath, "*.js", SearchOption.TopDirectoryOnly);
            foreach (string file in files)
            {
                LoadFile(file);
            }
        }

        /// <summary>
        /// 加载文件
        /// </summary>
        /// <param name="file">文件</param>
        private static void LoadFile(string file)
        {
            string lowerFile = file.ToLowerInvariant();
            if (
                Global.Settings.Serein.Function.JSPatternToSkipLoadingSpecifiedFile
                    .Where((pattern) => lowerFile.EndsWith(pattern.ToLowerInvariant()))
                    .Any()
            )
            {
                return;
            }

            string @namespace = Path.GetFileNameWithoutExtension(file);
            Utils.Output.Logger.Output(LogType.Plugin_Notice, $"正在加载{Path.GetFileName(file)}");

            try
            {
                PreLoadConfig config = new();
                if (Directory.Exists(Path.Combine(PluginPath, @namespace)))
                {
                    string configPath = Path.Combine(PluginPath, @namespace, "PreLoadConfig.json");
                    if (File.Exists(configPath))
                    {
                        config = JsonConvert.DeserializeObject<PreLoadConfig>(
                            File.ReadAllText(configPath)
                        );
                    }
                    File.WriteAllText(configPath, config.ToJson(Formatting.Indented));
                }

                Plugin plugin = new(@namespace, config) { File = file };
                PluginDict.Add(@namespace, plugin);

                plugin.Engine!.Run(File.ReadAllText(file), out string exceptionMessage);
                if (!string.IsNullOrEmpty(exceptionMessage))
                {
                    Utils.Output.Logger.Output(
                        LogType.Plugin_Error,
                        $"[{@namespace}]",
                        exceptionMessage
                    );
                    plugin.Dispose();
                }
                else
                {
                    plugin.Available = true;
                }
            }
            catch (Exception e)
            {
                Utils.Output.Logger.Output(LogType.Plugin_Error, $"[{@namespace}]", e.Message);
                Utils.Output.Logger.Output(LogType.Debug, e);
            }
        }

        /// <summary>
        /// 重新加载插件
        /// </summary>
        public static void Reload()
        {
            if (_loading)
            {
                return;
            }
            _loading = true;
            Utils.Output.Logger.Output(LogType.Plugin_Clear);
            JSFunc.ClearAllTimers();
            JSFunc.Trigger(EventType.PluginsReload);

            MessageBus.DisposeAll();
            CommandVariablesDict.Clear();
            VariablesExportedDict.Clear();
            PluginDict.Values.ToList().ForEach((plugin) => plugin.Dispose());
            PluginDict.Clear();

            JSFunc.CurrentID = 0;
            Load();
        }
    }
}
