using Serein.Server;
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

        private void Start_Click(object sender, RoutedEventArgs e)
            => ServerManager.Start();
        private void Stop_Click(object sender, RoutedEventArgs e)
            => ServerManager.Stop();
        private void Restart_Click(object sender, RoutedEventArgs e)
            => ServerManager.RestartRequest();
        private void Kill_Click(object sender, RoutedEventArgs e)
            => ServerManager.Kill();

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            ServerManager.InputCommand(InputBox.Text);
            InputBox.Text = "";
        }

        private void InputBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    ServerManager.InputCommand(InputBox.Text);
                    InputBox.Text = "";
                    e.Handled = true;
                    break;
                case Key.Up:
                case Key.PageUp:
                    if (ServerManager.CommandListIndex > 0)
                        ServerManager.CommandListIndex--;
                    if (ServerManager.CommandListIndex >= 0 && ServerManager.CommandListIndex < ServerManager.CommandList.Count)
                        InputBox.Text = ServerManager.CommandList[ServerManager.CommandListIndex];
                    InputBox.SelectionStart = InputBox.Text.Length;
                    break; 
                case Key.Down:
                case Key.PageDown:
                    if (ServerManager.CommandListIndex < ServerManager.CommandList.Count)
                        ServerManager.CommandListIndex++;
                    if (ServerManager.CommandListIndex >= 0 && ServerManager.CommandListIndex < ServerManager.CommandList.Count)
                        InputBox.Text = ServerManager.CommandList[ServerManager.CommandListIndex];
                    else if (ServerManager.CommandListIndex == ServerManager.CommandList.Count && ServerManager.CommandList.Count != 0)
                        InputBox.Text = "";
                    InputBox.SelectionStart = InputBox.Text.Length;
                    break;
            }
        }

        public void AppendText(string Line)
        {
            Dispatcher.Invoke(() =>
            {
                PanelWebBrowser.Document.InvokeScript("AppendText", new object[] { Line });
                Catalog.Server.Cache.Add(Line);
            });
            if (Catalog.Server.Cache.Count > 25)
                Catalog.Server.Cache.RemoveRange(0, Catalog.Server.Cache.Count - 25);
        }

        private void UpdateInfos()
        {
            Dispatcher.Invoke(() =>
            {
                Status.Content = ServerManager.Status ? "已启动" : "未启动";
                Version.Content = ServerManager.Status ? ServerManager.Version : "-";
                Version.Content = ServerManager.Status ? ServerManager.Version : "-";
                if (ServerManager.Finished)
                {
                    Version.Content = ServerManager.Status ? ServerManager.Version : "-";
                    Difficulity.Content = ServerManager.Status ? ServerManager.Difficulty : "-";
                    Level.Content = ServerManager.Status ? ServerManager.LevelName : "-";
                }
                Time.Content = ServerManager.Status ? ServerManager.GetTime() : "-";
                CPUPerc.Content = ServerManager.Status ? "%" + ServerManager.CPUPersent.ToString("N2") : "-";
                if (ServerManager.Status)
                    Catalog.Server.Cache.Clear();
            });
        }

        private void PanelWebBrowser_DocumentCompleted(object sender, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e)
            => Catalog.Server.Cache.ForEach((Text) => PanelWebBrowser.Document.InvokeScript("AppendText", new object[] { Text }));
    }
}
