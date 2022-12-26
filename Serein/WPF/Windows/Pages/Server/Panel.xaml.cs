using Serein.Base;
using Serein.Items;
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
            Restored = false;
            Catalog.Server.Panel = this;
        }

        private bool Restored = false;

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
                    {
                        ServerManager.CommandListIndex--;
                    }
                    if (ServerManager.CommandListIndex > 0)
                    {
                        ServerManager.CommandListIndex--;
                    }
                    InputBox.SelectionStart = InputBox.Text.Length;
                    break;
                case Key.Down:
                case Key.PageDown:
                    if (ServerManager.CommandListIndex < ServerManager.CommandHistory.Count)
                    {
                        ServerManager.CommandListIndex++;
                    }
                    if (ServerManager.CommandListIndex < ServerManager.CommandHistory.Count)
                    {
                        ServerManager.CommandListIndex++;
                    }
                    if (ServerManager.CommandListIndex < ServerManager.CommandHistory.Count)
                    {
                        ServerManager.CommandListIndex++;
                    }
                    InputBox.SelectionStart = InputBox.Text.Length;
                    break;
            }
        }

        public void AppendText(string Line)
            => Dispatcher.Invoke(() => PanelWebBrowser.Document.InvokeScript("AppendText", new object[] { Line }));

        private void UpdateInfos()
            => Dispatcher.Invoke(() =>
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
                CPUPerc.Content = ServerManager.Status ? "%" + ServerManager.CPUUsage.ToString("N2") : "-";
                Catalog.MainWindow.UpdateTitle(ServerManager.Status ? ServerManager.StartFileName : null);
            });

        private void UiPage_Loaded(object sender, RoutedEventArgs e)
        {
            Timer Restorer = new Timer(500) { AutoReset = true };
            Restorer.Elapsed += (_sender, _e) => Dispatcher.Invoke(() =>
            {
                Logger.Out(LogType.Debug, string.Join(";", Catalog.Server.Cache));
                if (!Restored && PanelWebBrowser.ReadyState == System.Windows.Forms.WebBrowserReadyState.Complete)
                {
                    Catalog.Server.Cache.ForEach((Text) => AppendText(Text));
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
