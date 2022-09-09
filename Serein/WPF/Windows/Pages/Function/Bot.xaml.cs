using Serein.Base;
using System;
using System.Timers;
using System.Windows;
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
            BotWebBrowser.Navigate(@"file:\\\" + Global.Path + "console\\console.html?type=bot");
            Timer UpdateInfoTimer = new Timer(2000) { AutoReset = true };
            UpdateInfoTimer.Elapsed += (sender, e) => UpdateInfos();
            UpdateInfoTimer.Start();
            Window.Function.Bot = this;
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
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

        private void UpdateInfos()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                Status.Content = Websocket.Status ? "已连接" : "未连接";
                ID.Content = Message.SelfId ?? "-";
                MessageReceived.Content = Message.MessageReceived ?? "-";
                MessageSent.Content = Message.MessageSent ?? "-";
                TimeSpan t = DateTime.Now - Websocket.StartTime;
                Time.Content = Websocket.Status ? t.TotalSeconds < 3600 ? $"{t.TotalSeconds / 60:N1}m" : t.TotalHours < 120 ? $"{t.TotalMinutes / 60:N1}h" : $"{t.TotalHours / 24:N2}d" : "-";
            }
            ));
        }
    }
}
