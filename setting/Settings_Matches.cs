using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Serein
{
    internal class Settings_Matches
    {
        public string Version { get; set; } = @"(\d+\.\d+\.\d+\.\d+)";
        public string Difficulty { get; set; } = "(PEACEFUL|EASY|NORMAL|DIFFICULT[^Y])";
        public string LevelName { get; set; } = "Level Name: (.+?)$";
        public string Finished { get; set; } = "(Done|Started)";
        public Settings_Matches()
        {
            if (!Directory.Exists(Global.SettingPath))
            {
                _ = Directory.CreateDirectory(Global.SettingPath);
            }
            if (File.Exists(Global.SettingPath + "\\Matches.ini"))
            {
                FileStream IniFile = new(Global.SettingPath + "\\Matches.ini", FileMode.Open);
                StreamReader Reader = new(IniFile, Encoding.UTF8);
                string Line, Value, Type;
                while ((Line = Reader.ReadLine()) != null)
                {
                    if (Line.Contains("="))
                    {
                        Line = Line.Trim();
                        Value = Line[(Line.IndexOf("=") + 1)..].Trim();
                        Type = Line[..Line.IndexOf("=")].Trim();
                        try
                        {
                            _ = Regex.Match(string.Empty, Value);
                        }
                        catch
                        {
                            continue;
                        }
                        if (Type == "Verison")
                        {
                            Version = Value;
                        }
                        else if (Type == "Difficulty")
                        {
                            Difficulty = Value;
                        }
                        else if (Type == "LevelName")
                        {
                            LevelName = Value;
                        }
                        else if (Type == "Finished")
                        {
                            Finished = Value;
                        }
                    }
                }
                IniFile.Close();
                IniFile.Dispose();
                Reader.Close();
                Reader.Dispose();
            }
            else
            {
                StreamWriter IniStreamWriter = new(Global.SettingPath + "\\Matches.ini", false, Encoding.UTF8);
                IniStreamWriter.Write("# 在此处自定义捕获信息的正则表达式\n" +
                $"Version={Version}\n" +
                $"Difficulty={Difficulty}\n" +
                $"LevelName={LevelName}"
                    );
                IniStreamWriter.Close();
                IniStreamWriter.Dispose();
            }
        }
    }
}
