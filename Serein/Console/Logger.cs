using Serein.Items;
using Serein.Server;
using System;

namespace Serein
{
    internal static class Logger
    {
        public static int Type = 0;
        private static readonly object Lock = new object();

        public static void Out(LogType Type, params object[] objects)
        {
            string Line = string.Empty;
            foreach (var o in objects)
            {
                if (o != null) { Line += o.ToString() + " "; }
            }
            Line = Line.TrimEnd();
            switch (Type)
            {
                case LogType.Debug:
                    if (Global.Settings.Serein.Debug) { WriteLine(4, Line); }
                    break;
                case LogType.Info:
                case LogType.Plugin_Notice:
                case LogType.Server_Notice:
                    WriteLine(1, Line);
                    break;
                case LogType.Warn:
                case LogType.Plugin_Warn:
                    WriteLine(2, Line);
                    break;
                case LogType.Error:
                case LogType.Bot_Error:
                case LogType.Plugin_Error:
                    WriteLine(3, Line);
                    break;
                case LogType.Null:
                case LogType.Server_Output:
                case LogType.Bot_Output:
                    WriteLine(0, Line);
                    break;
                case LogType.Bot_Receive:
                    WriteLine(0, $"\x1b[92m[↓]\x1b[0m{Line}");
                    break;
                case LogType.Bot_Send:
                    WriteLine(0, $"\x1b[36m[↑]\x1b[0m{Line}");
                    break;
                case LogType.Plugin_Info:
                    WriteLine(0, $"\x1b[94m[插件]\x1b[0m{Line}");
                    break;
            }
        }

        /// <summary>
        /// 处理输出消息
        /// </summary>
        /// <param name="Level">输出等级</param>
        /// <param name="Line">输出行</param>
        private static void WriteLine(int Level, string Line)
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

        public static bool MsgBox(string Text, string Caption, int Buttons, int Icon)
        {
            switch (Icon)
            {
                case 48:
                    Out(LogType.Warn, Text);
                    break;
                case 16:
                    Out(LogType.Error, Text);
                    break;
            }
            return true;
        }
    }
}
