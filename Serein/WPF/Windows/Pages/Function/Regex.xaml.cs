using Serein.Base;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;

namespace Serein.Windows.Pages.Function
{
    public partial class Regex : UiPage
    {
        private int ActionType;
        public Regex()
        {
            InitializeComponent();
            Load();
            Catalog.Function.Regex = this;
        }

        public void Load() => Load(null);

        public void Load(string filename)
        {
            IO.ReadRegex(filename);
            RegexListView.Items.Clear();
            foreach (Items.Regex regex in Global.RegexList)
            {
                RegexListView.Items.Add(regex);
            }
        }

        private void Save()
        {
            List<Items.Regex> list = new();
            foreach (var obj in RegexListView.Items)
            {
                if (obj is Items.Regex regex && regex != null)
                {
                    list.Add(regex);
                }
            }
            Global.RegexList = list;
            IO.SaveRegex();
        }

        private void RegexListView_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            Edit.IsEnabled = RegexListView.SelectedIndex != -1;
            Delete.IsEnabled = RegexListView.SelectedIndex != -1;
            Clear.IsEnabled = RegexListView.Items.Count > 0;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Wpf.Ui.Controls.MenuItem menuItem && menuItem != null)
            {
                if (ActionType != 0)
                {
                    Catalog.MainWindow.RegexEditor.Hide();
                    ActionType = 0;
                }
                string Tag = menuItem.Tag as string ?? string.Empty;
                switch (Tag)
                {
                    case "Add":
                        Catalog.MainWindow.OpenRegexEditor();
                        ActionType = 1;
                        break;
                    case "Edit":
                        if (RegexListView.SelectedItem is Items.Regex selectedItem && selectedItem != null)
                        {
                            Catalog.MainWindow.OpenRegexEditor(
                                selectedItem.Area,
                                selectedItem.IsAdmin,
                                selectedItem.Expression,
                                selectedItem.Command,
                                selectedItem.Remark
                                );
                            ActionType = 2;
                        }
                        break;
                    case "Delete":
                        if (Logger.MsgBox("确定删除此行数据？\n它将会永远失去！（真的很久！）", "Serein", 1, 48) && RegexListView.SelectedIndex >= 0)
                        {
                            RegexListView.Items.RemoveAt(RegexListView.SelectedIndex);
                            Save();
                        }
                        break;
                    case "Clear":
                        if (Logger.MsgBox("确定删除所有数据？\n它将会永远失去！（真的很久！）", "Serein", 1, 48) && RegexListView.Items.Count > 0)
                        {
                            RegexListView.Items.Clear();
                            Save();
                        }
                        break;
                    case "Refresh":
                        Load();
                        break;
                    case "LookupCommand":
                        Process.Start(new ProcessStartInfo("https://serein.cc/#/Function/Command") { UseShellExecute = true });
                        break;
                    case "LookupVariables":
                        Process.Start(new ProcessStartInfo("https://serein.cc/#/Function/Variables") { UseShellExecute = true });
                        break;
                }
            }
        }

        public bool Confirm(int AreaIndex, bool IsAdmin, string Regex, string Command, string Remark)
        {
            if (Base.Command.GetType(Command) < 0)
            {
                Catalog.MainWindow.OpenSnackbar("编辑失败", "命令不合法", SymbolRegular.Warning24);
            }
            else
            {
                try
                {
                    System.Text.RegularExpressions.Regex.IsMatch(string.Empty, Regex);
                    if (ActionType == 1)
                    {
                        if (RegexListView.SelectedIndex >= 0)
                        {
                            RegexListView.Items.Insert(
                                RegexListView.SelectedIndex,
                                new Items.Regex
                                {
                                    Area = AreaIndex,
                                    Expression = Regex,
                                    Command = Command,
                                    Remark = Remark,
                                    IsAdmin = IsAdmin
                                });
                        }
                        else
                        {
                            RegexListView.Items.Add(
                                new Items.Regex()
                                {
                                    Area = AreaIndex,
                                    Expression = Regex,
                                    Command = Command,
                                    Remark = Remark,
                                    IsAdmin = IsAdmin
                                });
                        }
                    }
                    else if (ActionType == 2 && RegexListView.SelectedItem is Items.Regex selectedItem && selectedItem != null)
                    {
                        selectedItem.Area = AreaIndex;
                        selectedItem.Expression = Regex;
                        selectedItem.Command = Command;
                        selectedItem.Remark = Remark;
                        selectedItem.IsAdmin = IsAdmin;
                        RegexListView.SelectedItem = selectedItem;
                    }
                    Save();
                    Load();
                    ActionType = 0;
                    return true;
                }
                catch
                {
                    Catalog.MainWindow.OpenSnackbar("编辑失败", "正则不合法", SymbolRegular.Warning24);
                }
            }
            return false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (RegexListView.SelectedIndex != -1 && sender is Wpf.Ui.Controls.Button button && button != null)
            {
                int Index = RegexListView.SelectedIndex;
                string Tag = button.Tag as string ?? string.Empty;
                switch (Tag)
                {
                    case "Up":
                        RegexListView.Items.Insert(Index - 1, RegexListView.SelectedItem);
                        RegexListView.Items.RemoveAt(Index + 1);
                        RegexListView.SelectedIndex = Index - 1;
                        break;
                    case "Down":
                        RegexListView.Items.Insert(Index + 2, RegexListView.SelectedItem);
                        RegexListView.Items.RemoveAt(Index);
                        RegexListView.SelectedIndex = Index + 1;
                        break;
                }
                Save();
                UpdateButton();
            }
        }

        private void RegexListView_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateButton();

        private void UpdateButton()
        {
            Up.IsEnabled = RegexListView.SelectedIndex > 0;
            Down.IsEnabled = RegexListView.SelectedIndex >= 0 && RegexListView.SelectedIndex < RegexListView.Items.Count - 1;
        }
    }
}
