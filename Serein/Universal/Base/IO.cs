using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serein.Items;
using Serein.JSPlugin;
using Serein.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;

namespace Serein.Base
{
    internal static class IO
    {
        /// <summary>
        /// 旧文本
        /// </summary>
        private static string OldSettings = string.Empty, OldMembers = string.Empty;

        /// <summary>
        /// 保存更新设置定时器
        /// </summary>
        private static readonly Timer timer = new(2000) { AutoReset = true };

        /// <summary>
        /// 启动保存和更新设置定时器
        /// </summary>
        public static void StartSavingAndUpdating()
        {
            timer.Elapsed += (sender, e) => UpdateSettings();
            timer.Elapsed += (sender, e) => SaveSettings();
            timer.Elapsed += (sender, e) => SaveMember();
            timer.Start();
        }

        /// <summary>
        /// 获取完整路径
        /// </summary>
        /// <param name="paths">路径</param>
        /// <returns>完整路径</returns>
        public static string GetPath(params string[] paths)
        {
            string combinedPath = Global.Path;
            foreach (string i in paths)
            {
                combinedPath = Path.Combine(combinedPath, i);
            }
            return combinedPath;
        }

        /// <summary>
        /// 读取所有文件
        /// </summary>
        /// <param name="skipLoadingPlugins">跳过插件加载</param>
        public static void ReadAll(bool skipLoadingPlugins = false)
        {
            ReadRegex();
            ReadMember();
            ReadSettings();
            if (!skipLoadingPlugins)
            {
                System.Threading.Tasks.Task.Run(JSPluginManager.Load);
            }
        }

        /// <summary>
        /// 读取正则文件
        /// </summary>
        /// <param name="filename">路径</param>
        public static void ReadRegex(string filename = null)
        {
            filename ??= GetPath("data", "regex.json");
            if (File.Exists(filename))
            {
                StreamReader streamReader = new(filename, Encoding.UTF8);
                if (filename.ToUpper().EndsWith(".TSV"))
                {
                    string line;
                    List<Regex> list = new();
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        Regex Item = new();
                        Item.FromText(line);
                        if (!Item.Check())
                        {
                            continue;
                        }
                        list.Add(Item);
                    }
                    Global.UpdateRegexItems(list);
                }
                else if (filename.ToUpper().EndsWith(".JSON"))
                {
                    string text = streamReader.ReadToEnd();
                    if (string.IsNullOrEmpty(text))
                    {
                        return;
                    }
                    try
                    {
                        JObject jsonObject = (JObject)JsonConvert.DeserializeObject(text);
                        if (jsonObject["type"].ToString().ToUpper() != "REGEX")
                        {
                            return;
                        }
                        Global.UpdateRegexItems(((JArray)jsonObject["data"]).ToObject<List<Regex>>());
                    }
                    catch { }
                }
                streamReader.Close();
                streamReader.Dispose();
            }
            else
            {
                SaveRegex();
            }
        }

        /// <summary>
        /// 保存正则
        /// </summary>
        public static void SaveRegex()
        {
            if (!Directory.Exists(GetPath("data")))
            {
                Directory.CreateDirectory(GetPath("data"));
            }
            JObject jsonObject = new()
            {
                { "type", "REGEX" },
                { "comment", "非必要请不要直接修改文件，语法错误可能导致数据丢失" },
                { "data", JArray.FromObject(Global.RegexItems) }
            };
            File.WriteAllText(GetPath("data", "regex.json"), jsonObject.ToString());
        }

