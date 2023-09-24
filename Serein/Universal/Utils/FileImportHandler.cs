#if !CONSOLE

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serein.Core.Server;
using Serein.Utils.IO;
using Serein.Utils.Output;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#if WPF
using Serein.Windows;
#endif

namespace Serein.Utils
{
    internal static class FileImportHandler
    {
        /// <summary>
        /// 触发器入口
        /// </summary>
        /// <param name="datas">数据</param>
        public static void Trigger(Array datas)
        {
            List<string> tempList = new();
            foreach (object obj in datas)
            {
                if (obj is not null)
                {
                    tempList.Add(obj.ToString() ?? string.Empty);
                }
            }
            List<string> list = tempList.Where((i) => !string.IsNullOrEmpty(i)).ToList();

            if (list.Count == 1 && (TryAsStartFile(list[0]) || TryReadAsDataFile(list[0])))
            {
                return;
            }
            if (list.Count > 0)
            {
                PluginManager.TryImport(list);
            }
        }

        /// <summary>
        /// 尝试作为启动文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>处理结果</returns>
        private static bool TryAsStartFile(string path)
        {
            string extension = Path.GetExtension(path).ToLowerInvariant();
            if (extension != ".exe" && extension != ".bat" && extension != ".cmd")
            {
                return false;
            }

            if (MsgBox.Show($"是否以{Path.GetFileName(path)}为启动文件？", true))
            {
                Global.Settings.Server.Path = path;

#if WINFORM
                Program.Ui?.Invoke(Program.Ui.LoadSettings);
                Program.Ui?.Invoke(Program.Ui.LoadPlugins);
#elif WPF
                Catalog.Settings.Server?.Dispatcher.Invoke(Catalog.Settings.Server.Load);
                Catalog.Server.Plugins?.Dispatcher.Invoke(Catalog.Server.Plugins.Load);
#endif
            }
            return true;
        }

        /// <summary>
        /// 尝试作为数据文件读取
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>处理结果</returns>
        private static bool TryReadAsDataFile(string path)
        {
            string content = File.ReadAllText(path);
            string extension = Path.GetExtension(path).ToLowerInvariant();
            if (extension == ".json")
            {
                JObject? jobject = JsonConvert.DeserializeObject<JObject>(content);
                if (jobject is null)
                {
                    MsgBox.Show("文件为空");
                    return true;
                }

                string? fileType = jobject["type"]?.ToString().ToLowerInvariant();

                switch (fileType)
                {
                    case "regex":
                        if (MsgBox.Show("是否导入此正则文件？", true))
                        {
                            Data.ParseRegex(jobject, MsgBox.Show("是否与原有的正则合并？\n否则将替换原有正则", true));

#if WINFORM
                            Program.Ui?.Invoke(Program.Ui.LoadRegex);
#elif WPF
                            Catalog.Function.Regex?.Dispatcher.Invoke(Catalog.Function.Regex.Load);
#endif
                        }
                        break;

                    case "schedule":
                    case "task":
                        if (MsgBox.Show("是否导入此任务文件？", true))
                        {
                            Data.ParseSchedule(jobject);

#if WINFORM
                            Program.Ui?.Invoke(Program.Ui.LoadSchedule);
#elif WPF
                            Catalog.Function.Schedule?.Dispatcher.Invoke(
                                Catalog.Function.Schedule.Load
                            );
#endif
                        }
                        break;

                    default:
                        MsgBox.Show("不支持导入此文件");
                        break;
                }

                return true;
            }
            else if (extension == ".tsv")
            {
                switch (Path.GetFileNameWithoutExtension(path))
                {
                    case "regex":
                        if (MsgBox.Show("是否导入此正则文件？", true))
                        {
                            Data.ParseRegex(content.Split('\n'));

#if WINFORM
                            Program.Ui?.Invoke(Program.Ui.LoadRegex);
#elif WPF
                            Catalog.Function.Regex?.Dispatcher.Invoke(Catalog.Function.Regex.Load);
#endif
                        }
                        break;

                    case "task":
                        if (MsgBox.Show("是否导入此任务文件？", true))
                        {
                            Data.ParseSchedule(content.Split('\n'));

#if WINFORM
                            Program.Ui?.Invoke(Program.Ui.LoadSchedule);
#elif WPF
                            Catalog.Function.Schedule?.Dispatcher.Invoke(
                                Catalog.Function.Schedule.Load
                            );
#endif
                        }
                        break;

                    default:
                        MsgBox.Show("不支持导入此文件");
                        break;
                }

                return true;
            }
            return false;
        }
    }
}

#endif
