using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

using Serein.Core.Models.Commands;
using Serein.Core.Services.Data;
using Serein.Core.Utils.Extensions;
using Serein.Lite.Utils.Native;

namespace Serein.Lite.Ui.Function;

public partial class MatchPage : UserControl
{
    private static readonly Dictionary<MatchFieldType, string> MatchFieldTypeTexts =
        new()
        {
            [MatchFieldType.Disabled] = "禁用",
            [MatchFieldType.ServerOutput] = "服务器输出",
            [MatchFieldType.ServerInput] = "服务器输入",
            [MatchFieldType.GroupMsg] = "群聊消息",
            [MatchFieldType.PrivateMsg] = "私聊消息",
            [MatchFieldType.SelfMsg] = "自身消息",
        };

    private readonly MatchesProvider _matchesProvider;

    public MatchPage(MatchesProvider matchesProvider)
    {
        _matchesProvider = matchesProvider;

        InitializeComponent();
        LoadData();
        MatchListView.SetExploreTheme();
    }

    private void UpdateText()
    {
        ToolStripStatusLabel.Text =
            MatchListView.SelectedItems.Count > 0
                ? $"共{MatchListView.Items.Count}项；已选择{MatchListView.SelectedItems.Count}项"
                : $"共{MatchListView.Items.Count}项";
    }

    private void LoadData()
    {
        MatchListView.BeginUpdate();
        MatchListView.Items.Clear();

        lock (_matchesProvider.Value)
            foreach (var match in _matchesProvider.Value)
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
                item.SubItems.Add(match.Restrictions);

                MatchListView.Items.Add(item);
            }
        MatchListView.EndUpdate();
        UpdateText();
    }

    private void SaveData()
    {
        lock (_matchesProvider.Value)
        {
            _matchesProvider.Value.Clear();
            foreach (ListViewItem item in MatchListView.Items)
            {
                if (item.Tag is Match match)
                {
                    _matchesProvider.Value.Add(match);
                }
            }
        }

        _matchesProvider.SaveAsyncWithDebounce();
    }

    private void MatchListView_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateText();
    }

    private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
    {
        DeleteToolStripMenuItem.Enabled = MatchListView.SelectedItems.Count > 0;
        EditToolStripMenuItem.Enabled = MatchListView.SelectedItems.Count == 1;
        ClearToolStripMenuItem.Enabled = MatchListView.Items.Count > 0;
    }

    private void MatchListView_KeyDown(object sender, KeyEventArgs e)
    {
        if (!e.Control || MatchListView.Items.Count <= 1 || MatchListView.SelectedItems.Count != 1)
            return;

        ListViewItem item;
        var i = MatchListView.SelectedItems[0].Index;
        if (e.KeyCode == Keys.Up && i > 0)
        {
            MatchListView.BeginUpdate();
            item = MatchListView.Items[i - 1];
            MatchListView.Items.Remove(item);
            MatchListView.Items.Insert(i, item);
            MatchListView.EndUpdate();
            SaveData();
        }
        else if (e.KeyCode == Keys.Down && i >= 0 && i < MatchListView.Items.Count - 1)
        {
            MatchListView.BeginUpdate();
            item = MatchListView.Items[i + 1];
            MatchListView.Items.Remove(item);
            MatchListView.Items.Insert(i, item);
            MatchListView.EndUpdate();
            SaveData();
        }
    }

    private void LookUpIntroDocsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        "https://sereindev.github.io/".OpenInBrowser();
    }

    private void LookUpVariablesDocsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        "https://sereindev.github.io/docs/guidance/variables".OpenInBrowser();
    }

    private void ImportToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var dialog = new OpenFileDialog();
        dialog.ShowDialog();
    }

    private void AddToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var match = new Match();
        if (new MatchEditor(match).ShowDialog() == DialogResult.OK)
        {
            _matchesProvider.Value.Add(match);
            _matchesProvider.SaveAsyncWithDebounce();
            LoadData();
        }
    }

    private void EditToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (
            MatchListView.SelectedItems.Cast<ListViewItem>().FirstOrDefault()?.Tag is Match match
            && new MatchEditor(match).ShowDialog() == DialogResult.OK
        )
        {
            _matchesProvider.SaveAsyncWithDebounce();
            LoadData();
        }
    }

    private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (
            MessageBox.Show(
                "你确定要删除所选项吗？",
                "Serein.Lite",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            ) != DialogResult.Yes
        )
            return;

        foreach (var item in MatchListView.SelectedItems.Cast<ListViewItem>())
        {
            MatchListView.Items.Remove(item);
        }
        SaveData();
        UpdateText();
    }

    private void ClearToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (
            MessageBox.Show(
                "你确定要删除所有项吗？",
                "Serein.Lite",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            ) != DialogResult.Yes
        )
            return;

        _matchesProvider.Value.Clear();
        _matchesProvider.SaveAsyncWithDebounce();
        LoadData();
    }

    private void RefreshToolStripMenuItem_Click(object sender, EventArgs e)
    {
        LoadData();
    }
}
