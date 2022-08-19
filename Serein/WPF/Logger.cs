using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serein
{
    internal static class Logger
    {
        public static int Type = 2;
        public static void Out(int Type, params object[] objects)
        {

        }

        public static bool MsgBox(string Text, string Caption, int Buttons, int Icon)
        {
            return true;
        }
    }
}
