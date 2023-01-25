using System;
using System.Windows.Forms;

namespace Serein.Ui
{
    public partial class Ui : Form
    {
        private void ShowToolTip(object sender, string str)
        {
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip((Control)sender, str);
        }

        private void SettingServerPath_MouseHover(object sender, EventArgs e)
            => ShowToolTip(sender, "设置服务端的路径");
        private void SettingServerPathLabel_MouseHover(object sender, EventArgs e)
            => ShowToolTip(sender, "设置服务端的路径");
        private void SettingServerEnableRestart_MouseHover(object sender, EventArgs e)
            => ShowToolTip(sender, "若服务器进程退出时返回代码不为0则自动重启");
        private void SettingServerEnableOutputCommand_MouseHover(object sender, EventArgs e)
            => ShowToolTip(sender, "将输入的指令在控制台中显示");
        private void SettingServerEnableLog_MouseHover(object sender, EventArgs e)
            => ShowToolTip(sender, "将控制台输出和输入的指令一并保存到日志文件");
        private void SettingServerStopCommand_MouseHover(object sender, EventArgs e)
            => ShowToolTip(sender, "设置使用关服功能时执行的命令（使用英文分号\";\"分隔）");
        private void SettingServerStopCommandLabel_MouseHover(object sender, EventArgs e)
            => ShowToolTip(sender, "设置使用关服功能时执行的命令（使用英文分号\";\"分隔）");
        private void SettingServerAutoStop_MouseHover(object sender, EventArgs e)
            => ShowToolTip(sender, "Serein发生崩溃时，若服务器正在运行则自动关闭服务器（建议开启）");
        private void SettingServerEnableUnicode_MouseHover(object sender, EventArgs e)
            => ShowToolTip(sender, "将所有指令中的非ASCII字符转为Unicode后输出\n例：\"你好\"→\"\\u4f60\\u597d\"");
        private void SettingServerOutputEncoding_MouseHover(object sender, EventArgs e)
            => ShowToolTip(sender, "指定服务器输出的编码格式（重启服务器生效）");
        private void SettingServerOutputEncodingLabel_MouseHover(object sender, EventArgs e)
            => ShowToolTip(sender, "指定服务器输出的编码格式（重启服务器生效）");
        private void SettingServerEncodingLabel_MouseHover(object sender, EventArgs e)
            => ShowToolTip(sender, "指定输出到服务器的编码格式（重启服务器生效）");
        private void SettingServerEncoding_MouseHover(object sender, EventArgs e)
            => ShowToolTip(sender, "指定输出到服务器的编码格式（重启服务器生效）");
        private void SettingServerType_MouseHover(object sender, EventArgs e)
            => ShowToolTip(sender, "指定服务器的类型\n用于获取服务器的Motd");
        private void SettingServerTypeLabel_MouseHover(object sender, EventArgs e)
            => ShowToolTip(sender, "指定服务器的类型\n用于获取服务器的Motd");
        private void SettingServerPortLabel_MouseHover(object sender, EventArgs e)
            => ShowToolTip(sender, "指定服务器的本地端口\n用于获取服务器的Motd\n建议填IPv4的端口");
        private void SettingServerLineTerminator_MouseHover(object sender, EventArgs e)
            => ShowToolTip(sender, "当前使用的行结束符字符串");
        private void SettingServerLineTerminatorLabel_MouseHover(object sender, EventArgs e)
            => ShowToolTip(sender, "当前使用的行结束符字符串");

