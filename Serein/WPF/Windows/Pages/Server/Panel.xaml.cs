using Serein.Utils;
using Serein.Core.Server;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Documents;
using Wpf.Ui.Controls;

namespace Serein.Windows.Pages.Server
{
    public partial class Panel : UiPage
    {
        private readonly Timer UpdateInfoTimer = new Timer(2000) { AutoReset = true };

        public Panel()
        {
            InitializeComponent();
            UpdateInfoTimer.Elapsed += (_, _) => UpdateInfos();
            UpdateInfoTimer.Start();
            PanelRichTextBox.Document.Blocks.Clear();
            Catalog.Server.Cache.ForEach((line) => Dispatcher.Invoke(() => Append(LogPreProcessing.Color(line.LogType, line.Text))));
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
                    if (ServerManager.CommandHistoryIndex > 0)
                    {
                        ServerManager.CommandHistoryIndex--;
                    }
                    if (ServerManager.CommandHistoryIndex >= 0 && ServerManager.CommandHistoryIndex < ServerManager.CommandHistory.Count)
                    {
                        InputBox.Text = ServerManager.CommandHistory[ServerManager.CommandHistoryIndex];
                    }
                    InputBox.SelectionStart = InputBox.Text.Length;
                    e.Handled = true;
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
                    e.Handled = true;
                    break;
            }
        }

        public void Append(Paragraph paragraph)
            => Dispatcher.Invoke(() =>
            {
                PanelRichTextBox.Document = PanelRichTextBox.Document ?? new();
                PanelRichTextBox.Document.Blocks.Add(paragraph);
                while (PanelRichTextBox.Document.Blocks.Count > 250)
                {
                    PanelRichTextBox.Document.Blocks.Remove(PanelRichTextBox.Document.Blocks.FirstBlock);
                }
                PanelRichTextBox.ScrollToEnd();
            });

        private void UpdateInfos()
            => Dispatcher.Invoke(() =>
            {
                Status.Content = ServerManager.Status ? "已启动" : "未启动";
                if (ServerManager.Status)
                {
                    Version.Content = ServerManager.Motd != null && !string.IsNullOrEmpty(ServerManager.Motd.Version) ? ServerManager.Motd.Version : "-";
                    PlayCount.Content = ServerManager.Motd != null ? $"{ServerManager.Motd.OnlinePlayer}/{ServerManager.Motd.MaxPlayer}" : "-";
                }
                else
                {
                    PlayCount.Content = "-";
                    Version.Content = "-";
                }
                Difficulity.Content = ServerManager.Status && !string.IsNullOrEmpty(ServerManager.Difficulty) ? ServerManager.Difficulty : "-";
                Time.Content = ServerManager.Status ? ServerManager.Time : "-";
                CPUPerc.Content = ServerManager.Status ? "%" + ServerManager.CPUUsage.ToString("N1") : "-";
                Catalog.MainWindow.UpdateTitle(ServerManager.Status ? ServerManager.StartFileName : null);
            });
    }
}
