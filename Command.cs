using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Serein
{
    class Command
    {
        public static void command(string command)
        {

        }

        public static void command(string command,MatchCollection MsgMatchCollection,int user_id, int group_id=-1)
        {

        }

        public static void test(RegexItem item)
        {
            MessageBox.Show($"{item.Regex}\n{item.Command}");
        }
    }
}
