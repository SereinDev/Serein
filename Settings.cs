using System.IO;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
namespace Serein
{
    public class Settings_Server
    {
        public string Path { get; set; } = "";
        public bool EnableRestart { get; set; } = false;
        public bool EnableOutputCommand { get; set; } = true;
        public bool EnableLog { get; set; } = false;
        public int OutputStyle { get; set; } = 0;
    }
    public class Settings_Bot
    {
        public bool EnableLog { get; set; } = false;
        public bool GivePermissionToAllAdmin { get; set; } = false;
        public long[] GroupList { get; set; } = { };
        public long[] PermissionList { get; set; } = { };
        public int Port { get; set; } = 6700;
    }
    public class Settings_Serein
    {
        public int UpdateInfoType { get; set; } = 0;
        public bool EnableGetAnnouncement { get; set; } = true;
    }
    partial class Settings
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
                ServerStreamWriter = new StreamWriter(Global.SettingPath + "\\server.json", false, Encoding.UTF8);
                ServerStreamWriter.Write(
                    JsonConvert.SerializeObject(Global.Settings_server, Formatting.Indented)
                    );
                ServerStreamWriter.Close();
                BotStreamWriter = new StreamWriter(Global.SettingPath + "\\bot.json", false, Encoding.UTF8);
                BotStreamWriter.Write(
                    JsonConvert.SerializeObject(Global.Settings_bot, Formatting.Indented)
                    );
                BotStreamWriter.Close();
                SereinStreamWriter = new StreamWriter(Global.SettingPath + "\\serein.json", false, Encoding.UTF8);
                SereinStreamWriter.Write(
                    JsonConvert.SerializeObject(Global.Settings_serein, Formatting.Indented)
                    );
                SereinStreamWriter.Close();

            }
        }
        public static void ReadSettings()
        {
            if (File.Exists(Global.SettingPath + "\\server.json"))
            {
                Global.Settings_server = JsonConvert.DeserializeObject<Settings_Server>(
                    File.ReadAllText(Global.SettingPath + "\\server.json", Encoding.UTF8)
                    );
                if (Global.Settings_server == null)
                {
                    Global.Settings_server = new Settings_Server();
                }
            }
            if (File.Exists(Global.SettingPath + "\\bot.json"))
            {
                Global.Settings_bot = JsonConvert.DeserializeObject<Settings_Bot>(
                    File.ReadAllText(Global.SettingPath + "\\bot.json", Encoding.UTF8)
                    );
                if (Global.Settings_bot == null)
                {
                    Global.Settings_bot = new Settings_Bot();
                }
            }
            if (File.Exists(Global.SettingPath + "\\serein.json"))
            {
                Global.Settings_serein = JsonConvert.DeserializeObject<Settings_Serein>(
                    File.ReadAllText(Global.SettingPath + "\\serein.json", Encoding.UTF8)
                    );
                if (Global.Settings_serein == null)
                {
                    Global.Settings_serein = new Settings_Serein();
                }
            }
        }

    }
}
