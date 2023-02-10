using Serein.Core;
using Serein.Extensions;
using Serein.Utils;
using System;
using System.Timers;
using System.Windows;
using System.Windows.Documents;
using Wpf.Ui.Controls;

namespace Serein.Windows.Pages.Function
{
    public partial class Bot : UiPage
    {
        private readonly Timer UpdateInfoTimer = new Timer(2000) { AutoReset = true };

        public Bot()
        {
            InitializeComponent();
            UpdateInfoTimer.Elapsed += (_, _) => UpdateInfos();
            UpdateInfoTimer.Start();
            BotRichTextBox.Document.Blocks.Clear();
            Catalog.Function.BotCache.ForEach((line) => Dispatcher.Invoke(() => Append(LogPreProcessing.Color(line.LogType, line.Text))));
            Catalog.Function.Bot = this;
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
            => Websocket.Open();
        private void Disconnect_Click(object sender, RoutedEventArgs e)
            => Websocket.Close();

        public void Append(Paragraph paragraph)
            => Dispatcher.Invoke(() =>
            {
                BotRichTextBox.Document = BotRichTextBox.Document ?? new();
                BotRichTextBox.Document.Blocks.Add(paragraph);
                while (BotRichTextBox.Document.Blocks.Count > 250)
                {
                    BotRichTextBox.Document.Blocks.Remove(BotRichTextBox.Document.Blocks.FirstBlock);
                }
                BotRichTextBox.ScrollToEnd();
            });

        private void UpdateInfos()
            => Dispatcher.Invoke(() =>
            {
                Status.Content = Websocket.Status ? "已连接" : "未连接";
                ID.Content = Websocket.Status ? (Matcher.SelfId ?? "-") : "-";
                MessageReceived.Content = Websocket.Status ? (Matcher.MessageReceived ?? "-") : "-";
                MessageSent.Content = Websocket.Status ? (Matcher.MessageSent ?? "-") : "-";
                TimeSpan t = DateTime.Now - Websocket.StartTime;
                Time.Content = Websocket.Status ? t.ToCustomString() : "-";
            });
    }
}
