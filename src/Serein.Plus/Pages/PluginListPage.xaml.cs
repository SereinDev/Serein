using System.Collections.ObjectModel;

using iNKORE.UI.WPF.Modern.Controls;

using Serein.Core.Models.Plugins;
using Serein.Core.Services.Plugins;
using Serein.Core.Services.Plugins.Js;
using Serein.Core.Services.Plugins.Net;

namespace Serein.Plus.Pages;

public partial class PluginListPage : Page
{
    private readonly PluginManager _pluginManager;
    private readonly JsPluginLoader _jsPluginLoader;
    private readonly NetPluginLoader _netPluginLoader;

    private readonly ObservableCollection<IPlugin> _pluginInfos;

    public PluginListPage(PluginManager pluginManager, JsPluginLoader jsPluginLoader, NetPluginLoader netPluginLoader)
    {
        _pluginManager = pluginManager;
        _jsPluginLoader = jsPluginLoader;
        _netPluginLoader = netPluginLoader;
        _pluginInfos = [];

        InitializeComponent();
        UpdatePlugins();

        PluginDataGrid.ItemsSource = _pluginInfos;
        _pluginManager.PluginsLoaded += (_, _) => Dispatcher.Invoke(UpdatePlugins);
    }

    private void UpdatePlugins()
    {
        _pluginInfos.Clear();

        foreach (var plugin in _jsPluginLoader.Plugins.Values)
            _pluginInfos.Add(plugin);

        foreach (var plugin in _netPluginLoader.Plugins.Values)
            _pluginInfos.Add(plugin);
    }
}
