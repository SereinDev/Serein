using Newtonsoft.Json;
using Serein.Base;
using Serein.Extensions;
using Serein.Settings;
using Serein.Utils.Output;
using System;
using System.IO;
using System.Text;

namespace Serein.Utils.IO
{
    internal static class Setting
    {
        /// <summary>
        /// 旧文本
        /// </summary>
        private static string _oldSettings = string.Empty;

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
            Directory.CreateDirectory("settings");
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
                Directory.CreateDirectory("settings");
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
            Directory.CreateDirectory("settings");
            lock (Global.Settings.Event)
            {
                File.WriteAllText(Path.Combine("settings", "Event.json"), JsonConvert.SerializeObject(Global.Settings.Event, Formatting.Indented));
            }
        }


        /// <summary>
        /// 文件读写锁
        /// </summary>
        public struct FileLock
        {
            public static readonly object
                Settings = new();
        }
    }
}