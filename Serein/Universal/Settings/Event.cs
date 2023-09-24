using Serein.Base;
using Serein.Utils.Output;
using System;

namespace Serein.Settings
{
    internal class Event
    {
        public static string Notice { get; } =
            "在这里你可以自定义每个事件触发时执行的命令。参考：https://serein.cc/docs/guide/command、https://serein.cc/docs/guide/event";

        public string[] BindingSucceed { get; set; } = new[] { "g|[CQ:at,qq=%ID%] 绑定成功" };

        public string[] BindingFailDueToOccupation { get; set; } =
            new[] { "g|[CQ:at,qq=%ID%] 该游戏名称被占用" };

        public string[] BindingFailDueToInvalid { get; set; } =
            new[] { "g|[CQ:at,qq=%ID%] 该游戏名称无效" };

        public string[] BindingFailDueToAlreadyBinded { get; set; } =
            new[] { "g|[CQ:at,qq=%ID%] 你已经绑定过了" };

        public string[] UnbindingSucceed { get; set; } = new[] { "g|[CQ:at,qq=%ID%] 解绑成功" };

        public string[] UnbindingFail { get; set; } = new[] { "g|[CQ:at,qq=%ID%] 该账号未绑定" };

        public string[] BinderDisable { get; set; } = new[] { "g|[CQ:at,qq=%ID%] 绑定功能已被禁用" };

        public string[] ServerStart { get; set; } = new[] { "g|服务器正在启动" };

        public string[] ServerStop { get; set; } = new[] { "g|服务器已关闭" };

        public string[] ServerExitUnexpectedly { get; set; } = new[] { "g|服务器异常关闭" };

        public string[] GroupIncrease { get; set; } = new[] { "g|欢迎[CQ:at,qq=%ID%]入群~" };

        public string[] GroupDecrease { get; set; } =
            new[] { "g|用户%ID%退出了群聊，已自动解绑游戏ID", "unbind|%ID%" };

        public string[] GroupPoke { get; set; } = new[] { "g|别戳我……(*/ω＼*)" };

        public string[] SereinCrash { get; set; } =
            new[] { "g|唔……发生了一点小问题(っ °Д °;)っ\n请查看Serein错误弹窗获取更多信息" };

        public string[] RequestingMotdpeSucceed { get; set; } =
            new[]
            {
                "g|服务器描述：%Description%\n版本：%Version%(%Protocol%)\n在线玩家：%OnlinePlayer%/%MaxPlayer%\n游戏模式：%GameMode%\n延迟：%Latency%ms"
            };

        public string[] RequestingMotdjeSucceed { get; set; } =
            new[]
            {
                "g|服务器描述：%Description%\n版本：%Version%(%Protocol%)\n在线玩家：%OnlinePlayer%/%MaxPlayer%\n延迟：%Latency%ms\n%Favicon%"
            };

        public string[] RequestingMotdFail { get; set; } = new[] { "g|Motd获取失败\n详细原因：%Exception%" };

        public string[] PermissionDeniedFromPrivateMsg { get; set; } = new[] { "p|你没有执行这个命令的权限" };

        public string[] PermissionDeniedFromGroupMsg { get; set; } =
            new[] { "g|[CQ:at,qq=%ID%] 你没有执行这个命令的权限" };

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
                case EventType.BinderDisable:
                    BinderDisable = commands;
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
                    Logger.Output(LogType.Debug, new ArgumentOutOfRangeException(nameof(commands)));
                    break;
            }
        }

        public string[] Get(EventType type) =>
            type switch
            {
                EventType.BindingSucceed => BindingSucceed,
                EventType.BindingFailDueToOccupation => BindingFailDueToOccupation,
                EventType.BindingFailDueToInvalid => BindingFailDueToInvalid,
                EventType.BindingFailDueToAlreadyBinded => BindingFailDueToAlreadyBinded,
                EventType.UnbindingSucceed => UnbindingSucceed,
                EventType.UnbindingFail => UnbindingFail,
                EventType.BinderDisable => BinderDisable,
                EventType.ServerStart => ServerStart,
                EventType.ServerStop => ServerStop,
                EventType.ServerExitUnexpectedly => ServerExitUnexpectedly,
                EventType.GroupIncrease => GroupIncrease,
                EventType.GroupDecrease => GroupDecrease,
                EventType.GroupPoke => GroupPoke,
                EventType.SereinCrash => SereinCrash,
                EventType.RequestingMotdpeSucceed => RequestingMotdpeSucceed,
                EventType.RequestingMotdjeSucceed => RequestingMotdjeSucceed,
                EventType.RequestingMotdFail => RequestingMotdFail,
                EventType.PermissionDeniedFromPrivateMsg => PermissionDeniedFromPrivateMsg,
                EventType.PermissionDeniedFromGroupMsg => PermissionDeniedFromGroupMsg,
                _ => Array.Empty<string>(),
            };
    }
}
