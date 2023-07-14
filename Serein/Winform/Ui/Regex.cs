using Serein.Base;
using Serein.Utils.IO;
using Serein.Ui.ChildrenWindow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace Serein.Ui
{
    public partial class Ui : Form
    {
        private void RegexList_ItemDrag(object sender, ItemDragEventArgs e)
        {
            _itemDraged = (ListViewItem)e.Item!;
            _isDragging = true;
        }

        private void RegexList_MouseUp(object sender, MouseEventArgs e)
        {
            _isDragging = false;
            if ((RegexList.SelectedItems.Count != 0) && (_itemDraged != null))
            {
                if (_itemDraged.Index != RegexList.SelectedItems[0].Index && _itemDraged.Index >= 0)
                {
                    RegexList.Items.RemoveAt(_itemDraged.Index);
                    RegexList.Items.Insert(RegexList.SelectedItems[0].Index, _itemDraged);
                    FileSaveregex();
                    _itemDraged = null;
                }
            }
        }

        private void RegexList_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)
        {
            e.Item.Selected = _isDragging;
        }

        private void RegexContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            RegexContextMenuStrip_Edit.Enabled = RegexList.SelectedItems.Count > 0;
            RegexContextMenuStrip_Delete.Enabled = RegexList.SelectedItems.Count > 0;
            RegexContextMenuStrip_Clear.Enabled = RegexList.Items.Count > 0;
        }

        private void RegexContextMenuStrip_Add_Click(object sender, EventArgs e)
        {
            RegexEditor regexEditer = new();
            regexEditer.ShowDialog(this);
            if (regexEditer.CancelFlag)
            {
                return;
            }
            AddRegex(
                regexEditer.Area.SelectedIndex,
                regexEditer.RegexTextBox.Text,
                regexEditer.IsAdmin.Checked,
                regexEditer.RemarkTextBox.Text,
                regexEditer.CommandTextBox.Text
                );
            FileSaveregex();
        }

        private void RegexContextMenuStrip_Edit_Click(object sender, EventArgs e)
        {
            if (RegexList.SelectedItems.Count <= 0)
            {
                return;
            }
            RegexEditor regexEditer = new();
            int index = Array.IndexOf(Regex.AreaArray, RegexList.SelectedItems[0].SubItems[1].Text);
            regexEditer.UpdateInfo(
                index,
                RegexList.SelectedItems[0].Text,
                RegexList.SelectedItems[0].SubItems[2].Text == "是",
                RegexList.SelectedItems[0].SubItems[3].Text,
                RegexList.SelectedItems[0].SubItems[4].Text
                );
            regexEditer.ShowDialog(this);
            if (regexEditer.CancelFlag)
            {
                return;
            }
            string isAdminText = regexEditer.Area.SelectedIndex <= 1 || regexEditer.Area.SelectedIndex == 4 ? "-" : regexEditer.IsAdmin.Checked ? "是" : "否";
            RegexList.SelectedItems[0].Text = regexEditer.RegexTextBox.Text;
            RegexList.SelectedItems[0].SubItems[1].Text = Regex.AreaArray[regexEditer.Area.SelectedIndex];
            RegexList.SelectedItems[0].SubItems[2].Text = isAdminText;
            RegexList.SelectedItems[0].SubItems[3].Text = regexEditer.RemarkTextBox.Text;
            RegexList.SelectedItems[0].SubItems[4].Text = regexEditer.CommandTextBox.Text;
            FileSaveregex();
        }

        private void RegexContextMenuStrip_Clear_Click(object sender, EventArgs e)
        {
            if (RegexList.Items.Count > 0)
            {
                int result = (int)MessageBox.Show(
                    "确定删除所有记录？\n" +
                    "它将会永远失去！（真的很久！）", "Serein",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Information
                    );
                if (result == 1)
                {
                    RegexList.Items.Clear();
                    FileSaveregex();
                }
            }
        }

        private void RegexContextMenuStrip_Delete_Click(object sender, EventArgs e)
        {
            if (RegexList.SelectedItems.Count > 0)
            {
                int result = (int)MessageBox.Show(
                    "确定删除此行记录？\n" +
                    "它将会永远失去！（真的很久！）", "Serein",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Information
                    );
                if (result == 1)
                {
                    RegexList.Items.RemoveAt(RegexList.SelectedItems[0].Index);
                    FileSaveregex();
                }
            }
        }

        private void AddRegex(int areaIndex, string regex, bool isAdmin, string remark, string command, long[]? ignored = null)
        {
            if (
              string.IsNullOrWhiteSpace(regex) || string.IsNullOrEmpty(regex) ||
              string.IsNullOrWhiteSpace(command) || string.IsNullOrEmpty(command)
              )
            {
                return;
            }
            string isAdminText = string.Empty;
            ListViewItem item = new(regex);
            item.Tag = ignored;
            isAdminText = areaIndex == 4 || areaIndex <= 1 ? "-" : isAdmin ? "是" : "否";
            item.SubItems.Add(Regex.AreaArray[areaIndex]);
            item.SubItems.Add(isAdminText);
            item.SubItems.Add(remark);
            item.SubItems.Add(command);
            if (RegexList.InvokeRequired)
            {
                Action actionDelegate = () =>
                {
                    RegexList.Items.Add(item);
                };
                ServerPanelInfoPlayerCount2.Invoke(actionDelegate);
            }
            else
            {
                if (RegexList.SelectedItems.Count > 0)
                {
                    RegexList.Items.Insert(RegexList.SelectedItems[0].Index + 1, item);
                }
                else
                {
                    RegexList.Items.Add(item);
                }
            }
        }

        private void RegexContextMenuStripRefresh_Click(object sender, EventArgs e)
        {
            Data.ReadRegex();
            LoadRegex();
            FileSaveregex();
        }

        public void LoadRegex()
        {
            RegexList.BeginUpdate();
            RegexList.Items.Clear();
            foreach (Regex item in Global.RegexList)
            {
                if (item.Check())
                {
                    AddRegex(item.Area, item.Expression, item.IsAdmin, item.Remark, item.Command, item.Ignored);
                }
            }
            RegexList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            RegexList.EndUpdate();
        }

        private void FileSaveregex()
        {
            List<Regex> regexItems = new();
            foreach (ListViewItem listViewItem in RegexList.Items)
            {
                regexItems.Add(new Regex
                {
                    Expression = listViewItem.Text,
                    Area = Array.IndexOf(Regex.AreaArray, listViewItem.SubItems[1].Text),
                    IsAdmin = listViewItem.SubItems[2].Text == "是",
                    Remark = listViewItem.SubItems[3].Text,
                    Ignored = (listViewItem.Tag as long[]) ?? new long[] { },
                    Command = listViewItem.SubItems[4].Text
                });
            }
            Global.RegexList = regexItems;
            Data.FileSaveregex();
        }

        private void RegexContextMenuStrip_Variables_Click(object sender, EventArgs e) => Process.Start(new ProcessStartInfo("https://serein.cc/docs/guide/variables") { UseShellExecute = true });
        private void RegexContextMenuStrip_Command_Click(object sender, EventArgs e) => Process.Start(new ProcessStartInfo("https://serein.cc/docs/guide/command") { UseShellExecute = true });
    }
}
