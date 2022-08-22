using Microsoft.Win32;
using Serein.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
using System.Net;
using Wpf.Ui.Mvvm.Contracts;

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
            Load();
            Window.Server.Plugins = this;
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
            return WebUtility.UrlDecode(new Uri(Base).MakeRelativeUri(new Uri(Path)).OriginalString);
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
            OpenFileDialog Dialog = new OpenFileDialog()
            {
                Filter = "所有文件|*.*",
                Multiselect = true
            };
            if (Dialog.ShowDialog() == true)
            {
                string Msg = PluginManager.Add(Dialog.FileNames.ToList());
                if (!string.IsNullOrEmpty(Msg))
                {
                    Window.MainWindow.OpenSnackbar("导入失败", Msg, SymbolRegular.ErrorCircle24);
                }
                Load();
            }
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
