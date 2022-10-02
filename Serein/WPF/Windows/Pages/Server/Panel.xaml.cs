using Serein.Server;
using System;
using System.Timers;
using System.Windows;
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
            PanelWebBrowser.Navigate(@"file:\\\" + Global.Path + "console\\console.html?type=panel");
            Timer UpdateInfoTimer = new Timer(2000) { AutoReset = true };
            UpdateInfoTimer.Elapsed += (sender, e) => UpdateInfos();
            UpdateInfoTimer.Start();
            Catalog.Server.Panel = this;
        }

        private void Start_Click(object sender, RoutedEventArgs e) => ServerManager.Start();

        private void Stop_Click(object sender, RoutedEventArgs e) => ServerManager.Stop();

        private void Restart_Click(object sender, RoutedEventArgs e) => ServerManager.RestartRequest();

        private void Kill_Click(object sender, RoutedEventArgs e) => ServerManager.Kill();

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
            }
        }

        public void AppendText(string Line)
        {
            Dispatcher.Invoke(() =>
            {
                PanelWebBrowser.Document.InvokeScript("AppendText", new object[] { Line });
            });
        }

        private void UpdateInfos()
        {
            Dispatcher.Invoke(() =>
            {
                Status.Content = ServerManager.Status ? "已启动" : "未启动";
                Version.Content = ServerManager.Status ? ServerManager.Version : "-";
                Difficulity.Content = ServerManager.Status ? ServerManager.Difficulty : "-";
                Level.Content = ServerManager.Status ? ServerManager.LevelName : "-";
                Time.Content = ServerManager.Status ? ServerManager.GetTime() : "-";
                CPUPerc.Content = ServerManager.Status ? ServerManager.CPUPersent.ToString("N2") : "-";
            });
        }
    }
}
