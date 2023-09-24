using Serein.Core.JSPlugin;
using Serein.Utils;
using System.Diagnostics;
using System.Threading.Tasks;
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
            lock (Catalog.Function.PluginCache)
            {
                Catalog.Function.PluginCache.ForEach(
                    (line) => Dispatcher.Invoke(() => Append(LogPreProcessing.Color(line)))
                );
            }
            Catalog.Function.JSPlugin = this;
        }

        public void Append(Paragraph paragraph) =>
            Dispatcher.Invoke(() =>
            {
                PluginRichTextBox.Document = PluginRichTextBox.Document ?? new();
                PluginRichTextBox.Document.Blocks.Add(paragraph);
                while (PluginRichTextBox.Document.Blocks.Count > 250)
                {
                    PluginRichTextBox.Document.Blocks.Remove(
                        PluginRichTextBox.Document.Blocks.FirstBlock
                    );
                }
                PluginRichTextBox.ScrollToEnd();
            });

        public void LoadPublicly() => Dispatcher.Invoke(Load);

        private void Load()
        {
            JSPluginListView.Items.Clear();
            foreach (Plugin plugin in JSPluginManager.PluginDict.Values)
            {
                if (plugin is null)
                {
                    continue;
                }
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
                        Task.Run(JSPluginManager.Reload);
                        break;
                    case "ClearConsole":
                        PluginRichTextBox.Document.Blocks.Clear();
                        Catalog.Function.PluginCache.Clear();
                        break;
                    case "LookupDocs":
                        Process.Start(
                            new ProcessStartInfo("https://serein.cc/docs/development/intro")
                            {
                                UseShellExecute = true
                            }
                        );
                        break;
                    case "GotoMarket":
                        Process.Start(
                            new ProcessStartInfo("https://market.serein.cc/")
                            {
                                UseShellExecute = true
                            }
                        );
                        break;
                    case "Disable":
                        if (
                            JSPluginListView.SelectedItem is ListViewItem listViewItem
                            && JSPluginManager.PluginDict.TryGetValue(
                                listViewItem.Tag.ToString() ?? string.Empty,
                                out Plugin? plugin
                            )
                        )
                        {
                            plugin.Dispose();
                            Load();
                        }
                        break;
                }
            }
        }

        private void JSPluginListView_ContextMenuOpening(object sender, ContextMenuEventArgs e) =>
            Disable.IsEnabled =
                JSPluginListView.SelectedIndex != -1
                && JSPluginListView.SelectedItem is ListViewItem item
                && JSPluginManager.PluginDict.TryGetValue(item.Tag.ToString()!, out Plugin? plugin)
                && plugin.Available;
    }
}
