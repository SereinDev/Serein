using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Serein.Core.Models.Server;
using Serein.Core.Services.Servers;
using Serein.Core.Utils.Extensions;
using Serein.Lite.Utils;
using Serein.Lite.Utils.Native;

namespace Serein.Lite.Ui.Servers;

public partial class PluginManagerForm : Form
{
    private readonly Server _server;
    private readonly ListViewGroup _jsGroup = new("JavaScript");
    private readonly ListViewGroup _luaGroup = new("Lua");
    private readonly ListViewGroup _jarGroup = new("Java");
    private readonly ListViewGroup _pyGroup = new("Python");
    private readonly ListViewGroup _dllGroup = new("动态链接库");

    private IEnumerable<ServerPlugin?> SelectedPlugins =>
        _pluginListView
            .SelectedItems.OfType<ListViewItem>()
            .Select((item) => item.Tag as ServerPlugin);

    public PluginManagerForm(Server server)
    {
        _server = server;

        InitializeComponent();
        UpdateText();

        _pluginListView.Groups.AddRange([_dllGroup, _jarGroup, _jsGroup, _luaGroup, _pyGroup]);
    }

    private void LoadData()
    {
        _pluginListView.BeginUpdate();
        _pluginListView.Items.Clear();

        foreach (var group in _pluginListView.Groups)
        {
            if (group is ListViewGroup listViewGroup)
            {
                listViewGroup.Items.Clear();
            }
        }

        foreach (var plugin in _server.PluginManager.Plugins)
        {
            var item = new ListViewItem(plugin.FriendlyName)
            {
                Tag = plugin,
                ToolTipText = plugin.Path,
            };

            if (plugin.FileInfo.Exists)
            {
                item.SubItems.Add(plugin.FileInfo.Length.ToSizeString());
            }
            else
            {
                item.SubItems.Add("-");
            }

            if (!plugin.IsEnabled)
            {
                item.ForeColor = Color.DarkGray;
                item.Text += " (已禁用)";
            }

            switch (plugin.Type)
            {
                case PluginType.Library:
                    _dllGroup.Items.Add(item);
                    break;

                case PluginType.Java:
                    _jarGroup.Items.Add(item);
                    break;

                case PluginType.JavaScript:
                    _jsGroup.Items.Add(item);
                    break;

                case PluginType.Lua:
                    _luaGroup.Items.Add(item);
                    break;

                case PluginType.Python:
                    _pyGroup.Items.Add(item);
                    break;
            }

            _pluginListView.Items.Add(item);
        }

        _pluginListView.EndUpdate();
    }

    private void UpdateText()
    {
        _toolStripStatusLabel.Text =
            _pluginListView.SelectedItems.Count > 0
                ? $"共{_pluginListView.Items.Count}项；已选择{_pluginListView.SelectedItems.Count}项"
                : $"共{_pluginListView.Items.Count}项";
    }