        /// <summary>
        /// 读取成员文件
        /// </summary>
        public static void ReadMember()
        {
            if (!Directory.Exists(GetPath("data")))
            {
                Directory.CreateDirectory("data");
            }
            if (File.Exists(GetPath("data", "members.json")))
            {
                string Text = File.ReadAllText(
                    GetPath("data", "members.json"),
                    Encoding.UTF8
                    );
                if (!string.IsNullOrEmpty(Text))
                {
                    try
                    {
                        JObject jsonObject = (JObject)JsonConvert.DeserializeObject(Text);
                        if (jsonObject["type"].ToString().ToUpper() != "MEMBERS")
                        {
                            return;
                        }
                        List<Member> list = ((JArray)jsonObject["data"]).ToObject<List<Member>>();
                        list.Sort((item1, item2) => item1.ID > item2.ID ? 1 : -1);
                        Dictionary<long, Member> dictionary = new();
                        list.ForEach((x) => dictionary.Add(x.ID, x));
                        Global.UpdateMemberItems(dictionary);
                    }
                    catch { }
                }
            }
            else
            {
                SaveMember();
            }
        }

        /// <summary>
        /// 保存成员数据
        /// </summary>
        public static void SaveMember()
        {
            List<Member> list = Global.MemberItems.Values.ToList();
            list.Sort((item1, item2) => item1.ID > item2.ID ? 1 : -1);
            Dictionary<long, Member> dictionary = new();
            list.ForEach((x) => dictionary.Add(x.ID, x));
            Global.UpdateMemberItems(dictionary);
            if (JsonConvert.SerializeObject(dictionary) == OldMembers)
            {
                return;
            }
            OldMembers = JsonConvert.SerializeObject(dictionary);
            if (!Directory.Exists(GetPath("data")))
            {
                Directory.CreateDirectory(GetPath("data"));
            }
            JObject jsonObject = new()
            {
                { "type", "MEMBERS" },
                { "comment", "非必要请不要直接修改文件，语法错误可能导致数据丢失" },
                { "data", JArray.FromObject(list) }
            };
            File.WriteAllText(
                GetPath("data", "members.json"),
                jsonObject.ToString(),
                Encoding.UTF8
                );
        }

