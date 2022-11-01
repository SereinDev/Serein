using Serein.Base;
using Serein.Server;
using System;
using System.Text.RegularExpressions;


namespace Serein.Console
{
    internal class Input
    {
        /// <summary>
        /// 处理输入消息
        /// </summary>
        /// <param name="Line">输入行</param>
        public static void Process(string Line)
        {
            if (!ServerManager.Status || Line.StartsWith("serein"))
            {
                Line = Regex.Replace(Line, @"^serein\s?", string.Empty).ToLower();
                switch (Line)
                {
                    case "exit":
                        if (ServerManager.Status)
                            Logger.Out(Items.LogType.Warn, "服务器未关闭");
                        else
                            Environment.Exit(0);
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
                            Logger.Out(Items.LogType.Info, "重新加载成功");
                        }
                        catch (Exception e)
                        {
                            Logger.Out(Items.LogType.Error, "加载失败:" + e.Message);
                        }
                        break;
                    case "help":
                        Logger.Out(
                            Items.LogType.Info,
                            "\r\n" +
                            "\x1b[0mstart\t\t启动服务器\r\n" +
                            "\x1b[0mstop\t\t关闭服务器\r\n" +
                            "\x1b[0mkill\t\t强制结束服务器\r\n" +
                            "\x1b[0mconnect\t\t连接机器人\r\n" +
                            "\x1b[0mclose\t\t断开机器人连接\r\n" +
                            "\x1b[0mreload\t\t重新加载所有文件\r\n" +
                            "\x1b[0mexit\t\t退出Serein\r\n" +
                            "\x1b[0mhelp\t\t显示此页面\r\n" +
                            "\x1b[0mversion\t\t查看版本信息\r\n");
                        break;
                    case "version":
                        Logger.Out(Items.LogType.Info,
                            $"Serein {Global.VERSION}\r\n" +
                            "\x1b[36m ____ \r\n" +
                            "\x1b[36m/\\  _`\\                        __            \r\n" +
                            "\x1b[36m\\ \\,\\L\\_\\     __   _ __    __ /\\_\\    ___    \r\n" +
                            "\x1b[36m \\/_\\__ \\   /'__`\\/\\`'__\\/'__`\\/\\ \\ /' _ `\\  \r\n" +
                            "\x1b[36m   /\\ \\L\\ \\/\\  __/\\ \\ \\//\\  __/\\ \\ \\/\\ \\/\\ \\ \r\n" +
                            "\x1b[36m   \\ `\\____\\ \\____\\\\ \\_\\\\ \\____\\\\ \\_\\ \\_\\ \\_\\\r\n" +
                            "\x1b[36m    \\/_____/\\/____/ \\/_/ \\/____/ \\/_/\\/_/\\/_/\r\n" +
                            "\x1b[36m    \r\n" +
                            "\x1b[0m" +
                            $"编译类型：{Global.BuildInfo.Type}\r\n" +
                            $"编译时间：{Global.BuildInfo.Time}\r\n" +
                            $"编译路径：{Global.BuildInfo.Dir}\r\n" +
                            $"系统类型：{Global.BuildInfo.OS}\r\n" +
                            $"详细信息：{Global.BuildInfo.Detail.Replace("\r", string.Empty)}\r\n" +
                            "=================================================\r\n" +
                            "    \x1b[4m\x1b[36mhttps://github.com/Zaitonn/Serein\x1b[0m\r\n" +
                            " Copyright © 2022 Zaitonn. All Rights Reserved.\r\n");
                        break;
                    default:
                        Logger.Out(Items.LogType.Warn, "未知的命令，请检查后重试或输入\"help\"获取更多信息");
                        break;
                }
            }
            else
                ServerManager.InputCommand(Line);
        }
    }
}
