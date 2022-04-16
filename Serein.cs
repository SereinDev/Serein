using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Windows.Forms;

namespace Serein
{
    public partial class Serein : Form
    {
        public Serein()
        {
            InitializeComponent();
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "console.html"))
            {
                MessageBox.Show($"文件  {AppDomain.CurrentDomain.BaseDirectory}console.html  已丢失");
                System.Environment.Exit(0);
            }
            string VERSION = "Testing 2022";
            PanelConsoleWebBrowser.Navigate(@"file:\\\" + AppDomain.CurrentDomain.BaseDirectory + "console.html?from=panel");
            BotWebBrowser.Navigate(@"file:\\\" + AppDomain.CurrentDomain.BaseDirectory + "console.html?from=bot");
            Global.PanelConsoleWebBrowser = PanelConsoleWebBrowser;
            Global.BotWebBrowser = BotWebBrowser;
            SettingSereinVersion.Text = $"Version:{VERSION}";
            Settings.ReadSettings();
            LoadSettings();
            Settings.StartSaveSettings();
        }

        public void LoadSettings()
        {
            SettingServerEnableRestart.Checked = Global.Settings_server.EnableRestart;
            SettingServerEnableOutputCommand.Checked = Global.Settings_server.EnableOutputCommand;
            SettingServerEnableLog.Checked = Global.Settings_server.EnableLog;
            SettingServerOutputStyle.SelectedIndex = Global.Settings_server.OutputStyle;
            SettingBotListenPort.Value = Global.Settings_bot.ListenPort;
            SettingBotSendPort.Value = Global.Settings_bot.SendPort;
            SettingBotEnableLog.Checked = Global.Settings_bot.EnableLog;
            SettingBotGivePermissionToAllAdmin.Checked = Global.Settings_bot.GivePermissionToAllAdmin;
            SettingSereinEnableGetUpdate.Checked = Global.Settings_serein.EnableGetUpdate;
            SettingSereinEnableGetAnnouncement.Checked = Global.Settings_serein.EnableGetAnnouncement;
            SettingServerPath.Text = Global.Settings_server.Path;
            SettingBotPath.Text = Global.Settings_bot.Path;
            SettingBotPermissionList.Text = string.Join(",", Global.Settings_bot.PermissionList);
            SettingBotGroupList.Text = string.Join(",", Global.Settings_bot.GroupList);
        }
        private void SettingBotSupportedLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/Mrs4s/go-cqhttp");
        }
        
        delegate void PanelConsoleWebBrowser_Delegate(object[] objects);
        private void PanelConsoleWebBrowser_AppendText(object[] objects)
        {
            PanelConsoleWebBrowser.Document.InvokeScript("AppendText", objects);
            
        }
        public void PanelConsoleWebBrowser_Invoke(string str)
        {
            object[] objects1 = { str };
            object[] objects2 = { objects1 };
            Invoke((PanelConsoleWebBrowser_Delegate)PanelConsoleWebBrowser_AppendText, objects2);
        }
        private void PanelControlStart_Click(object sender, EventArgs e)
        {
            Server.Start();
        }
        private void PanelControlStop_Click(object sender, EventArgs e)
        {
            Server.Stop();
        }
        private void PanelControlRestart_Click(object sender, EventArgs e)
        {
            Server.RestartRequest();
        }
        private void PanelControlKill_Click(object sender, EventArgs e)
        {
            Server.Kill();
        }
        private void PanelConsoleEnter_Click(object sender, EventArgs e)
        {
            Server.InputCommand(PanelConsoleInput.Text);
            PanelConsoleInput.Text = "";
        }
        private void PanelConsoleInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Server.InputCommand(PanelConsoleInput.Text);
                PanelConsoleInput.Text = "";
            }
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
        private void SettingBotListenPort_ValueChanged(object sender, EventArgs e)
        {
            Global.Settings_bot.ListenPort = (int)SettingBotListenPort.Value;
        }
        private void SettingBotSendPort_ValueChanged(object sender, EventArgs e)
        {
            Global.Settings_bot.SendPort = (int)SettingBotSendPort.Value;
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
                Global.Settings_bot.GroupList = Array.ConvertAll(
                    SettingBotGroupList.Text.Split(','),
                    s => int.TryParse(s, out int i) ? i : 0
                    );
            }
            else
            {
                SettingBotGroupList.Text = Regex.Replace(SettingBotGroupList.Text, @"[^\d,]","");
                SettingBotGroupList.Focus();
                SettingBotGroupList.Select(SettingBotGroupList.TextLength, 0);
                SettingBotGroupList.ScrollToCaret();
            }
        }
        private void SettingBotPermissionList_TextChanged(object sender, EventArgs e)
        {
            if (Regex.IsMatch(SettingBotPermissionList.Text, @"^[\d,]+?$"))
            {
                Global.Settings_bot.PermissionList = Array.ConvertAll(
                    SettingBotPermissionList.Text.Split(','),
                    s => int.TryParse(s, out int i) ? i : 0
                    );
            }
            else
            {
                SettingBotPermissionList.Text = Regex.Replace(SettingBotPermissionList.Text, @"[^\d,]", "");
                SettingBotPermissionList.Focus();
                SettingBotPermissionList.Select(SettingBotPermissionList.TextLength, 0);
                SettingBotPermissionList.ScrollToCaret();
            }
        }
        private void SettingSereinEnableGetUpdate_CheckedChanged(object sender, EventArgs e)
        {
            Global.Settings_serein.EnableGetUpdate = SettingSereinEnableGetUpdate.Checked;
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
            }
        }
        private void SettingBotPathSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "go-cqhttp可执行文件|go-cqhttp.exe;go-cqhttp_windows_armv7.exe;go-cqhttp_windows_386.exe;go-cqhttp_windows_arm64.exe;go-cqhttp_windows_amd64.exe"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                SettingBotPath.Text = dialog.FileName;
                Global.Settings_bot.Path = dialog.FileName;

            }
        }
    }
}
