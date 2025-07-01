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
    public string Id => _idTextBox.Text;

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
            _idTextBox.ReadOnly = true;
            _idTextBox.Text = id;
        }

        _group = group;
        _groupManager = groupManager;
        _permissionManager = permissionManager;

        SyncData();
    }

    private void SyncData()
    {
        _memberListView.SetExploreTheme();
        _permissionListView.SetExploreTheme();
        _nameTextBox.Text = _group.Name;
        _descriptionTextBox.Text = _group.Description;
        _priorityNumericUpDown.Value = _group.Priority;
        _parentsTextBox.Text = string.Join("\r\n", _group.Parents);

        foreach (var kv in _group.Nodes)
        {
            var item = new ListViewItem(kv.Key) { Tag = kv };

            if (_permissionManager.Nodes.TryGetValue(kv.Key, out var description))
            {
                item.SubItems.Add(description);
            }
            else
            {
                item.SubItems.Add(string.Empty);
            }

            item.SubItems.Add(kv.Value is not null ? kv.Value.ToString() : "Null");
            _permissionListView.Items.Add(item);
        }
    }

    private void PermissionComboBox_DropDown(object sender, EventArgs e)
    {
        _permissionComboBox.Items.Clear();

        lock (_permissionManager.Nodes)
        {
            foreach (var key in _permissionManager.Nodes.Keys)
            {
                _permissionComboBox.Items.Add(key);
            }
        }
    }

    private void PermissionListView_SelectedIndexChanged(object sender, EventArgs e)
    {
        _permissionComboBox.Enabled = _permissionListView.SelectedItems.Count == 1;
        _valueComboBox.Enabled = _permissionListView.SelectedItems.Count == 1;

        _permissionComboBox.Text =
            _permissionListView.SelectedItems.Count == 1
                ? _permissionListView.SelectedItems[0].Text
                : "选择一项进行修改";
        _valueComboBox.Text =
            _permissionListView.SelectedItems.Count == 1
                ? _permissionListView.SelectedItems[0].SubItems[2].Text
                : string.Empty;
    }

    private void PermissionComboBox_TextUpdate(object sender, EventArgs e)
    {
        if (_permissionListView.SelectedItems.Count == 1)
        {
            _permissionListView.SelectedItems[0].Text = _permissionComboBox.Text;
            _permissionListView.SelectedItems[0].SubItems[1].Text =
                _permissionManager.Nodes.TryGetValue(_permissionComboBox.Text, out var description)
                    ? description
                    : string.Empty;
        }
    }

    private void ValueComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_permissionListView.SelectedItems.Count == 1)
        {
            _permissionListView.SelectedItems[0].SubItems[2].Text = _valueComboBox.Text;
        }
    }

    private void PermissionContextMenuStrip_Opening(object sender, CancelEventArgs e)
    {
        _deletePermissionToolStripMenuItem.Enabled = _permissionListView.SelectedItems.Count == 1;
    }

    private void AddPermissionToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var item = new ListViewItem(".");
        item.SubItems.Add(string.Empty);
        item.SubItems.Add("Null");

        if (_permissionListView.SelectedItems.Count == 1)
        {
            _permissionListView.Items.Insert(_permissionListView.SelectedItems[0].Index, item);
        }
        else
        {
            _permissionListView.Items.Add(item);
        }
    }

    private void DeletePermissionToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (_permissionListView.SelectedItems.Count == 1)
        {
            _permissionListView.Items.RemoveAt(_permissionListView.SelectedItems[0].Index);
        }
    }

    private void ConfirmButton_Click(object sender, EventArgs e)
    {
        lock (_group)
        {
            try
            {
                GroupManager.ValidateGroupId(Id);

                if (!_idTextBox.ReadOnly && _groupManager.Ids.Contains(Id))
                {
                    throw new InvalidOperationException("此Id已被占用");
                }

                _group.Name = _nameTextBox.Text;
                _group.Description = _descriptionTextBox.Text;
                _group.Priority = (int)_priorityNumericUpDown.Value;
                _group.Parents = _parentsTextBox.Text.Split(
                    "\r\n",
                    StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
                );

                var permissions = new Dictionary<string, bool?>();
                foreach (var item in _permissionListView.Items)
                {
                    if (item is ListViewItem listViewItem)
                    {
                        permissions[listViewItem.Text] = listViewItem.SubItems[2].Text switch
                        {
                            "True" => true,
                            "False" => false,
                            _ => null,
                        };
                    }
                }
                _group.Nodes = permissions;

                var list = new List<string>();
                foreach (var item in _memberListView.Items)
                {
                    if (item is ListViewItem listViewItem)
                    {
                        list.Add(listViewItem.Text);
                    }
                }
                _group.Users = [.. list];

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBoxHelper.ShowWarningMsgBox(ex.Message);
            }
        }
    }

    private void MemberContextMenuStrip_Opening(object sender, CancelEventArgs e)
    {
        _deleteMemberToolStripMenuItem.Enabled = _memberListView.SelectedItems.Count == 1;
    }

    private void AddMemberToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (_memberListView.SelectedItems.Count == 1)
        {
            _memberListView.Items.Insert(_memberListView.SelectedItems[0].Index, "0");
        }
        else
        {
            _memberListView.Items.Add("0");
        }
    }

    private void DeleteMemberToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (_memberListView.SelectedItems.Count == 1)
        {
            _memberListView.Items.RemoveAt(_memberListView.SelectedItems[0].Index);
        }
    }

    private void IdTextBox_Enter(object sender, EventArgs e)
    {
        _errorProvider.Clear();
    }

    private void IdTextBox_Validating(object sender, CancelEventArgs e)
    {
        if (string.IsNullOrEmpty(_idTextBox.Text))
        {
            _errorProvider.SetError(_idTextBox, "Id不能为空");
        }
        else if (!IdRegex().IsMatch(_idTextBox.Text))
        {
            _errorProvider.SetError(_idTextBox, "Id只能由字母、数字和下划线组成");
        }
        else if (_idTextBox.Text.Length <= 2)
        {
            _errorProvider.SetError(_idTextBox, "Id长度太短");
        }
    }

    private void ParentsTextBox_Enter(object sender, EventArgs e)
    {
        _errorProvider.Clear();
    }

    private void ParentsTextBox_Validating(object sender, CancelEventArgs e)
    {
        var nonExistent = _parentsTextBox
            .Text.Split(
                "\r\n",
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
            )
            .Where((id) => !_groupManager.Ids.Contains(id));

        if (nonExistent.Any())
        {
            _errorProvider.SetError(
                _parentsTextBox,
                $"以下权限组不存在：\r\n" + string.Join("\r\n", nonExistent)
            );
        }
    }
}
