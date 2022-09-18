﻿using Serein.Base;
using Serein.Items;
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
        public string[] areas = { "禁用", "控制台", "消息（群聊）", "消息（私聊）", "消息（自身发送）" };

        private void RegexList_ItemDrag(object sender, ItemDragEventArgs e)
        {
            ItemDraged = (ListViewItem)e.Item;
            IsDragging = true;
        }

        private void RegexList_MouseUp(object sender, MouseEventArgs e)
        {
            IsDragging = false;
            if ((RegexList.SelectedItems.Count != 0) && (ItemDraged != null))
            {
                if (ItemDraged.Index != RegexList.SelectedItems[0].Index && ItemDraged.Index >= 0)
                {
                    RegexList.Items.RemoveAt(ItemDraged.Index);
                    RegexList.Items.Insert(RegexList.SelectedItems[0].Index, ItemDraged);
                    SaveRegex();
                    ItemDraged = null;
                }
            }
        }

        private void RegexList_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)
        {
            e.Item.Selected = IsDragging;
        }

        private void RegexContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            RegexContextMenuStrip_Edit.Enabled = RegexList.SelectedItems.Count > 0;
            RegexContextMenuStrip_Delete.Enabled = RegexList.SelectedItems.Count > 0;
            RegexContextMenuStrip_Clear.Enabled = RegexList.Items.Count > 0;
        }

        private void RegexContextMenuStrip_Add_Click(object sender, EventArgs e)
        {
            RegexEditor regexEditer = new RegexEditor();
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
            SaveRegex();
        }

        private void RegexContextMenuStrip_Edit_Click(object sender, EventArgs e)
        {
            if (RegexList.SelectedItems.Count <= 0)
            {
                return;
            }
            RegexEditor regexEditer = new RegexEditor();
            int index = Array.IndexOf(areas, RegexList.SelectedItems[0].SubItems[1].Text);
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
            RegexList.SelectedItems[0].SubItems[1].Text = areas[regexEditer.Area.SelectedIndex];
            RegexList.SelectedItems[0].SubItems[2].Text = isAdminText;
            RegexList.SelectedItems[0].SubItems[3].Text = regexEditer.RemarkTextBox.Text;
            RegexList.SelectedItems[0].SubItems[4].Text = regexEditer.CommandTextBox.Text;
            SaveRegex();
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
                    SaveRegex();
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
                    SaveRegex();
                }
            }
        }

        private void AddRegex(int areaIndex, string regex, bool isAdmin, string remark, string command)
        {
            if (
              string.IsNullOrWhiteSpace(regex) || string.IsNullOrEmpty(regex) ||
              string.IsNullOrWhiteSpace(command) || string.IsNullOrEmpty(command)
              )
            {
                return;
            }
            string isAdminText = string.Empty;
            ListViewItem Item = new ListViewItem(regex);
            isAdminText = areaIndex == 4 || areaIndex <= 1 ? "-" : isAdmin ? "是" : "否";
            Item.SubItems.Add(areas[areaIndex]);
            Item.SubItems.Add(isAdminText);
            Item.SubItems.Add(remark);
            Item.SubItems.Add(command);
            if (RegexList.InvokeRequired)
            {
                Action<ListViewItem> actionDelegate = (x) =>
                {
                    RegexList.Items.Add(Item);
                };
                PanelInfoLevel2.Invoke(actionDelegate, Item);
            }
            else
            {
                if (RegexList.SelectedItems.Count > 0)
                {
                    RegexList.Items.Insert(RegexList.SelectedItems[0].Index + 1, Item);
                }
                else
                {
                    RegexList.Items.Add(Item);
                }
            }
        }

        private void RegexContextMenuStripRefresh_Click(object sender, EventArgs e)
        {
            RegexList.BeginUpdate();
            LoadRegex();
            SaveRegex();
            RegexList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            RegexList.EndUpdate();
        }

        private void LoadRegex(string FileName = null)
        {
            RegexList.BeginUpdate();
            RegexList.Items.Clear();
            Loader.ReadRegex(FileName);
            foreach (Regex Item in Global.RegexItems)
            {
                if (Item.Check())
                {
                    AddRegex(Item.Area, Item.Expression, Item.IsAdmin, Item.Remark, Item.Command);
                }
            }
            RegexList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            RegexList.EndUpdate();
        }

        private void SaveRegex()
        {
            List<Regex> regexItems = new List<Regex>();
            foreach (ListViewItem Item in RegexList.Items)
            {
                regexItems.Add(new Regex()
                {
                    Expression = Item.Text,
                    Area = Array.IndexOf(areas, Item.SubItems[1].Text),
                    IsAdmin = Item.SubItems[2].Text == "是",
                    Remark = Item.SubItems[3].Text,
                    Command = Item.SubItems[4].Text
                });
            }
            Global.UpdateRegexItems(regexItems);
            Loader.SaveRegex();
        }

        private void RegexContextMenuStrip_Variables_Click(object sender, EventArgs e) => Process.Start(new ProcessStartInfo("https://serein.cc/Variables.html") { UseShellExecute = true });
        private void RegexContextMenuStrip_Command_Click(object sender, EventArgs e) => Process.Start(new ProcessStartInfo("https://serein.cc/Command.html") { UseShellExecute = true });
    }
}