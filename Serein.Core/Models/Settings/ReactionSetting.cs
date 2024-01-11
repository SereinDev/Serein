using System;
using System.ComponentModel;

namespace Serein.Core.Models.Settings;

public class ReactionSetting : INotifyPropertyChanged
{
    public static string Notice { get; } =
        "在这里你可以自定义每个事件触发时执行的命令。参考：https://serein.cc/docs/guide/command、https://serein.cc/docs/guide/event";

    public string[] BindingSucceed { get; set; } = { "g|[CQ:at,qq=%ID%] 绑定成功" };

    public string[] BindingFailDueToOccupation { get; set; } = { "g|[CQ:at,qq=%ID%] 该游戏名称被占用" };

    public string[] BindingFailDueToInvalid { get; set; } = { "g|[CQ:at,qq=%ID%] 该游戏名称无效" };

    public string[] BindingFailDueToAlreadyBinded { get; set; } = { "g|[CQ:at,qq=%ID%] 你已经绑定过了" };

    public string[] UnbindingSucceed { get; set; } = { "g|[CQ:at,qq=%ID%] 解绑成功" };

    public string[] UnbindingFail { get; set; } = { "g|[CQ:at,qq=%ID%] 该账号未绑定" };

    public string[] BinderDisable { get; set; } = { "g|[CQ:at,qq=%ID%] 绑定功能已被禁用" };

    public string[] ServerStart { get; set; } = { "g|服务器正在启动" };

    public string[] ServerStop { get; set; } = { "g|服务器已关闭" };

    public string[] ServerExitUnexpectedly { get; set; } = { "g|服务器异常关闭" };

    public string[] GroupIncrease { get; set; } = { "g|欢迎[CQ:at,qq=%ID%]入群~" };

    public string[] GroupDecrease { get; set; } = { "g|用户%ID%退出了群聊，已自动解绑游戏ID", "unbind|%ID%" };

    public string[] GroupPoke { get; set; } = { "g|别戳我……(*/ω＼*)" };

    public string[] SereinCrash { get; set; } = { "g|唔……发生了一点小问题(っ °Д °;)っ\n请查看Serein错误弹窗获取更多信息" };

    public string[] RequestingMotdpeSucceed { get; set; } =
        {
            "g|服务器描述：%Description%\n版本：%Version%(%Protocol%)\n在线玩家：%OnlinePlayer%/%MaxPlayer%\n游戏模式：%GameMode%\n延迟：%Latency%ms"
        };

    public string[] RequestingMotdjeSucceed { get; set; } =
        {
            "g|服务器描述：%Description%\n版本：%Version%(%Protocol%)\n在线玩家：%OnlinePlayer%/%MaxPlayer%\n延迟：%Latency%ms\n%Favicon%"
        };

    public string[] RequestingMotdFail { get; set; } = { "g|Motd获取失败\n详细原因：%Exception%" };

    public string[] PermissionDeniedFromPrivateMsg { get; set; } = { "p|你没有执行这个命令的权限" };

    public string[] PermissionDeniedFromGroupMsg { get; set; } =
        { "g|[CQ:at,qq=%ID%] 你没有执行这个命令的权限" };

#pragma warning disable CS0067
    public event PropertyChangedEventHandler? PropertyChanged;
}
