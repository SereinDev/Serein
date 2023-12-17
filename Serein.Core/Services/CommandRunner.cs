using System.Text.RegularExpressions;
using RegexMatch = System.Text.RegularExpressions.Match;

using Serein.Core.Models.Commands;

namespace Serein.Core.Services;

public class CommandRunner
{
    public void Run(CommandOrigin commandOrigin, string command, RegexMatch match) { }
}
