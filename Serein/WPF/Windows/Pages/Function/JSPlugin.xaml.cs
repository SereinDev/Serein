using Serein.Base;
using Serein.Items;
using Serein.JSPlugin;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace Serein.Windows.Pages.Function
{
    public partial class JSPlugin : UiPage
    {
        public JSPlugin()
        {
            InitializeComponent();
            ResourcesManager.InitConsole();
            PluginWebBrowser.ScriptErrorsSuppressed = true;
            PluginWebBrowser.IsWebBrowserContextMenuEnabled = false;
            PluginWebBrowser.WebBrowserShortcutsEnabled = false;
            PluginWebBrowser.Navigate(@"file:\\\" + Global.Path + $"console\\console.html?type=plugin&theme={(Theme.GetAppTheme() == ThemeType.Light ? "light" : "dark")}");
            Load();
            Catalog.Function.JSPlugin = this;
        }

        private bool Restored;

        public void AppendText(string line)
            => Dispatcher.Invoke(() => PluginWebBrowser.Document.InvokeScript("AppendText", new[] { line }));

        public void LoadPublicly()
            => Dispatcher.Invoke(Load);

        private void Load()
        {
            JSPluginListView.Items.Clear();
            foreach (Plugin plugin in JSPluginManager.PluginDict.Values)
            {
                ListViewItem listViewItem = new ListViewItem
                {
                    Content = plugin,
                    Tag = plugin.Namespace,
                    Opacity = plugin.Available ? 1 : 0.5
                };
                JSPluginListView.Items.Add(listViewItem);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Wpf.Ui.Controls.MenuItem menuItem && menuItem != null)
            {
                switch (menuItem.Tag)
                {
                    case "Reload":
                        JSPluginManager.Reload();
                        Load();
                        break;
                    case "ClearConsole":
                        AppendText("#clear");
                        Catalog.Function.PluginCache.Clear();
                        break;
                    case "LookupDocs":
                        Process.Start(new ProcessStartInfo("https://serein.cc/#/Function/JSDocs/README") { UseShellExecute = true });
                        break;
                    case "Disable":
                        if (JSPluginListView.SelectedItem is ListViewItem listViewItem && JSPluginManager.PluginDict.ContainsKey(listViewItem.Tag.ToString()))
                        {
                            JSPluginManager.PluginDict[listViewItem.Tag.ToString()].Dispose();
                            Load();
                        }
                        break;
                }
            }
        }

        private void UiPage_Loaded(object sender, RoutedEventArgs e)
        {
            Timer restorer = new Timer(500) { AutoReset = true };
            restorer.Elapsed += (_, _) => Dispatcher.Invoke(() =>
            {
                Logger.Output(LogType.Debug, string.Join(";", Catalog.Function.PluginCache));
                if (!Restored && PluginWebBrowser.ReadyState == System.Windows.Forms.WebBrowserReadyState.Complete)
                {
                    Catalog.Function.PluginCache.ForEach((Text) => AppendText(Text));
                    Restored = true;
                }
                if (Restored)
                {
                    restorer.Stop();
                    restorer.Dispose();
                }
            });
            restorer.Start();
        }

        private void JSPluginListView_ContextMenuOpening(object sender, ContextMenuEventArgs e)
            => Disable.IsEnabled =
            JSPluginListView.SelectedIndex != -1 &&
            JSPluginListView.SelectedItem is ListViewItem item &&
            JSPluginManager.PluginDict.ContainsKey(item.Tag.ToString()) &&
            JSPluginManager.PluginDict[item.Tag.ToString()].Available;
    }
}
