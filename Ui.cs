using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Windows.Forms;

namespace Serein
{
    public partial class Ui : Form
    {
        public Ui()
        {
            InitializeComponent();
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "console.html"))
            {
                MessageBox.Show($"文件  {AppDomain.CurrentDomain.BaseDirectory}console.html  已丢失");
                Environment.Exit(0);
            }
            PanelConsoleWebBrowser.Navigate(@"file:\\\" + AppDomain.CurrentDomain.BaseDirectory + "console.html?from=panel");
            BotWebBrowser.Navigate(@"file:\\\" + AppDomain.CurrentDomain.BaseDirectory + "console.html?from=bot");
            Global.PanelConsoleWebBrowser = PanelConsoleWebBrowser;
            Global.BotWebBrowser = BotWebBrowser;
            SettingSereinVersion.Text = $"Version:{Global.VERSION}";
            Settings.ReadSettings();
            LoadSettings();
            Settings.StartSaveSettings();
            ShowBalloonTip($"Hello!\n这是测试版（{Global.VERSION}），不建议用于生产环境哦qwq");
            LoadPlugins();
        }
        public void LoadPlugins()
        {
            if (Plugins.GetPluglin() != null) 
            {
                PluginList.BeginUpdate();
                PluginList.Clear();
                string[] Files = Plugins.GetPluglin();
                ListViewGroup PluginGroupJs = new ListViewGroup("Js", HorizontalAlignment.Left);
                ListViewGroup PluginGroupDll = new ListViewGroup("Dll", HorizontalAlignment.Left);
                ListViewGroup PluginGroupJar = new ListViewGroup("Jar", HorizontalAlignment.Left);
                ListViewGroup PluginGroupPy = new ListViewGroup("Py", HorizontalAlignment.Left);
                ListViewGroup PluginGroupGo = new ListViewGroup("Go", HorizontalAlignment.Left);
                PluginList.Groups.Add(PluginGroupJs);
                PluginList.Groups.Add(PluginGroupDll);
                PluginList.Groups.Add(PluginGroupJar);
                PluginList.Groups.Add(PluginGroupPy);
                PluginList.Groups.Add(PluginGroupGo);
                PluginList.Groups.Add(PluginGroupJs);
                foreach (string PluginFile in Files)
                {
                    string PluginName = Path.GetFileName(PluginFile);
                    ListViewItem Item = new ListViewItem();
                    Item.Text = PluginName;
                    bool added = true;
                    if (PluginFile.ToUpper().EndsWith(".JS"))
                    {
                        PluginGroupJs.Items.Add(Item);
                    }
                    else if (PluginFile.ToUpper().EndsWith(".DLL"))
                    {
                        PluginGroupDll.Items.Add(Item);
                    }
                    else if (PluginFile.ToUpper().EndsWith(".JAR"))
                    {
                        PluginGroupJar.Items.Add(Item);
                    }
                    else if (PluginFile.ToUpper().EndsWith(".PY"))
                    {
                        PluginGroupPy.Items.Add(Item);
                    }
                    else if (PluginFile.ToUpper().EndsWith(".GO"))
                    {
                        PluginGroupGo.Items.Add(Item);
                    }
                    else
                    {
                        added = false;
                    }
                    if (added)
                    {
                        PluginList.Items.Add(Item);
                    }
                }
                PluginList.EndUpdate();
            }
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
            PanelConsoleInput.Clear();
        }
        private void PanelConsoleInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                Server.InputCommand(PanelConsoleInput.Text);
                PanelConsoleInput.Clear();
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
                List<long> list= new List<long>();
                foreach(string qq in SettingBotPermissionList.Text.Split(','))
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
            SettingBotPermissionList.Text= Regex.Replace(SettingBotPermissionList.Text, @",+", ",");
            SettingBotPermissionList.Text= Regex.Replace(SettingBotPermissionList.Text, @"^,", "");
            SettingBotPermissionList.Focus();
            SettingBotPermissionList.Select(SettingBotPermissionList.TextLength, 0);
            SettingBotPermissionList.ScrollToCaret();
            
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
        private void SereinIcon_MouseClick(object sender, MouseEventArgs e)
        {
            Visible = true;
            ShowInTaskbar = true;
            WindowState = FormWindowState.Normal;
            Activate();
        }
        private void Serein_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Server.Status)
            {
                e.Cancel = true;
                Visible = false;
                ShowInTaskbar = false;
                ShowBalloonTip("服务器进程仍在运行中\n(已自动最小化至托盘，点击托盘图标即可复原窗口)");
                //MessageBox.Show("服务器进程仍在运行中", "Serein", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void ShowBalloonTip(string text)
        {
            SereinIcon.BalloonTipTitle = "Serein";
            SereinIcon.BalloonTipText = text;
            SereinIcon.ShowBalloonTip(10000);
        }

        private void SereinIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            Visible = true;
            ShowInTaskbar = true;
            WindowState = FormWindowState.Normal;
            Activate();
        }

        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadPlugins();
        }
    }
}
