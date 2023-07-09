using Serein.Core.Generic;
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
        private readonly Timer _updateInfoTimer = new Timer(2000) { AutoReset = true };

        public Bot()
        {
            InitializeComponent();
            _updateInfoTimer.Elapsed += (_, _) => UpdateInfos();
            _updateInfoTimer.Start();
            BotRichTextBox.Document.Blocks.Clear();
            lock (Catalog.Function.BotCache)
            {
                Catalog.Function.BotCache.ForEach((line) => Dispatcher.Invoke(() => Append(LogPreProcessing.Color(line))));
            }
            Catalog.Function.Bot = this;
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            Websocket.Open();
            UpdateInfos();
        }

        private void Disconnect_Click(object sender, RoutedEventArgs e)
        {
            Websocket.Close();
            UpdateInfos();
        }

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
                ID.Content = PacketHandler.SelfId;
                MessageReceived.Content = PacketHandler.MessageReceived;
                MessageSent.Content = PacketHandler.MessageSent;
                Time.Content = Websocket.Status ? (DateTime.Now - Websocket.StartTime).ToCustomString() : "-";
            });
    }
}
