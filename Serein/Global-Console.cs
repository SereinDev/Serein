using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Serein.Console;
using System.Threading.Tasks;

namespace Serein
{
    partial class Global
    {
        public static bool Console = true;
        public static void Debug(object o)
        {
        }
        public static void Logger(int Type, params object[] objects)
        {
            string Line = string.Empty;
            foreach (var o in objects)
            {
                if (o != null) { Line += o.ToString() + " "; }
            }
            Line = Line.TrimEnd();
            switch (Type)
            {
                case 999:
                    if (Settings.Serein.Debug) { Output.Logger(4, Line); }
                    break;
                case 1:
                    Output.Logger(1, Line);
                    break;
                case 2:
                    Output.Logger(2, Line);
                    break;
                case 3:
                    Output.Logger(3, Line);
                    break;
                case 10:
                    Output.Logger(0, Line);
                    break;
                case 11:
                    Output.Logger(1, Line);
                    break;
                case 20:
                    Output.Logger(0, Line);
                    break;
                case 21:
                    Output.Logger(1, Line);
                    break;
                case 22:
                    Output.Logger(0, $"\x1b[92m[↓]\x1b[0m{Line}");
                    break;
                case 23:
                    Output.Logger(0, $"\x1b[36m[↑]\x1b[0m{Line}");
                    break;
                case 24:
                    Output.Logger(3, Line);
                    break;
                case 30:
                    Output.Logger(0, Line);
                    break;
                case 31:
                    Output.Logger(1, Line);
                    break;
                case 32:
                    Output.Logger(3, Line);
                    break;
                case 33:
                    Output.Logger(0, $"\x1b[94m[插件]\x1b[0m{Line}");
                    break;
            }
        }
    }
}
