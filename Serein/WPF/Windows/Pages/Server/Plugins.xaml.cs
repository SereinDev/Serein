﻿using Microsoft.Win32;
using Serein.Base;
using Serein.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace Serein.Windows.Pages.Server
{
    public partial class Plugins : UiPage
    {
        /// <summary>
        /// 可识别的扩展名
        /// </summary>
        private static readonly List<string> Extensions = new List<string>()
        {
            ".dll",".js",".jar",".py",".lua",".ts",".lock"
        };

        public Plugins()
        {
            InitializeComponent();
            Load();
            Catalog.Server.Plugins = this;
        }

        /// <summary>
        /// 加载插件列表
        /// </summary>
        public void Load()
        {
            PluginsListview.Items.Clear();
            if (PluginManager.Get() != null)
            {
                foreach (string plugin in PluginManager.Get())
                {
                    if (Extensions.Contains(Path.GetExtension(plugin).ToLowerInvariant()))
                    {
                        ListViewItem listViewItem = new()
                        {
                            Content = PluginManager.GetRelativeUri(plugin),
                            Opacity = Path.GetExtension(plugin).ToLowerInvariant() != ".lock" ? 1 : 0.5
                        };
                        PluginsListview.Items.Add(listViewItem);
                    }
                }
            }
        }

        private void PluginsListview_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            Delete.IsEnabled = PluginsListview.SelectedIndex != -1;
            Enable.IsEnabled = PluginsListview.SelectedIndex != -1;
            Disable.IsEnabled = PluginsListview.SelectedIndex != -1;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Wpf.Ui.Controls.MenuItem menuItem && menuItem != null)
            {
                string Tag = menuItem.Tag as string ?? string.Empty;
                switch (Tag)
                {
                    case "Refresh":
                        Load();
                        break;
                    case "Disable":
                    case "Enable":
                    case "Delete":
                        List<string> Files = new List<string>();
                        foreach (object File in PluginsListview.SelectedItems)
                        {
                            if (File != null)
                            {
                                Files.Add(PluginManager.GetAbsoluteUri((File as ListViewItem).Content.ToString()));
                            }
                        }
                        switch (Tag)
                        {
                            case "Disable":
                                PluginManager.Disable(Files);
                                break;
                            case "Enable":
                                PluginManager.Enable(Files);
                                break;
                            case "Delete":
                                PluginManager.Remove(Files);
                                break;
                        }
                        Load();
                        break;
                    case "Import":
                        OpenFileDialog Dialog = new()
                        {
                            Filter = "所有文件|*.*",
                            Multiselect = true
                        };
                        if (Dialog.ShowDialog() ?? false)
                        {
                            PluginManager.Add(Dialog.FileNames);
                        }
                        Load();
                        break;
                    case "OpenFolder":
                        if (PluginsListview.SelectedIndex < 0)
                        {
                            PluginManager.OpenFolder();
                        }
                        else
                        {
                            PluginManager.OpenFolder(PluginManager.GetAbsoluteUri((PluginsListview.SelectedItems[0] as ListViewItem).Content.ToString()));
                        }
                        break;
                    default:
                        Logger.Out(Items.LogType.Debug, new ArgumentException());
                        break;
                }
            }
        }
    }
}