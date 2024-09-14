using System;
using System.Windows;
using System.Windows.Controls;

using iNKORE.UI.WPF.Modern.Controls;

using Serein.Core.Services.Plugins;
using Serein.Core.Services.Plugins.Js;
using Serein.Core.Services.Plugins.Net;
using Serein.Plus.Services;
using Serein.Plus.ViewModels;

using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace Serein.Plus.Pages;

public partial class PluginConsolePage : Page
{
    private readonly InfoBarProvider _infoBarProvider;
    private readonly PluginManager _pluginManager;
    private readonly NetPluginLoader _netPluginLoader;
    private readonly JsPluginLoader _jsPluginLoader;

    public PluginConsolePage(
        PluginConsoleViewModel pluginConsoleViewModel,
        InfoBarProvider infoBarProvider,
        PluginManager pluginManager,
        NetPluginLoader netPluginLoader,
        JsPluginLoader jsPluginLoader
        )
    {
        ViewModel = pluginConsoleViewModel;
        _infoBarProvider = infoBarProvider;
        _pluginManager = pluginManager;
        _netPluginLoader = netPluginLoader;
        _jsPluginLoader = jsPluginLoader;
        DataContext = ViewModel;

        InitializeComponent();
        UpdateViewModel();
        Console.EnableLogLevelHighlight();

        _pluginManager.PluginsLoaded += (_, _) => UpdateViewModel();
    }

    public PluginConsoleViewModel ViewModel { get; }

    private void UpdateViewModel()
    {
        ViewModel.JavaScriptPluginCount = _jsPluginLoader.Plugins.Count;
        ViewModel.NetPluginCount = _netPluginLoader.Plugins.Count;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        switch ((sender as Button)?.Tag?.ToString())
        {
            case "Clear":
                Console.Clear();
                break;

            case "Reload":
                try
                {
                    _pluginManager.Reload();
                }
                catch (Exception ex)
                {
                    _infoBarProvider.Enqueue("重新加载插件失败", ex.Message, InfoBarSeverity.Error);
                }
                break;
        }
    }
}
