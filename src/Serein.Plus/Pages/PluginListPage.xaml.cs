using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using iNKORE.UI.WPF.Modern.Controls;
using Serein.Core.Models.Plugins;
using Serein.Core.Services.Plugins;
using Serein.Core.Services.Plugins.Js;
using Serein.Core.Services.Plugins.Net;
using Serein.Plus.Services;
using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace Serein.Plus.Pages;

public partial class PluginListPage : Page
{
    private readonly PluginConsolePage _consolePage;
    private readonly InfoBarProvider _infoBarProvider;
    private readonly PluginManager _pluginManager;
    private readonly JsPluginLoader _jsPluginLoader;
    private readonly NetPluginLoader _netPluginLoader;

    private readonly ObservableCollection<KeyValuePair<string, IPlugin>> _pluginInfos;

    public PluginListPage(
        PluginConsolePage consolePage,
        InfoBarProvider infoBarProvider,
        PluginManager pluginManager,
        JsPluginLoader jsPluginLoader,
        NetPluginLoader netPluginLoader
    )
    {
        _consolePage = consolePage;
        _infoBarProvider = infoBarProvider;
        _pluginManager = pluginManager;
        _jsPluginLoader = jsPluginLoader;
        _netPluginLoader = netPluginLoader;
        _pluginInfos = [];

        InitializeComponent();
        UpdatePlugins();

        PluginListView.ItemsSource = _pluginInfos;
        _pluginManager.PluginsLoaded += (_, _) => Dispatcher.Invoke(UpdatePlugins);
    }

    private void UpdatePlugins()
    {
        _pluginInfos.Clear();

        foreach (var kv in _jsPluginLoader.Plugins)
        {
            _pluginInfos.Add(new(kv.Key, kv.Value));
        }

        foreach (var kv in _netPluginLoader.Plugins)
        {
            _pluginInfos.Add(new(kv.Key, kv.Value));
        }
    }

    private void MenuItem_Click(object sender, RoutedEventArgs e)
    {
        var tag = (sender as MenuItem)?.Tag as string;
        try
        {
            switch (tag)
            {
                case "OpenDoc":
                    break;

                case "Reload":
                    Task.Run(_pluginManager.Reload)
                        .ContinueWith(
                            (task) =>
                            {
                                if (task.IsFaulted && task.Exception is not null)
                                {
                                    _infoBarProvider.Enqueue(
                                        "重新加载插件失败",
                                        task.Exception.InnerException!.Message,
                                        InfoBarSeverity.Error
                                    );
                                }
                            }
                        );
                    break;

                case "ClearConsole":
                    _consolePage.Console.Clear();
                    break;

                case "Disable":
                    if (PluginListView.SelectedItem is KeyValuePair<string, IPlugin> kv)
                    {
                        kv.Value.Disable();
                        _infoBarProvider.Enqueue(
                            $"插件（Id={kv.Key}）禁用成功",
                            string.Empty,
                            InfoBarSeverity.Success
                        );
                    }
                    break;
            }
        }
        catch (Exception ex)
        {
            if (tag == "Disable")
            {
                _infoBarProvider.Enqueue("禁用失败", ex.Message, InfoBarSeverity.Error);
            }
        }
    }

    private void PluginListView_ContextMenuOpening(object sender, ContextMenuEventArgs e)
    {
        DisableMenuItem.IsEnabled = PluginListView.SelectedIndex >= 0;
    }

    private void PluginListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        StatusBar.Text = PluginListView.SelectedItem is KeyValuePair<string, IPlugin> kv
            ? $"Id={kv.Key}\r\nPath={kv.Value.FileName}"
            : string.Empty;
    }
}
