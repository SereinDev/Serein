using Serein.Base;
using Serein.Items;
using Serein.Items.Motd;
using Serein.Server;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace Serein.Windows.Pages.Server
{
    public partial class Panel : UiPage
    {
        private readonly Timer UpdateInfoTimer = new Timer(2000) { AutoReset = true };

        public Panel()
        {
            InitializeComponent();
            ResourcesManager.InitConsole();
            PanelWebBrowser.ScriptErrorsSuppressed = true;
            PanelWebBrowser.IsWebBrowserContextMenuEnabled = false;
            PanelWebBrowser.WebBrowserShortcutsEnabled = false;
            PanelWebBrowser.Navigate(@"file:\\\" + Global.Path + $"console\\console.html?type=panel&theme={(Theme.GetAppTheme() == ThemeType.Light ? "light" : "dark")}");
            UpdateInfoTimer.Elapsed += (_, _) => UpdateInfos();
            UpdateInfoTimer.Start();
            Catalog.Server.Panel = this;
        }

        private bool Restored;

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
                    if (ServerManager.CommandHistoryIndex > 0)
                    {
                        ServerManager.CommandHistoryIndex--;
                    }
                    if (ServerManager.CommandHistoryIndex >= 0 && ServerManager.CommandHistoryIndex < ServerManager.CommandHistory.Count)
                    {
                        InputBox.Text = ServerManager.CommandHistory[ServerManager.CommandHistoryIndex];
                    }
                    InputBox.SelectionStart = InputBox.Text.Length;
                    break;
                case Key.Down:
                case Key.PageDown:
                    if (ServerManager.CommandHistoryIndex < ServerManager.CommandHistory.Count)
                    {
                        ServerManager.CommandHistoryIndex++;
                    }
                    if (ServerManager.CommandHistoryIndex >= 0 && ServerManager.CommandHistoryIndex < ServerManager.CommandHistory.Count)
                    {
                        InputBox.Text = ServerManager.CommandHistory[ServerManager.CommandHistoryIndex];
                    }
                    else if (ServerManager.CommandHistoryIndex == ServerManager.CommandHistory.Count && ServerManager.CommandHistory.Count != 0)
                    {
                        InputBox.Text = string.Empty;
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
                if (ServerManager.Status)
                {
                    Motd motd;
                    if (Global.Settings.Server.Type == 1)
                    {
                        motd = new Motdpe($"127.0.0.1:{Global.Settings.Server.Port}");
                    }
                    else
                    {
                        motd = new Motdje($"127.0.0.1:{Global.Settings.Server.Port}");
                    }
                    Version.Content = motd != null ? motd.Version : "-";
                    PlayCount.Content = motd != null ? $"{motd.OnlinePlayer}/{motd.MaxPlayer}" : "-";
                }
                else
                {
                    PlayCount.Content = "-";
                    Version.Content = "-";
                }
                Difficulity.Content = ServerManager.Status ? ServerManager.Difficulty : "-";
                Time.Content = ServerManager.Status ? ServerManager.GetTime() : "-";
                CPUPerc.Content = ServerManager.Status ? "%" + ServerManager.CPUUsage.ToString("N1") : "-";
                Catalog.MainWindow.UpdateTitle(ServerManager.Status ? ServerManager.StartFileName : null);
            });

        private void UiPage_Loaded(object sender, RoutedEventArgs e)
        {
            Timer restorer = new Timer(500) { AutoReset = true };
            restorer.Elapsed += (_, _) => Dispatcher.Invoke(() =>
            {
                Logger.Out(LogType.Debug, string.Join(";", Catalog.Server.Cache));
                if (!Restored && PanelWebBrowser.ReadyState == System.Windows.Forms.WebBrowserReadyState.Complete)
                {
                    Catalog.Server.Cache.ForEach((Text) => AppendText(Text));
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
    }
}
