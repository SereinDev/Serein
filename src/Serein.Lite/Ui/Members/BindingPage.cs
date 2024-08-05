using System;
using System.Windows.Forms;

using Serein.Core.Models.Bindings;
using Serein.Core.Services.Bindings;
using Serein.Lite.Utils.Native;

namespace Serein.Lite.Ui.Members;

public partial class BindingPage : UserControl, IUpdateablePage
{
    private readonly BindingManager _bindingManager;

    public BindingPage(BindingManager bindingManager)
    {
        _bindingManager = bindingManager;
        InitializeComponent();

        LoadData();
        BindingListView.SetExploreTheme();
    }

    private void LoadData()
    {
        BindingListView.BeginUpdate();
        BindingListView.Items.Clear();

        _bindingManager.Records.ForEach((record) =>
        {
            var item = new ListViewItem(record.UserId.ToString())
            {
                Tag = record
            };
            item.SubItems.Add(record.ShownName);
            item.SubItems.Add(string.Join(";", record.GameIds));
        });

        BindingListView.EndUpdate();
    }

    void BindingListView_SelectedIndexChanged(object sender, EventArgs e)
    {
        ToolStripStatusLabel.Text =
            BindingListView.SelectedItems.Count != 1
            || BindingListView.SelectedItems[0].Tag is not BindingRecord record
                ? "-"
                : "更新时间：" + record.Time.ToString("G");
    }

    public void UpdatePage() => LoadData();
}
