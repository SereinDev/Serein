using Serein.Base;
using Serein.Items;
using Serein.Ui.ChildrenWindow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Serein.Ui
{
    public partial class Ui : Form
    {
        private string[] Roles_Chinese = { "群主", "管理员", "成员" };
        private void LoadMembers()
        {
            Members.Load();
            MemberList.BeginUpdate();
            MemberList.Items.Clear();
            foreach (MemberItem memberItem in Global.MemberItems)
            {
                ListViewItem Item = new ListViewItem();
                Item.Text = memberItem.ID.ToString();
                Item.SubItems.Add(Roles_Chinese[memberItem.Role]);
                Item.SubItems.Add(memberItem.Nickname);
                Item.SubItems.Add(memberItem.Card);
                Item.SubItems.Add(memberItem.GameID);
                MemberList.Items.Add(Item);
            }
            MemberList.EndUpdate();
        }
        private void SaveMembers()
        {
            List<MemberItem> MemberItems = new List<MemberItem>();
            foreach (ListViewItem Item in MemberList.Items)
            {
                MemberItem memberItem = new MemberItem()
                {
                    ID = long.TryParse(Item.Text, out long i) ? i : -1,
                    Role = Array.IndexOf(Roles_Chinese, Item.SubItems[1].Text),
                    Nickname = Item.SubItems[2].Text,
                    Card = Item.SubItems[3].Text,
                    GameID = Item.SubItems[4].Text
                };
                if (memberItem.ID != -1)
                {
                    MemberItems.Add(memberItem);
                }
            }
            Global.UpdateMemberItems(MemberItems);
            Members.Save();
        }
        private void MemberContextMenuStrip_Refresh_Click(object sender, EventArgs e)
        {
            LoadMembers();
        }
        private void MemberContextMenuStrip_Edit_Click(object sender, EventArgs e)
        {
            if (MemberList.SelectedItems.Count > 0)
            {
                MemberInfoEditer Editer = new MemberInfoEditer(MemberList.SelectedItems[0]);
                Editer.ShowDialog(this);
                if (!Editer.CancelFlag)
                {
                    MemberList.SelectedItems[0].SubItems[4].Text = Editer.GameIDBox.Text;
                    SaveMembers();
                }
            }
        }
        private void MemberContextMenuStrip_Remove_Click(object sender, EventArgs e)
        {
            if (MemberList.SelectedItems.Count > 0)
            {
                int result = (int)MessageBox.Show(
                    "确定删除此行数据？\n" +
                    "它将会永远失去！（真的很久！）", "Serein",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Information
                    );
                if (result == 1)
                {
                    MemberList.Items.RemoveAt(MemberList.SelectedItems[0].Index);
                    SaveMembers();
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
    }
}
