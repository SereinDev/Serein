using Serein.Core.JSPlugin;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Serein.Ui
{
    public partial class Ui : Form
    {
        private delegate void JSPluginWebBrowser_Delegate(object[] objects);

        private void JSPluginWebBrowser_AppendText(object[] objects) => JSPluginWebBrowser.Document.InvokeScript("AppendText", objects);

        public void JSPluginWebBrowser_Invoke(string str)
        {
            object[] objects1 = { str };
            object[] objects2 = { objects1 };
            Invoke((JSPluginWebBrowser_Delegate)JSPluginWebBrowser_AppendText, objects2);
        }

        public void LoadJSPluginPublicly()
        {
            Invoke(LoadJSPlugin);
        }

        private void LoadJSPlugin()
        {
            JSPluginList.BeginUpdate();
            JSPluginList.Items.Clear();
            foreach (Plugin plugin in JSPluginManager.PluginDict.Values)
            {
                if (plugin is null)
                {
                    continue;
                }
                ListViewItem listViewItem = new(plugin.DisplayedName)
                {
                    ForeColor = plugin.Available ? ForeColor : Color.Gray,
                    Tag = plugin.Namespace
                };
                listViewItem.SubItems.Add(plugin.DisplayedVersion);
                listViewItem.SubItems.Add(plugin.DisplayedAuthor);
                listViewItem.SubItems.Add(plugin.DisplayedDescription);
                JSPluginList.Items.Add(listViewItem);
            }
            JSPluginList.EndUpdate();
            JSPluginList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void JSPluginListContextMenuStrip_Reload_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                JSPluginManager.Reload();
                Invoke(LoadJSPlugin);
            });
        }

        private void JSPluginListContextMenuStrip_Disable_Click(object sender, EventArgs e)
        {
            if (JSPluginList.SelectedItems.Count > 0)
            {
                string @namespace = JSPluginList.SelectedItems[0].Tag as string ?? string.Empty;
                if (JSPluginManager.PluginDict.ContainsKey(@namespace))
                {
                    JSPluginManager.PluginDict[@namespace].Dispose();
                    LoadJSPlugin();
                }
            }
        }

        private void JSPluginListContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
         => JSPluginListContextMenuStrip_Disable.Enabled =
            JSPluginList.SelectedItems.Count > 0 &&
            JSPluginList.SelectedItems[0].ForeColor != Color.Gray;

        private void JSPluginListContextMenuStrip_ClearConsole_Click(object sender, EventArgs e) => JSPluginWebBrowser_Invoke("#clear");
        private void JSPluginListContextMenuStrip_Docs_Click(object sender, EventArgs e) => Process.Start(new ProcessStartInfo("https://serein.cc/docs/development/intro") { UseShellExecute = true });
    }
}
