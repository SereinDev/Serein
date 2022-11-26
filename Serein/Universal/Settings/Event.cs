using Serein.Items;
using System;

namespace Serein.Settings
{
    internal class Event
    {
        public string Notice { get; } = "在这里你可以自定义每个事件触发时执行的命令。参考：https://serein.cc/#/Function/Command、https://serein.cc/#/Function/Event";

        public string[] BindingSucceed { get; set; } = new string[]
        {
             "g|[CQ:at,qq=%ID] 绑定成功"
        };

        public string[] BindingFailDueToOccupation { get; set; } = new string[]
        {
            "g|[CQ:at,qq=%ID%] 该游戏名称被占用"
        };

        public string[] BindingFailDueToInvalid { get; set; } = new string[]
        {
            "g|[CQ:at,qq=%ID%] 该游戏名称无效"
        };

        public string[] BindingFailDueToAlreadyBinded { get; set; } = new string[]
        {
             "g|[CQ:at,qq=%ID%] 你已经绑定过了"
        };

        public string[] UnbindingSucceed { get; set; } = new string[]
        {
             "g|[CQ:at,qq=%ID%] 解绑成功"
        };

        public string[] UnbindingFail { get; set; } = new string[]
        {
             "g|[CQ:at,qq=%ID%] 该账号未绑定"
        };

        public string[] ServerStart { get; set; } = new string[]
        {
            "g|服务器正在启动"
        };

        public string[] ServerStop { get; set; } = new string[]
        {
            "g|服务器已关闭"
        };

        public string[] ServerExitUnexpectedly { get; set; } = new string[]
        {
            "g|服务器异常关闭"
        };

        public string[] GroupIncrease { get; set; } = new string[]
        {
            "g|欢迎[CQ:at,qq=%ID%]入群~"
        };

        public string[] GroupDecrease { get; set; } = new string[]
        {
            "g|用户%ID%退出了群聊，已自动解绑游戏ID",
            "unbind|%ID%"
        };

        public string[] GroupPoke { get; set; } = new string[]
        {
            "g|别戳我……(*/ω＼*)"
        };

        public string[] SereinCrash { get; set; } = new string[]
        {
            "g|唔……发生了一点小问题(っ °Д °;)っ\n请查看Serein错误弹窗获取更多信息"
        };

        public string[] RequestingMotdpeSucceed { get; set; } = new string[]
        {
            "g|服务器描述：%Description%\n版本：%Version%(%Protocol%)\n在线玩家：%OnlinePlayer%/%MaxPlayer%\n游戏模式：%GameMode%\n延迟：%Delay%ms"
        };

        public string[] RequestingMotdjeSucceed { get; set; } = new string[]
        {
            "g|服务器描述：%Description%\n版本：%Version%(%Protocol%)\n在线玩家：%OnlinePlayer%/%MaxPlayer%\n延迟：%Delay%ms\n%Favicon%"
        };

        public string[] RequestingMotdFail { get; set; } = new string[]
        {
            "g|Motd获取失败\n详细原因：%Exception%"
        };

        public string[] PermissionDeniedFromPrivateMsg { get; set; } = new string[]
        {
            "p|你没有执行这个命令的权限"
        };

        public string[] PermissionDeniedFromGroupMsg { get; set; } = new string[]
        {
            "g|[CQ:at,qq=%ID%] 你没有执行这个命令的权限"
        };

