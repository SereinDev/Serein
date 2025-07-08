using System;
using System.Windows.Forms;
using Serein.Core.Models.Bindings;
using Serein.Core.Services.Bindings;
using Serein.Lite.Models;
using Serein.Lite.Utils.Native;

namespace Serein.Lite.Ui.Members;

public partial class BindingPage : UserControl, IUpdateablePage
{
    private readonly BindingManager _bindingManager;

    public BindingPage(BindingManager bindingManager)
    {
        _bindingManager = bindingManager;
        InitializeComponent();

        _bindingListView.SetExploreTheme();
        LoadData();
    }

    private void LoadData()
    {
        _bindingListView.BeginUpdate();
        _bindingListView.Items.Clear();

        foreach (var record in _bindingManager.Records)
        {
            var item = new ListViewItem(record.UserId.ToString()) { Tag = record };
            item.SubItems.Add(record.ShownName);
            item.SubItems.Add(string.Join(";", record.GameIds));
            _bindingListView.Items.Add(item);
        }

        _bindingListView.EndUpdate();
    }

    void BindingListView_SelectedIndexChanged(object sender, EventArgs e)
    {
        _toolStripStatusLabel.Text =
            _bindingListView.SelectedItems.Count != 1
            || _bindingListView.SelectedItems[0].Tag is not BindingRecord record
                ? "-"
                : "更新时间：" + record.Time.ToString("G");
    }

    public void UpdatePage() => LoadData();

    private void RefreshToolStripMenuItem_Click(object sender, EventArgs e)
    {
        LoadData();
    }
}
