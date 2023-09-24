using Serein.Core.Common;
using Serein.Utils.IO;
using Serein.Utils.Output;
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
        private int _actionType;

        public Member()
        {
            InitializeComponent();
            Catalog.Function.Member = this;
            Load();
        }

        public void Load()
        {
            MemberListView.Items.Clear();
            foreach (Base.Member member in Global.MemberDict.Values.ToList())
            {
                MemberListView.Items.Add(member);
            }
        }

        private void Save()
        {
            Dictionary<long, Base.Member> dictionary = new Dictionary<long, Base.Member>();
            foreach (var obj in MemberListView.Items)
            {
                if (obj is Base.Member member && member != null)
                {
                    dictionary.Add(member.ID, member);
                }
            }
            lock (Global.MemberDict)
            {
                Global.MemberDict = dictionary;
            }
            Data.SaveMember();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Wpf.Ui.Controls.MenuItem menuItem && menuItem != null)
            {
                if (_actionType != 0)
                {
                    Catalog.MainWindow?.MemberEditor.Hide();
                    _actionType = 0;
                }
                string tag = menuItem.Tag as string ?? string.Empty;
                switch (tag)
                {
                    case "Add":
                        _actionType = 1;
                        Catalog.MainWindow?.OpenMemberEditor();
                        break;
                    case "Edit":
                        if (
                            MemberListView.SelectedItem is Base.Member selectedItem
                            && selectedItem != null
                        )
                        {
                            _actionType = 2;
                            Catalog.MainWindow?.OpenMemberEditor(
                                false,
                                selectedItem.ID.ToString(),
                                selectedItem.GameID
                            );
                        }
                        else
                        {
                            _actionType = 0;
                        }
                        break;
                    case "Delete":
                        if (
                            MsgBox.Show("确定删除此行数据？\n它将会永远失去！（真的很久！）", true)
                            && MemberListView.SelectedIndex >= 0
                        )
                        {
                            MemberListView.Items.RemoveAt(MemberListView.SelectedIndex);
                            Save();
                        }
                        break;
                    case "Refresh":
                        Data.ReadMember();
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
            switch (_actionType)
            {
                case 1:
                    if (!long.TryParse(ID, out long IDNumber))
                    {
                        Catalog.MainWindow?.OpenSnackbar("绑定失败", "ID不合法", SymbolRegular.Warning24);
                    }
                    else if (!Binder.Bind(IDNumber, GameID))
                    {
                        Catalog.MainWindow?.OpenSnackbar(
                            "绑定失败",
                            "该ID已绑定 / 游戏名称不合法 / 游戏名称已被绑定",
                            SymbolRegular.Warning24
                        );
                    }
                    else
                    {
                        Catalog.MainWindow?.OpenSnackbar(
                            "绑定成功",
                            $"{ID} -> {GameID}",
                            SymbolRegular.Checkmark24
                        );
                        _actionType = 0;
                        Load();
                        return true;
                    }
                    break;
                case 2:
                    if (
                        !System.Text.RegularExpressions.Regex.IsMatch(
                            GameID,
                            Global.Settings.Serein.Function.RegexForCheckingGameID
                        )
                    )
                    {
                        Catalog.MainWindow?.OpenSnackbar(
                            "绑定失败",
                            "游戏名称不合法",
                            SymbolRegular.Warning24
                        );
                    }
                    else if (Binder.GameIDs.Contains(GameID))
                    {
                        Catalog.MainWindow?.OpenSnackbar(
                            "绑定失败",
                            "游戏名称已被绑定",
                            SymbolRegular.Warning24
                        );
                    }
                    else
                    {
                        Catalog.MainWindow?.OpenSnackbar(
                            "绑定成功",
                            $"{ID} -> {GameID}",
                            SymbolRegular.Checkmark24
                        );
                        if (
                            MemberListView.SelectedItem is Base.Member selectedItem
                            && selectedItem != null
                        )
                        {
                            selectedItem.GameID = GameID;
                            MemberListView.SelectedItem = selectedItem;
                            Save();
                            Load();
                        }
                        _actionType = 0;
                        return true;
                    }
                    break;
            }
            return false;
        }
    }
}
