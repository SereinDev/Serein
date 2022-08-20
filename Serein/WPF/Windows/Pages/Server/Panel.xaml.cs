using Serein.Server;
using System;
using System.Reflection;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wpf.Ui.Controls;

namespace Serein.Windows.Pages.Server
{
    public partial class Panel : UiPage
    {
        public Panel()
        {
            InitializeComponent();
            PanelWebBrowser.ScriptErrorsSuppressed = true;
            PanelWebBrowser.IsWebBrowserContextMenuEnabled = false;
            PanelWebBrowser.WebBrowserShortcutsEnabled = false;
            PanelWebBrowser.Navigate(@"file:\\\" + Directory.GetCurrentDirectory() + "\\console\\console.html?type=panel");
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            ServerManager.Start();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            ServerManager.Stop();
        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            ServerManager.RestartRequest();
        }

        private void Kill_Click(object sender, RoutedEventArgs e)
        {
            ServerManager.Kill();
        }

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            ServerManager.InputCommand(InputBox.Text);
            InputBox.Text = "";
        }

        private void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ServerManager.InputCommand(InputBox.Text);
                InputBox.Text = "";
                e.Handled = true;
                AppendText("11111");
            }
        }

        public void AppendText(string Line)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                PanelWebBrowser.Document.InvokeScript("AppendText", new object[] { Line });
            }));
        }
    }
}
