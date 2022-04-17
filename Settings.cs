using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
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
        public string Path { get; set; } = "";
        public bool EnableLog { get; set; } = false;
        public bool GivePermissionToAllAdmin { get; set; } = false;
        public long[] GroupList { get; set; } = { };
        public long[] PermissionList{ get; set; } = { };
        public int ListenPort { get; set; } = 8000;
        public int SendPort { get; set; } = 5700;
    }
    public class Settings_Serein
    {
        public bool EnableGetUpdate { get; set; } = true;
        public bool EnableGetAnnouncement{ get; set; } = true;
    }
    class Settings
    {
        public static Thread thread = new Thread(SaveSettings);
        public static void StartSaveSettings()
        {
            thread.IsBackground = true;
            thread.Start();
        }
        public static void SaveSettings()
        {
            while (true)
            {
                StreamWriter ServerStreamWriter, BotStreamWriter, SereinStreamWriter;
                thread.Join(2500);
                if (!Directory.Exists(Global.SETTINGPATH))
                {
                    Directory.CreateDirectory(Global.SETTINGPATH);
                }
                ServerStreamWriter = new StreamWriter(Global.SETTINGPATH + "\\server.json");
                ServerStreamWriter.WriteAsync(
                    JsonConvert.SerializeObject(Global.Settings_server, Formatting.Indented)
                    );
                ServerStreamWriter.Close();
                BotStreamWriter = new StreamWriter(Global.SETTINGPATH + "\\bot.json");
                BotStreamWriter.WriteAsync(
                    JsonConvert.SerializeObject(Global.Settings_bot, Formatting.Indented)
                    );
                BotStreamWriter.Close();
                SereinStreamWriter = new StreamWriter(Global.SETTINGPATH + "\\serein.json");
                SereinStreamWriter.WriteAsync(
                    JsonConvert.SerializeObject(Global.Settings_serein, Formatting.Indented)
                    );
                SereinStreamWriter.Close();

            }
        }
        public static void ReadSettings()
        {
            if (File.Exists(Global.SETTINGPATH + "\\server.json"))
            {
                Global.Settings_server = JsonConvert.DeserializeObject<Settings_Server>(
                    File.ReadAllText(Global.SETTINGPATH + "\\server.json")
                    );
                if (Global.Settings_server == null)
                {
                    Global.Settings_server = new Settings_Server();
                }
            }
            if (File.Exists(Global.SETTINGPATH + "\\bot.json"))
            {
                Global.Settings_bot = JsonConvert.DeserializeObject<Settings_Bot>(
                    File.ReadAllText(Global.SETTINGPATH + "\\bot.json")
                    );
                if (Global.Settings_bot == null)
                {
                    Global.Settings_bot = new Settings_Bot();
                }
            }
            if (File.Exists(Global.SETTINGPATH + "\\serein.json"))
            {
                Global.Settings_serein=JsonConvert.DeserializeObject<Settings_Serein>(
                    File.ReadAllText(Global.SETTINGPATH + "\\serein.json")
                    );
                if (Global.Settings_serein == null)
                {
                    Global.Settings_serein = new Settings_Serein();
                }
            }
        }
        
    }
}
