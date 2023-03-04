using Serein.Core.Generic;
using Serein.Extensions;
using Serein.Utils;
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

        public void Load() => Load(null, false);

        public void Load(string filename, bool append)
        {
            IO.ReadRegex(filename, append);
            RegexListView.Items.Clear();
            foreach (Base.Regex regex in Global.RegexList)
            {
                RegexListView.Items.Add(regex);
            }
        }

        private void Save()
        {
            List<Base.Regex> list = new();
            foreach (var obj in RegexListView.Items)
            {
                if (obj is Base.Regex regex && regex != null)
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
            Copy.IsEnabled = RegexListView.SelectedIndex != -1;
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
                        if (RegexListView.SelectedItem is Base.Regex selectedItem1 && selectedItem1 != null)
                        {
                            Catalog.MainWindow.OpenRegexEditor(
                                selectedItem1.Area,
                                selectedItem1.IsAdmin,
                                selectedItem1.Expression,
                                selectedItem1.Command,
                                selectedItem1.Remark
                                );
                            ActionType = 2;
                        }
                        break;
                    case "Copy":
                        if (RegexListView.SelectedItem is Base.Regex selectedItem2 && selectedItem2 != null)
                        {
                            RegexListView.Items.Insert(RegexListView.SelectedIndex, selectedItem2);
                            Save();
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

        public bool Confirm(int areaIndex, bool needAdmin, string regex, string command, string remark)
        {
            if (Command.GetType(command) < 0)
            {
                Catalog.MainWindow.OpenSnackbar("编辑失败", "命令不合法", SymbolRegular.Warning24);
            }
            else
            {
                if (regex.TestRegex())
                {
                    Catalog.MainWindow.OpenSnackbar("编辑失败", "正则不合法", SymbolRegular.Warning24);
                    return false;
                }
                if (ActionType == 1)
                {
                    if (RegexListView.SelectedIndex >= 0)
                    {
                        RegexListView.Items.Insert(
                            RegexListView.SelectedIndex,
                            new Base.Regex
                            {
                                Area = areaIndex,
                                Expression = regex,
                                Command = command,
                                Remark = remark,
                                IsAdmin = needAdmin
                            });
                    }
                    else
                    {
                        RegexListView.Items.Add(
                            new Base.Regex
                            {
                                Area = areaIndex,
                                Expression = regex,
                                Command = command,
                                Remark = remark,
                                IsAdmin = needAdmin
                            });
                    }
                }
                else if (ActionType == 2 && RegexListView.SelectedItem is Base.Regex selectedItem && selectedItem != null)
                {
                    selectedItem.Area = areaIndex;
                    selectedItem.Expression = regex;
                    selectedItem.Command = command;
                    selectedItem.Remark = remark;
                    selectedItem.IsAdmin = needAdmin;
                    RegexListView.SelectedItem = selectedItem;
                }
                Save();
                Load();
                ActionType = 0;
                return true;
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
