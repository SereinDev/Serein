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
