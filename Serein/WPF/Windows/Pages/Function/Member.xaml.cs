using Serein.Base;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;

namespace Serein.Windows.Pages.Function
{
    public partial class Member : UiPage
    {
        private int ActionType;
        public Member()
        {
            InitializeComponent();
            Catalog.Function.Member = this;
            Load(false);
        }

        private void Load(bool FromFile = true)
        {
            if (FromFile)
            {
                IO.ReadMember();
            }
            MemberListView.Items.Clear();
            foreach (Items.Member member in Global.MemberItems.Values.ToList())
            {
                MemberListView.Items.Add(member);
            }
        }

        private void Save()
        {
            Dictionary<long, Items.Member> dictionary = new Dictionary<long, Items.Member>();
            foreach (var obj in MemberListView.Items)
            {
                if (obj is Items.Member member && member != null)
                {
                    dictionary.Add(member.ID, member);
                }
            }
            Global.UpdateMemberItems(dictionary);
            IO.SaveMember();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Wpf.Ui.Controls.MenuItem menuItem && menuItem != null)
            {
                if (ActionType != 0)
                {
                    Catalog.MainWindow.MemberEditor.Hide();
                    ActionType = 0;
                }
                string Tag = menuItem.Tag as string ?? string.Empty;
                switch (Tag)
                {
                    case "Add":
                        ActionType = 1;
                        Catalog.MainWindow.OpenMemberEditor();
                        break;
                    case "Edit":
                        if (MemberListView.SelectedItem is Items.Member selectedItem && selectedItem != null)
                        {
                            ActionType = 2;
                            Catalog.MainWindow.OpenMemberEditor(false, selectedItem.ID.ToString(), selectedItem.GameID);
                        }
                        else
                        {
                            ActionType = 0;
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

        public bool Confirm(string ID, string GameID)
        {
            switch (ActionType)
            {
                case 1:
                    if (!long.TryParse(ID, out long IDNumber))
                    {
                        Catalog.MainWindow.OpenSnackbar("绑定失败", "ID不合法", SymbolRegular.Warning24);
                    }
                    else if (!Binder.Bind(IDNumber, GameID))
                    {
                        Catalog.MainWindow.OpenSnackbar("绑定失败", "该ID已绑定 / 游戏名称不合法 / 游戏名称已被绑定", SymbolRegular.Warning24);
                    }
                    else
                    {
                        Catalog.MainWindow.OpenSnackbar("绑定成功", $"{ID} -> {GameID}", SymbolRegular.Checkmark24);
                        ActionType = 0;
                        Load();
                        return true;
                    }
                    break;
                case 2:
                    if (!System.Text.RegularExpressions.Regex.IsMatch(GameID, @"^[a-zA-Z0-9_\s-]{4,16}$"))
                    {
                        Catalog.MainWindow.OpenSnackbar("绑定失败", "游戏名称不合法", SymbolRegular.Warning24);
                    }
                    else if (Binder.GameIDs.Contains(GameID))
                    {
                        Catalog.MainWindow.OpenSnackbar("绑定失败", "游戏名称已被绑定", SymbolRegular.Warning24);
                    }
                    else
                    {
                        Catalog.MainWindow.OpenSnackbar("绑定成功", $"{ID} -> {GameID}", SymbolRegular.Checkmark24);
                        if (MemberListView.SelectedItem is Items.Member selectedItem && selectedItem != null)
                        {
                            selectedItem.GameID = GameID;
                            MemberListView.SelectedItem = selectedItem;
                            Save();
                            Load();
                        }
                        ActionType = 0;
                        return true;
                    }
                    break;
            }
            return false;
        }
    }
}
