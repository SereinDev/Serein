using System;
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

public partial class SchedulePage : UserControl, IUpdateablePage
{
    private readonly ScheduleProvider _scheduleProvider;

    public SchedulePage(ScheduleProvider scheduleProvider)
    {
        _scheduleProvider = scheduleProvider;

        InitializeComponent();
        _scheduleListView.SetExploreTheme();
        LoadData();
    }

    private void SaveData()
    {
        lock (_scheduleProvider.Value)
        {
            _scheduleProvider.Value.Clear();
            foreach (ListViewItem item in _scheduleListView.Items)
            {
                if (item.Tag is Schedule schedule)
                {
                    _scheduleProvider.Value.Add(schedule);
                }
            }
        }

        _scheduleProvider.SaveAsyncWithDebounce();
    }

    private void UpdateText()
    {
        _toolStripStatusLabel.Text =
            _scheduleListView.SelectedItems.Count == 0 ? $"共{_scheduleListView.Items.Count}项"
            : _scheduleListView.SelectedItems.Count == 1
            && _scheduleListView.SelectedItems[0].Tag is Schedule schedule
            && schedule.Crontab is not null
                ? $"共{_scheduleListView.Items.Count}项；已选择1项；预计下一次执行时间：{schedule.Crontab.GetNextOccurrence(DateTime.Now):f}"
            : $"共{_scheduleListView.Items.Count}项；已选择{_scheduleListView.SelectedItems.Count}项";
    }

    private void LoadData()
    {
        _scheduleListView.BeginUpdate();
        _scheduleListView.Items.Clear();

        lock (_scheduleProvider.Value)
        {
            foreach (var schedule in _scheduleProvider.Value)
            {
                var item = new ListViewItem(schedule.Expression) { Tag = schedule };
                item.SubItems.Add(schedule.Command);
                item.SubItems.Add(schedule.Description);
                item.Checked = schedule.IsEnabled;

                _scheduleListView.Items.Add(item);
            }
        }
        _scheduleListView.EndUpdate();
        UpdateText();
    }

    private void ScheduleListView_KeyDown(object sender, KeyEventArgs e)
    {
        if (
            !e.Control
            || _scheduleListView.Items.Count <= 1
            || _scheduleListView.SelectedItems.Count != 1
        )
        {
            return;
        }

        ListViewItem item;
        var i = _scheduleListView.SelectedItems[0].Index;
        if (e.KeyCode == Keys.Up && i > 0)
        {
            _scheduleListView.BeginUpdate();

            item = _scheduleListView.Items[i - 1];
            _scheduleListView.Items.Remove(item);
            _scheduleListView.Items.Insert(i, item);
            _scheduleListView.EndUpdate();
            SaveData();
        }
        else if (e.KeyCode == Keys.Down && i >= 0 && i < _scheduleListView.Items.Count - 1)
        {
            _scheduleListView.BeginUpdate();

            item = _scheduleListView.Items[i + 1];
            _scheduleListView.Items.Remove(item);
            _scheduleListView.Items.Insert(i, item);
            _scheduleListView.EndUpdate();
            SaveData();
        }
    }

    private void ScheduleListView_ItemChecked(object sender, ItemCheckedEventArgs e)
    {
        if (e.Item.Tag is Schedule schedule)
        {
            schedule.IsEnabled = e.Item.Checked;
            _scheduleProvider.SaveAsyncWithDebounce();
        }
    }

    private void ScheduleListView_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateText();
    }

    private void AddToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var schedule = new Schedule();
        if (new ScheduleEditor(schedule).ShowDialog() == DialogResult.OK)
        {
            _scheduleProvider.Value.Add(schedule);
            _scheduleProvider.SaveAsyncWithDebounce();
            LoadData();
        }
    }

    private void EditToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (
            _scheduleListView.SelectedItems.Cast<ListViewItem>().FirstOrDefault()?.Tag
                is Schedule schedule
            && new ScheduleEditor(schedule).ShowDialog() == DialogResult.OK
        )
        {
            _scheduleProvider.SaveAsyncWithDebounce();
            LoadData();
        }
    }

    private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (!MessageBoxHelper.ShowDeleteConfirmation("确定要删除所选项吗？"))
        {
            return;
        }

        foreach (var item in _scheduleListView.SelectedItems.Cast<ListViewItem>())
        {
            _scheduleListView.Items.Remove(item);
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

        _scheduleProvider.Value.Clear();
        _scheduleProvider.SaveAsyncWithDebounce();
        LoadData();
    }

    private void RefreshToolStripMenuItem_Click(object sender, EventArgs e)
    {
        _scheduleProvider.Read();
        LoadData();
    }

    private void LookUpIntroDocsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        UrlConstants.DocsSchedule.OpenInBrowser();
    }

    private void LookUpVariablesDocsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        UrlConstants.DocsVariables.OpenInBrowser();
    }

    private void ListViewContextMenuStrip_Opening(object sender, CancelEventArgs e)
    {
        _deleteToolStripMenuItem.Enabled = _scheduleListView.SelectedItems.Count > 0;
        _editToolStripMenuItem.Enabled = _scheduleListView.SelectedItems.Count == 1;
        _clearToolStripMenuItem.Enabled = _scheduleListView.Items.Count > 0;
    }

    public void UpdatePage() => LoadData();
}
