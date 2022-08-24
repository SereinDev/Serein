using Serein.Base;
using Serein.Items;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;

namespace Serein.Windows.Pages.Function
{
    public partial class Member : UiPage
    {
        private object Lock = new object();
        private int ActionType = 0;
        public Member()
        {
            InitializeComponent();
            Window.Function.Member = this;
            Load();
        }

        private void Load()
        {
            Loader.LoadMember();
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
            Loader.SaveMember();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Wpf.Ui.Controls.MenuItem Item && Item != null)
            {
                if(ActionType != 0)
                {
                    Window.MainWindow.MemberEditor.Hide();
                    ActionType = 0;
                }
                string Tag = Item.Tag as string ?? string.Empty;
                switch (Tag)
                {
                    case "Add":
                        lock (Lock)
                        {
                            ActionType = 1;
                                Window.MainWindow.ID.IsEnabled = true;
                            Window.MainWindow.ID.Text = "";
                            Window.MainWindow.GameID.Text = "";
                            Window.MainWindow.MemberEditor.ShowAndWaitAsync();
                        }
                        break;
                    case "Edit":
                        lock (Lock)
                        {
                            if (MemberListView.SelectedItem is MemberItem SelectedItem && SelectedItem != null)
                            {
                                ActionType = 2;
                                Window.MainWindow.ID.IsEnabled = false;
                                Window.MainWindow.ID.Text = SelectedItem.ID.ToString();
                                Window.MainWindow.GameID.Text = SelectedItem.GameID;
                                Window.MainWindow.MemberEditor.ShowAndWaitAsync();
                            }
                            else
                            {
                                ActionType = 0;
                            }
                        }
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

        public void Confirm()
        {
            switch (ActionType)
            {
                case 1:
                    if (!long.TryParse(Window.MainWindow.ID.Text, out long IDNumber))
                    {
                        Window.MainWindow.OpenSnackbar("绑定失败", "ID不合法", SymbolRegular.Warning24);
                    }
                    else if (!Members.Bind(IDNumber, Window.MainWindow.GameID.Text))
                    {
                        Window.MainWindow.OpenSnackbar("绑定失败", "该ID已绑定 / 游戏名称不合法 / 游戏名称已被绑定", SymbolRegular.Warning24);
                    }
                    else
                    {
                        Window.MainWindow.OpenSnackbar("绑定成功", $"{IDNumber} -> {Window.MainWindow.GameID.Text}", SymbolRegular.Checkmark24);
                        Window.MainWindow.MemberEditor.Hide();
                            ActionType = 0;
                    }
                    break;
                case 2:
                    if (!System.Text.RegularExpressions.Regex.IsMatch(Window.MainWindow.GameID.Text, @"^[a-zA-Z0-9_\s-]{4,16}$"))
                    {
                        Window.MainWindow.OpenSnackbar("绑定失败", "游戏名称不合法", SymbolRegular.Warning24);
                    }else if (Members.GameIDs.Contains(Window.MainWindow.GameID.Text))
                    {
                        Window.MainWindow.OpenSnackbar("绑定失败", "游戏名称已被绑定", SymbolRegular.Warning24);
                    }
                    else
                    {
                        Window.MainWindow.OpenSnackbar("绑定成功", $"{Window.MainWindow.ID.Text} -> {Window.MainWindow.GameID.Text}", SymbolRegular.Checkmark24);
                        Window.MainWindow.MemberEditor.Hide();
                        if (MemberListView.SelectedItem is MemberItem SelectedItem && SelectedItem != null)
                        {
                            SelectedItem.GameID = Window.MainWindow.GameID.Text;
                            MemberListView.SelectedItem = SelectedItem;
                            Save();
                            Load();
                        }
                        ActionType = 0;
                    }
                    break;
            }
        }
    }
}
