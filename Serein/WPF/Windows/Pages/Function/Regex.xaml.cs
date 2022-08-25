using System;
using System.Collections.Generic;
using System.Linq;
using Serein.Items;
using Serein.Base;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
            foreach (RegexItem Item in Global.RegexItems)
            {
                RegexListView.Items.Add(Item);
            }
        }

        private void Save()
        {
            List<RegexItem> Items = new List<RegexItem>();
            foreach (var obj in RegexListView.Items)
            {
                if (obj is RegexItem Item && Item != null)
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
                    Window.MainWindow.MemberEditor.Hide();
                    ActionType = 0;
                }
                string Tag = Item.Tag as string ?? string.Empty;
                switch (Tag)
                {
                    case "Add":
                    case "Edit":
                    case "Delete":
                        if (Logger.MsgBox("确定删除此行数据？\n它将会永远失去！（真的很久！）", "Serein", 1, 48) && RegexListView.SelectedIndex >= 0)
                        {
                            RegexListView.Items.RemoveAt(RegexListView.SelectedIndex);
                            Save();
                        }
                        break;
                    case "Clear":
                        if (Logger.MsgBox("确定删除所有数据？\n它将会永远失去！（真的很久！）", "Serein", 1, 48) && RegexListView.SelectedIndex >= 0)
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

        private void UiPage_Drop(object sender, DragEventArgs e)
        {
            Array Data = (Array)e.Data.GetData(DataFormats.FileDrop);
            if (Data.Length > 0)
            {
                Window.MainWindow.OpenSnackbar("", Data.GetValue(0).ToString(), Wpf.Ui.Common.SymbolRegular.Sanitize20);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Wpf.Ui.Controls.Button Item && Item != null)
            {
                string Tag = Item.Tag as string ?? string.Empty;
                switch (Tag) { }
            }
        }
    }
}
