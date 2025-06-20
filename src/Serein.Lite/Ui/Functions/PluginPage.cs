using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Serein.Core.Models.Plugins;
using Serein.Core.Services.Plugins;
using Serein.Core.Services.Plugins.Js;
using Serein.Core.Services.Plugins.Net;
using Serein.Core.Utils;
using Serein.Core.Utils.Extensions;
using Serein.Lite.Utils;
using Serein.Lite.Utils.Native;

namespace Serein.Lite.Ui.Functions;

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

        _pluginListView.SetExploreTheme();

        _pluginManager = pluginManager;
        _netPluginLoader = netPluginLoader;
        _jsPluginLoader = jsPluginLoader;

        _pluginManager.PluginsReloading += (_, _) => Invoke(ConsoleWebBrowser.ClearLines);
        _pluginManager.PluginsLoaded += (_, _) => Invoke(SyncPlugins);
        SyncPlugins();
    }

    private void SyncPlugins()
    {
        _pluginListView.BeginUpdate();
        _pluginListView.Items.Clear();

        var plugins = _jsPluginLoader
            .Plugins.Select(kv => new KeyValuePair<string, IPlugin>(kv.Key, kv.Value))
            .Concat(
                _netPluginLoader.Plugins.Select(kv => new KeyValuePair<string, IPlugin>(
                    kv.Key,
                    kv.Value
                ))
            );

        foreach (var kv in plugins)
        {
            var item = new ListViewItem(kv.Value.Info.Name) { Tag = kv.Value };
            item.SubItems.Add(kv.Value.IsEnabled ? "启用" : "禁用");
            item.SubItems.Add(kv.Value.Info.Version.ToString());
            item.SubItems.Add(string.Join(',', kv.Value.Info.Authors.Select(x => x.Name)));
            item.SubItems.Add(kv.Value.Info.Description);
            _pluginListView.Items.Add(item);
        }

        _pluginListView.EndUpdate();

        _countDynamicLabel.Text = plugins.Count().ToString();
        _netCountDynamicLabel.Text = _netPluginLoader.Plugins.Count.ToString();
        _jsCountDynamicLabel.Text = _jsPluginLoader.Plugins.Count.ToString();
    }

    private void PluginListView_SelectedIndexChanged(object sender, EventArgs e)
    {
        pluginInfoLabel.Text =
            _pluginListView.SelectedItems.Count == 1
            && _pluginListView.SelectedItems[0].Tag is IPlugin plugin
                ? $"Id: {plugin.Info.Id} ({plugin.FileName})"
                : string.Empty;
    }

    private void ListViewContextMenuStrip_Opening(object sender, CancelEventArgs e)
    {
        _disableToolStripMenuItem.Enabled =
            _pluginListView.SelectedItems.Count == 1
            && _pluginListView.SelectedItems[0].Tag is IPlugin plugin
            && plugin.IsEnabled;
    }

    private void DisableToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (
            _pluginListView.SelectedItems.Count == 1
            && _pluginListView.SelectedItems[0].Tag is IPlugin plugin
            && plugin.IsEnabled
        )
        {
            plugin.Disable();
            _pluginListView.SelectedItems[0].ForeColor = Color.DarkGray;
            _pluginListView.SelectedItems[0].SubItems[1].Text = "禁用";
        }
    }

    private void ReloadToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (!_pluginManager.IsLoading && !_pluginManager.IsReloading)
        {
            Task.Run(_pluginManager.Reload);
        }
        else
        {
            MessageBoxHelper.ShowWarningMsgBox("正在加载插件中");
        }
    }

    private void ClearToolStripMenuItem_Click(object sender, EventArgs e)
    {
        ConsoleWebBrowser.ClearLines();
    }

    private void LookUpDocsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        UrlConstants.DocsPlugins.OpenInBrowser();
    }

    private void ReloadButton_Click(object sender, EventArgs e)
    {
        if (!_pluginManager.IsLoading && !_pluginManager.IsReloading)
        {
            Task.Run(_pluginManager.Reload);
        }
        else
        {
            MessageBoxHelper.ShowWarningMsgBox("正在加载插件中");
        }
    }

    private void ClearConsoleButton_Click(object sender, EventArgs e)
    {
        ConsoleWebBrowser.ClearLines();
    }
}
