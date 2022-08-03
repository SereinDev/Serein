using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Serein.Server;

namespace Serein.Base
{
    internal class Console
    {
        private static object Lock = new object();
        public static void WriteLine(int Level, params object[] objects)
        {
            lock (Lock)
            {
                switch (Level)
                {
                    case 0:
                        System.Console.ForegroundColor = ConsoleColor.Cyan;
                        System.Console.Write("[Serein][Info]");
                        System.Console.ForegroundColor = ConsoleColor.White;
                        break;
                    case 1:
                        System.Console.ForegroundColor = ConsoleColor.Yellow;
                        System.Console.Write("[Serein][Warn]");
                        break;
                    case 2:
                        System.Console.ForegroundColor = ConsoleColor.Red;
                        System.Console.Write("[Serein][Error]");
                        break;
                    case 3:
                        System.Console.ForegroundColor = ConsoleColor.Magenta;
                        System.Console.Write("[Serein][Debug]");
                        break;
                    default:
                        System.Console.ForegroundColor = ConsoleColor.White;
                        break;
                }
                foreach (var obj in objects)
                {
                    if (obj != null)
                    {
                        System.Console.Write(" " + obj.ToString());
                    }
                }
                System.Console.Write("\r\n");
                System.Console.ForegroundColor = ConsoleColor.White;
            }
        }
        public static void Control()
        {
            System.Console.Title = "Serein " + Global.VERSION;
            WriteLine(0, "Welcome.");
            while (true)
            {
                string Line = System.Console.ReadLine().Trim();
                ProcessInput(Line);
            }
        }
        public static void ProcessInput(string Line)
        {
            if (!ServerManager.Status || Line.StartsWith("serein"))
            {
                Line = Regex.Replace(Line, @"^serein\s?", string.Empty).ToLower();
                switch (Line)
                {
                    case "exit":
                        if (ServerManager.Status)
                        {
                            WriteLine(1, "服务器未关闭");
                        }
                        else
                        {
                            Environment.Exit(0);
                        }
                        break;
                    case "start":
                        ServerManager.Start();
                        break;
                    case "stop":
                        ServerManager.Stop();
                        break;
                    case "kill":
                        ServerManager.Kill();
                        break;
                    case "connect":
                        Websocket.Connect();
                        break;
                    case "close":
                        Websocket.Close();
                        break;
                    case "reload":
                        try
                        {
                            Settings.Base.ReadSettings();
                            WriteLine(0, "设置加载成功");
                        }
                        catch (Exception e)
                        {
                            WriteLine(0, "设置加载失败:" + e.Message);
                        }
                        break;
                    default:
                        WriteLine(1, "未知的命令，请检查后重试");
                        break;
                }
            }
            else
            {
                ServerManager.InputCommand(Line);
            }
        }
    }
}
