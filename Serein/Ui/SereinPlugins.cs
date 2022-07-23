using Serein.Plugin;
using System;
using System.Windows.Forms;
using Newtonsoft.Json;

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
            Global.Debug(JsonConvert.SerializeObject(Plugins.PluginItems));
            SereinPluginsList.EndUpdate();
        }
    }
}
