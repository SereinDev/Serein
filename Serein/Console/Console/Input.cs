using Serein.Base;
using Serein.JSPlugin;
using Serein.Server;
using System;
using System.Text.RegularExpressions;


namespace Serein.Console
{
    internal static class Input
    {
        /// <summary>
        /// 处理输入消息
        /// </summary>
        /// <param name="line">输入行</param>
        public static void Process(string line)
        {
            if (!ServerManager.Status || line.StartsWith("serein"))
            {
                line = Regex.Replace(line, @"^serein\s?", string.Empty).ToLowerInvariant();
                switch (line)
                {
                    case "exit":
                        if (ServerManager.Status)
                        {
                            Logger.Out(Items.LogType.Warn, "服务器未关闭");
                        }
                        else
                        {
                            JSFunc.Trigger(Items.EventType.SereinClose);
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
                            IO.ReadAll(true);
                            JSPluginManager.Reload();
                            Logger.Out(Items.LogType.Info, "重新加载成功");
                        }
                        catch (Exception e)
                        {
                            Logger.Out(Items.LogType.Error, "加载失败:" + e.Message);
                        }
                        break;
                    case "clear":
                        System.Console.Clear();
                        break;
                    case "help":
                        Logger.Out(
                            Items.LogType.Info,
                            "start  \t启动服务器\r\n" +
                            "stop   \t关闭服务器\r\n" +
                            "kill   \t强制结束服务器\r\n" +
                            "connect\t连接机器人\r\n" +
                            "close  \t断开机器人连接\r\n" +
                            "clear  \t清屏\r\n" +
                            "reload \t重新加载所有文件\r\n" +
                            "help   \t显示此页面\r\n" +
                            "version\t查看版本信息\r\n" +
                            "exit   \t退出Serein\r\n"
                            );
                        break;
                    case "version":
                        Logger.Out(Items.LogType.Info,
                            $"Serein {Global.VERSION}\r\n" +
                            $"编译类型：{Global.BuildInfo.Type}\r\n" +
                            $"编译时间：{Global.BuildInfo.Time}\r\n" +
                            $"编译路径：{Global.BuildInfo.Dir}\r\n" +
                            $"系统类型：{Global.BuildInfo.OS}\r\n" +
                            $"详细信息：{Global.BuildInfo.Detail.Replace("\r", string.Empty)}\r\n" +
                            "=========================================\r\n" +
                            "GitHub Repo: \x1b[4m\x1b[36mhttps://github.com/Zaitonn/Serein\x1b[0m\r\n" +
                            "Copyright © 2022 Zaitonn. All Rights Reserved.\r\n");
                        break;
                    default:
                        Logger.Out(Items.LogType.Warn, "未知的命令，请检查后重试或输入\"help\"获取更多信息");
                        break;
                }
            }
            else
            {
                ServerManager.InputCommand(line);
            }
        }
    }
}
