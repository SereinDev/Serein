using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
            PluginWebBrowser.Navigate(@"file:\\\" + Directory.GetCurrentDirectory() + "\\console\\console.html?type=plugin");
            Window.Function.JSPlugin = this;
        }

        public void AppendText(string Line)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                PluginWebBrowser.Document.InvokeScript("AppendText", new[] { Line });
            }));
        }
    }
}
