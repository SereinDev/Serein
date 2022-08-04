using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serein.Server;

namespace Serein.Console
{
    internal class Output
    {
        private static object Lock = new object();
        public static void Logger(int Level, string Line)
        {
            if (Line == "#clear") { return; }
            lock (Lock)
            {
                switch (Level)
                {
                    case 1:
                        System.Console.ForegroundColor = ConsoleColor.Cyan;
                        if (ServerManager.Status) { System.Console.Write("[Serein]"); }
                        System.Console.Write("[Info]");
                        System.Console.ForegroundColor = ConsoleColor.White;
                        break;
                    case 2:
                        System.Console.ForegroundColor = ConsoleColor.Yellow;
                        if (ServerManager.Status) { System.Console.Write("[Serein]"); }
                        System.Console.Write("[Warn]");
                        break;
                    case 3:
                        System.Console.ForegroundColor = ConsoleColor.Red;
                        if (ServerManager.Status) { System.Console.Write("[Serein]"); }
                        System.Console.Write("[Error]");
                        break;
                    case 4:
                        System.Console.ForegroundColor = ConsoleColor.Magenta;
                        if (ServerManager.Status) { System.Console.Write("[Serein]"); }
                        System.Console.Write("[Debug]");
                        break;
                    default:
                        System.Console.ForegroundColor = ConsoleColor.White;
                        break;
                }
                System.Console.WriteLine(Line);
                System.Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
