using Ookii.Dialogs.Wpf;
using Serein.Settings;
using Serein.Ui.ChildrenWindow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Serein.Ui
{
    public partial class Ui : Form
    {
        private void LoadSettings()
        {
            SettingServerEnableRestart.Checked = Global.Settings.Server.EnableRestart;
            SettingServerEnableOutputCommand.Checked = Global.Settings.Server.EnableOutputCommand;
            SettingServerEnableLog.Checked = Global.Settings.Server.EnableLog;
            SettingServerEnableUnicode.Checked = Global.Settings.Server.EnableUnicode;
            SettingServerOutputStyle.SelectedIndex = Global.Settings.Server.OutputStyle;
            SettingServerEncoding.SelectedIndex = Global.Settings.Server.InputEncoding;
            SettingServerAutoStop.Checked = Global.Settings.Server.AutoStop;
            SettingServerPath.Text = Global.Settings.Server.Path;
            SettingServerStopCommand.Text = string.Join(";", Global.Settings.Server.StopCommands);
            SettingServerPort.Value = Global.Settings.Server.Port;
            SettingServerType.SelectedIndex = Global.Settings.Server.Type;
            SettingBotPermissionList.Text = string.Join(";", Global.Settings.Bot.PermissionList);
            SettingBotGroupList.Text = string.Join(";", Global.Settings.Bot.GroupList);
            SettingBotUri.Text = Global.Settings.Bot.Uri;
            SettingBotAuthorization.Text = Regex.Replace(Global.Settings.Bot.Authorization, ".", "*");
            SettingBotEnbaleOutputData.Checked = Global.Settings.Bot.EnbaleOutputData;
            SettingBotEnableLog.Checked = Global.Settings.Bot.EnableLog;
            SettingBotAutoEscape.Checked = Global.Settings.Bot.AutoEscape;
            SettingBotRestart.Checked = Global.Settings.Bot.AutoReconnect;
            SettingBotGivePermissionToAllAdmin.Checked = Global.Settings.Bot.GivePermissionToAllAdmin;
            SettingSereinEnableGetUpdate.Checked = Global.Settings.Serein.EnableGetUpdate;
            SettingSereinEnableDPIAware.Checked = Global.Settings.Serein.DPIAware;
            if (!Global.Settings.Serein.Debug)
            {
                Debug.Parent = null;
            }
        }

        private void SettingServerEnableRestart_CheckedChanged(object sender, EventArgs e) => Global.Settings.Server.EnableRestart = SettingServerEnableRestart.Checked;
        private void SettingServerEnableOutputCommand_CheckedChanged(object sender, EventArgs e) => Global.Settings.Server.EnableOutputCommand = SettingServerEnableOutputCommand.Checked;
        private void SettingServerEnableLog_CheckedChanged(object sender, EventArgs e) => Global.Settings.Server.EnableLog = SettingServerEnableLog.Checked;
        private void SettingServerEnableUnicode_CheckedChanged(object sender, EventArgs e) => Global.Settings.Server.EnableUnicode = SettingServerEnableUnicode.Checked;
        private void SettingServerOutputStyle_SelectedIndexChanged(object sender, EventArgs e) => Global.Settings.Server.OutputStyle = SettingServerOutputStyle.SelectedIndex;
        private void SettingServerEncoding_SelectedValueChanged(object sender, EventArgs e) => Global.Settings.Server.InputEncoding = SettingServerEncoding.SelectedIndex;
        private void SettingServerPort_ValueChanged(object sender, EventArgs e) => Global.Settings.Server.Port = (int)SettingServerPort.Value;
        private void SettingServerType_SelectedIndexChanged(object sender, EventArgs e) => Global.Settings.Server.Type = SettingServerType.SelectedIndex;
        private void SettingBotUri_TextChanged(object sender, EventArgs e) => Global.Settings.Bot.Uri = SettingBotUri.Text;
        private void SettingBotAuthorization_Enter(object sender, EventArgs e) => SettingBotAuthorization.Text = Global.Settings.Bot.Authorization;
        private void SettingBotEnableLog_CheckedChanged(object sender, EventArgs e) => Global.Settings.Bot.EnableLog = SettingBotEnableLog.Checked;
        private void SettingBotGivePermissionToAllAdmin_CheckedChanged(object sender, EventArgs e) => Global.Settings.Bot.GivePermissionToAllAdmin = SettingBotGivePermissionToAllAdmin.Checked;
        private void SettingBotEnbaleOutputData_CheckedChanged(object sender, EventArgs e) => Global.Settings.Bot.EnbaleOutputData = SettingBotEnbaleOutputData.Checked;
        private void SettingBotRestart_CheckedChanged(object sender, EventArgs e) => Global.Settings.Bot.AutoReconnect = SettingBotRestart.Checked;
        private void SettingBotAutoEscape_CheckedChanged(object sender, EventArgs e) => Global.Settings.Bot.AutoEscape = SettingBotAutoEscape.Checked;
        private void SettingSereinEnableGetUpdate_CheckedChanged(object sender, EventArgs e) => Global.Settings.Serein.EnableGetUpdate = SettingSereinEnableGetUpdate.Checked;
        private void SettingServerAutoStop_CheckedChanged(object sender, EventArgs e) => Global.Settings.Server.AutoStop = SettingServerAutoStop.Checked;
        private void SettingSereinAbout_Click(object sender, EventArgs e) => Process.Start(new ProcessStartInfo("https://serein.cc/#/About") { UseShellExecute = true });
        private void SettingSereinPage_Click(object sender, EventArgs e) => Process.Start(new ProcessStartInfo("https://serein.cc") { UseShellExecute = true });
        private void SettingSereinTutorial_Click(object sender, EventArgs e) => Process.Start(new ProcessStartInfo("https://serein.cc/#/Tutorial/README") { UseShellExecute = true });
        private void SettingSereinDownload_Click(object sender, EventArgs e) => Process.Start(new ProcessStartInfo("https://github.com/Zaitonn/Serein/releases/latest") { UseShellExecute = true });
        private void SettingEventListContextMenuStrip_Docs_Click(object sender, EventArgs e) => Process.Start(new ProcessStartInfo("https://serein.cc/#/Function/Event") { UseShellExecute = true });
        private void SettingSereinEnableDPIAware_CheckedChanged(object sender, EventArgs e) => Global.Settings.Serein.DPIAware = SettingSereinEnableDPIAware.Checked;

        private void SettingBotAuthorization_Leave(object sender, EventArgs e)
        {
            Global.Settings.Bot.Authorization = SettingBotAuthorization.Text;
            SettingBotAuthorization.Text = Regex.Replace(SettingBotAuthorization.Text, ".", "*");
        }

        private void SettingBotGroupList_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(SettingBotGroupList.Text))
            {
                Global.Settings.Bot.GroupList.Clear();
            }
            else if (Regex.IsMatch(SettingBotGroupList.Text, @"^[\d;]+?$"))
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
            if (string.IsNullOrEmpty(SettingBotPermissionList.Text))
            {
                Global.Settings.Bot.PermissionList.Clear();
            }
            else if (Regex.IsMatch(SettingBotPermissionList.Text, @"^[\d;]+?$"))
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

        private void SettingServerStopCommand_TextChanged(object sender, EventArgs e)
        {
            Global.Settings.Server.StopCommands = SettingServerStopCommand.Text.Split(';');
        }

        private void SettingServerStopCommand_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(SettingServerStopCommand.Text) || string.IsNullOrWhiteSpace(SettingServerStopCommand.Text))
            {
                Global.Settings.Server.StopCommands = new[] { "stop" };
                SettingServerStopCommand.Text = "stop";
            }
        }

        private void SettingEventTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node == null)
            {
                return;
            }
            string[] TargetEvent = new string[] { };
            switch (e.Node.Name)
            {
                case "BindSuccess":
                    TargetEvent = Global.Settings.Event.Bind_Success;
                    break;
                case "BindOccupied":
                    TargetEvent = Global.Settings.Event.Bind_Occupied;
                    break;
                case "BindInvalid":
                    TargetEvent = Global.Settings.Event.Bind_Invalid;
                    break;
                case "BindAlready":
                    TargetEvent = Global.Settings.Event.Bind_Already;
                    break;
                case "UnbindSuccess":
                    TargetEvent = Global.Settings.Event.Unbind_Success;
                    break;
                case "UnbindFailure":
                    TargetEvent = Global.Settings.Event.Unbind_Failure;
                    break;
                case "ServerStart":
                    TargetEvent = Global.Settings.Event.Server_Start;
                    break;
                case "ServerStop":
                    TargetEvent = Global.Settings.Event.Server_Stop;
                    break;
                case "ServerError":
                    TargetEvent = Global.Settings.Event.Server_Error;
                    break;
                case "GroupIncrease":
                    TargetEvent = Global.Settings.Event.Group_Increase;
                    break;
                case "GroupDecrease":
                    TargetEvent = Global.Settings.Event.Group_Decrease;
                    break;
                case "GroupPoke":
                    TargetEvent = Global.Settings.Event.Group_Poke;
                    break;
                case "SereinCrash":
                    TargetEvent = Global.Settings.Event.Serein_Crash;
                    break;
                case "MotdpeSuccess":
                    TargetEvent = Global.Settings.Event.Motdpe_Success;
                    break;
                case "MotdjeSuccess":
                    TargetEvent = Global.Settings.Event.Motdje_Success;
                    break;
                case "MotdFailure":
                    TargetEvent = Global.Settings.Event.Motd_Failure;
                    break;
                case "PermissionDeniedPrivate":
                    TargetEvent = Global.Settings.Event.PermissionDenied_Private;
                    break;
                case "PermissionDeniedGroup":
                    TargetEvent = Global.Settings.Event.PermissionDenied_Group;
                    break;
            }
            SettingEventList.BeginUpdate();
            SettingEventList.Items.Clear();
            TargetEvent.ToList().ForEach((Command) => { SettingEventList.Items.Add(Regex.Replace(Command, @"(\n|\r|\\n|\\r)+", "\\n")); });
            SettingEventList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            SettingEventList.EndUpdate();
        }

        private void SettingEventListContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            SettingEventListContextMenuStrip_Edit.Enabled = SettingEventList.SelectedItems.Count > 0;
            SettingEventListContextMenuStrip_Remove.Enabled = SettingEventList.SelectedItems.Count > 0;
            SettingEventListContextMenuStrip_Add.Enabled = false;
            if (SettingEventTreeView.SelectedNode != null)
            {
                typeof(Event).GetProperties().ToList().ForEach((x) =>
                {
                    if (!SettingEventListContextMenuStrip_Add.Enabled && x.Name.Replace("_", "") == SettingEventTreeView.SelectedNode.Name)
                    {
                        SettingEventListContextMenuStrip_Add.Enabled = true;
                    }
                });
            }
        }

        private void SettingEventListContextMenuStrip_Add_Click(object sender, EventArgs e)
        {
            EventEditor Editor = new EventEditor();
            Editor.ShowDialog();
            if (!Editor.CancelFlag)
            {
                if (SettingEventList.SelectedItems.Count > 0)
                {
                    SettingEventList.Items.Insert(SettingEventList.SelectedItems[0].Index + 1, Regex.Replace(Editor.CommandTextBox.Text, @"(\n|\r|\\n|\\r)+", "\\n"));
                }
                else
                {
                    SettingEventList.Items.Add(Regex.Replace(Editor.CommandTextBox.Text, @"(\n|\r|\\n|\\r)+", "\\n"));
                }
            }
            SaveEventCommand();
        }

        private void SettingEventListContextMenuStrip_Edit_Click(object sender, EventArgs e)
        {
            EventEditor Editor = new EventEditor(Regex.Replace(SettingEventList.SelectedItems[0].Text, "\\n", "\r\n"));
            Editor.ShowDialog();
            if (!Editor.CancelFlag)
            {
                SettingEventList.SelectedItems[0].Text = Regex.Replace(Editor.CommandTextBox.Text, @"(\n|\r|\\n|\\r)+", "\\n");
            }
            SaveEventCommand();
        }

        private void SettingEventListContextMenuStrip_Remove_Click(object sender, EventArgs e)
        {
            if ((int)MessageBox.Show(
                    "确定删除此行命令？\n" +
                    "它将会永远失去！（真的很久！）", "Serein",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Information
                    ) == 1)
            {
                SettingEventList.SelectedItems[0].Remove();
                SaveEventCommand();
            }
        }

        private void SaveEventCommand()
        {
            if (SettingEventTreeView.SelectedNode != null)
            {
                bool Available = false;
                typeof(Event).GetProperties().ToList().ForEach((x) =>
                {
                    if (!Available && x.Name.Replace("_", "") == SettingEventTreeView.SelectedNode.Name)
                    {
                        Available = true;
                    }
                });
                if (Available)
                {
                    switch (SettingEventTreeView.SelectedNode.Name)
                    {
                        case "BindSuccess":
                            Global.Settings.Event.Bind_Success = GetEventCommands();
                            break;
                        case "BindOccupied":
                            Global.Settings.Event.Bind_Occupied = GetEventCommands();
                            break;
                        case "BindInvalid":
                            Global.Settings.Event.Bind_Invalid = GetEventCommands();
                            break;
                        case "BindAlready":
                            Global.Settings.Event.Bind_Already = GetEventCommands();
                            break;
                        case "UnbindSuccess":
                            Global.Settings.Event.Unbind_Success = GetEventCommands();
                            break;
                        case "UnbindFailure":
                            Global.Settings.Event.Unbind_Failure = GetEventCommands();
                            break;
                        case "ServerStart":
                            Global.Settings.Event.Server_Start = GetEventCommands();
                            break;
                        case "ServerStop":
                            Global.Settings.Event.Server_Stop = GetEventCommands();
                            break;
                        case "ServerError":
                            Global.Settings.Event.Server_Error = GetEventCommands();
                            break;
                        case "GroupIncrease":
                            Global.Settings.Event.Group_Increase = GetEventCommands();
                            break;
                        case "GroupDecrease":
                            Global.Settings.Event.Group_Decrease = GetEventCommands();
                            break;
                        case "GroupPoke":
                            Global.Settings.Event.Group_Poke = GetEventCommands();
                            break;
                        case "SereinCrash":
                            Global.Settings.Event.Serein_Crash = GetEventCommands();
                            break;
                        case "MotdpeSuccess":
                            Global.Settings.Event.Motdpe_Success = GetEventCommands();
                            break;
                        case "MotdjeSuccess":
                            Global.Settings.Event.Motdje_Success = GetEventCommands();
                            break;
                        case "MotdFailure":
                            Global.Settings.Event.Motd_Failure = GetEventCommands();
                            break;
                        case "PermissionDeniedPrivate":
                            Global.Settings.Event.PermissionDenied_Private = GetEventCommands();
                            break;
                        case "PermissionDeniedGroup":
                            Global.Settings.Event.PermissionDenied_Group = GetEventCommands();
                            break;
                    }
                }
            }
        }

        private string[] GetEventCommands()
        {
            List<string> Commands = new List<string>();
            foreach (ListViewItem Item in SettingEventList.Items)
            {
                Commands.Add(Item.Text.Replace("\\n", "\n"));
            }
            return Commands.ToArray();
        }

        private void SettingSereinVersion_Click(object sender, EventArgs e)
        {
            Ookii.Dialogs.Wpf.TaskDialog TaskDialog = new Ookii.Dialogs.Wpf.TaskDialog
            {
                Buttons = {
                        new Ookii.Dialogs.Wpf.TaskDialogButton(ButtonType.Ok)
                    },
                MainInstruction = "详细信息",
                WindowTitle = "Serein",
                Content = "" +
                    $"当前版本：{Global.VERSION}\n" +
                    $"编译类型：{Global.BuildInfo.Type}\n" +
                    $"编译时间：{Global.BuildInfo.Time}\n" +
                    $"编译路径：{Global.BuildInfo.Dir}\n" +
                    $"系统类型：{Global.BuildInfo.OS}\n" +
                    $"详细信息：{Global.BuildInfo.Detail.Replace("\r", string.Empty)}",
                Footer = "开源地址：<a href=\"https://github.com/Zaitonn/Serein\">GitHub</a>",
                FooterIcon = Ookii.Dialogs.Wpf.TaskDialogIcon.Information,
                EnableHyperlinks = true
            };
            TaskDialog.HyperlinkClicked += (sneder, _e) => Process.Start(new ProcessStartInfo(_e.Href) { UseShellExecute = true });
            TaskDialog.ShowDialog();
        }
    }
}
