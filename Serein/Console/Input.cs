using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Serein.Base;
using Serein.Server;

namespace Serein.Console
{
    internal class Input
    {
        public static void Process(string Line)
        {
            if (!ServerManager.Status || Line.StartsWith("serein"))
            {
                Line = Regex.Replace(Line, @"^serein\s?", string.Empty).ToLower();
                switch (Line)
                {
                    case "exit":
                        if (ServerManager.Status)
                        {
                            Output.Logger(2, "服务器未关闭");
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
                            Base.Load();
                            Output.Logger(1, "重新加载成功");
                        }
                        catch (Exception e)
                        {
                            Output.Logger(3, "加载失败:" + e.Message);
                        }
                        break;
                    case "help":
                        Output.Logger(
                            0,
                            "\x1b[96m ____ \r\n" +
                            "\x1b[96m/\\  _`\\                        __            \r\n" +
                            "\x1b[96m\\ \\,\\L\\_\\     __   _ __    __ /\\_\\    ___    \r\n" +
                            "\x1b[96m \\/_\\__ \\   /'__`\\/\\`'__\\/'__`\\/\\ \\ /' _ `\\  \r\n" +
                            "\x1b[96m   /\\ \\L\\ \\/\\  __/\\ \\ \\//\\  __/\\ \\ \\/\\ \\/\\ \\ \r\n" +
                            "\x1b[96m   \\ `\\____\\ \\____\\\\ \\_\\\\ \\____\\\\ \\_\\ \\_\\ \\_\\\r\n" +
                            "\x1b[96m    \\/_____/\\/____/ \\/_/ \\/____/ \\/_/\\/_/\\/_/\r\n" +
                            "\x1b[96m    \r\n\x1b[96m    https://github.com/Zaitonn/Serein\r\n" +
                            "\x1b[96m Copyright © 2022 Zaitonn. All Rights Reserved.\r\n" +
                            "\x1b[0m=================================================\r\n" +
                            "start\t\t启动服务器\r\n" +
                            "stop\t\t关闭服务器\r\n" +
                            "kill\t\t强制结束服务器\r\n" +
                            "connect\t\t连接机器人\r\n" +
                            "close\t\t断开机器人连接\r\n" +
                            "reload\t\t重新加载所有文件\r\n" +
                            "exit\t\t退出Serein\r\n" +
                            "help\t\t显示此页面\r\n" +
                            "=================================================");
                        break;
                    default:
                        Output.Logger(2, "未知的命令，请检查后重试");
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
