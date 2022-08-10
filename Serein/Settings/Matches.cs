using System.Text.RegularExpressions;

namespace Serein.Settings
{
    internal class Matches
    {
        public string Version { get; set; } = @"(\d+\.\d+\.\d+\.\d+)";
        public string Difficulty { get; set; } = "(PEACEFUL|EASY|NORMAL|HARD|DIFFICULT[^Y])";
        public string LevelName { get; set; } = "Level Name: (.+?)$";
        public string Finished { get; set; } = "(Done|Started)";
        public string[] MuiltLines { get; set; } =
        {
            @"players\sonline:"
        };

        public Matches()
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
        }
    }
}