        /// <summary>
        /// 读取任务文件
        /// </summary>
        /// <param name="filename">路径</param>
        public static void ReadTask(string filename = null)
        {
            filename ??= GetPath("data", "task.json");
            if (File.Exists(filename))
            {
                StreamReader streamReader = new(filename, Encoding.UTF8);
                if (filename.ToUpper().EndsWith(".TSV"))
                {
                    string line;
                    List<Task> items = new();
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        Task task = new();
                        task.FromText(line);
                        if (!task.Check())
                        {
                            continue;
                        }
                        items.Add(task);
                    }
                    Global.UpdateTaskItems(items);
                }
                else if (filename.ToUpper().EndsWith(".JSON"))
                {
                    string text = streamReader.ReadToEnd();
                    if (string.IsNullOrEmpty(text))
                    {
                        return;
                    }
                    try
                    {
                        JObject jsonObject = (JObject)JsonConvert.DeserializeObject(text);
                        if (jsonObject["type"].ToString().ToUpper() != "TASK")
                        {
                            return;
                        }
                        Global.UpdateTaskItems(((JArray)jsonObject["data"]).ToObject<List<Task>>());
                        lock (Global.TaskItems)
                        {
                            Global.TaskItems.ForEach((Task) => Task.Check());
                        }
                    }
                    catch { }
                }
                streamReader.Close();
                streamReader.Dispose();
            }
            else
            {
                SaveTask();
            }
        }

        /// <summary>
        /// 保存任务
        /// </summary>
        public static void SaveTask()
        {
            if (!Directory.Exists(GetPath("data")))
            {
                Directory.CreateDirectory(GetPath("data"));
            }
            JObject jsonObject = new()
            {
                { "type", "TASK" },
                { "comment", "非必要请不要直接修改文件，语法错误可能导致数据丢失" },
                { "data", JArray.FromObject(Global.TaskItems) }
            };
            File.WriteAllText(GetPath("data", "task.json"), jsonObject.ToString());
        }

        /// <summary>
        /// 读取设置
        /// </summary>
        public static void ReadSettings()
        {
            if (!Directory.Exists(GetPath("settings")))
            {
                Directory.CreateDirectory(GetPath("settings"));
                Global.FirstOpen = true;
                return;
            }
            if (File.Exists(GetPath("settings", "Server.json")))
            {
                Global.Settings.Server = JsonConvert.DeserializeObject<Settings.Server>(File.ReadAllText(GetPath("settings", "Server.json"), Encoding.UTF8)) ?? new Settings.Server();
            }
            if (File.Exists(GetPath("settings", "Serein.json")))
            {
                Global.Settings.Serein = JsonConvert.DeserializeObject<Settings.Serein>(File.ReadAllText(GetPath("settings", "Serein.json"), Encoding.UTF8)) ?? new Settings.Serein();
            }
            if (File.Exists(GetPath("settings", "Bot.json")))
            {
                Global.Settings.Bot = JsonConvert.DeserializeObject<Bot>(File.ReadAllText(GetPath("settings", "Bot.json"), Encoding.UTF8)) ?? new Settings.Bot();
            }
            if (File.Exists(GetPath("settings", "Matches.json")))
            {
                Global.Settings.Matches = JsonConvert.DeserializeObject<Matches>(File.ReadAllText(GetPath("settings", "Matches.json"), Encoding.UTF8)) ?? new Matches();
                File.WriteAllText(GetPath("settings", "Matches.json"), JsonConvert.SerializeObject(Global.Settings.Matches, Formatting.Indented));
            }
            else
            {
                File.WriteAllText(GetPath("settings", "Matches.json"), JsonConvert.SerializeObject(new Matches(), Formatting.Indented));
            }
            if (File.Exists(GetPath("settings", "Event.json")))
            {
                Global.Settings.Event = JsonConvert.DeserializeObject<Settings.Event>(File.ReadAllText(GetPath("settings", "Event.json"), Encoding.UTF8)) ?? new Settings.Event();
                File.WriteAllText(GetPath("settings", "Event.json"), JsonConvert.SerializeObject(Global.Settings.Event, Formatting.Indented));
                SaveEventSetting();
            }
            else
            {
                File.WriteAllText(GetPath("settings", "Event.json"), JsonConvert.SerializeObject(new Settings.Event(), Formatting.Indented));
            }
            if (((IList<string>)Environment.GetCommandLineArgs()).Contains("debug"))
            {
                Global.Settings.Serein.DevelopmentTool.EnableDebug = true;
            }
        }

        /// <summary>
        /// 更新设置
        /// </summary>
        public static void UpdateSettings()
        {
            try
            {
                if (File.Exists(GetPath("settings", "Matches.json")))
                {
                    Global.Settings.Matches = JsonConvert.DeserializeObject<Matches>(File.ReadAllText(GetPath("settings", "Matches.json"), Encoding.UTF8));
                }
            }
            catch (Exception e)
            {
                Logger.Out(LogType.Debug, "Fail to update Matches.json:", e.ToString());
            }
        }

        /// <summary>
        /// 保存设置
        /// </summary>
        public static void SaveSettings()
        {
            string newSettings = JsonConvert.SerializeObject(Global.Settings);
            if (newSettings != OldSettings)
            {
                OldSettings = newSettings;
                File.WriteAllText(GetPath("settings", "Server.json"), JsonConvert.SerializeObject(Global.Settings.Server, Formatting.Indented));
                File.WriteAllText(GetPath("settings", "Bot.json"), JsonConvert.SerializeObject(Global.Settings.Bot, Formatting.Indented));
                File.WriteAllText(GetPath("settings", "Serein.json"), JsonConvert.SerializeObject(Global.Settings.Serein, Formatting.Indented));
            }
        }

        /// <summary>
        /// 保存事件设置
        /// </summary>
        public static void SaveEventSetting()
        {
            lock (Global.Settings.Event)
            {
                File.WriteAllText(GetPath("settings", "Event.json"), JsonConvert.SerializeObject(Global.Settings.Event, Formatting.Indented));
            }
        }
    }
}