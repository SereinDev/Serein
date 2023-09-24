﻿using Serein.Base;
using Serein.Base.Packets;
using Serein.Utils.IO;
using Serein.Ui.ChildrenWindow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace Serein.Ui
{
    public partial class Ui : Form
    {
        public void LoadMember()
        {
            MemberList.BeginUpdate();
            MemberList.Items.Clear();
            foreach (Member member in Global.MemberDict.Values.ToList())
            {
                ListViewItem listViewItem = new();
                listViewItem.Text = member.ID.ToString();
                listViewItem.SubItems.Add(Sender.RoleNames[member.Role]);
                listViewItem.SubItems.Add(member.Nickname);
                listViewItem.SubItems.Add(member.Card);
                listViewItem.SubItems.Add(member.GameID);
                MemberList.Items.Add(listViewItem);
            }
            MemberList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            MemberList.EndUpdate();
        }

        private void SaveMember()
        {
            Dictionary<long, Member> memberDict = new();
            foreach (ListViewItem listViewItem in MemberList.Items)
            {
                Member member =
                    new()
                    {
                        ID = long.TryParse(listViewItem.Text, out long i) ? i : -1,
                        Role = Array.IndexOf(Sender.RoleNames, listViewItem.SubItems[1].Text),
                        Nickname = listViewItem.SubItems[2].Text,
                        Card = listViewItem.SubItems[3].Text,
                        GameID = listViewItem.SubItems[4].Text
                    };
                if (member.ID != -1)
                {
                    memberDict.Add(member.ID, member);
                }
            }
            lock (Global.MemberDict)
            {
                Global.MemberDict = memberDict;
            }
            Data.SaveMember();
        }

        private void MemberContextMenuStrip_Edit_Click(object sender, EventArgs e)
        {
            if (MemberList.SelectedItems.Count > 0)
            {
                MemberInfoEditor editor = new(MemberList.SelectedItems[0]);
                editor.ShowDialog(this);
                if (!editor.CancelFlag)
                {
                    MemberList.SelectedItems[0].SubItems[4].Text = editor.GameIDBox.Text;
                    SaveMember();
                }
            }
        }

        private void MemberContextMenuStrip_Remove_Click(object sender, EventArgs e)
        {
            if (MemberList.SelectedItems.Count > 0)
            {
                int result = (int)
                    MessageBox.Show(
                        "确定删除此行数据？\n" + "它将会永远失去！（真的很久！）",
                        "Serein",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Information
                    );
                if (result == 1)
                {
                    MemberList.Items.RemoveAt(MemberList.SelectedItems[0].Index);
                    SaveMember();
                }
            }
        }

        private void MemberContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            if (MemberList.SelectedItems.Count == 1)
            {
                MemberContextMenuStrip_Edit.Enabled = true;
                MemberContextMenuStrip_Remove.Enabled = true;
            }
            else
            {
                MemberContextMenuStrip_Edit.Enabled = false;
                MemberContextMenuStrip_Remove.Enabled = false;
            }
        }

        private void MemberContextMenuStrip_Refresh_Click(object sender, EventArgs e)
        {
            Data.ReadMember();
            LoadMember();
        }
    }
}
