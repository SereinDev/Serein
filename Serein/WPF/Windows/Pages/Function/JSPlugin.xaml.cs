using Serein.Items;
using Serein.Plugin;
using System.Diagnostics;
using System.Timers;
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
            Restored = false;
            Catalog.Function.JSPlugin = this;
        }

        private bool Restored = false;

        public void AppendText(string Line)
            => Dispatcher.Invoke(() => PluginWebBrowser.Document.InvokeScript("AppendText", new[] { Line }));

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
                        Catalog.Function.PluginCache.Clear();
                        break;
                    case "LookupDocs":
                        Process.Start(new ProcessStartInfo("https://serein.cc/#/Function/JSDocs") { UseShellExecute = true });
                        break;
                }
            }
        }

        private void UiPage_Loaded(object sender, RoutedEventArgs e)
        {
            Timer Restorer = new Timer(500) { AutoReset = true };
            Restorer.Elapsed += (_sender, _e) => Dispatcher.Invoke(() =>
            {
                Logger.Out(LogType.Debug, "[JSPlugin:Restore]", string.Join(";", Catalog.Function.PluginCache));
                if (!Restored && PluginWebBrowser.ReadyState == System.Windows.Forms.WebBrowserReadyState.Complete)
                {
                    Catalog.Function.PluginCache.ForEach((Text) => AppendText(Text));
                    Restored = true;
                }
                if (Restored)
                {
                    Restorer.Stop();
                    Restorer.Dispose();
                }
            });
            Restorer.Start();
        }
    }
}
