using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Serein.Core.Models.Plugins;
using Serein.Core.Services.Plugins;
using Serein.Core.Services.Plugins.Js;
using Serein.Core.Services.Plugins.Net;

namespace Serein.Lite.Ui.Function;

public partial class PluginPage : UserControl
{
    private readonly PluginManager _pluginManager;
    private readonly NetPluginLoader _netPluginLoader;
    private readonly JsPluginLoader _jsPluginLoader;

    public PluginPage(
        PluginManager pluginManager,
        NetPluginLoader netPluginLoader,
        JsPluginLoader jsPluginLoader
    )
    {
        InitializeComponent();

        _pluginManager = pluginManager;
        _netPluginLoader = netPluginLoader;
        _jsPluginLoader = jsPluginLoader;

        _pluginManager.PluginsReloading += (_, _) => Invoke(SyncPlugins);
        SyncPlugins();
    }

    private void SyncPlugins()
    {
        PluginListView.BeginUpdate();
        PluginListView.Items.Clear();
        ConsoleWebBrowser.ClearLines();

        var plugins = _jsPluginLoader
            .Plugins
            .Select(kv => new KeyValuePair<string, IPlugin>(kv.Key, kv.Value))
            .Concat(
                _netPluginLoader
                .Plugins
                .Select(kv => new KeyValuePair<string, IPlugin>(kv.Key, kv.Value)
                ));

        foreach (var kv in plugins)
        {
            var item = new ListViewItem(kv.Value.Info.Name);
            item.SubItems.Add(kv.Value.IsEnabled ? "启用" : "禁用");
            item.SubItems.Add(kv.Value.Info.Version.ToString());
            item.SubItems.Add(string.Join(',', kv.Value.Info.Authors.Select(x => x.Name)));
            item.SubItems.Add(kv.Value.Info.Description);
            PluginListView.Items.Add(item);
        }

        PluginListView.EndUpdate();
    }

    private void PluginListView_SelectedIndexChanged(object sender, EventArgs e)
    {
        ToolStripStatusLabel.Text =
            PluginListView.SelectedItems.Count == 1
            && PluginListView.SelectedItems[0].Tag is IPlugin plugin
                ? $"{plugin.FileName}"
                : string.Empty;
    }
}
