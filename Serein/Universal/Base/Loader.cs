using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serein.Items;
using Serein.Plugin;
using Serein.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Timers;

namespace Serein.Base
{
    internal static class Loader
    {
        private static string OldSettings = string.Empty;

        public static void StartSavingAndUpdating()
        {
            Timer timer = new Timer(2000) { AutoReset = true };
            timer.Elapsed += (sender, e) => UpdateSettings();
            timer.Elapsed += (sender, e) => SaveSettings();
            timer.Start();
        }

        public static void ReadAll()
        {
            ReadRegex();
            ReadMember();
            ReadSettings();
            Plugins.Load();
        }

        public static void ReadRegex(string FileName = null)
        {
            if (string.IsNullOrEmpty(FileName))
            {
                FileName = $"{Global.Path}\\data\\regex.json";
            }
            if (File.Exists(FileName))
            {
                StreamReader Reader = new StreamReader(
                    File.Open(
                        FileName,
                        FileMode.Open
                    ),
                    Encoding.UTF8
                    );
                if (FileName.ToUpper().EndsWith(".TSV"))
                {
                    string Line;
                    List<RegexItem> Items = new List<RegexItem>();
                    while ((Line = Reader.ReadLine()) != null)
                    {
                        RegexItem Item = new RegexItem();
                        Item.ConvertToItem(Line);
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
                        Global.UpdateRegexItems(((JArray)JsonObject["data"]).ToObject<List<RegexItem>>());
                    }
                    catch { }
                }
                Reader.Close();
                Reader.Dispose();
            }
        }

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
                        Global.UpdateMemberItems(((JArray)JsonObject["data"]).ToObject<List<MemberItem>>());
                    }
                    catch { }
                }
            }
            List<MemberItem> memberItems = Global.MemberItems;
            memberItems.Sort(
                (Item1, Item2) =>
                {
                    return Item1.ID > Item2.ID ? 1 : -1;
                }
                );
            Global.UpdateMemberItems(memberItems);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        public static void SaveMember()
        {
            List<MemberItem> memberItems = Global.MemberItems;
            memberItems.Sort(
                (Item1, Item2) =>
                {
                    return Item1.ID > Item2.ID ? 1 : -1;
                }
                );
            Global.UpdateMemberItems(memberItems);
            if (!Directory.Exists(Global.Path + "\\data"))
            {
                Directory.CreateDirectory(Global.Path + "\\data");
            }
            JObject ListJObject = new JObject();
            JArray ListJArray = new JArray();
            foreach (MemberItem Item in Global.MemberItems)
            {
                ListJArray.Add(JObject.FromObject(Item));
            }
            ListJObject.Add("type", "MEMBERS");
            ListJObject.Add("comment", "非必要请不要直接修改文件，语法错误可能导致数据丢失");
            ListJObject.Add("data", ListJArray);
            File.WriteAllText(
                $"{Global.Path}\\data\\members.json",
                ListJObject.ToString(),
                Encoding.UTF8
                );
        }

        public static void ReadTask(string FileName = null)
        {
            if (string.IsNullOrEmpty(FileName))
            {
                FileName = $"{Global.Path}\\data\\task.json";
            }
            if (File.Exists(FileName))
            {
                StreamReader Reader = new StreamReader(
                    File.Open(
                        FileName,
                        FileMode.Open
                    ),
                    Encoding.UTF8
                    );
                if (FileName.ToUpper().EndsWith(".TSV"))
                {
                    string Line;
                    List<TaskItem> Items = new List<TaskItem>();
                    while ((Line = Reader.ReadLine()) != null)
                    {
                        TaskItem Item = new TaskItem();
                        Item.ConvertToItem(Line);
                        if (!Item.CheckItem())
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
                        Global.UpdateTaskItems(((JArray)JsonObject["data"]).ToObject<List<TaskItem>>());
                    }
                    catch { }
                }
                Reader.Close();
                Reader.Dispose();
            }
        }

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
                Logger.Out(999, "[Setting] Fail to update Matches.json:", e.ToString());
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
                Logger.Out(999, "[Setting] Fail to update Event.json:", e.ToString());
            }
        }

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
    }
}