using System;
using System.Windows.Forms;

namespace Serein
{
    public partial class Ui : Form
    {
        private void ShowToolTip(object sender, string str)
        {
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip((Control)sender, str);
        }
        private void SettingServerPath_MouseHover(object sender, EventArgs e)
        {
            ShowToolTip(sender, "设置服务端的路径");
        }
        private void SettingServerPathLabel_MouseHover(object sender, EventArgs e)
        {
            ShowToolTip(sender, "设置服务端的路径");
        }
        private void SettingServerEnableRestart_MouseHover(object sender, EventArgs e)
        {
            ShowToolTip(sender, "若服务器进程退出时返回代码不为0则自动重启");
        }
        private void SettingServerEnableOutputCommand_MouseHover(object sender, EventArgs e)
        {
            ShowToolTip(sender, "将输入的指令在控制台中显示");
        }
        private void SettingServerEnableLog_MouseHover(object sender, EventArgs e)
        {
            ShowToolTip(sender, "将控制台输出和输入的指令一并保存到日志文件");
        }
        private void SettingServerOutputStyle_MouseHover(object sender, EventArgs e)
        {
            ShowToolTip(sender,
                "指定控制台的输出样式\n" +
                "-默认 禁用彩色输出\n" +
                "-原始彩色 按照原控制台的样式输出（推荐）\n" +
                "-语法高亮 匹配部分文本并高亮（可在preset.css中设置）");
        }
        private void SettingServerOutputStyleLabel_MouseHover(object sender, EventArgs e)
        {
            ShowToolTip(sender,
               "指定控制台的输出样式\n" +
               "-默认 禁用彩色输出\n" +
               "-原始彩色 按照原控制台的样式输出（推荐）\n" +
               "-语法高亮 匹配部分文本并高亮（可在preset.css中设置）");
        }
        private void SettingServerStopCommand_MouseHover(object sender, EventArgs e)
        {
            ShowToolTip(sender, "设置使用关服功能时执行的命令（使用英文分号\";\"分隔）");
        }
        private void SettingServerStopCommandLabel_MouseHover(object sender, EventArgs e)
        {
            ShowToolTip(sender, "设置使用关服功能时执行的命令（使用英文分号\";\"分隔）");
        }

        private void SettingServerAutoStop_MouseHover(object sender, EventArgs e)
        {
            ShowToolTip(sender, "此程序发生崩溃时，若服务器正在运行则自动关闭服务器（建议开启）");
        }
        private void SettingBotPortLabel_MouseHover(object sender, EventArgs e)
        {
            ShowToolTip(sender, $"设置Websocket的端口\n即Websocket正向服务器应在127.0.0.1:{SettingBotPort.Value}开启");
        }
        private void SettingBotEnableLog_MouseHover(object sender, EventArgs e)
        {
            ShowToolTip(sender, "将收到的数据包以文本格式保存到日志文件");
        }
        private void SettingBotGivePermissionToAllAdmin_MouseHover(object sender, EventArgs e)
        {
            ShowToolTip(sender, "使群聊的管理员和群主也有管理权限");
        }
        private void SettingBotGroup_MouseHover(object sender, EventArgs e)
        {
            ShowToolTip(sender, "设定要监听消息的群聊");
        }
        private void SettingBotGroupList_MouseHover(object sender, EventArgs e)
        {
            ShowToolTip(sender, "设定要监听消息的群聊");
        }
        private void SettingBotPermissionList_MouseHover(object sender, EventArgs e)
        {
            ShowToolTip(sender, "设定有管理权限的用户");
        }
        private void SettingBotPermission_MouseHover(object sender, EventArgs e)
        {
            ShowToolTip(sender, "设定有管理权限的用户");
        }
        private void SettingSereinEnableGetAnnouncement_MouseHover(object sender, EventArgs e)
        {
            ShowToolTip(sender, "启动后自动获取公告（建议开启）");
        }
        private void SettingSereinUpdateInfoType_MouseHover(object sender, EventArgs e)
        {
            ShowToolTip(sender,
                "启动后自动检查更新\n" +
                "-禁用 禁止获取更新信息\n" +
                "-仅正式版 获取Release中的正式版（推荐）\n" +
                "-仅测试版 获取来自最新的提交（不一定保证稳定，使用前请自行备份）"
                );
        }
        private void SettingSereinUpdateInfoTypeLabel_MouseHover(object sender, EventArgs e)
        {
            ShowToolTip(sender,
                "启动后自动检查更新\n" +
                "-禁用 禁止获取更新信息\n" +
                "-仅正式版 获取Release中的正式版（推荐）\n" +
                "-仅测试版 获取来自最新的提交（不一定保证稳定，使用前请自行备份）"
                );
        }
        private void SettingSereinAbout_MouseHover(object sender, EventArgs e)
        {
            ShowToolTip(sender, "打开帮助页面（https://zaitonn.github.io/Serein/About）");
        }
    }
}
