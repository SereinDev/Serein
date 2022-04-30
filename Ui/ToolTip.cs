using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace Serein
{
    class ToolTips
    {

    }
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
        private void SettingBotPortLabel_MouseHover(object sender, EventArgs e)
        {
            ShowToolTip(sender, $"设置Websocket的端口\n即go-http的ws正向服务器应在127.0.0.1:{SettingBotPort.Value}开启");
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
        private void SettingBotClearCache_MouseHover(object sender, EventArgs e)
        {
            ShowToolTip(sender, "退出当前账号并清除消息缓存");
        }
        private void SettingSereinEnableGetUpdate_MouseHover(object sender, EventArgs e)
        {
            ShowToolTip(sender, "启动后每隔10分钟检查一次更新（建议开启）");
        }
        private void SettingSereinEnableGetAnnouncement_MouseHover(object sender, EventArgs e)
        {
            ShowToolTip(sender, "启动后获取一次公告（建议开启）");
        }
    }
}
