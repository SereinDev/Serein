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
    public partial class Ui : Form
    {
        public void LoadSettings()
        {
            SettingServerEnableRestart.Checked = Global.Settings_server.EnableRestart;
            SettingServerEnableOutputCommand.Checked = Global.Settings_server.EnableOutputCommand;
            SettingServerEnableLog.Checked = Global.Settings_server.EnableLog;
            SettingServerOutputStyle.SelectedIndex = Global.Settings_server.OutputStyle;
            SettingBotPort.Value = Global.Settings_bot.Port;
            SettingBotEnableLog.Checked = Global.Settings_bot.EnableLog;
            SettingBotGivePermissionToAllAdmin.Checked = Global.Settings_bot.GivePermissionToAllAdmin;
            SettingSereinUpdateInfoType.SelectedIndex = Global.Settings_serein.UpdateInfoType;
            SettingSereinEnableGetAnnouncement.Checked = Global.Settings_serein.EnableGetAnnouncement;
            SettingServerPath.Text = Global.Settings_server.Path;
            SettingBotPermissionList.Text = string.Join(",", Global.Settings_bot.PermissionList);
            SettingBotGroupList.Text = string.Join(",", Global.Settings_bot.GroupList);
        }
        private void SettingServerEnableRestart_CheckedChanged(object sender, EventArgs e)
        {
            Global.Settings_server.EnableRestart = SettingServerEnableRestart.Checked;
        }
        private void SettingServerEnableOutputCommand_CheckedChanged(object sender, EventArgs e)
        {
            Global.Settings_server.EnableOutputCommand = SettingServerEnableOutputCommand.Checked;
        }
        private void SettingServerEnableLog_CheckedChanged(object sender, EventArgs e)
        {
            Global.Settings_server.EnableLog = SettingServerEnableLog.Checked;
        }
        private void SettingServerOutputStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            Global.Settings_server.OutputStyle = SettingServerOutputStyle.SelectedIndex;
        }
        private void SettingBotPort_ValueChanged(object sender, EventArgs e)
        {
            Global.Settings_bot.Port = (int)SettingBotPort.Value;
        }
        private void SettingBotEnableLog_CheckedChanged(object sender, EventArgs e)
        {
            Global.Settings_bot.EnableLog = SettingBotEnableLog.Checked;
        }
        private void SettingBotGivePermissionToAllAdmin_CheckedChanged(object sender, EventArgs e)
        {
            Global.Settings_bot.GivePermissionToAllAdmin = SettingBotGivePermissionToAllAdmin.Checked;

        }
        private void SettingBotGroupList_TextChanged(object sender, EventArgs e)
        {
            if (Regex.IsMatch(SettingBotGroupList.Text, @"^[\d,]+?$"))
            {
                List<long> list = new List<long>();
                foreach (string qq in SettingBotGroupList.Text.Split(','))
                {
                    if (qq.Length >= 6 && qq.Length <= 16)
                    {
                        long.TryParse(qq, out long qq_);
                        list.Add(qq_);
                    }
                }
                Global.Settings_bot.GroupList = list.Distinct().ToArray();
            }
            SettingBotGroupList.Text = Regex.Replace(SettingBotGroupList.Text, @"[^\d,]", ",");
            SettingBotGroupList.Text = Regex.Replace(SettingBotGroupList.Text, @",+", ",");
            SettingBotGroupList.Text = Regex.Replace(SettingBotGroupList.Text, "^,", "");
            SettingBotGroupList.Focus();
            SettingBotGroupList.Select(SettingBotGroupList.TextLength, 0);
            SettingBotGroupList.ScrollToCaret();
        }
        private void SettingBotPermissionList_TextChanged(object sender, EventArgs e)
        {
            if (Regex.IsMatch(SettingBotPermissionList.Text, @"^[\d,]+?$"))
            {
                List<long> list = new List<long>();
                foreach (string qq in SettingBotPermissionList.Text.Split(','))
                {
                    if (qq.Length >= 5 && qq.Length <= 13)
                    {
                        long.TryParse(qq, out long qq_);
                        list.Add(qq_);
                    }
                }
                Global.Settings_bot.PermissionList = list.Distinct().ToArray();
            }
            SettingBotPermissionList.Text = Regex.Replace(SettingBotPermissionList.Text, @"[^\d,]", ",");
            SettingBotPermissionList.Text = Regex.Replace(SettingBotPermissionList.Text, @",+", ",");
            SettingBotPermissionList.Text = Regex.Replace(SettingBotPermissionList.Text, @"^,", "");
            SettingBotPermissionList.Focus();
            SettingBotPermissionList.Select(SettingBotPermissionList.TextLength, 0);
            SettingBotPermissionList.ScrollToCaret();
        }

        private void SettingSereinUpdateInfoType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Global.Settings_serein.UpdateInfoType = SettingSereinUpdateInfoType.SelectedIndex;
        }

        private void SettingSereinEnableGetAnnouncement_CheckedChanged(object sender, EventArgs e)
        {
            Global.Settings_serein.EnableGetAnnouncement = SettingSereinEnableGetAnnouncement.Checked;
        }
        private void SettingServerPathSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "支持的文件(*.exe *.bat)|*.exe;*.bat"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                SettingServerPath.Text = dialog.FileName;
                Global.Settings_server.Path = dialog.FileName;
                LoadPlugins();
            }
        }

        private void SettingSereinAbout_Click(object sender, EventArgs e)
        {
            Process.Start("https://zaitonn.github.io/Serein/About");
        }
        private void SettingBotSupportedLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/Mrs4s/go-cqhttp");
        }
        private void SettingSereinPage_Click(object sender, EventArgs e)
        {
            Process.Start("https://zaitonn.github.io/Serein");
        }
    }

}
