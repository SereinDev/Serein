using Serein.Utils;
using Serein.Core.JSPlugin;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Wpf.Ui.Controls;

namespace Serein.Windows.Pages.Function
{
    public partial class JSPlugin : UiPage
    {
        public JSPlugin()
        {
            InitializeComponent();
            Load();
            PluginRichTextBox.Document.Blocks.Clear();
            Catalog.Function.PluginCache.ForEach((line) => Dispatcher.Invoke(() => Append(LogPreProcessing.Color(line))));
            Catalog.Function.JSPlugin = this;
        }

        public void Append(Paragraph paragraph)
            => Dispatcher.Invoke(() =>
            {
                PluginRichTextBox.Document = PluginRichTextBox.Document ?? new();
                PluginRichTextBox.Document.Blocks.Add(paragraph);
                while (PluginRichTextBox.Document.Blocks.Count > 250)
                {
                    PluginRichTextBox.Document.Blocks.Remove(PluginRichTextBox.Document.Blocks.FirstBlock);
                }
                PluginRichTextBox.ScrollToEnd();
            });

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
                        PluginRichTextBox.Document.Blocks.Clear();
                        Catalog.Function.PluginCache.Clear();
                        break;
                    case "LookupDocs":
                        Process.Start(new ProcessStartInfo("https://serein.cc/#/Function/JSDocs/README") { UseShellExecute = true });
                        break;
                    case "GotoMarket":
                        Process.Start(new ProcessStartInfo("https://serein.cc/Extension/") { UseShellExecute = true });
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

        private void JSPluginListView_ContextMenuOpening(object sender, ContextMenuEventArgs e)
            => Disable.IsEnabled =
            JSPluginListView.SelectedIndex != -1 &&
            JSPluginListView.SelectedItem is ListViewItem item &&
            JSPluginManager.PluginDict.ContainsKey(item.Tag.ToString()) &&
            JSPluginManager.PluginDict[item.Tag.ToString()].Available;
    }
}
