using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Serein
{
    internal class Settings_Matches
    {
        public string Version { get; set; } = @"(\d+\.\d+\.\d+\.\d+)";
        public string Difficulty { get; set; } = "(PEACEFUL|EASY|NORMAL|HARD|DIFFICULT[^Y])";
        public string LevelName { get; set; } = "Level Name: (.+?)$";
        public string Finished { get; set; } = "(Done|Started)";
        public string ipv4Port { get; set; } = @"ipv4.+?(\d+?)$";
        public Settings_Matches()
        {
            try
            {
                Regex.Match(string.Empty, Version);
            }
            catch { Version = @"(\d+\.\d+\.\d+\.\d+)"; }
            try
            {
                Regex.Match(string.Empty, Difficulty);
            }
            catch { Difficulty = "(PEACEFUL|EASY|NORMAL|DIFFICULT[^Y])"; }
            try
            {
                Regex.Match(string.Empty, LevelName);
            }
            catch { Version = "Level Name: (.+?)$"; }
            try
            {
                Regex.Match(string.Empty, Finished);
            }
            catch { Finished = "(Done|Started)"; }
            try
            {
                Regex.Match(string.Empty, ipv4Port);
            }
            catch { ipv4Port = @"ipv4.+?(\d+?)$"; }
            if (!Directory.Exists(Global.SettingPath))
            {
                Directory.CreateDirectory(Global.SettingPath);
            }
            StreamWriter IniStreamWriter = new StreamWriter(File.Open(Global.SettingPath + "\\Matches.json", FileMode.OpenOrCreate), Encoding.UTF8);
            IniStreamWriter.Write(JsonConvert.SerializeObject(Global.Settings_Matches, Formatting.Indented));
            IniStreamWriter.Close();
            IniStreamWriter.Dispose();
        }
    }
}
