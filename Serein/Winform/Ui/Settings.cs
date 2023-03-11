using Ookii.Dialogs.Wpf;
using Serein.Utils;
using Serein.Settings;
using Serein.Ui.ChildrenWindow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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
            SettingServerOutputEncoding.SelectedIndex = Global.Settings.Server.OutputEncoding;
            SettingServerAutoStop.Checked = Global.Settings.Server.AutoStop;
            SettingServerPath.Text = Global.Settings.Server.Path;
            SettingServerStopCommand.Text = string.Join(";", Global.Settings.Server.StopCommands);
            SettingServerPort.Value = Global.Settings.Server.Port;
            SettingServerType.SelectedIndex = Global.Settings.Server.Type;
            SettingServerLineTerminator.Text = Global.Settings.Server.LineTerminator.Replace("\r", "\\r").Replace("\n", "\\n");
            SettingBotPermissionList.Text = string.Join(";", Global.Settings.Bot.PermissionList);
            SettingBotGroupList.Text = string.Join(";", Global.Settings.Bot.GroupList);
            SettingBotUri.Text = Global.Settings.Bot.Uri;
            SettingBotAuthorization.Text = Regex.Replace(Global.Settings.Bot.Authorization, ".", "*");
            SettingBotEnbaleOutputData.Checked = Global.Settings.Bot.EnbaleOutputData;
            SettingBotEnableLog.Checked = Global.Settings.Bot.EnableLog;
            SettingBotAutoEscape.Checked = Global.Settings.Bot.AutoEscape;
            SettingBotRestart.Checked = Global.Settings.Bot.AutoReconnect;
            SettingBotGivePermissionToAllAdmin.Checked = Global.Settings.Bot.GivePermissionToAllAdmin;
            SettingBotEnbaleParseAt.Checked = Global.Settings.Bot.EnbaleParseAt;
            SettingSereinEnableGetUpdate.Checked = Global.Settings.Serein.EnableGetUpdate;
            SettingSereinAutoUpdate.Checked = Global.Settings.Serein.AutoUpdate;
            SettingSereinAutoUpdate.Enabled = SettingSereinEnableGetUpdate.Checked;
            SettingSereinEnableDPIAware.Checked = Global.Settings.Serein.DPIAware;
            if (!Global.Settings.Serein.DevelopmentTool.EnableDebug)
            {
                Debug.Parent = null;
            }
            ServerPanel.Parent = Global.Settings.Serein.PagesDisplayed.ServerPanel ? ServerPanel.Parent : null;
            ServerPluginManager.Parent = Global.Settings.Serein.PagesDisplayed.ServerPluginManager ? ServerPluginManager.Parent : null;
            Regular.Parent = Global.Settings.Serein.PagesDisplayed.RegexList ? Regular.Parent : null;
            Schedule.Parent = Global.Settings.Serein.PagesDisplayed.Schedule ? Schedule.Parent : null;
            Bot.Parent = Global.Settings.Serein.PagesDisplayed.Bot ? Bot.Parent : null;
            JSPlugin.Parent = Global.Settings.Serein.PagesDisplayed.JSPlugin ? JSPlugin.Parent : null;
            Member.Parent = Global.Settings.Serein.PagesDisplayed.Member ? Member.Parent : null;
            Setting.Parent = Global.Settings.Serein.PagesDisplayed.Settings ? Setting.Parent : null;
        }

        #region 服务器
        private void SettingServerEnableRestart_CheckedChanged(object sender, EventArgs e)
           => Global.Settings.Server.EnableRestart = SettingServerEnableRestart.Checked;
        private void SettingServerEnableOutputCommand_CheckedChanged(object sender, EventArgs e)
            => Global.Settings.Server.EnableOutputCommand = SettingServerEnableOutputCommand.Checked;
        private void SettingServerEnableLog_CheckedChanged(object sender, EventArgs e)
            => Global.Settings.Server.EnableLog = SettingServerEnableLog.Checked;
        private void SettingServerEnableUnicode_CheckedChanged(object sender, EventArgs e)
            => Global.Settings.Server.EnableUnicode = SettingServerEnableUnicode.Checked;
        private void SettingServerOutputStyle_SelectedIndexChanged(object sender, EventArgs e)
            => Global.Settings.Server.OutputStyle = SettingServerOutputStyle.SelectedIndex;
        private void SettingServerOutputEncoding_SelectedIndexChanged(object sender, EventArgs e)
            => Global.Settings.Server.OutputEncoding = SettingServerOutputEncoding.SelectedIndex;
        private void SettingServerEncoding_SelectedValueChanged(object sender, EventArgs e)
            => Global.Settings.Server.InputEncoding = SettingServerEncoding.SelectedIndex;
        private void SettingServerPort_ValueChanged(object sender, EventArgs e)
            => Global.Settings.Server.Port = (int)SettingServerPort.Value;
        private void SettingServerAutoStop_CheckedChanged(object sender, EventArgs e)
            => Global.Settings.Server.AutoStop = SettingServerAutoStop.Checked;
        private void SettingServerType_SelectedIndexChanged(object sender, EventArgs e)
            => Global.Settings.Server.Type = SettingServerType.SelectedIndex;
        private void SettingServerLineTerminator_TextChanged(object sender, EventArgs e)
            => Global.Settings.Server.LineTerminator = SettingServerLineTerminator.Text.Replace("\\r", "\r").Replace("\\n", "\n");

        private void SettingServerPathSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new()
            {
                InitialDirectory = !string.IsNullOrEmpty(Global.Settings.Server.Path) && File.Exists(Global.Settings.Server.Path) ? Global.Settings.Server.Path : Global.PATH,
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

        #endregion
        #region Serein
        private void SettingSereinEnableGetUpdate_Click(object sender, EventArgs e)
        {
            Global.Settings.Serein.EnableGetUpdate = SettingSereinEnableGetUpdate.Checked;
            SettingSereinAutoUpdate.Checked = SettingSereinEnableGetUpdate.Checked ? SettingSereinAutoUpdate.Checked : false;
            SettingSereinAutoUpdate.Enabled = SettingSereinEnableGetUpdate.Checked;
            Global.Settings.Serein.AutoUpdate = SettingSereinAutoUpdate.Checked;
        }
        private void SettingSereinAutoUpdate_Click(object sender, EventArgs e)
            => Global.Settings.Serein.AutoUpdate = SettingSereinAutoUpdate.Checked;
        private void SettingSereinAbout_Click(object sender, EventArgs e)
            => Process.Start(new ProcessStartInfo("https://serein.cc/#/More/About") { UseShellExecute = true });
        private void SettingSereinPage_Click(object sender, EventArgs e)
            => Process.Start(new ProcessStartInfo("https://serein.cc") { UseShellExecute = true });
        private void SettingSereinExtension_Click(object sender, EventArgs e)
            => Process.Start(new ProcessStartInfo("https://serein.cc/Extension") { UseShellExecute = true });
        private void SettingSereinDownload_Click(object sender, EventArgs e)
            => Process.Start(new ProcessStartInfo("https://github.com/Zaitonn/Serein/releases/latest") { UseShellExecute = true });
        private void SettingEventListContextMenuStrip_Docs_Click(object sender, EventArgs e)
            => Process.Start(new ProcessStartInfo("https://serein.cc/#/Function/Event") { UseShellExecute = true });
        private void SettingSereinEnableDPIAware_CheckedChanged(object sender, EventArgs e)
            => Global.Settings.Serein.DPIAware = SettingSereinEnableDPIAware.Checked;

        #endregion

        #region 机器人
        private void SettingBotUri_TextChanged(object sender, EventArgs e)
            => Global.Settings.Bot.Uri = SettingBotUri.Text;
        private void SettingBotAuthorization_Enter(object sender, EventArgs e)
            => SettingBotAuthorization.Text = Global.Settings.Bot.Authorization;
        private void SettingBotEnableLog_CheckedChanged(object sender, EventArgs e)
            => Global.Settings.Bot.EnableLog = SettingBotEnableLog.Checked;
        private void SettingBotGivePermissionToAllAdmin_CheckedChanged(object sender, EventArgs e)
            => Global.Settings.Bot.GivePermissionToAllAdmin = SettingBotGivePermissionToAllAdmin.Checked;
        private void SettingBotEnbaleOutputData_CheckedChanged(object sender, EventArgs e)
            => Global.Settings.Bot.EnbaleOutputData = SettingBotEnbaleOutputData.Checked;
        private void SettingBotRestart_CheckedChanged(object sender, EventArgs e)
            => Global.Settings.Bot.AutoReconnect = SettingBotRestart.Checked;
        private void SettingBotAutoEscape_CheckedChanged(object sender, EventArgs e)
            => Global.Settings.Bot.AutoEscape = SettingBotAutoEscape.Checked;
        private void SettingBotEnbaleParseAt_CheckedChanged(object sender, EventArgs e)
            => Global.Settings.Bot.EnbaleParseAt = SettingBotEnbaleParseAt.Checked;

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
                    if (qq.Length >= 6 && qq.Length <= 16 && long.TryParse(qq, out long qq_))
                    {
                        list.Add(qq_);
                    }
                }
                Global.Settings.Bot.GroupList = list;
            }
            string text = Regex.Replace(SettingBotGroupList.Text, @"[^\d;]", ";");
            text = Regex.Replace(text, @";+", ";");
            text = Regex.Replace(text, "^;", string.Empty);
            if (text != SettingBotGroupList.Text)
            {
                SettingBotGroupList.Text = text;
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
                    if (qq.Length >= 5 && qq.Length <= 13 && long.TryParse(qq, out long qq_))
                    {
                        list.Add(qq_);
                    }
                }
                Global.Settings.Bot.PermissionList = list;
            }
            string text = Regex.Replace(SettingBotPermissionList.Text, @"[^\d,]", ";");
            text = Regex.Replace(text, @";+", ";");
            text = Regex.Replace(text, "^;", string.Empty);
            if (text != SettingBotPermissionList.Text)
            {
                SettingBotPermissionList.Text = text;
                SettingBotPermissionList.Focus();
                SettingBotPermissionList.Select(SettingBotPermissionList.TextLength, 0);
                SettingBotPermissionList.ScrollToCaret();
            }
        }
        #endregion
        #region 事件

        private void SettingEventTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node == null || !Enum.IsDefined(typeof(Base.EventType), SettingEventTreeView.SelectedNode.Name))
            {
                SettingEventList.Items.Clear();
                return;
            }
            SettingEventList.BeginUpdate();
            SettingEventList.Items.Clear();
            Global.Settings.Event.Get(
                (Base.EventType)Enum.Parse(typeof(Base.EventType), SettingEventTreeView.SelectedNode.Name))
                .ToList().ForEach((Command) => SettingEventList.Items.Add(Regex.Replace(Command, @"(\n|\r|\\n|\\r)+", "\\n"))
                );
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
            if (SettingEventTreeView.SelectedNode != null && Enum.IsDefined(typeof(Base.EventType), SettingEventTreeView.SelectedNode.Name))
            {
                Global.Settings.Event.Edit(GetEventCommands(), (Base.EventType)Enum.Parse(typeof(Base.EventType), SettingEventTreeView.SelectedNode.Name));
            }
            IO.SaveEventSetting();
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

        private void SettingSereinVersion_Click(object sender, EventArgs _)
        {
            Ookii.Dialogs.Wpf.TaskDialog taskDialog = new Ookii.Dialogs.Wpf.TaskDialog
            {
                Buttons = {
                        new Ookii.Dialogs.Wpf.TaskDialogButton(ButtonType.Ok)
                    },
                MainInstruction = "当前版本详细信息",
                WindowTitle = "Serein",
                Content = "" +
                    $"当前版本：{Global.VERSION}\n" +
                    $"编译类型：{Global.BuildInfo.Type}\n" +
                    $"编译时间：{Global.BuildInfo.Time}\n" +
                    $"详细信息：{Global.BuildInfo.Detail.Replace("\r", string.Empty)}",
                Footer = "开源地址：<a href=\"https://github.com/Zaitonn/Serein\">GitHub</a>",
                FooterIcon = Ookii.Dialogs.Wpf.TaskDialogIcon.Information,
                EnableHyperlinks = true
            };
            taskDialog.HyperlinkClicked += (_, e) => Process.Start(new ProcessStartInfo(e.Href) { UseShellExecute = true });
            taskDialog.ShowDialog();
        }
        #endregion
    }
}
