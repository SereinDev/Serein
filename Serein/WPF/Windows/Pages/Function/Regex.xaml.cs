using System;
using System.Collections.Generic;
using System.Linq;
using Serein.Items;
using Serein.Base;
using System.Text;
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
            Loader.LoadRegex();
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
                }
            }
        }
    }
}
