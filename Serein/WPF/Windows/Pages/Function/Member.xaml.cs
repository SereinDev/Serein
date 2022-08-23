using Serein.Base;
using Serein.Items;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace Serein.Windows.Pages.Function
{
    public partial class Member : UiPage
    {
        public Member()
        {
            InitializeComponent();
            Window.Function.Member = this;
            Load();
        }

        private void Load()
        {
            Members.Load();
            MemberListView.Items.Clear();
            foreach (MemberItem Item in Global.MemberItems)
            {
                MemberListView.Items.Add(Item);
            }
        }

        private void Save()
        {
            List<MemberItem> Items = new List<MemberItem>();
            foreach (var obj in MemberListView.Items)
            {
                if (obj is MemberItem Item && Item != null)
                {
                    Items.Add(Item);
                }
            }
            Global.UpdateMemberItems(Items);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Wpf.Ui.Controls.MenuItem Item && Item != null)
            {
                string Tag = Item.Tag as string ?? string.Empty;
                switch (Tag)
                {
                    case "Add":
                    case "Edit":
                        break;
                    case "Delete":
                        if (Logger.MsgBox("确定删除此行数据？\n它将会永远失去！（真的很久！）", "Serein", 1, 48) && MemberListView.SelectedIndex >= 0)
                        {
                            MemberListView.Items.RemoveAt(MemberListView.SelectedIndex);
                            Save();
                        }
                        break;
                    case "Refresh":
                        Load();
                        break;
                }
            }
        }

        private void MemberListView_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            Edit.IsEnabled = MemberListView.SelectedIndex != -1;
            Delete.IsEnabled = MemberListView.SelectedIndex != -1;
        }
    }
}
