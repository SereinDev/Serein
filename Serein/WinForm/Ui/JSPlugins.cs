using Serein.JSPlugin;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Serein.Ui
{
    public partial class Ui : Form
    {
        private delegate void SereinPluginsWebBrowser_Delegate(object[] objects);

        private void SereinPluginsWebBrowser_AppendText(object[] objects) => SereinPluginsWebBrowser.Document.InvokeScript("AppendText", objects);

        public void SereinPluginsWebBrowser_Invoke(string str)
        {
            object[] objects1 = { str };
            object[] objects2 = { objects1 };
            Invoke((SereinPluginsWebBrowser_Delegate)SereinPluginsWebBrowser_AppendText, objects2);
        }

        private void LoadSereinPlugin()
        {
            SereinPluginsList.BeginUpdate();
            SereinPluginsList.Items.Clear();
            foreach (Plugin Item in JSPluginManager.PluginDict.Values)
            {
                ListViewItem item = new ListViewItem(Item.Name)
                {
                    ForeColor = Item.Available ? ForeColor : Color.Gray,
                    Tag = Item.Namespace
                };
                item.SubItems.Add(Item.Version);
                item.SubItems.Add(Item.Author);
                item.SubItems.Add(Item.Description);
                SereinPluginsList.Items.Add(item);
            }
            SereinPluginsList.EndUpdate();
            SereinPluginsList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void SereinPluginsListContextMenuStrip_Reload_Click(object sender, EventArgs e)
        {
            JSPluginManager.Reload();
            LoadSereinPlugin();
        }

        private void SereinPluginsListContextMenuStrip_Disable_Click(object sender, EventArgs e)
        {
            if (SereinPluginsList.SelectedItems.Count > 0)
            {
                string Namespace = SereinPluginsList.SelectedItems[0].Tag as string ?? string.Empty;
                if (JSPluginManager.PluginDict.ContainsKey(Namespace))
                {
                    JSPluginManager.PluginDict[Namespace].Dispose();
                    LoadSereinPlugin();
                }
            }
        }

        private void SereinPluginsListContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
         => SereinPluginsListContextMenuStrip_Disable.Enabled =
            SereinPluginsList.SelectedItems.Count > 0 &&
            SereinPluginsList.SelectedItems[0].ForeColor != Color.Gray;

        private void SereinPluginsListContextMenuStrip_ClearConsole_Click(object sender, EventArgs e) => SereinPluginsWebBrowser_Invoke("#clear");
        private void SereinPluginsListContextMenuStrip_Docs_Click(object sender, EventArgs e) => Process.Start(new ProcessStartInfo("https://serein.cc/#/Function/JSDocs") { UseShellExecute = true });
    }
}
