using Serein.Items;
using Serein.Base;
using System;

namespace Serein.Settings
{
    internal class Event
    {
        public string Notice { get; } = "在这里你可以自定义每个事件触发时执行的命令。参考：https://serein.cc/#/Function/Command、https://serein.cc/#/Function/Event";

        public string[] BindingSucceed { get; set; } = new[]
        {
             "g|[CQ:at,qq=%ID%] 绑定成功"
        };

        public string[] BindingFailDueToOccupation { get; set; } = new[]
        {
            "g|[CQ:at,qq=%ID%] 该游戏名称被占用"
        };

        public string[] BindingFailDueToInvalid { get; set; } = new[]
        {
            "g|[CQ:at,qq=%ID%] 该游戏名称无效"
        };

        public string[] BindingFailDueToAlreadyBinded { get; set; } = new[]
        {
             "g|[CQ:at,qq=%ID%] 你已经绑定过了"
        };

        public string[] UnbindingSucceed { get; set; } = new[]
        {
             "g|[CQ:at,qq=%ID%] 解绑成功"
        };

        public string[] UnbindingFail { get; set; } = new[]
        {
             "g|[CQ:at,qq=%ID%] 该账号未绑定"
        };

        public string[] ServerStart { get; set; } = new[]
        {
            "g|服务器正在启动"
        };

        public string[] ServerStop { get; set; } = new[]
        {
            "g|服务器已关闭"
        };

        public string[] ServerExitUnexpectedly { get; set; } = new[]
        {
            "g|服务器异常关闭"
        };

        public string[] GroupIncrease { get; set; } = new[]
        {
            "g|欢迎[CQ:at,qq=%ID%]入群~"
        };

        public string[] GroupDecrease { get; set; } = new[]
        {
            "g|用户%ID%退出了群聊，已自动解绑游戏ID",
            "unbind|%ID%"
        };

        public string[] GroupPoke { get; set; } = new[]
        {
            "g|别戳我……(*/ω＼*)"
        };

        public string[] SereinCrash { get; set; } = new[]
        {
            "g|唔……发生了一点小问题(っ °Д °;)っ\n请查看Serein错误弹窗获取更多信息"
        };

        public string[] RequestingMotdpeSucceed { get; set; } = new[]
        {
            "g|服务器描述：%Description%\n版本：%Version%(%Protocol%)\n在线玩家：%OnlinePlayer%/%MaxPlayer%\n游戏模式：%GameMode%\n延迟：%Delay%ms"
        };

        public string[] RequestingMotdjeSucceed { get; set; } = new[]
        {
            "g|服务器描述：%Description%\n版本：%Version%(%Protocol%)\n在线玩家：%OnlinePlayer%/%MaxPlayer%\n延迟：%Delay%ms\n%Favicon%"
        };

        public string[] RequestingMotdFail { get; set; } = new[]
        {
            "g|Motd获取失败\n详细原因：%Exception%"
        };

        public string[] PermissionDeniedFromPrivateMsg { get; set; } = new[]
        {
            "p|你没有执行这个命令的权限"
        };

        public string[] PermissionDeniedFromGroupMsg { get; set; } = new[]
        {
            "g|[CQ:at,qq=%ID%] 你没有执行这个命令的权限"
        };

        public void Edit(string[] commands, EventType type)
        {
            switch (type)
            {
                case EventType.BindingSucceed:
                    BindingSucceed = commands;
                    break;
                case EventType.BindingFailDueToOccupation:
                    BindingFailDueToOccupation = commands;
                    break;
                case EventType.BindingFailDueToInvalid:
                    BindingFailDueToInvalid = commands;
                    break;
                case EventType.BindingFailDueToAlreadyBinded:
                    BindingFailDueToAlreadyBinded = commands;
                    break;
                case EventType.UnbindingSucceed:
                    UnbindingSucceed = commands;
                    break;
                case EventType.UnbindingFail:
                    UnbindingFail = commands;
                    break;
                case EventType.ServerStart:
                    ServerStart = commands;
                    break;
                case EventType.ServerStop:
                    ServerStop = commands;
                    break;
                case EventType.ServerExitUnexpectedly:
                    ServerExitUnexpectedly = commands;
                    break;
                case EventType.GroupIncrease:
                    GroupIncrease = commands;
                    break;
                case EventType.GroupDecrease:
                    GroupDecrease = commands;
                    break;
                case EventType.GroupPoke:
                    GroupPoke = commands;
                    break;
                case EventType.SereinCrash:
                    SereinCrash = commands;
                    break;
                case EventType.RequestingMotdpeSucceed:
                    RequestingMotdpeSucceed = commands;
                    break;
                case EventType.RequestingMotdjeSucceed:
                    RequestingMotdjeSucceed = commands;
                    break;
                case EventType.RequestingMotdFail:
                    RequestingMotdFail = commands;
                    break;
                case EventType.PermissionDeniedFromPrivateMsg:
                    PermissionDeniedFromPrivateMsg = commands;
                    break;
                case EventType.PermissionDeniedFromGroupMsg:
                    PermissionDeniedFromGroupMsg = commands;
                    break;
                default:
                    Logger.Out(LogType.Debug, new ArgumentOutOfRangeException(nameof(commands)));
                    break;
            }
        }

        public string[] Get(EventType Type)
        {
            switch (Type)
            {
                case EventType.BindingSucceed:
                    return BindingSucceed;
                case EventType.BindingFailDueToOccupation:
                    return BindingFailDueToOccupation;
                case EventType.BindingFailDueToInvalid:
                    return BindingFailDueToInvalid;
                case EventType.BindingFailDueToAlreadyBinded:
                    return BindingFailDueToAlreadyBinded;
                case EventType.UnbindingSucceed:
                    return UnbindingSucceed;
                case EventType.UnbindingFail:
                    return UnbindingFail;
                case EventType.ServerStart:
                    return ServerStart;
                case EventType.ServerStop:
                    return ServerStop;
                case EventType.ServerExitUnexpectedly:
                    return ServerExitUnexpectedly;
                case EventType.GroupIncrease:
                    return GroupIncrease;
                case EventType.GroupDecrease:
                    return GroupDecrease;
                case EventType.GroupPoke:
                    return GroupPoke;
                case EventType.SereinCrash:
                    return SereinCrash;
                case EventType.RequestingMotdpeSucceed:
                    return RequestingMotdpeSucceed;
                case EventType.RequestingMotdjeSucceed:
                    return RequestingMotdjeSucceed;
                case EventType.RequestingMotdFail:
                    return RequestingMotdFail;
                case EventType.PermissionDeniedFromPrivateMsg:
                    return PermissionDeniedFromPrivateMsg;
                case EventType.PermissionDeniedFromGroupMsg:
                    return PermissionDeniedFromGroupMsg;
                default:
                    return Array.Empty<string>();
            }
        }
    }
}
