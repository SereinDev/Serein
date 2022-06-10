using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Serein
{
    public partial class Ui : Form
    {
        public void LoadSettings()
        {
            SettingServerEnableRestart.Checked = Global.Settings_Server.EnableRestart;
            SettingServerEnableOutputCommand.Checked = Global.Settings_Server.EnableOutputCommand;
            SettingServerEnableLog.Checked = Global.Settings_Server.EnableLog;
            SettingServerOutputStyle.SelectedIndex = Global.Settings_Server.OutputStyle;
            SettingServerAutoStop.Checked = Global.Settings_Server.AutoStop;
            SettingServerPath.Text = Global.Settings_Server.Path;
            SettingServerStopCommand.Text = Global.Settings_Server.StopCommand;
            SettingBotPermissionList.Text = string.Join(";", Global.Settings_Bot.PermissionList);
            SettingBotGroupList.Text = string.Join(";", Global.Settings_Bot.GroupList);
            SettingBotPort.Value = Global.Settings_Bot.Port;
            SettingBotEnbaleOutputData.Checked = Global.Settings_Bot.EnbaleOutputData;
            SettingBotEnableLog.Checked = Global.Settings_Bot.EnableLog;
            SettingBotGivePermissionToAllAdmin.Checked = Global.Settings_Bot.GivePermissionToAllAdmin;
            SettingSereinEnableGetUpdate.Checked = Global.Settings_Serein.EnableGetUpdate;
            SettingSereinEnableGetAnnouncement.Checked = Global.Settings_Serein.EnableGetAnnouncement;
            if (!Global.Settings_Serein.Debug)
            {
                Debug.Parent = null;
            }
        }
        private void SettingServerEnableRestart_CheckedChanged(object sender, EventArgs e)
        {
            Global.Settings_Server.EnableRestart = SettingServerEnableRestart.Checked;
        }
        private void SettingServerEnableOutputCommand_CheckedChanged(object sender, EventArgs e)
        {
            Global.Settings_Server.EnableOutputCommand = SettingServerEnableOutputCommand.Checked;
        }
        private void SettingServerEnableLog_CheckedChanged(object sender, EventArgs e)
        {
            Global.Settings_Server.EnableLog = SettingServerEnableLog.Checked;
        }
        private void SettingServerOutputStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            Global.Settings_Server.OutputStyle = SettingServerOutputStyle.SelectedIndex;
        }
        private void SettingBotPort_ValueChanged(object sender, EventArgs e)
        {
            Global.Settings_Bot.Port = (int)SettingBotPort.Value;
        }
        private void SettingBotEnableLog_CheckedChanged(object sender, EventArgs e)
        {
            Global.Settings_Bot.EnableLog = SettingBotEnableLog.Checked;
        }
        private void SettingBotGivePermissionToAllAdmin_CheckedChanged(object sender, EventArgs e)
        {
            Global.Settings_Bot.GivePermissionToAllAdmin = SettingBotGivePermissionToAllAdmin.Checked;

        }
        private void SettingBotEnbaleOutputData_CheckedChanged(object sender, EventArgs e)
        {
            Global.Settings_Bot.EnbaleOutputData = SettingBotEnbaleOutputData.Checked;
        }
        private void SettingBotGroupList_TextChanged(object sender, EventArgs e)
        {
            if (Regex.IsMatch(SettingBotGroupList.Text, @"^[\d;]+?$"))
            {
                List<long> list = new();
                foreach (string qq in SettingBotGroupList.Text.Split(';'))
                {
                    if (qq.Length is >= 6 and <= 16)
                    {
                        long.TryParse(qq, out long qq_);
                        list.Add(qq_);
                    }
                }
                Global.Settings_Bot.GroupList = list.Distinct().ToList();
            }
            string Text = Regex.Replace(SettingBotGroupList.Text, @"[^\d;]", ";");
            Text = Regex.Replace(Text, @";+", ";");
            Text = Regex.Replace(Text, "^;", string.Empty);
            if (Text != SettingBotGroupList.Text)
            {
                SettingBotGroupList.Text = Text;
                SettingBotGroupList.Focus();
                SettingBotGroupList.Select(SettingBotGroupList.TextLength, 0);
                SettingBotGroupList.ScrollToCaret();
            }

        }
        private void SettingBotPermissionList_TextChanged(object sender, EventArgs e)
        {
            if (Regex.IsMatch(SettingBotPermissionList.Text, @"^[\d;]+?$"))
            {
                List<long> list = new();
                foreach (string qq in SettingBotPermissionList.Text.Split(';'))
                {
                    if (qq.Length is >= 5 and <= 13)
                    {
                        long.TryParse(qq, out long qq_);
                        list.Add(qq_);
                    }
                }
                Global.Settings_Bot.PermissionList = list.Distinct().ToList();
            }
            string Text = Regex.Replace(SettingBotPermissionList.Text, @"[^\d,]", ";");
            Text = Regex.Replace(Text, @";+", ";");
            Text = Regex.Replace(Text, "^;", string.Empty);
            if (Text != SettingBotPermissionList.Text)
            {
                SettingBotPermissionList.Text = Text;
                SettingBotPermissionList.Focus();
                SettingBotPermissionList.Select(SettingBotPermissionList.TextLength, 0);
                SettingBotPermissionList.ScrollToCaret();
            }
        }
        private void SettingSereinEnableGetUpdate_CheckedChanged(object sender, EventArgs e)
        {
            Global.Settings_Serein.EnableGetUpdate = SettingSereinEnableGetUpdate.Checked;
        }
        private void SettingSereinEnableGetAnnouncement_CheckedChanged(object sender, EventArgs e)
        {
            Global.Settings_Serein.EnableGetAnnouncement = SettingSereinEnableGetAnnouncement.Checked;
        }
        private void SettingServerPathSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new()
            {
                Filter = "支持的文件(*.exe *.bat)|*.exe;*.bat"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                SettingServerPath.Text = dialog.FileName;
                Global.Settings_Server.Path = dialog.FileName;
                LoadPlugins();
            }
        }
        private void SettingServerAutoStop_CheckedChanged(object sender, EventArgs e)
        {
            Global.Settings_Server.AutoStop = SettingServerAutoStop.Checked;
        }
        private void SettingServerStopCommand_TextChanged(object sender, EventArgs e)
        {
            Global.Settings_Server.StopCommand = string.IsNullOrEmpty(SettingServerStopCommand.Text) || string.IsNullOrWhiteSpace(SettingServerStopCommand.Text)
                ? "stop"
                : SettingServerStopCommand.Text;
        }

        private void SettingServerStopCommand_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(SettingServerStopCommand.Text) || string.IsNullOrWhiteSpace(SettingServerStopCommand.Text))
            {
                Global.Settings_Server.StopCommand = "stop";
                SettingServerStopCommand.Text = "stop";
            }
        }
        private void SettingSereinAbout_Click(object sender, EventArgs e)
        {
            Process.Start("https://zaitonn.github.io/Serein/About");
        }
        private void SettingSereinPage_Click(object sender, EventArgs e)
        {
            Process.Start("https://zaitonn.github.io/Serein");
        }
        private void SettingSereinHelp_Click(object sender, EventArgs e)
        {
            Process.Start("https://zaitonn.github.io/Serein/Help");
        }

        private void SettingSereinTutorial_Click(object sender, EventArgs e)
        {
            Process.Start("https://zaitonn.github.io/Serein/Tutorial");
        }
        private void SettingSereinDownload_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/Zaitonn/Serein/releases/latest");
        }
    }

}
