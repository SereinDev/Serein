using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Serein.Ui
{
    public partial class Ui : Form
    {
        public void LoadSettings()
        {
            SettingServerEnableRestart.Checked = Global.Settings.Server.EnableRestart;
            SettingServerEnableOutputCommand.Checked = Global.Settings.Server.EnableOutputCommand;
            SettingServerEnableLog.Checked = Global.Settings.Server.EnableLog;
            SettingServerEnableUnicode.Checked = Global.Settings.Server.EnableUnicode;
            SettingServerOutputStyle.SelectedIndex = Global.Settings.Server.OutputStyle;
            SettingServerEncoding.SelectedIndex = Global.Settings.Server.EncodingIndex;
            SettingServerAutoStop.Checked = Global.Settings.Server.AutoStop;
            SettingServerPath.Text = Global.Settings.Server.Path;
            SettingServerStopCommand.Text = Global.Settings.Server.StopCommand;
            SettingServerPort.Value = Global.Settings.Server.Port;
            SettingServerType.SelectedIndex = Global.Settings.Server.Type;
            SettingBotPermissionList.Text = string.Join(";", Global.Settings.Bot.PermissionList);
            SettingBotGroupList.Text = string.Join(";", Global.Settings.Bot.GroupList);
            SettingBotUri.Text = Global.Settings.Bot.Uri;
            SettingBotAuthorization.Text = Regex.Replace(Global.Settings.Bot.Authorization, ".", "*");
            SettingBotEnbaleOutputData.Checked = Global.Settings.Bot.EnbaleOutputData;
            SettingBotEnableLog.Checked = Global.Settings.Bot.EnableLog;
            SettingBotRestart.Checked = Global.Settings.Bot.Restart;
            SettingBotGivePermissionToAllAdmin.Checked = Global.Settings.Bot.GivePermissionToAllAdmin;
            SettingSereinEnableGetUpdate.Checked = Global.Settings.Serein.EnableGetUpdate;
            SettingSereinEnableGetAnnouncement.Checked = Global.Settings.Serein.EnableGetAnnouncement;
            SettingSereinEnableDPIAware.Checked = Global.Settings.Serein.DPIAware;
            if (!Global.Settings.Serein.Debug)
            {
                Debug.Parent = null;
            }
        }
        private void SettingServerEnableRestart_CheckedChanged(object sender, EventArgs e)
        {
            Global.Settings.Server.EnableRestart = SettingServerEnableRestart.Checked;
        }
        private void SettingServerEnableOutputCommand_CheckedChanged(object sender, EventArgs e)
        {
            Global.Settings.Server.EnableOutputCommand = SettingServerEnableOutputCommand.Checked;
        }
        private void SettingServerEnableLog_CheckedChanged(object sender, EventArgs e)
        {
            Global.Settings.Server.EnableLog = SettingServerEnableLog.Checked;
        }
        private void SettingServerEnableUnicode_CheckedChanged(object sender, EventArgs e)
        {
            Global.Settings.Server.EnableUnicode = SettingServerEnableUnicode.Checked;
        }
        private void SettingServerOutputStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            Global.Settings.Server.OutputStyle = SettingServerOutputStyle.SelectedIndex;
        }
        private void SettingServerEncoding_SelectedValueChanged(object sender, EventArgs e)
        {
            Global.Settings.Server.EncodingIndex = SettingServerEncoding.SelectedIndex;
        }
        private void SettingServerPort_ValueChanged(object sender, EventArgs e)
        {
            Global.Settings.Server.Port = (int)SettingServerPort.Value;
        }
        private void SettingServerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Global.Settings.Server.Type = SettingServerType.SelectedIndex;
        }
        private void SettingBotUri_TextChanged(object sender, EventArgs e)
        {
            Global.Settings.Bot.Uri = SettingBotUri.Text;
        }
        private void SettingBotAuthorization_Enter(object sender, EventArgs e)
        {
            SettingBotAuthorization.Text = Global.Settings.Bot.Authorization;
        }
        private void SettingBotAuthorization_Leave(object sender, EventArgs e)
        {
            Global.Settings.Bot.Authorization = SettingBotAuthorization.Text;
            SettingBotAuthorization.Text = Regex.Replace(SettingBotAuthorization.Text, ".", "*");
        }
        private void SettingBotEnableLog_CheckedChanged(object sender, EventArgs e)
        {
            Global.Settings.Bot.EnableLog = SettingBotEnableLog.Checked;
        }
        private void SettingBotGivePermissionToAllAdmin_CheckedChanged(object sender, EventArgs e)
        {
            Global.Settings.Bot.GivePermissionToAllAdmin = SettingBotGivePermissionToAllAdmin.Checked;
        }
        private void SettingBotEnbaleOutputData_CheckedChanged(object sender, EventArgs e)
        {
            Global.Settings.Bot.EnbaleOutputData = SettingBotEnbaleOutputData.Checked;
        }
        private void SettingBotRestart_CheckedChanged(object sender, EventArgs e)
        {
            Global.Settings.Bot.Restart = SettingBotRestart.Checked;
        }
        private void SettingBotGroupList_TextChanged(object sender, EventArgs e)
        {
            if (Regex.IsMatch(SettingBotGroupList.Text, @"^[\d;]+?$"))
            {
                List<long> list = new List<long>();
                foreach (string qq in SettingBotGroupList.Text.Split(';'))
                {
                    if (qq.Length >= 6 && qq.Length <= 16)
                    {
                        long.TryParse(qq, out long qq_);
                        list.Add(qq_);
                    }
                }
                Global.Settings.Bot.GroupList = list;
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
                List<long> list = new List<long>();
                foreach (string qq in SettingBotPermissionList.Text.Split(';'))
                {
                    if (qq.Length >= 5 && qq.Length <= 13)
                    {
                        long.TryParse(qq, out long qq_);
                        list.Add(qq_);
                    }
                }
                Global.Settings.Bot.PermissionList = list;
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
            Global.Settings.Serein.EnableGetUpdate = SettingSereinEnableGetUpdate.Checked;
        }
        private void SettingSereinEnableGetAnnouncement_CheckedChanged(object sender, EventArgs e)
        {
            Global.Settings.Serein.EnableGetAnnouncement = SettingSereinEnableGetAnnouncement.Checked;
        }
        private void SettingServerPathSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                Filter = "支持的文件(*.exe *.bat)|*.exe;*.bat"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                SettingServerPath.Text = dialog.FileName;
                Global.Settings.Server.Path = dialog.FileName;
                LoadPlugins();
            }
        }
        private void SettingServerAutoStop_CheckedChanged(object sender, EventArgs e)
        {
            Global.Settings.Server.AutoStop = SettingServerAutoStop.Checked;
        }
        private void SettingServerStopCommand_TextChanged(object sender, EventArgs e)
        {
            Global.Settings.Server.StopCommand = string.IsNullOrEmpty(SettingServerStopCommand.Text) || string.IsNullOrWhiteSpace(SettingServerStopCommand.Text)
                ? "stop"
                : SettingServerStopCommand.Text;
        }

        private void SettingServerStopCommand_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(SettingServerStopCommand.Text) || string.IsNullOrWhiteSpace(SettingServerStopCommand.Text))
            {
                Global.Settings.Server.StopCommand = "stop";
                SettingServerStopCommand.Text = "stop";
            }
        }
        private void SettingSereinAbout_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://zaitonn.github.io/Serein/About") { UseShellExecute = true });
        }
        private void SettingSereinPage_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://zaitonn.github.io/Serein") { UseShellExecute = true });
        }
        private void SettingSereinHelp_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://zaitonn.github.io/Serein/Help") { UseShellExecute = true });
        }
        private void SettingSereinTutorial_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://zaitonn.github.io/Serein/Tutorial") { UseShellExecute = true });
        }
        private void SettingSereinDownload_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://github.com/Zaitonn/Serein/releases/latest") { UseShellExecute = true });
        }
        private void SettingSereinEnableDPIAware_CheckedChanged(object sender, EventArgs e)
        {
            Global.Settings.Serein.DPIAware = SettingSereinEnableDPIAware.Checked;
        }
    }
}
