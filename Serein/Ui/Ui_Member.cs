using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Serein
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
                Item.SubItems.Add(memberItem.GameName);
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
                    GameName = Item.SubItems[4].Text
                };
                if (memberItem.ID != -1)
                {
                    MemberItems.Add(memberItem);
                }
            }
            Global.MemberItems = MemberItems;
            Members.Save();
        }
        private void MemberContextMenuStrip_Refresh_Click(object sender, EventArgs e)
        {
            Members.Load();
            LoadMembers();
        }
        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex == 5)
            {
                Members.Load();
                LoadMembers();
            }
        }
        private void MemberContextMenuStrip_Edit_Click(object sender, EventArgs e)
        {

        }
        private void MemberContextMenuStrip_Remove_Click(object sender, EventArgs e)
        {

        }
    }
}