        public void Edit(string[] Commands, EventType Type)
        {
            switch (Type)
            {
                case EventType.BindingSucceed:
                    Global.Settings.Event.BindingSucceed = Commands;
                    break;
                case EventType.BindingFailDueToOccupation:
                    Global.Settings.Event.BindingFailDueToOccupation = Commands;
                    break;
                case EventType.BindingFailDueToInvalid:
                    Global.Settings.Event.BindingFailDueToInvalid = Commands;
                    break;
                case EventType.BindingFailDueToAlreadyBinded:
                    Global.Settings.Event.BindingFailDueToAlreadyBinded = Commands;
                    break;
                case EventType.UnbindingSucceed:
                    Global.Settings.Event.UnbindingSucceed = Commands;
                    break;
                case EventType.UnbindingFail:
                    Global.Settings.Event.UnbindingFail = Commands;
                    break;
                case EventType.ServerStart:
                    Global.Settings.Event.ServerStart = Commands;
                    break;
                case EventType.ServerStop:
                    Global.Settings.Event.ServerStop = Commands;
                    break;
                case EventType.ServerExitUnexpectedly:
                    Global.Settings.Event.ServerExitUnexpectedly = Commands;
                    break;
                case EventType.GroupIncrease:
                    Global.Settings.Event.GroupIncrease = Commands;
                    break;
                case EventType.GroupDecrease:
                    Global.Settings.Event.GroupDecrease = Commands;
                    break;
                case EventType.GroupPoke:
                    Global.Settings.Event.GroupPoke = Commands;
                    break;
                case EventType.SereinCrash:
                    Global.Settings.Event.SereinCrash = Commands;
                    break;
                case EventType.RequestingMotdpeSucceed:
                    Global.Settings.Event.RequestingMotdpeSucceed = Commands;
                    break;
                case EventType.RequestingMotdjeSucceed:
                    Global.Settings.Event.RequestingMotdjeSucceed = Commands;
                    break;
                case EventType.RequestingMotdFail:
                    Global.Settings.Event.RequestingMotdFail = Commands;
                    break;
                case EventType.PermissionDeniedFromPrivateMsg:
                    Global.Settings.Event.PermissionDeniedFromPrivateMsg = Commands;
                    break;
                case EventType.PermissionDeniedFromGroupMsg:
                    Global.Settings.Event.PermissionDeniedFromGroupMsg = Commands;
                    break;
            }
        }

        public string[] Get(EventType Type)
        {
            switch (Type)
            {
                case EventType.BindingSucceed:
                    return Global.Settings.Event.BindingSucceed;
                case EventType.BindingFailDueToOccupation:
                    return Global.Settings.Event.BindingFailDueToOccupation;
                case EventType.BindingFailDueToInvalid:
                    return Global.Settings.Event.BindingFailDueToInvalid;
                case EventType.BindingFailDueToAlreadyBinded:
                    return Global.Settings.Event.BindingFailDueToAlreadyBinded;
                case EventType.UnbindingSucceed:
                    return Global.Settings.Event.UnbindingSucceed;
                case EventType.UnbindingFail:
                    return Global.Settings.Event.UnbindingFail;
                case EventType.ServerStart:
                    return Global.Settings.Event.ServerStart;
                case EventType.ServerStop:
                    return Global.Settings.Event.ServerStop;
                case EventType.ServerExitUnexpectedly:
                    return Global.Settings.Event.ServerExitUnexpectedly;
                case EventType.GroupIncrease:
                    return Global.Settings.Event.GroupIncrease;
                case EventType.GroupDecrease:
                    return Global.Settings.Event.GroupDecrease;
                case EventType.GroupPoke:
                    return Global.Settings.Event.GroupPoke;
                case EventType.SereinCrash:
                    return Global.Settings.Event.SereinCrash;
                case EventType.RequestingMotdpeSucceed:
                    return Global.Settings.Event.RequestingMotdpeSucceed;
                case EventType.RequestingMotdjeSucceed:
                    return Global.Settings.Event.RequestingMotdjeSucceed;
                case EventType.RequestingMotdFail:
                    return Global.Settings.Event.RequestingMotdFail;
                case EventType.PermissionDeniedFromPrivateMsg:
                    return Global.Settings.Event.PermissionDeniedFromPrivateMsg;
                case EventType.PermissionDeniedFromGroupMsg:
                    return Global.Settings.Event.PermissionDeniedFromGroupMsg;
                default:
                    return Array.Empty<string>();
            }
        }
    }
}
