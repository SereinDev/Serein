using System.Threading.Tasks;
using Serein.Base;
using Serein.Core.Generic;
using Serein.Extensions;
using Serein.Core.JSPlugin;
using Serein.Core.Server;
using System;


namespace Serein.Utils.Console
{
    internal static class Input
    {
        private static bool IsLoading;

        private const string HelpMenu =
@"Serein 帮助菜单：

  s|server <operation> 服务器
    operation:
      i|info    - 详细信息
      s         - 切换运行状态（运行时=stop，否则=start）
      start     - 启动
      stop      - 关闭
      k|kill    - 强制结束（需要重复输入以确认）

  ws|websocket <operation> WS客户端
    operation:
      i|info                - 详细信息
      o|open|connect        - 连接
      c|close|disconnect    - 断开

  e|execute <command> 执行指定Serein命令
    command: Serein命令

  sysinfo       - 系统信息
  clear         - 清屏
  wp|welpage    - 显示欢迎页
  r|reload      - 重新加载设置文件、数据文件和插件
  v|version     - 查看版本信息
  ?|h|help      - 显示此帮助页面
  exit          - 退出

Tip: 
  · 其中“<参数>”为必选参数，“|”表示该命令的别称
  · 以上命令对大小写均不敏感
  · 当服务器运行时，输入的内容将被作为命令发送至服务器；若要执行Serein命令请在原有命令前面加上“serein ”，如“serein help”";

