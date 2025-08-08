using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Serein.Core.Models.Commands;
using Serein.Core.Services.Data;
using Serein.Core.Utils;
using Serein.Core.Utils.Extensions;
using Serein.Lite.Models;
using Serein.Lite.Utils;
using Serein.Lite.Utils.Native;

namespace Serein.Lite.Ui.Functions;

public partial class MatchPage : UserControl, IUpdateablePage
{
    private static readonly FrozenDictionary<MatchFieldType, string> MatchFieldTypeTexts =
        new Dictionary<MatchFieldType, string>
        {
            [MatchFieldType.Disabled] = "禁用",
            [MatchFieldType.ServerOutput] = "服务器输出",
            [MatchFieldType.ServerInput] = "服务器输入",
            [MatchFieldType.GroupMsg] = "群聊消息",
            [MatchFieldType.PrivateMsg] = "私聊消息",
            [MatchFieldType.SelfMsg] = "自身消息",
            [MatchFieldType.ChannelMsg] = "频道消息",
            [MatchFieldType.GuildMsg] = "群组消息",
        }.ToFrozenDictionary();

    private readonly MatchProvider _matchProvider;

    public MatchPage(MatchProvider matchProvider)
    {
        _matchProvider = matchProvider;

        InitializeComponent();
        LoadData();
        _matchListView.SetExploreTheme();
    }

    private void UpdateText()
    {
        _toolStripStatusLabel.Text =
            _matchListView.SelectedItems.Count > 0
                ? $"共{_matchListView.Items.Count}项；已选择{_matchListView.SelectedItems.Count}项"
                : $"共{_matchListView.Items.Count}项";
    }

    private void LoadData()
    {
        _matchListView.BeginUpdate();
        _matchListView.Items.Clear();

        lock (_matchProvider.Value)
        {
            foreach (var match in _matchProvider.Value)
            {
                var item = new ListViewItem(match.RegExp) { Tag = match };
                item.SubItems.Add(MatchFieldTypeTexts[match.FieldType]);
                item.SubItems.Add(match.Command);
                item.SubItems.Add(match.Description);
                item.SubItems.Add(
                    match.FieldType is MatchFieldType.GroupMsg or MatchFieldType.PrivateMsg
                        ? match.RequireAdmin
                            ? "是"
                            : "否"
                        : "-"
                );
                item.SubItems.Add(match.Exclusions);

                _matchListView.Items.Add(item);
            }
        }
        _matchListView.EndUpdate();
        UpdateText();
    }

    private void SaveData()
    {
        lock (_matchProvider.Value)
        {
            _matchProvider.Value.Clear();
            foreach (ListViewItem item in _matchListView.Items)
            {
                if (item.Tag is Match match)
                {
                    _matchProvider.Value.Add(match);
                }
            }
        }

        _matchProvider.SaveAsyncWithDebounce();
    }

    private void MatchListView_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateText();
    }

    private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
    {
        _deleteToolStripMenuItem.Enabled = _matchListView.SelectedItems.Count > 0;
        _editToolStripMenuItem.Enabled = _matchListView.SelectedItems.Count == 1;
        _clearToolStripMenuItem.Enabled = _matchListView.Items.Count > 0;
    }

    private void MatchListView_KeyDown(object sender, KeyEventArgs e)
    {
        if (
            !e.Control
            || _matchListView.Items.Count <= 1
            || _matchListView.SelectedItems.Count != 1
        )
        {
            return;
        }

        ListViewItem item;
        var i = _matchListView.SelectedItems[0].Index;
        if (e.KeyCode == Keys.Up && i > 0)
        {
            _matchListView.BeginUpdate();
            item = _matchListView.Items[i - 1];
            _matchListView.Items.Remove(item);
            _matchListView.Items.Insert(i, item);
            _matchListView.EndUpdate();
            SaveData();
        }
        else if (e.KeyCode == Keys.Down && i >= 0 && i < _matchListView.Items.Count - 1)
        {
            _matchListView.BeginUpdate();
            item = _matchListView.Items[i + 1];
            _matchListView.Items.Remove(item);
            _matchListView.Items.Insert(i, item);
            _matchListView.EndUpdate();
            SaveData();
        }
    }

    private void LookUpIntroDocsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        UrlConstants.DocsMatch.OpenInBrowser();
    }

    private void LookUpVariablesDocsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        UrlConstants.DocsVariables.OpenInBrowser();
    }

    private void AddToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var match = new Match();
        if (new MatchEditor(match).ShowDialog() == DialogResult.OK)
        {
            _matchProvider.Value.Add(match);
            _matchProvider.SaveAsyncWithDebounce();
            LoadData();
        }
    }

    private void EditToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (
            _matchListView.SelectedItems.Cast<ListViewItem>().FirstOrDefault()?.Tag is Match match
            && new MatchEditor(match).ShowDialog() == DialogResult.OK
        )
        {
            _matchProvider.SaveAsyncWithDebounce();
            LoadData();
        }
    }

    private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (!MessageBoxHelper.ShowDeleteConfirmation("确定要删除所选项吗？"))
        {
            return;
        }

        foreach (var item in _matchListView.SelectedItems.Cast<ListViewItem>())
        {
            _matchListView.Items.Remove(item);
        }
        SaveData();
        UpdateText();
    }

    private void ClearToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (!MessageBoxHelper.ShowDeleteConfirmation("确定要删除所有项吗？"))
        {
            return;
        }

        _matchProvider.Value.Clear();
        _matchProvider.SaveAsyncWithDebounce();
        LoadData();
    }

    private void RefreshToolStripMenuItem_Click(object sender, EventArgs e)
    {
        _matchProvider.Read();
        LoadData();
    }

    public void UpdatePage() => LoadData();
}
