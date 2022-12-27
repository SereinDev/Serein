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
        private static readonly Timer _Timer = new Timer(2000) { AutoReset = true };

        /// <summary>
        /// 启动保存和更新设置定时器
        /// </summary>
        public static void StartSavingAndUpdating()
        {
            _Timer.Elapsed += (sender, e) => UpdateSettings();
            _Timer.Elapsed += (sender, e) => SaveSettings();
            _Timer.Elapsed += (sender, e) => SaveMember();
            _Timer.Start();
        }

        /// <summary>
        /// 获取完整路径
        /// </summary>
        /// <param name="Paths">路径</param>
        /// <returns>完整路径</returns>
        public static string GetPath(params string[] Paths)
        {
            string CombinedPath = Global.Path;
            foreach (string i in Paths)
                CombinedPath = Path.Combine(CombinedPath, i);
            return CombinedPath;
        }

        /// <summary>
        /// 读取所有文件
        /// </summary>
        /// <param name="SkipLoadingPlugins">跳过插件加载</param>
        public static void ReadAll(bool SkipLoadingPlugins = false)
        {
            ReadRegex();
            ReadMember();
            ReadSettings();
            if (!SkipLoadingPlugins)
            {
                System.Threading.Tasks.Task.Run(JSPluginManager.Load);
            }
        }

        /// <summary>
        /// 读取正则文件
        /// </summary>
        /// <param name="FileName">路径</param>
        public static void ReadRegex(string FileName = null)
        {
            FileName = FileName ?? GetPath("data", "regex.json");
            if (File.Exists(FileName))
            {
                StreamReader Reader = new StreamReader(FileName, Encoding.UTF8);
                if (FileName.ToUpper().EndsWith(".TSV"))
                {
                    string Line;
                    List<Regex> Items = new List<Regex>();
                    while ((Line = Reader.ReadLine()) != null)
                    {
                        Regex Item = new Regex();
                        Item.FromText(Line);
                        if (!Item.Check())
                        {
                            continue;
                        }
                        Items.Add(Item);
                    }
                    Global.UpdateRegexItems(Items);
                }
                else if (FileName.ToUpper().EndsWith(".JSON"))
                {
                    string Text = Reader.ReadToEnd();
                    if (string.IsNullOrEmpty(Text))
                    {
                        return;
                    }
                    try
                    {
                        JObject JsonObject = (JObject)JsonConvert.DeserializeObject(Text);
                        if (JsonObject["type"].ToString().ToUpper() != "REGEX")
                        {
                            return;
                        }
                        Global.UpdateRegexItems(((JArray)JsonObject["data"]).ToObject<List<Regex>>());
                    }
                    catch { }
                }
                Reader.Close();
                Reader.Dispose();
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
            JObject ListJObject = new JObject
            {
                { "type", "REGEX" },
                { "comment", "非必要请不要直接修改文件，语法错误可能导致数据丢失" },
                { "data", JArray.FromObject(Global.RegexItems) }
            };
            File.WriteAllText(GetPath("data", "regex.json"), ListJObject.ToString());
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
                        JObject JsonObject = (JObject)JsonConvert.DeserializeObject(Text);
                        if (JsonObject["type"].ToString().ToUpper() != "MEMBERS")
                        {
                            return;
                        }
                        List<Member> Items = ((JArray)JsonObject["data"]).ToObject<List<Member>>();
                        Items.Sort((Item1, Item2) => Item1.ID > Item2.ID ? 1 : -1);
                        Dictionary<long, Member> _Dictionary = new Dictionary<long, Member>();
                        Items.ForEach((x) => _Dictionary.Add(x.ID, x));
                        Global.UpdateMemberItems(_Dictionary);
                    }
                    catch { }
                }
            }
            else
                SaveMember();
        }

        /// <summary>
        /// 保存成员数据
        /// </summary>
        public static void SaveMember()
        {
            List<Member> Items = Global.MemberItems.Values.ToList();
            Items.Sort((Item1, Item2) => Item1.ID > Item2.ID ? 1 : -1);
            Dictionary<long, Member> _Dictionary = new Dictionary<long, Member>();
            Items.ForEach((x) => _Dictionary.Add(x.ID, x));
            Global.UpdateMemberItems(_Dictionary);
            if (JsonConvert.SerializeObject(_Dictionary) == OldMembers)
            {
                return;
            }
            OldMembers = JsonConvert.SerializeObject(_Dictionary);
            if (!Directory.Exists(GetPath("data")))
            {
                Directory.CreateDirectory(GetPath("data"));
            }
            JObject ListJObject = new JObject
            {
                { "type", "MEMBERS" },
                { "comment", "非必要请不要直接修改文件，语法错误可能导致数据丢失" },
                { "data", JArray.FromObject(Items) }
            };
            File.WriteAllText(
                GetPath("data", "members.json"),
                ListJObject.ToString(),
                Encoding.UTF8
                );
        }

        /// <summary>
        /// 读取任务文件
        /// </summary>
        /// <param name="FileName">路径</param>
        public static void ReadTask(string FileName = null)
        {
            FileName = FileName ?? GetPath("data", "task.json");
            if (File.Exists(FileName))
            {
                StreamReader Reader = new StreamReader(FileName, Encoding.UTF8);
                if (FileName.ToUpper().EndsWith(".TSV"))
                {
                    string Line;
                    List<Task> Items = new List<Task>();
                    while ((Line = Reader.ReadLine()) != null)
                    {
                        Task Item = new Task();
                        Item.ToObject(Line);
                        if (!Item.Check())
                        {
                            continue;
                        }
                        Items.Add(Item);
                    }
                    Global.UpdateTaskItems(Items);
                }
                else if (FileName.ToUpper().EndsWith(".JSON"))
                {
                    string Text = Reader.ReadToEnd();
                    if (string.IsNullOrEmpty(Text)) { return; }
                    try
                    {
                        JObject JsonObject = (JObject)JsonConvert.DeserializeObject(Text);
                        if (JsonObject["type"].ToString().ToUpper() != "TASK")
                        {
                            return;
                        }
                        Global.UpdateTaskItems(((JArray)JsonObject["data"]).ToObject<List<Task>>());
                        lock (Global.TaskItems)
                            Global.TaskItems.ForEach((Task) => Task.Check());
                    }
                    catch { }
                }
                Reader.Close();
                Reader.Dispose();
            }
            else
                SaveTask();
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
            JObject ListJObject = new JObject
            {
                { "type", "TASK" },
                { "comment", "非必要请不要直接修改文件，语法错误可能导致数据丢失" },
                { "data", JArray.FromObject(Global.TaskItems) }
            };
            File.WriteAllText(GetPath("data", "task.json"), ListJObject.ToString());
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
                Global.Settings.Bot = JsonConvert.DeserializeObject<Settings.Bot>(File.ReadAllText(GetPath("settings", "Bot.json"), Encoding.UTF8)) ?? new Settings.Bot();
            }
            if (File.Exists(GetPath("settings", "Matches.json")))
            {
                Global.Settings.Matches = JsonConvert.DeserializeObject<Matches>(File.ReadAllText(GetPath("settings", "Matches.json"), Encoding.UTF8)) ?? new Matches();
                File.WriteAllText(GetPath("settings", "Matches.json"), JsonConvert.SerializeObject(Global.Settings.Matches, Formatting.Indented));
            }
            else
                File.WriteAllText(GetPath("settings", "Matches.json"), JsonConvert.SerializeObject(new Matches(), Formatting.Indented));
            if (File.Exists(GetPath("settings", "Event.json")))
            {
                Global.Settings.Event = JsonConvert.DeserializeObject<Settings.Event>(File.ReadAllText(GetPath("settings", "Event.json"), Encoding.UTF8)) ?? new Settings.Event();
                SaveEventSetting();
            }
            else
                File.WriteAllText(GetPath("settings", "Event.json"), JsonConvert.SerializeObject(new Settings.Event(), Formatting.Indented));
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
            string NewSettings = JsonConvert.SerializeObject(Global.Settings);
            if (NewSettings != OldSettings)
            {
                OldSettings = NewSettings;
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
                File.WriteAllText(GetPath("settings", "Event.json"), JsonConvert.SerializeObject(Global.Settings.Event, Formatting.Indented));
        }
    }
}