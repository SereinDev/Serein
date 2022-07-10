using Newtonsoft.Json;
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
            SaveSettingsThread.Wait(2500);
            if (!Directory.Exists(Global.SettingPath))
            {
                Directory.CreateDirectory(Global.SettingPath);
            }
            StreamWriter IniStreamWriter = new StreamWriter(Global.SettingPath + "\\Matches.json", false, Encoding.UTF8);
            IniStreamWriter.Write(JsonConvert.SerializeObject(new Matches(), Formatting.Indented));
            IniStreamWriter.Close();
            IniStreamWriter.Dispose();
            while (true)
            {
                StreamWriter ServerStreamWriter, BotStreamWriter, SereinStreamWriter;
                SaveSettingsThread.Wait(2500);
                ServerStreamWriter = new StreamWriter(Global.SettingPath + "\\Server.json", false, Encoding.UTF8);
                ServerStreamWriter.Write(
                    JsonConvert.SerializeObject(Global.Settings.Server, Formatting.Indented)
                    );
                ServerStreamWriter.Close();
                ServerStreamWriter.Dispose();
                BotStreamWriter = new StreamWriter(Global.SettingPath + "\\Bot.json", false, Encoding.UTF8);
                BotStreamWriter.Write(
                    JsonConvert.SerializeObject(Global.Settings.Bot, Formatting.Indented)
                    );
                BotStreamWriter.Close();
                BotStreamWriter.Dispose();
                SereinStreamWriter = new StreamWriter(Global.SettingPath + "\\Serein.json", false, Encoding.UTF8);
                SereinStreamWriter.Write(
                    JsonConvert.SerializeObject(Global.Settings.Serein, Formatting.Indented)
                    );
                SereinStreamWriter.Close();
                SereinStreamWriter.Dispose();
            }
        }
        public static void ReadSettings()
        {
            if (!Directory.Exists(Global.SettingPath))
            {
                Directory.CreateDirectory(Global.SettingPath);
            }
            if (File.Exists(Global.SettingPath + "\\Server.json"))
            {
                Global.Settings.Server = JsonConvert.DeserializeObject<Server>(
                    File.ReadAllText(Global.SettingPath + "\\Server.json", Encoding.UTF8)
                    );
                if (Global.Settings.Server == null)
                {
                    Global.Settings.Server = new Server();
                }
            }
            if (File.Exists(Global.SettingPath + "\\Bot.json"))
            {
                Global.Settings.Bot = JsonConvert.DeserializeObject<Bot>(
                    File.ReadAllText(Global.SettingPath + "\\Bot.json", Encoding.UTF8)
                    );
                if (Global.Settings.Bot == null)
                {
                    Global.Settings.Bot = new Bot();
                }
            }
            if (File.Exists(Global.SettingPath + "\\Serein.json"))
            {
                Global.Settings.Serein = JsonConvert.DeserializeObject<Serein>(
                    File.ReadAllText(Global.SettingPath + "\\Serein.json", Encoding.UTF8)
                    );
                if (Global.Settings.Serein == null)
                {
                    Global.Settings.Serein = new Serein();
                }
            }
            if (File.Exists(Global.SettingPath + "\\Matches.json"))
            {
                Global.Settings.Matches = JsonConvert.DeserializeObject<Matches>(
                    File.ReadAllText(Global.SettingPath + "\\Matches.json", Encoding.UTF8)
                    );
                if (Global.Settings.Matches == null)
                {
                    Global.Settings.Matches = new Matches();
                    StreamWriter IniStreamWriter = new StreamWriter(Global.SettingPath + "\\Matches.json", false, Encoding.UTF8);
                    IniStreamWriter.Write(JsonConvert.SerializeObject(Global.Settings.Matches, Formatting.Indented));
                    IniStreamWriter.Close();
                    IniStreamWriter.Dispose();
                }
            }
            else
            {
                StreamWriter IniStreamWriter = new StreamWriter(File.Open(Global.SettingPath + "\\Matches.json", FileMode.OpenOrCreate), Encoding.UTF8);
                IniStreamWriter.Write(JsonConvert.SerializeObject(new Matches(), Formatting.Indented));
                IniStreamWriter.Close();
                IniStreamWriter.Dispose();
            }
        }
    }
}