        private void SettingBotUriLabel_MouseHover(object sender, EventArgs e)
            => ShowToolTip(sender, $"Websocket服务器的地址\n即Websocket服务器应在 ws://{Global.Settings.Bot.Uri} 上开启");
        private void SettingBotUri_MouseHover(object sender, EventArgs e)
            => ShowToolTip(sender, $"Websocket服务器的地址\n即Websocket服务器应在 ws://{Global.Settings.Bot.Uri} 上开启");
        private void SettingBotAuthorizationLabel_MouseHover(object sender, EventArgs e)
            => ShowToolTip(sender, "用于鉴权的Access-Token（可为空）\n在Websocket的Header添加Authorization字段用于鉴权，连接非本地的ws服务器时建议设置此项");
        private void SettingBotAuthorization_MouseHover(object sender, EventArgs e)
            => ShowToolTip(sender, "用于鉴权的Access-Token（可为空）\n在Websocket的Header添加Authorization字段用于鉴权，连接非本地的ws服务器时建议设置此项");
        private void SettingBotEnableLog_MouseHover(object sender, EventArgs e)
            => ShowToolTip(sender, "将收到的数据包以文本格式保存到日志文件");
        private void SettingBotGivePermissionToAllAdmin_MouseHover(object sender, EventArgs e)
            => ShowToolTip(sender, "使群聊的管理员和群主也有管理权限");
        private void SettingBotEnbaleOutputData_MouseHover(object sender, EventArgs e)
            => ShowToolTip(sender, "在机器人控制台中输出接收和发送的数据");
        private void SettingBotRestart_MouseHover(object sender, EventArgs e)
            => ShowToolTip(sender, "ws连接异常断开时自动重连");
        private void SettingBotAutoEscape_MouseHover(object sender, EventArgs e)
            => ShowToolTip(sender, "消息内容是否作为纯文本发送（即不解析CQ码）");
        private void SettingBotGroup_MouseHover(object sender, EventArgs e)
            => ShowToolTip(sender, "设定要监听消息的群聊");
        private void SettingBotGroupList_MouseHover(object sender, EventArgs e)
            => ShowToolTip(sender, "设定要监听消息的群聊");
        private void SettingBotPermissionList_MouseHover(object sender, EventArgs e)
            => ShowToolTip(sender, "设定有管理权限的用户");
        private void SettingBotPermission_MouseHover(object sender, EventArgs e)
            => ShowToolTip(sender, "设定有管理权限的用户");
        private void SettingBotEnbaleParseAt_MouseHover(object sender, EventArgs e)
            => ShowToolTip(sender, "自动替换\"@***\"和\"[CQ:at,qq=***]\"为@群昵称（仅输出到服务器时替换）");

        private void SettingSereinEnableGetAnnouncement_MouseHover(object sender, EventArgs e)
            => ShowToolTip(sender, "启动后自动获取公告（建议开启）");
        private void SettingSereinEnableGetUpdate_MouseHover(object sender, EventArgs e)
            => ShowToolTip(sender, "启动后自动获取更新（建议开启）");
        private void SettingSereinAbout_MouseHover(object sender, EventArgs e)
            => ShowToolTip(sender, "https://serein.cc/#/More/About");
        private void SettingSereinPage_MouseHover(object sender, EventArgs e)
            => ShowToolTip(sender, "https://serein.cc");
        private void SettingSereinExtension_MouseHover(object sender, EventArgs e)
            => ShowToolTip(sender, "https://serein.cc/Extension");
        private void SettingSereinDownload_MouseHover(object sender, EventArgs e)
            => ShowToolTip(sender, "https://github.com/Zaitonn/Serein/releases/latest");
        private void SettingSereinEnableDPIAware_MouseHover(object sender, EventArgs e)
            => ShowToolTip(sender, "启用DPI感知\n若界面控件错位或模糊可选择开启此项");

        private void SettingServerOutputStyle_MouseHover(object sender, EventArgs e)
            => ShowToolTip(sender,
                "指定控制台的输出样式\n" +
                "-无 禁用彩色输出\n" +
                "-原始彩色 按照原控制台的样式输出（推荐）\n" +
                "-语法高亮 匹配部分文本并高亮（可在preset.css中设置）\n" +
                "-混合 综合\"原始彩色\"和\"语法高亮\"的优点");

        private void SettingServerOutputStyleLabel_MouseHover(object sender, EventArgs e)
            => ShowToolTip(sender,
                "指定控制台的输出样式\n" +
                "-无 禁用彩色输出\n" +
                "-原始彩色 按照原控制台的样式输出（推荐）\n" +
                "-语法高亮 匹配部分文本并高亮（可在preset.css中设置）\n" +
                "-混合 综合\"原始彩色\"和\"语法高亮\"的优点");
    }
}