        /// <summary>
        /// 处理输入消息
        /// </summary>
        /// <param name="line">输入行</param>
        public static void Process(string? line)
        {
            if (line is null)
            {
                return;
            }
            if (!ServerManager.Status || line.StartsWith("serein"))
            {
                string[] args = System.Text.RegularExpressions.Regex.Replace(line, @"^serein\s?", string.Empty).ToLowerInvariant().Split(' ');
                switch (args[0])
                {
                    case "exit":
                        if (ServerManager.Status)
                        {
                            Logger.Output(LogType.Warn, "服务器未关闭");
                        }
                        else
                        {
                            JSFunc.Trigger(EventType.SereinClose);
                            Environment.Exit(0);
                        }
                        break;

                    case "s":
                    case "server":
                        if (args.Length == 2)
                        {
                            switch (args[1])
                            {
                                case "s":
                                    if (ServerManager.Status)
                                    {
                                        ServerManager.Stop();
                                    }
                                    else
                                    {
                                        ServerManager.Start();
                                    }
                                    break;
                                case "start":
                                    ServerManager.Start();
                                    break;
                                case "stop":
                                    ServerManager.Stop();
                                    break;
                                case "k":
                                case "kill":
                                    ServerManager.Kill();
                                    break;
                                case "i":
                                case "info":
                                    Logger.Output(LogType.Info,
                                        $"服务器状态      {(ServerManager.Status ? "已启动" : "未启动")}\n" +
                                        $"版本            {(ServerManager.Status && ServerManager.Motd != null && !string.IsNullOrEmpty(ServerManager.Motd.Version) ? ServerManager.Motd.Version : "-")}\n" +
                                        $"在线人数        {(ServerManager.Status && ServerManager.Motd != null ? $"{ServerManager.Motd.OnlinePlayer}/{ServerManager.Motd.MaxPlayer}" : "-")}\n" +
                                        $"运行时长        {(ServerManager.Status ? ServerManager.Time : "-")}\n" +
                                        $"服务器CPU占用   {(ServerManager.Status ? ServerManager.CPUUsage.ToString("N1") + "%" : "-")}"
                                        );
                                    break;
                                default:
                                    Logger.Output(LogType.Warn, "错误的参数：<operation>");
                                    break;
                            }
                        }
                        else
                        {
                            Logger.Output(LogType.Warn, "错误的参数：<operation>");
                        }
                        break;

                    case "ws":
                    case "websocket":
                        if (args.Length == 2)
                        {
                            switch (args[1])
                            {
                                case "o":
                                case "open":
                                case "connect":
                                    Websocket.Open();
                                    break;
                                case "c":
                                case "close":
                                case "disconnect":
                                    Websocket.Close();
                                    break;
                                case "i":
                                case "info":
                                    Logger.Output(LogType.Info,
                                        $"状态       {(Websocket.Status ? "已连接" : "未连接")}\n" +
                                        $"账号       {Matcher.SelfId}\n" +
                                        $"接收消息   {Matcher.MessageReceived}\n" +
                                        $"发送消息   {Matcher.MessageSent}\n" +
                                        $"连接时间   {(Websocket.Status ? (DateTime.Now - Websocket.StartTime).ToCustomString() : "-")}"
                                        );
                                    break;
                                default:
                                    Logger.Output(LogType.Warn, "错误的参数：<operation>");
                                    break;
                            }
                        }
                        else
                        {
                            Logger.Output(LogType.Warn, "错误的参数：<operation>");
                        }
                        break;

                    case "r":
                    case "reload":
                        if (IsLoading)
                        {
                            break;
                        }
                        Task.Run(() =>
                        {
                            IsLoading = true;
                            try
                            {
                                IO.ReadAll();
                                JSPluginManager.Reload();
                                Logger.Output(LogType.Info, "重新加载成功");
                            }
                            catch (Exception e)
                            {
                                Logger.Output(LogType.Error, "加载失败:" + e.Message);
                            }
                            IsLoading = false;
                        });
                        break;

                    case "clear":
                        System.Console.Clear();
                        break;

                    case "?":
                    case "？":
                    case "h":
                    case "help":
                        Logger.Output(LogType.Info, HelpMenu);
                        break;

                    case "v":
                    case "version":
                        Logger.Output(LogType.Info,
                            $"Serein {Global.VERSION}\n" +
                            $"编译类型   {Global.BuildInfo.Type}\n" +
                            $"编译时间   {Global.BuildInfo.Time}\n" +
                            $"详细信息   {Global.BuildInfo.Detail.Replace("\r", string.Empty)}\n" +
                            "\n" +
                            "交流群: https://jq.qq.com/?_wv=1027&k=XNZqPSPv\n" +
                            "官网: https://serein.cc\n" +
                            "GitHub Repo: https://github.com/Zaitonn/Serein\n" +
                            "Copyright © 2022 Zaitonn. All Rights Reserved.");
                        if (Global.Settings.Serein.AutoUpdate &&
                            Update.LastVersion != null &&
                            Update.IsReadyToUpdate &&
                            Environment.OSVersion.Platform == PlatformID.Win32NT)
                        {
                            Logger.Output(LogType.Info, $"新版本（{Update.LastVersion}）已下载完毕，重启即可更新");
                        }
                        break;

                    case "sysinfo":
                        Logger.Output(LogType.Info,
                            $"系统   {SystemInfo.OS}\n" +
                            $"内存   {SystemInfo.UsedRAM}/{SystemInfo.TotalRAM}MB （{SystemInfo.RAMUsage:N1}%）\n" +
#if UNIX
                            $"CPU    {SystemInfo.CPUName}\n" +
#else
                            $"CPU    {SystemInfo.CPUName} （{SystemInfo.CPUUsage:N1}%）\n" +
#endif
                            $"网速\n" +
                            $"  - 上传   {SystemInfo.UploadSpeed}\n" +
                            $"  - 下载   {SystemInfo.DownloadSpeed}"
                            );
                        break;

                    case "wp":
                    case "welpage":
                        Runtime.ShowWelcomePage();
                        break;

                    case "e":
                    case "execute":
                        string command = System.Text.RegularExpressions.Regex.Replace(line, @"^(e|execute)\s?", string.Empty);
                        CommandType type = Command.GetType(command);

                        if (type != CommandType.Invalid)
                        {
                            Logger.Output(LogType.Info, $"命令类型：{type}");
                            Command.Run(CommandOrigin.ConsoleExecute, command);
                        }
                        else
                        {
                            Logger.Output(LogType.Warn, "命令不合法");
                        }
                        break;

                    default:
                        Logger.Output(LogType.Warn, "未知的命令，请检查后重试或输入“help”获取更多信息");
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
