using Serein.Base;
using System.Collections.Generic;
using System.IO;
using System;
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
    public partial class Bot : UiPage
    {
        public Bot()
        {
            InitializeComponent();
            BotWebBrowser.ScriptErrorsSuppressed = true;
            BotWebBrowser.IsWebBrowserContextMenuEnabled = false;
            BotWebBrowser.WebBrowserShortcutsEnabled = false;
            BotWebBrowser.Navigate(@"file:\\\" + Directory.GetCurrentDirectory() + "\\console\\console.html?type=bot");
        }

        private void Connect_Click(object sender ,RoutedEventArgs e)
        {
            Websocket.Connect();
        }

        private void Disconnect_Click(object sender, RoutedEventArgs e)
        {
            Websocket.Close();
        }

        public void AppendText(string Line)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                BotWebBrowser.Document.InvokeScript("AppendText", new[] { Line });
            }));
        }
    }
}
