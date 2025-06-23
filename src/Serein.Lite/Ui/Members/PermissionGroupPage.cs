using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Serein.Core.Models.Permissions;
using Serein.Core.Services.Data;
using Serein.Core.Services.Permissions;
using Serein.Lite.Models;
using Serein.Lite.Utils;
using Serein.Lite.Utils.Native;

namespace Serein.Lite.Ui.Members;

public partial class PermissionGroupPage : UserControl, IUpdateablePage
{
    private readonly GroupManager _groupManager;
    private readonly PermissionGroupProvider _permissionGroupProvider;
    private readonly PermissionManager _permissionManager;

    public PermissionGroupPage(
        GroupManager groupManager,
        PermissionGroupProvider permissionGroupProvider,
        PermissionManager permissionManager
    )
    {
        InitializeComponent();
        _groupManager = groupManager;
        _permissionGroupProvider = permissionGroupProvider;
        _permissionManager = permissionManager;
        _groupListView.SetExploreTheme();
    }

    private void GroupContextMenuStrip_Opening(object sender, CancelEventArgs e)
    {
        _editToolStripMenuItem.Enabled = _groupListView.SelectedItems.Count == 1;
        _deleteToolStripMenuItem.Enabled =
            _groupListView.SelectedItems.Count == 1
            && _groupListView.SelectedItems[0].Text != "everyone";
    }

    private void LoadData()
    {
        _groupListView.BeginUpdate();
        _groupListView.Items.Clear();

        lock (_permissionGroupProvider.Value)
        {
            foreach (var kv in _permissionGroupProvider.Value)
            {
                var item = new ListViewItem(kv.Key) { Tag = kv };
                item.SubItems.Add(kv.Value.Name);
                item.SubItems.Add(kv.Value.Description);
                item.SubItems.Add(kv.Value.Priority.ToString());

                _groupListView.Items.Add(item);
            }
        }
        _groupListView.EndUpdate();
    }

    public void UpdatePage() => LoadData();

    private void AddToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var group = new Group();
        var dialog = new PermissionGroupEditor(_groupManager, _permissionManager, group);
        if (dialog.ShowDialog() != DialogResult.OK)
        {
            return;
        }

        _permissionGroupProvider.Value.TryAdd(dialog.Id, group);
        _permissionGroupProvider.SaveAsyncWithDebounce();

        LoadData();
    }

    private void EditToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (
            _groupListView.SelectedItems.Count != 1
            || _groupListView.SelectedItems[0].Tag is not KeyValuePair<string, Group> kv
        )
        {
            return;
        }

        var dialog = new PermissionGroupEditor(_groupManager, _permissionManager, kv.Value, kv.Key);
        if (dialog.ShowDialog() != DialogResult.OK)
        {
            return;
        }

        _permissionGroupProvider.SaveAsyncWithDebounce();
        LoadData();
    }

    private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (
            _groupListView.SelectedItems.Count != 1
            || !MessageBoxHelper.ShowDeleteConfirmation("你确定要删除所选项吗？")
        )
        {
            return;
        }

        _permissionGroupProvider.Value.Remove(_groupListView.SelectedItems[0].Text);
        _permissionGroupProvider.SaveAsyncWithDebounce();
        LoadData();
    }

    private void RefreshToolStripMenuItem_Click(object sender, EventArgs e)
    {
        _permissionGroupProvider.Read();
        LoadData();
    }
}
