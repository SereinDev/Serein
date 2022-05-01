using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Serein
{
    class Command
    {
        public static void command(string command, Match OutputMatch)
        {

        }

        public static void command(string command,Match MsgMatch,int user_id, int group_id=-1)
        {
            int type = GetType(command);
            if (type==-1)
            {
                return;
            }
            string value = GetValue(command, MsgMatch);
            if (type == 1)
            {
            }
            else if (type == 2 && Server.Status)
            {
                Server.InputCommand(value);
            }
            else if (type == 3)
            {
                Websocket.Send(false, value, Regex.Match(command, @"(\d+)\|").Groups[1].Value);
            }
            else if (type == 4)
            {
                Websocket.Send(true, value, Regex.Match(command, @"(\d+)\|").Groups[1].Value);
            }
            else if (type == 5)
            {
                Websocket.Send(false, value, group_id.ToString());
            }
            else if (type == 6)
            {
                Websocket.Send(true, value, user_id.ToString());
            }
            MessageBox.Show(command + "\n" + value + "\n" + type.ToString());
        }

        public static void test(RegexItem item)
        {
            MessageBox.Show($"{item.Regex}\n{item.Command}");
        }
        public static int GetType(string command)
        {
            if (!command.Contains("|"))
            {
                return -1;
            }
            if (!Regex.IsMatch(command, @"^.+?\|.+$", RegexOptions.IgnoreCase))
            {
                return -1;
            }
            if (Regex.IsMatch(command,@"^cmd\|",RegexOptions.IgnoreCase))
            {
                return 1;
            }
            else if(Regex.IsMatch(command, @"^s\|", RegexOptions.IgnoreCase)|| Regex.IsMatch(command, @"^server\|", RegexOptions.IgnoreCase))
            {
                return 2;
            }
            else if (Regex.IsMatch(command, @"^g:\d+\|", RegexOptions.IgnoreCase) || Regex.IsMatch(command, @"^group:\d+\|", RegexOptions.IgnoreCase))
            {
                return 3;
            }
            else if (Regex.IsMatch(command, @"^p:\d+\|", RegexOptions.IgnoreCase) || Regex.IsMatch(command, @"^private:\d+\|", RegexOptions.IgnoreCase))
            {
                return 4;
            }
            else if(Regex.IsMatch(command, @"^g\|", RegexOptions.IgnoreCase) || Regex.IsMatch(command, @"^group\|", RegexOptions.IgnoreCase))
            {
                return 5;
            }
            else if (Regex.IsMatch(command, @"^p\|", RegexOptions.IgnoreCase) || Regex.IsMatch(command, @"^private\|", RegexOptions.IgnoreCase))
            {
                return 6;
            }
            else
            {
                return -1;
            }
        }
        public static string  GetValue(string command, Match MsgMatch)
        {
            int index = command.IndexOf('|');
            string value = command.Substring(index+1);
            for (int i=1; i < MsgMatch.Groups.Count; i++)
            {
                value = value.Replace($"${i}", MsgMatch.Groups[i].Value);
            }
            return value;
        }
    }
}
