using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serein.Base;
using Serein.Core.Generic;
using Serein.Core.JSPlugin;
using Serein.Core.JSPlugin.Permission;
using Serein.Extensions;
using Serein.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;

#if WPF
using Serein.Windows;
#endif

namespace Serein.Utils
{
    internal static class IO
    {
        /// <summary>
        /// 旧文本
        /// </summary>
        private static string _oldSettings = string.Empty, _oldMembers = string.Empty;

#if !CONSOLE
        /// <summary>
        /// 保存更新设置计时器
        /// </summary>
        public static readonly Timer Timer = new(2000) { AutoReset = true };
#endif

        /// <summary>
        /// 懒惰计时器
        /// </summary>
        public static readonly Timer LazyTimer = new(60000) { AutoReset = true };

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
        public static void StartSaving()
        {
#if !CONSOLE
            Timer.Elapsed += (_, _) => SaveSettings();
            Timer.Start();
#endif
            LazyTimer.Elapsed += (_, _) => SaveMember();
            LazyTimer.Elapsed += (_, _) => SaveGroupCache();
            LazyTimer.Elapsed += (_, _) => SavePermissionGroups();
            LazyTimer.Start();
        }

        /// <summary>
        /// 读取所有文件
        /// </summary>
        public static void ReadAll()
        {
            ReadRegex();
            ReadMember();
            if (File.Exists(Path.Combine("data", "task.json")) && !File.Exists(Path.Combine("data", "schedule.json")))
            {
                ReadSchedule(Path.Combine("data", "task.json"));
                File.Delete(Path.Combine("data", "task.json"));
            }
            else
            {
                ReadSchedule();
            }
            ReadGroupCache();
            ReadPermissionGroups();
            ReadSettings();
            SaveSettings();
        }

