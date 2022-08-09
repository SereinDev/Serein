using Serein.Plugin;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Serein.Ui
{
    public partial class Ui : Form
    {
        private delegate void SereinPluginsWebBrowser_Delegate(object[] objects);

        private void SereinPluginsWebBrowser_AppendText(object[] objects)
        {
            SereinPluginsWebBrowser.Document.InvokeScript("AppendText", objects);
        }
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
            foreach (PluginItem Item in Plugins.PluginItems)
            {
                ListViewItem item = new ListViewItem(Item.Name);
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
            Plugins.Reload();
            LoadSereinPlugin();
        }
        private void SereinPluginsListContextMenuStrip_ClearConsole_Click(object sender, EventArgs e)
        {
            SereinPluginsWebBrowser_Invoke("#clear");
        }
        private void SereinPluginsListContextMenuStrip_Docs_Click(object sender, System.EventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://serein.cc/Serein/Javascript.html") { UseShellExecute = true });
        }
    }
}
