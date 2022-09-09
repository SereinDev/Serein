using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Serein.Settings
{
    internal class Base
    {
        public static Task SaveSettingsThread = new Task(SaveSettings);

        public static void StartSaveSettings()
        {
            SaveSettingsThread.Start();
        }

        public static void SaveSettings()
        {
            Logger.Out(999, "[Settings]", JsonConvert.SerializeObject(Global.Settings));
            SaveSettingsThread.Wait(2500);
            string OldSettings = JsonConvert.SerializeObject(Global.Settings);
            while (true)
            {
                SaveSettingsThread.Wait(2500);
                string NewSettings = JsonConvert.SerializeObject(Global.Settings);
                if (NewSettings != OldSettings)
                {
                    OldSettings = NewSettings;
                    File.WriteAllText(Global.SettingPath + "\\Server.json", JsonConvert.SerializeObject(Global.Settings.Server, Formatting.Indented));
                    File.WriteAllText(Global.SettingPath + "\\Bot.json", JsonConvert.SerializeObject(Global.Settings.Bot, Formatting.Indented));
                    File.WriteAllText(Global.SettingPath + "\\Serein.json", JsonConvert.SerializeObject(Global.Settings.Serein, Formatting.Indented));
                }
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
                        Global.Settings.Event = JsonConvert.DeserializeObject<Event>(File.ReadAllText(Global.SettingPath + "\\Event.json", Encoding.UTF8));
                    }
                }
                catch (Exception e)
                {
                    Logger.Out(999, "[Setting] Fail to update Event.json:", e.ToString());
                }
            }
        }
    }
}