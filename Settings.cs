using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Threading;

namespace Serein
{
    class Settings
    {
        public static Thread SaveSettingsThread = new Thread(SaveSettings);
        public static void StartSaveSettings()
        {
            SaveSettingsThread.IsBackground = true;
            SaveSettingsThread.Start();
        }
        public static void SaveSettings()
        {
            while (true)
            {
                StreamWriter ServerStreamWriter, BotStreamWriter, SereinStreamWriter;
                SaveSettingsThread.Join(2500);
                if (!Directory.Exists(Global.SettingPath))
                {
                    Directory.CreateDirectory(Global.SettingPath);
                }
                ServerStreamWriter = new StreamWriter(Global.SettingPath + "\\Server.json", false, Encoding.UTF8);
                ServerStreamWriter.Write(
                    JsonConvert.SerializeObject(Global.Settings_Server, Formatting.Indented)
                    );
                ServerStreamWriter.Close();
                ServerStreamWriter.Dispose();
                BotStreamWriter = new StreamWriter(Global.SettingPath + "\\Bot.json", false, Encoding.UTF8);
                BotStreamWriter.Write(
                    JsonConvert.SerializeObject(Global.Settings_Bot, Formatting.Indented)
                    );
                BotStreamWriter.Close();
                BotStreamWriter.Dispose();
                SereinStreamWriter = new StreamWriter(Global.SettingPath + "\\Serein.json", false, Encoding.UTF8);
                SereinStreamWriter.Write(
                    JsonConvert.SerializeObject(Global.Settings_Serein, Formatting.Indented)
                    );
                SereinStreamWriter.Close();
                SereinStreamWriter.Dispose();
            }
        }
        public static void ReadSettings()
        {
            if (File.Exists(Global.SettingPath + "\\Server.json"))
            {
                Global.Settings_Server = JsonConvert.DeserializeObject<Settings_Server>(
                    File.ReadAllText(Global.SettingPath + "\\Server.json", Encoding.UTF8)
                    );
                if (Global.Settings_Server == null)
                {
                    Global.Settings_Server = new Settings_Server();
                }
            }
            if (File.Exists(Global.SettingPath + "\\Bot.json"))
            {
                Global.Settings_Bot = JsonConvert.DeserializeObject<Settings_Bot>(
                    File.ReadAllText(Global.SettingPath + "\\Bot.json", Encoding.UTF8)
                    );
                if (Global.Settings_Bot == null)
                {
                    Global.Settings_Bot = new Settings_Bot();
                }
            }
            if (File.Exists(Global.SettingPath + "\\Serein.json"))
            {
                Global.Settings_Serein = JsonConvert.DeserializeObject<Settings_Serein>(
                    File.ReadAllText(Global.SettingPath + "\\Serein.json", Encoding.UTF8)
                    );
                if (Global.Settings_Serein == null)
                {
                    Global.Settings_Serein = new Settings_Serein();
                }
            }
        }
    }
}
