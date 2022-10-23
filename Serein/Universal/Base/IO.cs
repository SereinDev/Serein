using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serein.Items;
using Serein.Plugin;
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
        private static string OldSettings = string.Empty;

        /// <summary>
        /// 启动保存和更新设置定时器
        /// </summary>
        public static void StartSavingAndUpdating()
        {
            Timer _Timer = new Timer(2000) { AutoReset = true };
            _Timer.Elapsed += (sender, e) => UpdateSettings();
            _Timer.Elapsed += (sender, e) => SaveSettings();
            _Timer.Start();
        }

        /// <summary>
        /// 读取所有文件
        /// </summary>
        public static void ReadAll()
        {
            ReadRegex();
            ReadMember();
            ReadSettings();
            Plugins.Load();
        }

        /// <summary>
        /// 读取正则文件
        /// </summary>
        /// <param name="FileName">路径</param>
        public static void ReadRegex(string FileName = null)
        {
            FileName = FileName ?? $"{Global.Path}\\data\\regex.json";
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
                        Item.ToObject(Line);
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
                    if (string.IsNullOrEmpty(Text)) { return; }
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
        }

        /// <summary>
        /// 保存正则
        /// </summary>
        public static void SaveRegex()
        {
            if (!Directory.Exists(Global.Path + "\\data"))
            {
                Directory.CreateDirectory(Global.Path + "\\data");
            }
            JObject ListJObject = new JObject();
            ListJObject.Add("type", "REGEX");
            ListJObject.Add("comment", "非必要请不要直接修改文件，语法错误可能导致数据丢失");
            ListJObject.Add("data", JArray.FromObject(Global.RegexItems));
            File.WriteAllText($"{Global.Path}\\data\\regex.json", ListJObject.ToString());
        }

        /// <summary>
        /// 读取成员文件
        /// </summary>
        public static void ReadMember()
        {
            if (!Directory.Exists(Global.Path + "\\data"))
            {
                Directory.CreateDirectory(Global.Path + "\\data");
            }
            if (File.Exists($"{Global.Path}\\data\\members.json"))
            {
                string Text = File.ReadAllText(
                    $"{Global.Path}\\data\\members.json",
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
            if (!Directory.Exists(Global.Path + "\\data"))
            {
                Directory.CreateDirectory(Global.Path + "\\data");
            }
            JObject ListJObject = new JObject();
            ListJObject.Add("type", "MEMBERS");
            ListJObject.Add("comment", "非必要请不要直接修改文件，语法错误可能导致数据丢失");
            ListJObject.Add("data", JArray.FromObject(Items));
            File.WriteAllText(
                $"{Global.Path}\\data\\members.json",
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
            FileName = FileName ?? $"{Global.Path}\\data\\task.json";
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
                    }
                    catch { }
                }
                Reader.Close();
                Reader.Dispose();
            }
        }

        /// <summary>
        /// 保存任务
        /// </summary>
        public static void SaveTask()
        {
            if (!Directory.Exists(Global.Path + "\\data"))
            {
                Directory.CreateDirectory(Global.Path + "\\data");
            }
            JObject ListJObject = new JObject();
            ListJObject.Add("type", "TASK");
            ListJObject.Add("comment", "非必要请不要直接修改文件，语法错误可能导致数据丢失");
            ListJObject.Add("data", JArray.FromObject(Global.TaskItems));
            File.WriteAllText($"{Global.Path}\\data\\task.json", ListJObject.ToString());
        }

        /// <summary>
        /// 读取设置
        /// </summary>
        public static void ReadSettings()
        {
            if (!Directory.Exists(Global.SettingPath))
            {
                Directory.CreateDirectory(Global.SettingPath);
                return;
            }
            if (File.Exists(Global.SettingPath + "\\Server.json"))
            {
                Global.Settings.Server = JsonConvert.DeserializeObject<Settings.Server>(File.ReadAllText(Global.SettingPath + "\\Server.json", Encoding.UTF8))
                    ?? new Settings.Server();
            }
            if (File.Exists(Global.SettingPath + "\\Bot.json"))
            {
                Global.Settings.Bot = JsonConvert.DeserializeObject<Bot>(File.ReadAllText(Global.SettingPath + "\\Bot.json", Encoding.UTF8))
                    ?? new Bot();
            }
            if (File.Exists(Global.SettingPath + "\\Serein.json"))
            {
                Global.Settings.Serein = JsonConvert.DeserializeObject<Settings.Serein>(File.ReadAllText(Global.SettingPath + "\\Serein.json", Encoding.UTF8))
                    ?? new Settings.Serein();
            }
            if (File.Exists(Global.SettingPath + "\\Matches.json"))
            {
                Global.Settings.Matches = JsonConvert.DeserializeObject<Matches>(File.ReadAllText(Global.SettingPath + "\\Matches.json", Encoding.UTF8))
                    ?? new Matches();
            }
            else
            {
                File.WriteAllText(Global.SettingPath + "\\Matches.json", JsonConvert.SerializeObject(new Matches(), Formatting.Indented));
            }
            if (File.Exists(Global.SettingPath + "\\Event.json"))
            {
                Global.Settings.Event = JsonConvert.DeserializeObject<Settings.Event>(File.ReadAllText(Global.SettingPath + "\\Event.json", Encoding.UTF8)) ?? new Settings.Event();
            }
            else
            {
                File.WriteAllText(Global.SettingPath + "\\Matches.json", JsonConvert.SerializeObject(new Settings.Event(), Formatting.Indented));
            }
        }

        /// <summary>
        /// 更新设置
        /// </summary>
        public static void UpdateSettings()
        {
            try
            {
                if (File.Exists(Global.SettingPath + "\\Matches.json"))
                {
                    Global.Settings.Matches = JsonConvert.DeserializeObject<Matches>(File.ReadAllText(Global.SettingPath + "\\Matches.json", Encoding.UTF8));
                }
            }
            catch (Exception e)
            {
                Logger.Out(LogType.Debug, "[Setting] Fail to update Matches.json:", e.ToString());
            }
            try
            {
                if (File.Exists(Global.SettingPath + "\\Event.json"))
                {
                    Global.Settings.Event = JsonConvert.DeserializeObject<Settings.Event>(File.ReadAllText(Global.SettingPath + "\\Event.json", Encoding.UTF8));
                }
            }
            catch (Exception e)
            {
                Logger.Out(LogType.Debug, "[Setting] Fail to update Event.json:", e.ToString());
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
                File.WriteAllText(Global.SettingPath + "\\Server.json", JsonConvert.SerializeObject(Global.Settings.Server, Formatting.Indented));
                File.WriteAllText(Global.SettingPath + "\\Bot.json", JsonConvert.SerializeObject(Global.Settings.Bot, Formatting.Indented));
                File.WriteAllText(Global.SettingPath + "\\Serein.json", JsonConvert.SerializeObject(Global.Settings.Serein, Formatting.Indented));
            }
        }

        /// <summary>
        /// 保存事件设置
        /// </summary>
        public static void SaveEventSetting()
        {
            lock (Global.Settings.Event)
            {
                File.WriteAllText(Global.SettingPath + "\\Event.json", JsonConvert.SerializeObject(Global.Settings.Event, Formatting.Indented));
            }
        }
    }
}