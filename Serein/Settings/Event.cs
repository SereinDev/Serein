using System.Collections.Generic;

namespace Serein.Settings
{
    internal class Event
    {
        public List<string> Bind_Success { get; set; } = new List<string>()
        {
             "g|[CQ:at,qq=%ID%] 绑定成功"
        };
        public List<string> Bind_Occupied { get; set; } = new List<string>()
        {
            "g|[CQ:at,qq=%ID%] 该游戏名称被占用"
        };

        public List<string> Bind_Invalid { get; set; } = new List<string>()
        {
            "g|[CQ:at,qq=%ID%] 该游戏名称无效"
        };
        public List<string> Bind_Already { get; set; } = new List<string>()
        {
             "g|[CQ:at,qq=%ID%] 你已经绑定过了"
        };
        public List<string> Unbind_Success { get; set; } = new List<string>()
        {
             "g|[CQ:at,qq=%ID%] 解绑成功"
        };
        public List<string> Unbind_Failure { get; set; } = new List<string>()
        {
             "g|[CQ:at,qq=%ID%] 该账号未绑定"
        };
        public List<string> Server_Start { get; set; } = new List<string>()
        {
            "g|服务器正在启动"
        };
        public List<string> Server_Stop { get; set; } = new List<string>()
        {
            "g|服务器已关闭"
        };
        public List<string> Server_Error { get; set; } = new List<string>()
        {
            "g|服务器异常关闭"
        };
        public List<string> Group_Increase { get; set; } = new List<string>()
        {
            "g|欢迎[CQ:at,qq=%ID%]入群~"
        };
        public List<string> Group_Decrease { get; set; } = new List<string>()
        {
            "g|用户%ID%退出了群聊，已自动解绑游戏ID",
            "unbind|%ID%"
        };
        public List<string> Group_Poke { get; set; } = new List<string>()
        {
            "g|别戳我……(*/ω＼*)"
        };
        public List<string> Serein_Crash { get; set; } = new List<string>()
        {
            "g|唔……发生了一点小问题(っ °Д °;)っ\n请查看Serein错误弹窗获取更多信息"
        };
    }
}
