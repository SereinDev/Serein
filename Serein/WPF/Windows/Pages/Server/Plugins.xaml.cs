using System;
using System.Collections.Generic;
using System.IO;
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
using Serein.Server;
using Wpf.Ui.Controls;

namespace Serein.Windows.Pages.Server
{
    public partial class Plugins : UiPage
    {
        private static List<string> Extensions = new List<string>()
        {
            ".dll",".js",".jar",".py",".lua",".exe",".lock"
        };

        public Plugins()
        {
            InitializeComponent();
            Window.Server.Plugins = this;
            Load();
        }

        private void Load()
        {
            PluginsListview.Items.Clear();
            if (PluginManager.Get() != null)
            {
                foreach (string Plugin in PluginManager.Get())
                {
                    if (Extensions.Contains(Path.GetExtension(Plugin).ToLower()))
                    {
                        ListViewItem Item = new ListViewItem();
                        Item.Content = GetRelativeUri(PluginManager.PluginPath, Plugin);
                        if (Path.GetExtension(Plugin).ToLower() == ".lock") { Item.Foreground = Brushes.LightGray; }
                        PluginsListview.Items.Add(Item);
                    }
                }
            }
        }

        private string GetRelativeUri(string Base, string Path)
        {
            return new Uri(Base).MakeRelativeUri(new Uri(Path)).OriginalString;
        }

        private void PluginsListview_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            Import.IsEnabled = PluginsListview.SelectedIndex != -1;
            Delete.IsEnabled = PluginsListview.SelectedIndex != -1;
            Enable.IsEnabled = PluginsListview.SelectedIndex != -1;
            Disable.IsEnabled = PluginsListview.SelectedIndex != -1;
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Enable_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Disable_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OpenFolder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            Load();
        }
    }
}
