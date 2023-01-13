using System.Text.RegularExpressions;

namespace Serein.Settings
{
    internal class Matches
    {
        public string Difficulty = "(PEACEFUL|EASY|NORMAL|HARD|DIFFICULT[^Y])";

        public string LevelName = @"Level\sName:\s(.+?)$";

        public string[] MuiltLines =
        {
            @"players\sonline:"
        };

        public Matches()
        {
            try
            {
                Regex.Match(string.Empty, Difficulty);
            }
            catch
            {
                Difficulty = "(PEACEFUL|EASY|NORMAL|DIFFICULT[^Y])";
            }
            try
            {
                Regex.Match(string.Empty, LevelName);
            }
            catch
            {
                LevelName = "Level Name: (.+?)$";
            }
        }
    }
}
