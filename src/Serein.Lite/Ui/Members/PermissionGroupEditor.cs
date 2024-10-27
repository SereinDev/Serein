using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using Serein.Core.Services.Permissions;
using Serein.Lite.Utils;
using Serein.Lite.Utils.Native;

namespace Serein.Lite.Ui.Members;

public partial class PermissionGroupEditor : Form
{
    private readonly Core.Models.Permissions.Group _group;
    private readonly GroupManager _groupManager;
    private readonly PermissionManager _permissionManager;
    public string Id => IdTextBox.Text;

    [GeneratedRegex(@"^\w+$")]
    private static partial Regex IdRegex();

    public PermissionGroupEditor(
        GroupManager groupManager,
        PermissionManager permissionManager,
        Core.Models.Permissions.Group group,
        string? id = null
    )
    {
        InitializeComponent();

        if (!string.IsNullOrEmpty(id))
        {
            IdTextBox.ReadOnly = true;
            IdTextBox.Text = id;
        }

        _group = group;
        _groupManager = groupManager;
        _permissionManager = permissionManager;

        SyncData();
    }

    private void SyncData()
    {
        MemberListView.SetExploreTheme();
        PermissionListView.SetExploreTheme();
        NameTextBox.Text = _group.Name;
        DescriptionTextBox.Text = _group.Description;
        PriorityNumericUpDown.Value = _group.Priority;
        ParentsTextBox.Text = string.Join("\r\n", _group.Parents);

        foreach (var kv in _group.Nodes)
        {
            var item = new ListViewItem(kv.Key) { Tag = kv };

            if (_permissionManager.Nodes.TryGetValue(kv.Key, out var description))
                item.SubItems.Add(description);
            else
                item.SubItems.Add(string.Empty);

            item.SubItems.Add(kv.Value is not null ? kv.Value.ToString() : "Null");
            PermissionListView.Items.Add(item);
        }
    }

    private void PermissionComboBox_DropDown(object sender, EventArgs e)
    {
        PermissionComboBox.Items.Clear();

        lock (_permissionManager.Nodes)
            foreach (var key in _permissionManager.Nodes.Keys)
                PermissionComboBox.Items.Add(key);
    }

    private void PermissionListView_SelectedIndexChanged(object sender, EventArgs e)
    {
        PermissionComboBox.Enabled = PermissionListView.SelectedItems.Count == 1;
        ValueComboBox.Enabled = PermissionListView.SelectedItems.Count == 1;

        PermissionComboBox.Text =
            PermissionListView.SelectedItems.Count == 1
                ? PermissionListView.SelectedItems[0].Text
                : "选择一项进行修改";
        ValueComboBox.Text =
            PermissionListView.SelectedItems.Count == 1
                ? PermissionListView.SelectedItems[0].SubItems[2].Text
                : string.Empty;
    }

    private void PermissionComboBox_TextUpdate(object sender, EventArgs e)
    {
        if (PermissionListView.SelectedItems.Count == 1)
        {
            PermissionListView.SelectedItems[0].Text = PermissionComboBox.Text;
            PermissionListView.SelectedItems[0].SubItems[1].Text =
                _permissionManager.Nodes.TryGetValue(
                    PermissionComboBox.Text,
                    out var description
                )
                    ? description
                    : string.Empty;
        }
    }

    private void ValueComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (PermissionListView.SelectedItems.Count == 1)
            PermissionListView.SelectedItems[0].SubItems[2].Text = ValueComboBox.Text;
    }

    private void PermissionContextMenuStrip_Opening(object sender, CancelEventArgs e)
    {
        DeletePermissionToolStripMenuItem.Enabled = PermissionListView.SelectedItems.Count == 1;
    }

    private void AddPermissionToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var item = new ListViewItem(".");
        item.SubItems.Add(string.Empty);
        item.SubItems.Add("Null");

        if (PermissionListView.SelectedItems.Count == 1)
            PermissionListView.Items.Insert(PermissionListView.SelectedItems[0].Index, item);
        else
            PermissionListView.Items.Add(item);
    }

    private void DeletePermissionToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (PermissionListView.SelectedItems.Count == 1)
            PermissionListView.Items.RemoveAt(PermissionListView.SelectedItems[0].Index);
    }

    private void ConfirmButton_Click(object sender, EventArgs e)
    {
        lock (_group)
            try
            {
                GroupManager.ValidateGroupId(Id);

                if (!IdTextBox.ReadOnly && _groupManager.Ids.Contains(Id))
                    throw new InvalidOperationException("此Id已被占用");

                _group.Name = NameTextBox.Text;
                _group.Description = DescriptionTextBox.Text;
                _group.Priority = (int)PriorityNumericUpDown.Value;
                _group.Parents = ParentsTextBox.Text.Split(
                    "\r\n",
                    StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
                );

                var permissions = new Dictionary<string, bool?>();
                foreach (var item in PermissionListView.Items)
                {
                    if (item is ListViewItem listViewItem)
                        permissions[listViewItem.Text] = listViewItem.SubItems[2].Text switch
                        {
                            "True" => true,
                            "False" => false,
                            _ => null
                        };
                }
                _group.Nodes = permissions;

                var list = new List<long>();
                foreach (var item in MemberListView.Items)
                    if (
                        item is ListViewItem listViewItem
                        && long.TryParse(listViewItem.Text, out var id)
                        && !list.Contains(id)
                    )
                        list.Add(id);
                _group.Members = [.. list];

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBoxHelper.ShowWarningMsgBox(ex.Message);
            }
    }

    private void MemberContextMenuStrip_Opening(object sender, CancelEventArgs e)
    {
        DeleteMemberToolStripMenuItem.Enabled = MemberListView.SelectedItems.Count == 1;
    }

    private void AddMemberToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (MemberListView.SelectedItems.Count == 1)
            MemberListView.Items.Insert(MemberListView.SelectedItems[0].Index, "0");
        else
            MemberListView.Items.Add("0");
    }

    private void DeleteMemberToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (MemberListView.SelectedItems.Count == 1)
            MemberListView.Items.RemoveAt(MemberListView.SelectedItems[0].Index);
    }

    private void IdTextBox_Enter(object sender, EventArgs e)
    {
        ErrorProvider.Clear();
    }

    private void IdTextBox_Validating(object sender, CancelEventArgs e)
    {
        if (string.IsNullOrEmpty(IdTextBox.Text))
            ErrorProvider.SetError(IdTextBox, "Id不能为空");
        else if (!IdRegex().IsMatch(IdTextBox.Text))
            ErrorProvider.SetError(IdTextBox, "Id只能由字母、数字和下划线组成");
        else if (IdTextBox.Text.Length <= 2)
            ErrorProvider.SetError(IdTextBox, "Id长度太短");
    }

    private void ParentsTextBox_Enter(object sender, EventArgs e)
    {
        ErrorProvider.Clear();
    }

    private void ParentsTextBox_Validating(object sender, CancelEventArgs e)
    {
        var nonExistent = ParentsTextBox
            .Text.Split(
                "\r\n",
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
            )
            .Where((id) => !_groupManager.Ids.Contains(id));

        if (nonExistent.Any())
            ErrorProvider.SetError(
                ParentsTextBox,
                $"以下权限组不存在：\r\n" + string.Join("\r\n", nonExistent)
            );
    }
}
