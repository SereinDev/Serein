using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serein.Base;
using Serein.Core.Generic;
using Serein.Core.JSPlugin.Permission;
using Serein.Extensions;
using Serein.Utils.Output;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Serein.Utils.IO
{
    internal static class Data
    {
        /// <summary>
        /// 旧文本
        /// </summary>
        private static string _oldMembers = string.Empty;

        /// <summary>
        /// 消息弹窗
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="quiet">静处理</param>
        public static void ShowMsgBox(string text, bool quiet = false)
        {
            if (!quiet)
            {
                MsgBox.Show(text);
            }
        }

        #region 正则

        /// <summary>
        /// 解析正则
        /// </summary>
        /// <param name="jsonObject">JSON对象</param>
        /// <param name="appendMode">追加模式</param>
        /// <param name="quiet">静处理</param>
        public static void ParseRegex(JObject? jsonObject, bool appendMode = false, bool quiet = false)
        {
            if (jsonObject is null)
            {
                ShowMsgBox("正则文件为空", quiet);
                return;
            }

            if (jsonObject["type"]?.ToString().ToLowerInvariant() != "regex")
            {
                ShowMsgBox("正则文件无效", quiet);
                return;
            }

            if (jsonObject["data"] is null)
            {
                ShowMsgBox("正则文件的`data`字段数据为空", quiet);
                return;
            }

            try
            {
                if (appendMode)
                {
                    lock (Global.RegexList)
                    {
                        jsonObject["data"]!.ToObject<List<Regex>>()?.Where((i) => i is not null).ToList().ForEach((i) => Global.RegexList.Add(i));
                    }
                }
                else
                {
                    lock (Global.RegexList)
                    {
                        Global.RegexList = jsonObject["data"]?.ToObject<List<Regex>>() ?? new();
                    }
                }
                SaveRegex();
            }
            catch (Exception e)
            {
                ShowMsgBox("解析正则时出现问题\n" + e.Message);
                Logger.Output(LogType.Debug, e);
            }
        }

        /// <summary>
        /// 解析正则
        /// </summary>
        /// <param name="lines">行数组</param>
        /// <param name="appendMode">追加模式</param>
        public static void ParseRegex(string[] lines, bool appendMode = false)
        {
            List<Regex> list = appendMode ? Global.RegexList : new();
            foreach (string line in lines)
            {
                Regex regex = new();
                regex.FromText(line.Trim('\r', '\n'));
                if (!regex.Check())
                {
                    continue;
                }
                list.Add(regex);
            }
            Global.RegexList = list;
            SaveRegex();
        }

        /// <summary>
        /// 读取正则文件
        /// </summary>
        /// <param name="filename">路径</param>
        /// <param name="appendMode">追加模式</param>
        /// <param name="quiet">静处理</param>
        public static void ReadRegex(string? filename = null, bool appendMode = false, bool quiet = false)
        {
            Directory.CreateDirectory("data");
            filename ??= Path.Combine("data", "regex.json");

            if (!File.Exists(filename))
            {
                return;
            }

            string content = File.ReadAllText(filename);
            if (filename.ToLowerInvariant().EndsWith(".tsv"))
            {
                ParseRegex(content.Split('\n'), appendMode);
            }
            else if (filename.ToLowerInvariant().EndsWith(".json"))
            {
                JObject? jsonObject = JsonConvert.DeserializeObject<JObject>(content);
                ParseRegex(jsonObject, appendMode, quiet);
            }
        }

        /// <summary>
        /// 保存正则
        /// </summary>
        public static void SaveRegex()
        {
            Directory.CreateDirectory("data");
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

        #endregion


        #region 成员

        /// <summary>
        /// 读取成员文件
        /// </summary>
        public static void ReadMember()
        {
            Directory.CreateDirectory("data");
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
                    if (jsonObject?["type"]?.ToString().ToUpperInvariant() == "MEMBERS" && jsonObject["data"] is not null)
                    {
                        List<Member> list = jsonObject["data"]?.ToObject<List<Member>>() ?? new(); ;
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
            Directory.CreateDirectory("data");
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

        #endregion


        #region  任务

        /// <summary>
        /// 解析任务文件
        /// </summary>
        /// <param name="jsonObject"></param>
        /// <param name="quiet">静处理</param>
        public static void ParseSchedule(JObject? jsonObject, bool quiet = false)
        {
            if (jsonObject is null)
            {
                ShowMsgBox("任务文件为空", quiet);
                return;
            }

            if (jsonObject["type"]?.ToString().ToLowerInvariant() != "schedule")
            {
                ShowMsgBox("任务文件无效", quiet);
                return;
            }

            if (jsonObject["data"] is null)
            {
                ShowMsgBox("任务文件的`data`字段数据为空", quiet);
                return;
            }

            Global.Schedules = jsonObject["data"]?.ToObject<List<Schedule>>() ?? new();
            SaveSchedule();
        }

        /// <summary>
        /// 解析正则
        /// </summary>
        /// <param name="lines">行数组</param>
        /// <param name="appendMode">追加模式</param>
        public static void ParseSchedule(string[] lines)
        {
            List<Schedule> list = new();
            foreach (string line in lines)
            {
                Schedule? schedule = Schedule.FromText(line.Trim('\r', '\n'));
                if (schedule?.Check() ?? false)
                {
                    list.Add(schedule);
                }
            }
            Global.Schedules = list;
            SaveSchedule();
        }

        /// <summary>
        /// 读取任务文件
        /// </summary>
        /// <param name="filename">路径</param>
        /// <param name="quiet">静处理</param>
        public static void ReadSchedule(string? filename = null, bool quiet = false)
        {
            Directory.CreateDirectory("data");
            filename ??= Path.Combine("data", "schedule.json");
            if (!File.Exists(filename))
            {
                return;
            }

            string content = File.ReadAllText(filename);
            if (filename.ToLowerInvariant().EndsWith(".tsv"))
            {
                ParseSchedule(content.Split('\n'));
            }
            else if (filename.ToLowerInvariant().EndsWith(".json"))
            {
                JObject? jsonObject = JsonConvert.DeserializeObject<JObject>(content);
                ParseSchedule(jsonObject);
            }
            SaveSchedule();
        }

        /// <summary>
        /// 保存任务
        /// </summary>
        public static void SaveSchedule()
        {
            Directory.CreateDirectory("data");
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

        #endregion

        /// <summary>
        /// 保存群组缓存
        /// </summary>
        public static void SaveGroupCache()
        {
            if (Websocket.Status)
            {
                Directory.CreateDirectory("data");
                lock (Global.GroupCache)
                {
                    lock (FileLock.GroupCache)
                    {
                        File.WriteAllText(Path.Combine("data", "groupcache.json"), JsonConvert.SerializeObject(Global.GroupCache, Formatting.Indented));
                    }
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
            Directory.CreateDirectory("plugins");

            lock (Global.PermissionGroups)
            {
                lock (FileLock.PermissionGroups)
                {
                    try
                    {
                        File.WriteAllText(Path.Combine("plugins", "permission.json"), JsonConvert.SerializeObject(Global.PermissionGroups, Formatting.Indented));
                    }
                    catch (JsonSerializationException e)
                    {
                        throw new NotSupportedException(
                            "序列化权限组时出现异常：" + e.Message, e);
                    }
                }
            }
        }

        /// <summary>
        /// 读取权限组
        /// </summary>
        public static void ReadPermissionGroups()
        {
            string filename = Path.Combine("plugins", "permission.json");
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
                    File.WriteAllText(Path.Combine("plugins", "permission.json"), Global.PermissionGroups.ToJson(Formatting.Indented));
                }
            }
        }

        /// <summary>
        /// 文件读写锁
        /// </summary>
        public struct FileLock
        {
            public static readonly object
                Regex = new(),
                Schedule = new(),
                Member = new(),
                PermissionGroups = new(),
                GroupCache = new();
        }
    }
}