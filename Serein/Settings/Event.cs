using System.Collections.Generic;

namespace Serein.Settings
{
    internal class Event
    {
        public string Notice { get; } = "在这里你可以自定义每个基本事件触发时执行的命令。参考：https://zaitonn.github.io/Serein/Command.html";
        public string[] Bind_Success { get; set; } = new string[]
        {
             "g|[CQ:at,qq=%ID%] 绑定成功"
        };
        public string[] Bind_Occupied { get; set; } = new string[]
        {
            "g|[CQ:at,qq=%ID%] 该游戏名称被占用"
        };

        public string[] Bind_Invalid { get; set; } = new string[]
        {
            "g|[CQ:at,qq=%ID%] 该游戏名称无效"
        };
        public string[] Bind_Already { get; set; } = new string[]
        {
             "g|[CQ:at,qq=%ID%] 你已经绑定过了"
        };
        public string[] Unbind_Success { get; set; } = new string[]
        {
             "g|[CQ:at,qq=%ID%] 解绑成功"
        };
        public string[] Unbind_Failure { get; set; } = new string[]
        {
             "g|[CQ:at,qq=%ID%] 该账号未绑定"
        };
        public string[] Server_Start { get; set; } = new string[]
        {
            "g|服务器正在启动"
        };
        public string[] Server_Stop { get; set; } = new string[]
        {
            "g|服务器已关闭"
        };
        public string[] Server_Error { get; set; } = new string[]
        {
            "g|服务器异常关闭"
        };
        public string[] Group_Increase { get; set; } = new string[]
        {
            "g|欢迎[CQ:at,qq=%ID%]入群~"
        };
        public string[] Group_Decrease { get; set; } = new string[]
        {
            "g|用户%ID%退出了群聊，已自动解绑游戏ID",
            "unbind|%ID%"
        };
        public string[] Group_Poke { get; set; } = new string[]
        {
            "g|别戳我……(*/ω＼*)"
        };
        public string[] Serein_Crash { get; set; } = new string[]
        {
            "g|唔……发生了一点小问题(っ °Д °;)っ\n请查看Serein错误弹窗获取更多信息"
        };
        public string[] Motdpe_Success {get;set;}= new string[]
        {
            "g|服务器描述：%Description%\n版本：%Version%(%Protocol%)\n在线玩家：%OnlinePlayer%/%MaxPlayer%\n游戏模式：%GameMode%\n延迟：%Delay%ms"
        };
        public string[] Motdje_Success {get;set;}= new string[]
        {
            "g|服务器描述：%Description%\n版本：%Version%(%Protocol%)\n在线玩家：%OnlinePlayer%/%MaxPlayer%\n延迟：%Delay%ms"
        };
        public string[] Motd_Failure { get; set; } = new string[]
        {
            "g|服务器Motd获取失败\n%Exception%"
        };
    }
}
