using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serein.Extensions;
using Serein.Items;
using Serein.Properties;
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
        public static readonly Timer Timer = new(2000) { AutoReset = true };

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="path">路径</param>
        public static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// 启动保存和更新设置定时器
        /// </summary>
        public static void StartSavingAndUpdating()
        {
#if !CONSOLE
            Timer.Elapsed += (_, _) => UpdateSettings();
            Timer.Elapsed += (_, _) => SaveSettings();
#endif
            Timer.Elapsed += (_, _) => SaveMember();
            Timer.Elapsed += (_, _) => SaveGroupCache();
            Timer.Start();
        }

        /// <summary>
        /// 读取所有文件
        /// </summary>
        public static void ReadAll()
        {
            ReadRegex();
            ReadMember();
            ReadTask();
            ReadGroupCache();
            ReadSettings();
        }

        /// <summary>
        /// 读取正则文件
        /// </summary>
        /// <param name="filename">路径</param>
        public static void ReadRegex(string filename = null)
        {
            CreateDirectory("data");
            filename ??= Path.Combine("data", "regex.json");
            if (File.Exists(filename))
            {
                lock (FileLock.Regex)
                {
                    StreamReader streamReader = new(filename, Encoding.UTF8);
                    if (filename.ToLowerInvariant().EndsWith(".tsv"))
                    {
                        string line;
                        List<Regex> list = new();
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            Regex regex = new();
                            regex.FromText(line);
                            if (!regex.Check())
                            {
                                continue;
                            }
                            list.Add(regex);
                        }
                        Global.RegexList = list;
                    }
                    else if (filename.ToLowerInvariant().EndsWith(".json"))
                    {
                        string text = streamReader.ReadToEnd();
                        if (string.IsNullOrEmpty(text))
                        {
                            return;
                        }
                        JObject jsonObject = (JObject)JsonConvert.DeserializeObject(text);
                        if (jsonObject["type"].ToString().ToUpperInvariant() == "REGEX")
                        {
                            Global.RegexList = ((JArray)jsonObject["data"]).ToObject<List<Regex>>();
                        }
                    }
                    streamReader.Close();
                    streamReader.Dispose();
                }
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
            CreateDirectory("data");
            JObject jsonObject = new()
            {
                { "type", "REGEX" },
                { "comment", "非必要请不要直接修改文件，语法错误可能导致数据丢失" },
                { "data", JArray.FromObject(Global.RegexList) }
            };
            lock (FileLock.Regex)
            {
                File.WriteAllText(Path.Combine("data", "regex.json"), jsonObject.ToString());
            }
        }

        /// <summary>
        /// 读取成员文件
        /// </summary>
        public static void ReadMember()
        {
            CreateDirectory("data");
            if (File.Exists(Path.Combine("data", "members.json")))
            {
                string text;
                lock (FileLock.Member)
                {
                    text = File.ReadAllText(Path.Combine("data", "members.json"), Encoding.UTF8);
                }
                if (!string.IsNullOrEmpty(text))
                {
                    JObject jsonObject = (JObject)JsonConvert.DeserializeObject(text);
                    if (jsonObject["type"].ToString().ToUpperInvariant() == "MEMBERS")
                    {
                        List<Member> list = ((JArray)jsonObject["data"]).ToObject<List<Member>>();
                        list.Sort((item1, item2) => item1.ID > item2.ID ? 1 : -1);
                        Dictionary<long, Member> dictionary = new();
                        list.ForEach((x) => dictionary.Add(x.ID, x));
                        lock (Global.MemberDict)
                        {
                            Global.MemberDict = dictionary;
                        }
                    }
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
            CreateDirectory("data");
            List<Member> list = Global.MemberDict.Values.ToList();
            Dictionary<long, Member> dictionary = new();
            list.Sort((item1, item2) => item1.ID > item2.ID ? 1 : -1);
            list.ForEach((x) => dictionary.Add(x.ID, x));
            lock (Global.MemberDict)
            {
                Global.MemberDict = dictionary;
            }
            if (JsonConvert.SerializeObject(dictionary) != OldMembers)
            {
                OldMembers = JsonConvert.SerializeObject(dictionary);
                JObject jsonObject = new()
                {
                    { "type", "MEMBERS" },
                    { "comment", "非必要请不要直接修改文件，语法错误可能导致数据丢失" },
                    { "data", JArray.FromObject(list) }
                };
                lock (FileLock.Member)
                {
                    File.WriteAllText(
                        Path.Combine("data", "members.json"),
                        jsonObject.ToString(),
                        Encoding.UTF8
                        );
                }
            }
        }

        /// <summary>
        /// 读取任务文件
        /// </summary>
        /// <param name="filename">路径</param>
        public static void ReadTask(string filename = null)
        {
            CreateDirectory("data");
            filename ??= Path.Combine("data", "task.json");
            if (File.Exists(filename))
            {
                StreamReader streamReader = new(filename, Encoding.UTF8);
                if (filename.ToLowerInvariant().EndsWith(".tsv"))
                {
                    string line;
                    List<Task> list = new();
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        Task task = new();
                        task.FromText(line);
                        if (task.Check())
                        {
                            list.Add(task);
                        }
                    }
                    Global.TaskList = list;
                }
                else if (filename.ToLowerInvariant().EndsWith(".json"))
                {
                    string text = streamReader.ReadToEnd();
                    if (!string.IsNullOrEmpty(text))
                    {
                        JObject jsonObject = (JObject)JsonConvert.DeserializeObject(text);
                        if (jsonObject["type"].ToString().ToUpperInvariant() != "TASK")
                        {
                            Global.TaskList = ((JArray)jsonObject["data"]).ToObject<List<Task>>();
                        }
                    }
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
            CreateDirectory("data");
            JObject jsonObject = new()
            {
                { "type", "TASK" },
                { "comment", "非必要请不要直接修改文件，语法错误可能导致数据丢失" },
                { "data", JArray.FromObject(Global.TaskList) }
            };
            lock (FileLock.Task)
            {
                File.WriteAllText(Path.Combine("data", "task.json"), jsonObject.ToString());
            }
        }

        /// <summary>
        /// 读取设置
        /// </summary>
        public static void ReadSettings()
        {
            if (((IList<string>)Environment.GetCommandLineArgs()).Contains("debug"))
            {
                Global.Settings.Serein.DevelopmentTool.EnableDebug = true;
            }
            if (!Directory.Exists("settings"))
            {
                Global.FirstOpen = true;
                Directory.CreateDirectory("settings");
                File.WriteAllText(Path.Combine("settings", "Matches.json"), JsonConvert.SerializeObject(new Matches(), Formatting.Indented));
                File.WriteAllText(Path.Combine("settings", "Event.json"), JsonConvert.SerializeObject(new Event(), Formatting.Indented));
            }
            else
            {
                if (File.Exists(Path.Combine("settings", "Server.json")))
                {
                    Global.Settings.Server = JsonConvert.DeserializeObject<Settings.Server>(File.ReadAllText(Path.Combine("settings", "Server.json"), Encoding.UTF8)) ?? new Settings.Server();
                }
                if (File.Exists(Path.Combine("settings", "Serein.json")))
                {
                    Global.Settings.Serein = JsonConvert.DeserializeObject<Settings.Serein>(File.ReadAllText(Path.Combine("settings", "Serein.json"), Encoding.UTF8)) ?? new Settings.Serein();
                }
                if (File.Exists(Path.Combine("settings", "Bot.json")))
                {
                    Global.Settings.Bot = JsonConvert.DeserializeObject<Bot>(File.ReadAllText(Path.Combine("settings", "Bot.json"), Encoding.UTF8)) ?? new Settings.Bot();
                }
                if (File.Exists(Path.Combine("settings", "Matches.json")))
                {
                    Global.Settings.Matches = JsonConvert.DeserializeObject<Matches>(File.ReadAllText(Path.Combine("settings", "Matches.json"), Encoding.UTF8)) ?? new Matches();
                    File.WriteAllText(Path.Combine("settings", "Matches.json"), JsonConvert.SerializeObject(Global.Settings.Matches, Formatting.Indented));
                }
                if (File.Exists(Path.Combine("settings", "Event.json")))
                {
                    Global.Settings.Event = JsonConvert.DeserializeObject<Settings.Event>(File.ReadAllText(Path.Combine("settings", "Event.json"), Encoding.UTF8)) ?? new Settings.Event();
                    File.WriteAllText(Path.Combine("settings", "Event.json"), JsonConvert.SerializeObject(Global.Settings.Event, Formatting.Indented));
                }
            }
        }

        /// <summary>
        /// 更新设置
        /// </summary>
        public static void UpdateSettings()
        {
            CreateDirectory("settings");
            try
            {
                if (File.Exists(Path.Combine("settings", "Matches.json")))
                {
                    Global.Settings.Matches = JsonConvert.DeserializeObject<Matches>(File.ReadAllText(Path.Combine("settings", "Matches.json"), Encoding.UTF8));
                }
            }
            catch (Exception e)
            {
                Logger.Output(LogType.Debug, "Fail to update Matches.json:", e);
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
                CreateDirectory("settings");
                OldSettings = newSettings;
                lock (FileLock.Settings)
                {
                    File.WriteAllText(Path.Combine("settings", "Server.json"), JsonConvert.SerializeObject(Global.Settings.Server, Formatting.Indented));
                    File.WriteAllText(Path.Combine("settings", "Bot.json"), JsonConvert.SerializeObject(Global.Settings.Bot, Formatting.Indented));
                    File.WriteAllText(Path.Combine("settings", "Serein.json"), JsonConvert.SerializeObject(Global.Settings.Serein, Formatting.Indented));
                }
            }
        }

        /// <summary>
        /// 保存事件设置
        /// </summary>
        public static void SaveEventSetting()
        {
            CreateDirectory("settings");
            lock (Global.Settings.Event)
            {
                File.WriteAllText(Path.Combine("settings", "Event.json"), JsonConvert.SerializeObject(Global.Settings.Event, Formatting.Indented));
            }
        }

        /// <summary>
        /// 保存群组缓存
        /// </summary>
        public static void SaveGroupCache()
        {
            if (Websocket.Status)
            {
                CreateDirectory("data");
                lock (Global.GroupCache)
                {
                    File.WriteAllText(Path.Combine("data", "groupcache.json"), JsonConvert.SerializeObject(Global.GroupCache, Formatting.Indented));
                }
            }
        }

        /// <summary>
        /// 读取群组缓存
        /// </summary>
        public static void ReadGroupCache()
        {
            string filename = Path.Combine("data", "groupcache.json");
            if (File.Exists(filename))
            {
                lock (Global.GroupCache)
                {
                    lock (FileLock.GroupCache)
                    {
                        Global.GroupCache = JsonConvert.DeserializeObject<Dictionary<long, Dictionary<long, Member>>>(
                            File.ReadAllText(Path.Combine("data", "groupcache.json")));
                    }
                }
            }
            else
            {
                lock (FileLock.GroupCache)
                {
                    File.WriteAllText(Path.Combine("data", "groupcache.json"), Global.GroupCache.ToJson());
                }
            }
        }

        /// <summary>
        /// 控制台日志
        /// </summary>
        /// <param name="line">行文本</param>
        public static void ConsoleLog(string line)
        {
            if (Global.Settings.Server.EnableLog)
            {
                CreateDirectory(Path.Combine("logs", "console"));
                try
                {
                    lock (FileLock.Console)
                    {
                        File.AppendAllText(
                            Path.Combine("logs", "console", $"{DateTime.Now:yyyy-MM-dd}.log"),
                            LogPreProcessing.Filter(line.TrimEnd('\n', '\r')) + Environment.NewLine,
                            Encoding.UTF8
                        );
                    }
                }
                catch (Exception e)
                {
                    Logger.Output(LogType.Debug, e);
                }
            }
        }

        /// <summary>
        /// 机器人消息日志
        /// </summary>
        /// <param name="line">行文本</param>
        public static void MsgLog(string line)
        {
            if (Global.Settings.Bot.EnableLog)
            {
                CreateDirectory(Path.Combine("logs", "msg"));
                try
                {
                    lock (FileLock.Msg)
                    {
                        File.AppendAllText(
                            Path.Combine("logs", "msg", $"{DateTime.Now:yyyy-MM-dd}.log"),
                            LogPreProcessing.Filter(line.TrimEnd('\n', '\r')) + Environment.NewLine,
                            Encoding.UTF8
                        );
                    }
                }
                catch (Exception e)
                {
                    Logger.Output(LogType.Debug, e);
                }
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        public static void Update()
        {
            if (!Global.Settings.Serein.AutoUpdate || !Net.IsReadyToUpdate)
            {
                return;
            }
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                if (!File.Exists("Updater.exe"))
                {
                    using (FileStream fileStream = new("Updater.exe", FileMode.CreateNew))
                    {
                        fileStream.Write(Resources.Updater, 0, Resources.Updater.Length);
                    }
                }
                Process.Start(new ProcessStartInfo("Updater.exe")
                {
                    WorkingDirectory = Global.PATH,
                    UseShellExecute = false,
                });

            }
        }

        /// <summary>
        /// 文件读写锁
        /// </summary>
        public static class FileLock
        {
            public static object Console = new();
            public static object Msg = new();
            public static object Crash = new();
            public static object Debug = new();
            public static object Regex = new();
            public static object Task = new();
            public static object GroupCache = new();
            public static object Member = new();
            public static object Settings = new();
        }
    }
}