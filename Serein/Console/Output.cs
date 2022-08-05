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
                        System.Console.Write("\x1b[96m");
                        if (ServerManager.Status)
                        {
                            System.Console.Write("[Serein]");
                        }
                        System.Console.Write("[Info]\x1b[0m");
                        break;
                    case 2:
                        System.Console.Write("\x1b[93m");
                        if (ServerManager.Status)
                        {
                            System.Console.Write("[Serein]");
                        }
                        System.Console.Write("[Warn]");
                        break;
                    case 3:
                        System.Console.Write("\x1b[91m");
                        if (ServerManager.Status)
                        {
                            System.Console.Write("[Serein]");
                        }
                        System.Console.Write("[Error]");
                        break;
                    case 4:
                        System.Console.Write("\x1b[95m");
                        if (ServerManager.Status)
                        {
                            System.Console.Write("[Serein]");
                        }
                        System.Console.Write("[Debug]");
                        break;
                    default:
                        System.Console.Write("\x1b[97m");
                        break;
                }
                System.Console.WriteLine(Line + "\x1b[97m");
                System.Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
