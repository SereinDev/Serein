using Serein.Extensions;
using System.Linq;

namespace Serein.Settings
{
    internal class Matches
    {
        private const string _Difficulty = "(PEACEFUL|EASY|NORMAL|HARD|DIFFICULT[^Y])";

        public string Difficulty = "(PEACEFUL|EASY|NORMAL|HARD|DIFFICULT[^Y])";

        private const string _LevelName = @"Level\sName:\s(.+?)$";

        public string LevelName = @"Level\sName:\s(.+?)$";

        public string[] MuiltLines =
        {
            @"players\sonline:"
        };

        public Matches()
        {
            Difficulty = Difficulty.TestRegex() ? Difficulty : _Difficulty;
            LevelName = LevelName.TestRegex() ? LevelName : _LevelName;
        }
    }
}