    private void PluginListView_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateText();
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        Text = "插件管理 - " + _server.Configuration.Name;
        _pluginListView.SetExploreTheme();
        LoadData();
    }

    private void ListViewContextMenuStrip_Opening(object sender, CancelEventArgs e)
    {
        var selectedPlugins = SelectedPlugins;

        _removeToolStripMenuItem.Enabled = _pluginListView.SelectedItems.Count > 0;
        _enableToolStripMenuItem.Enabled =
            selectedPlugins.Any() && !selectedPlugins.Any((plugin) => plugin?.IsEnabled ?? false);
        _disableToolStripMenuItem.Enabled =
            selectedPlugins.Any() && !selectedPlugins.Any((plugin) => !plugin?.IsEnabled ?? false);
    }

    private void AddToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "可接受的插件文件|*.dll;*.jar;*.js;*.py;*.lua",
        };
        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            try
            {
                _server.PluginManager.Add(openFileDialog.FileNames);
            }
            catch (Exception ex)
            {
                MessageBoxHelper.ShowWarningMsgBox("导入失败：\r\n" + ex.Message);
            }
        }

        _server.PluginManager.Update();
        LoadData();
    }

    private void EnableToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var selectedPlugins = SelectedPlugins;
        foreach (var plugin in selectedPlugins)
        {
            if (plugin is not null)
            {
                try
                {
                    _server.PluginManager.Enable(plugin);
                }
                catch (Exception ex)
                {
                    MessageBoxHelper.ShowWarningMsgBox(
                        $"启用插件\"{plugin.FriendlyName}\"失败：\r\n" + ex.Message
                    );
                    break;
                }
            }
        }

        _server.PluginManager.Update();
        LoadData();
    }

    private void DisableToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var selectedPlugins = SelectedPlugins;
        foreach (var plugin in selectedPlugins)
        {
            if (plugin is not null)
            {
                try
                {
                    _server.PluginManager.Disable(plugin);
                }
                catch (Exception ex)
                {
                    MessageBoxHelper.ShowWarningMsgBox(
                        $"禁用插件\"{plugin.FriendlyName}\"失败：\r\n" + ex.Message
                    );
                    break;
                }
            }
        }
        _server.PluginManager.Update();
        LoadData();
    }

    private void RemoveToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var selectedPlugins = SelectedPlugins;

        var count = selectedPlugins.Count();
        if (
            count <= 0
            || !MessageBoxHelper.ShowDeleteConfirmation(
                count == 1
                    ? $"确定要删除\"{selectedPlugins.First()?.FriendlyName}\"吗？"
                    : $"确定要删除\"{selectedPlugins.First()?.FriendlyName}\"等{count}个插件吗？"
            )
        )
        {
            return;
        }

        foreach (var plugin in selectedPlugins)
        {
            if (plugin is not null)
            {
                try
                {
                    _server.PluginManager.Remove(plugin);
                }
                catch (Exception ex)
                {
                    MessageBoxHelper.ShowWarningMsgBox(
                        $"删除插件\"{plugin.FriendlyName}\"失败：\r\n" + ex.Message
                    );
                    break;
                }
            }
        }

        _server.PluginManager.Update();
        LoadData();
    }

    private void ShowInExplorerToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var selectedPlugin = SelectedPlugins.FirstOrDefault();
        if (selectedPlugin is not null)
        {
            selectedPlugin.Path.OpenInExplorer();
        }
        else if (Directory.Exists(_server.PluginManager.CurrentPluginsDirectory))
        {
            _server.PluginManager.CurrentPluginsDirectory.OpenInExplorer();
        }
    }

    private void RefreshToolStripMenuItem_Click(object sender, EventArgs e)
    {
        _server.PluginManager.Update();
        LoadData();
    }

    private void PluginManagerForm_DragDrop(object sender, DragEventArgs e)
    {
        FocusWindow();
        var files = e.Data?.GetData(DataFormats.FileDrop) as string[];

        if (files?.Length > 0)
        {
            var acceptable = files.Where(
                (f) => ServerPluginManager.AcceptableExtensions.Contains(Path.GetExtension(f))
            );
            if (
                acceptable.Any()
                && MessageBoxHelper.ShowQuestionMsgBox(
                    "是否将以下文件作为插件导入到服务器的插件文件夹？\r\n"
                        + string.Join("\r\n", files.Select((f) => Path.GetFileName(f)))
                )
            )
            {
                try
                {
                    _server.PluginManager.Add(acceptable.ToArray());
                }
                catch (Exception ex)
                {
                    MessageBoxHelper.ShowWarningMsgBox("导入失败：\r\n" + ex.Message);
                }
            }
        }
    }

    private void PluginManagerForm_DragEnter(object sender, DragEventArgs e)
    {
        e.Effect =
            e.Data?.GetDataPresent(DataFormats.FileDrop) == true
                ? DragDropEffects.Copy
                : DragDropEffects.None;
    }

    private void FocusWindow()
    {
        Visible = true;
        ShowInTaskbar = true;
        WindowState = FormWindowState.Normal;
        Activate();
    }
}
