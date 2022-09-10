using Serein.Base;
using System;
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
        private int ActionType = 0;
        public Regex()
        {
            InitializeComponent();
            Load();
            Window.Function.Regex = this;
        }

        private void Load()
        {
            Loader.ReadRegex();
            RegexListView.Items.Clear();
            foreach (Items.Regex Item in Global.RegexItems)
            {
                RegexListView.Items.Add(Item);
            }
        }

        private void Save()
        {
            List<Items.Regex> Items = new List<Items.Regex>();
            foreach (var obj in RegexListView.Items)
            {
                if (obj is Items.Regex Item && Item != null)
                {
                    Items.Add(Item);
                }
            }
            Global.UpdateRegexItems(Items);
            Loader.SaveRegex();
        }

        private void RegexListView_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            Edit.IsEnabled = RegexListView.SelectedIndex != -1;
            Delete.IsEnabled = RegexListView.SelectedIndex != -1;
            Clear.IsEnabled = RegexListView.Items.Count > 0;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Wpf.Ui.Controls.MenuItem Item && Item != null)
            {
                if (ActionType != 0)
                {
                    Window.MainWindow.RegexEditor.Hide();
                    ActionType = 0;
                }
                string Tag = Item.Tag as string ?? string.Empty;
                switch (Tag)
                {
                    case "Add":
                        Window.MainWindow.OpenRegexEditor();
                        ActionType = 1;
                        break;
                    case "Edit":
                        if (RegexListView.SelectedItem is Items.Regex SelectedItem && SelectedItem != null)
                        {
                            Window.MainWindow.OpenRegexEditor(
                                SelectedItem.Area,
                                SelectedItem.IsAdmin,
                                SelectedItem.Expression,
                                SelectedItem.Command,
                                SelectedItem.Remark
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
                        Process.Start(new ProcessStartInfo("https://serein.cc/Command.html") { UseShellExecute = true });
                        break;
                    case "LookupVariables":
                        Process.Start(new ProcessStartInfo("https://serein.cc/Variables.html") { UseShellExecute = true });
                        break;
                }
            }
        }

        public bool Confirm(int AreaIndex, bool IsAdmin, string Regex, string Command, string Remark)
        {
            if (Base.Command.GetType(Command) < 0)
            {
                Window.MainWindow.OpenSnackbar("编辑失败", "命令不合法", SymbolRegular.Warning24);
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
                                new Items.Regex()
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
                    else if (ActionType == 2 && RegexListView.SelectedItem is Items.Regex SelectedItem && SelectedItem != null)
                    {
                        SelectedItem.Area = AreaIndex;
                        SelectedItem.Expression = Regex;
                        SelectedItem.Command = Command;
                        SelectedItem.Remark = Remark;
                        SelectedItem.IsAdmin = IsAdmin;
                        RegexListView.SelectedItem = SelectedItem;
                    }
                    Save();
                    Load();
                    ActionType = 0;
                    return true;
                }
                catch
                {
                    Window.MainWindow.OpenSnackbar("编辑失败", "正则不合法", SymbolRegular.Warning24);
                }
            }
            return false;
        }

        private void UiPage_Drop(object sender, DragEventArgs e)
        {
            Array Data = (Array)e.Data.GetData(DataFormats.FileDrop);
            if (Data.Length > 0)
            {
                Window.MainWindow.OpenSnackbar("", Data.GetValue(0).ToString(), SymbolRegular.Sanitize20);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (RegexListView.SelectedIndex != -1 && sender is Wpf.Ui.Controls.Button Item && Item != null)
            {
                int Index = RegexListView.SelectedIndex;
                string Tag = Item.Tag as string ?? string.Empty;
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