        /// <summary>
        /// 读取正则文件
        /// </summary>
        /// <param name="filename">路径</param>
        public static void ReadRegex(string? filename = null, bool append = false)
        {
            CreateDirectory("data");
            filename ??= Path.Combine("data", "regex.json");
            if (!File.Exists(filename)) { return; }
            using (StreamReader streamReader = new(filename, Encoding.UTF8))
            {
                if (filename.ToLowerInvariant().EndsWith(".tsv"))
                {
                    string? line;
                    List<Regex> list = append ? Global.RegexList : new();
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
                    JObject? jsonObject = JsonConvert.DeserializeObject<JObject>(text);
                    if (jsonObject?["type"]?.ToString().ToUpperInvariant() == "REGEX")
                    {
                        if (append)
                        {
                            lock (Global.RegexList)
                            {
                                ((JArray)jsonObject["data"]!).ToObject<List<Regex>>()?.Where((i) => i is not null).ToList().ForEach((i) => Global.RegexList.Add(i));
                            }
                        }
                        else
                        {
                            lock (Global.RegexList)
                            {
                                Global.RegexList = ((JArray?)jsonObject["data"])?.ToObject<List<Regex>>() ?? new();
                            }
                        }
                    }
                    else if (!string.IsNullOrEmpty(filename))
                    {
                        Logger.MsgBox("不支持导入此文件", "Serein", 0, 48);
                    }
                }
            }
            SaveRegex();
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
                    JObject? jsonObject = JsonConvert.DeserializeObject<JObject>(text);
                    if (jsonObject?["type"]?.ToString().ToUpperInvariant() == "MEMBERS" && jsonObject["data"]?.HasValues == true)
                    {
                        List<Member> list = ((JArray?)jsonObject["data"])?.ToObject<List<Member>>() ?? new(); ;
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
            if (JsonConvert.SerializeObject(dictionary) != _oldMembers)
            {
                _oldMembers = JsonConvert.SerializeObject(dictionary);
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
        public static void ReadSchedule(string? filename = null)
        {
            CreateDirectory("data");
            filename ??= Path.Combine("data", "schedule.json");
            if (!File.Exists(filename)) { return; }

            using (StreamReader streamReader = new(filename, Encoding.UTF8))
            {
                if (filename.ToLowerInvariant().EndsWith(".tsv"))
                {
                    string? line;
                    List<Schedule> list = new();
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        Schedule? schedule = Schedule.FromText(line);
                        if (schedule is not null && schedule.Check())
                        {
                            list.Add(schedule);
                        }
                    }
                    Global.Schedules = list;
                }
                else if (filename.ToLowerInvariant().EndsWith(".json"))
                {
                    string text = streamReader.ReadToEnd();
                    if (!string.IsNullOrEmpty(text))
                    {
                        JObject? jsonObject = JsonConvert.DeserializeObject<JObject>(text);
                        if ((jsonObject?["type"]?.ToString().ToUpperInvariant() == "SCHEDULE" ||
                            jsonObject?["type"]?.ToString().ToUpperInvariant() == "TASK") &&
                            jsonObject["data"] != null)
                        {
                            Global.Schedules = ((JArray)jsonObject["data"]!).ToObject<List<Schedule>>()!;
                        }
                        else if (!string.IsNullOrEmpty(filename))
                        {
                            Logger.MsgBox("不支持导入此文件", "Serein", 0, 48);
                        }
                    }
                }
            }
            SaveSchedule();
        }

        /// <summary>
        /// 保存任务
        /// </summary>
        public static void SaveSchedule()
        {
            CreateDirectory("data");
            JObject jsonObject = new()
            {
                { "type", "SCHEDULE" },
                { "comment", "非必要请不要直接修改文件，语法错误可能导致数据丢失" },
                { "data", JArray.FromObject(Global.Schedules) }
            };
            lock (FileLock.Schedule)
            {
                File.WriteAllText(Path.Combine("data", "schedule.json"), jsonObject.ToString());
            }
        }

        /// <summary>
        /// 读取设置
        /// </summary>
        public static void ReadSettings()
        {
            if (!Directory.Exists("settings"))
            {
                Global.FirstOpen = true;
                Directory.CreateDirectory("settings");
                File.WriteAllText(Path.Combine("settings", "Matches.json"), JsonConvert.SerializeObject(new Matches(), Formatting.Indented));
                File.WriteAllText(Path.Combine("settings", "Event.json"), JsonConvert.SerializeObject(new Event(), Formatting.Indented));
                return;
            }
            if (File.Exists(Path.Combine("settings", "Server.json")))
            {
                Global.Settings.Server = JsonConvert.DeserializeObject<Settings.Server>(File.ReadAllText(Path.Combine("settings", "Server.json"), Encoding.UTF8)) ?? new();
            }
            if (File.Exists(Path.Combine("settings", "Serein.json")))
            {
                Global.Settings.Serein = JsonConvert.DeserializeObject<Settings.Serein>(File.ReadAllText(Path.Combine("settings", "Serein.json"), Encoding.UTF8)) ?? new();
                if (!Global.Settings.Serein.Function.RegexForCheckingGameID.TestRegex())
                {
                    throw new NotSupportedException("“Serein.Function.RegexForCheckingGameID”不合法，请修改“settings/Serein.json”后重试");
                }
            }
            if (File.Exists(Path.Combine("settings", "Bot.json")))
            {
                Global.Settings.Bot = JsonConvert.DeserializeObject<Bot>(File.ReadAllText(Path.Combine("settings", "Bot.json"), Encoding.UTF8)) ?? new();
            }
            if (File.Exists(Path.Combine("settings", "Matches.json")))
            {
                Global.Settings.Matches = JsonConvert.DeserializeObject<Matches>(File.ReadAllText(Path.Combine("settings", "Matches.json"), Encoding.UTF8)) ?? new();
                File.WriteAllText(Path.Combine("settings", "Matches.json"), JsonConvert.SerializeObject(Global.Settings.Matches, Formatting.Indented));
            }
            if (File.Exists(Path.Combine("settings", "Event.json")))
            {
                Global.Settings.Event = JsonConvert.DeserializeObject<Settings.Event>(File.ReadAllText(Path.Combine("settings", "Event.json"), Encoding.UTF8)) ?? new();
                File.WriteAllText(Path.Combine("settings", "Event.json"), JsonConvert.SerializeObject(Global.Settings.Event, Formatting.Indented));
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
                    Global.Settings.Matches = JsonConvert.DeserializeObject<Matches>(File.ReadAllText(Path.Combine("settings", "Matches.json"), Encoding.UTF8))!;
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
            if (newSettings != _oldSettings)
            {
                CreateDirectory("settings");
                _oldSettings = newSettings;
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
                        Global.GroupCache = JsonConvert.DeserializeObject<Dictionary<long, Dictionary<long, Member>>>(File.ReadAllText(Path.Combine("data", "groupcache.json"))) ?? new();
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
        /// 保存权限组
        /// </summary>
        public static void SavePermissionGroups()
        {
            CreateDirectory("data");
            lock (Global.PermissionGroups)
                lock (FileLock.PermissionGroups)
                {
                    try
                    {
                        File.WriteAllText(Path.Combine("data", "permission.json"), JsonConvert.SerializeObject(Global.PermissionGroups, Formatting.Indented));
                    }
                    catch (Newtonsoft.Json.JsonSerializationException e)
                    {
                        throw new NotSupportedException(
                            "序列化权限组时出现异常。" + e.Message == "Error getting value from 'Params' on 'Esprima.Ast.ArrowFunctionExpression'." ?
                            "检查一下你是不是把函数塞到权限字典里了！！！" : string.Empty, e);
                    }
                }
        }

        /// <summary>
        /// 读取权限组
        /// </summary>
        public static void ReadPermissionGroups()
        {
            string filename = Path.Combine("data", "permission.json");
            if (File.Exists(filename))
            {
                lock (Global.PermissionGroups)
                    lock (FileLock.PermissionGroups)
                    {
                        Global.PermissionGroups = JsonConvert.DeserializeObject<Dictionary<string, PermissionGroup>>(File.ReadAllText(Path.Combine("data", "permission.json"))) ?? new();
                    }
            }
            else
            {
                lock (FileLock.PermissionGroups)
                {
                    File.WriteAllText(Path.Combine("data", "permission.json"), Global.PermissionGroups.ToJson(Formatting.Indented));
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
        /// 热重载
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="allowToReloadPlugin">允许重新加载插件</param>
        public static void Reload(string type, bool allowToReloadPlugin = false)
        {
            switch (type?.ToLowerInvariant())
            {
                case null:
                case "all":
                    ReadAll();
                    if (allowToReloadPlugin)
                    {
                        JSPluginManager.Reload();
                    }
#if WINFORM
                    Program.Ui?.Invoke(Program.Ui.LoadRegex);
                    Program.Ui?.Invoke(Program.Ui.LoadMember);
                    Program.Ui?.Invoke(Program.Ui.LoadSchedule);
                    Program.Ui?.Invoke(Program.Ui.LoadSettings);
                    if (allowToReloadPlugin)
                    {
                        Program.Ui?.Invoke(Program.Ui.LoadJSPluginPublicly);
                    }
#elif WPF
                    Catalog.Function.Regex?.Dispatcher.Invoke(Catalog.Function.Regex.Load);
                    Catalog.Function.Member?.Dispatcher.Invoke(Catalog.Function.Member.Load);
                    Catalog.Function.Schedule?.Dispatcher.Invoke(Catalog.Function.Schedule.Load);
                    if (allowToReloadPlugin)
                    {
                        Catalog.Function.JSPlugin?.LoadPublicly();
                    }
#endif
                    break;
                case "regex":
                    ReadRegex();
#if WINFORM
                    Program.Ui?.Invoke(Program.Ui.LoadRegex);
#elif WPF
                    Catalog.Function.Regex?.Dispatcher.Invoke(Catalog.Function.Regex.Load);
#endif
                    break;
                case "member":
                    ReadMember();
#if WINFORM
                    Program.Ui?.Invoke(Program.Ui.LoadMember);
#elif WPF
                    Catalog.Function.Member?.Dispatcher.Invoke(Catalog.Function.Member.Load);
#endif
                    break;
                case "schedule":
                    ReadSchedule();
#if WINFORM
                    Program.Ui?.Invoke(Program.Ui.LoadSchedule);
#elif WPF
                    Catalog.Function.Schedule?.Dispatcher.Invoke(Catalog.Function.Schedule.Load);
#endif
                    break;
                case "groupcache":
                    ReadGroupCache();
                    break;
                case "settings":
                    ReadSettings();
#if WINFORM
                    Program.Ui?.Invoke(Program.Ui.LoadSettings);
#endif
                    break;
                case "permissiongroup":
                    ReadPermissionGroups();
                    break;
                case "plugin":
                case "plugins":
                    if (allowToReloadPlugin)
                    {
                        JSPluginManager.Reload();
#if WINFORM
                        Program.Ui?.Invoke(Program.Ui.LoadJSPluginPublicly);
#elif WPF
                        Catalog.Function.JSPlugin?.LoadPublicly();
#endif
                        break;
                    }
                    throw new InvalidOperationException("权限不足");
                default:
                    throw new ArgumentException("重新加载类型未知");
            }
        }

        /// <summary>
        /// 文件读写锁
        /// </summary>
        public static class FileLock
        {
            public static readonly object
                Console = new(),
                Msg = new(),
                Crash = new(),
                Debug = new(),
                Regex = new(),
                Schedule = new(),
                GroupCache = new(),
                Member = new(),
                Settings = new(),
                PermissionGroups = new();
        }
    }
}