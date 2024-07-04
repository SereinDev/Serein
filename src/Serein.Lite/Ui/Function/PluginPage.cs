using System;
using System.Windows.Forms;

using Serein.Core.Models.Plugins;

namespace Serein.Lite.Ui.Function;

public partial class PluginPage : UserControl
{
    public PluginPage()
    {
        InitializeComponent();
    }

    private void PluginListView_SelectedIndexChanged(object sender, EventArgs e)
    {
        ToolStripStatusLabel.Text =
            PluginListView.SelectedItems.Count == 1
            && PluginListView.SelectedItems[0].Tag is IPlugin plugin
                ? $"{plugin.FileName}"
                : string.Empty;
    }
}
