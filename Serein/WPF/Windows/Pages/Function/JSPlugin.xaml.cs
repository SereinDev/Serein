using Serein.Plugin;
using System;
using System.Diagnostics;
using System.Windows;
using Wpf.Ui.Controls;

namespace Serein.Windows.Pages.Function
{
    public partial class JSPlugin : UiPage
    {
        public JSPlugin()
        {
            InitializeComponent();
            PluginWebBrowser.ScriptErrorsSuppressed = true;
            PluginWebBrowser.IsWebBrowserContextMenuEnabled = false;
            PluginWebBrowser.WebBrowserShortcutsEnabled = false;
            PluginWebBrowser.Navigate(@"file:\\\" + Global.Path + "console\\console.html?type=plugin");
            Load();
            Catalog.Function.JSPlugin = this;
        }

        public void AppendText(string Line)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                PluginWebBrowser.Document.InvokeScript("AppendText", new[] { Line });
            }));
        }

        private void Load()
        {
            JSPluginListView.Items.Clear();
            foreach (PluginItem Item in Plugins.PluginItems)
            {
                JSPluginListView.Items.Add(Item);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem Item && Item != null)
            {
                switch (Item.Tag)
                {
                    case "Reload":
                        Plugins.Reload();
                        Load();
                        break;
                    case "ClearConsole":
                        AppendText("#clear");
                        break;
                    case "LookupDocs":
                        Process.Start(new ProcessStartInfo("https://serein.cc/Javascript.html") { UseShellExecute = true });
                        break;
                }
            }
        }
    }
}
