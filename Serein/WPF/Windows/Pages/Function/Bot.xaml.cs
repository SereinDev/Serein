using Serein.Base;
using Serein.Items;
using System;
using System.Timers;
using System.Windows;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace Serein.Windows.Pages.Function
{
    public partial class Bot : UiPage
    {
        public Bot()
        {
            InitializeComponent();
            ResourcesManager.InitConsole();
            BotWebBrowser.ScriptErrorsSuppressed = true;
            BotWebBrowser.IsWebBrowserContextMenuEnabled = false;
            BotWebBrowser.WebBrowserShortcutsEnabled = false;
            BotWebBrowser.Navigate(@"file:\\\" + Global.Path + $"console\\console.html?type=bot&theme={(Theme.GetAppTheme() == ThemeType.Light ? "light" : "dark")}");
            Timer UpdateInfoTimer = new Timer(2000) { AutoReset = true };
            UpdateInfoTimer.Elapsed += (sender, e) => UpdateInfos();
            UpdateInfoTimer.Start();
            Catalog.Function.Bot = this;
        }

        private bool Restored;

        private void Connect_Click(object sender, RoutedEventArgs e)
            => Websocket.Connect();
        private void Disconnect_Click(object sender, RoutedEventArgs e)
            => Websocket.Close();
        public void AppendText(string Line)
            => Dispatcher.Invoke(() => BotWebBrowser.Document.InvokeScript("AppendText", new[] { Line }));

        private void UpdateInfos()
            => Dispatcher.Invoke(() =>
            {
                Status.Content = Websocket.Status ? "已连接" : "未连接";
                ID.Content = Websocket.Status ? (Matcher.SelfId ?? "-") : "-";
                MessageReceived.Content = Websocket.Status ? (Matcher.MessageReceived ?? "-") : "-";
                MessageSent.Content = Websocket.Status ? (Matcher.MessageSent ?? "-") : "-";
                TimeSpan t = DateTime.Now - Websocket.StartTime;
                Time.Content = Websocket.Status ? t.TotalSeconds < 3600 ? $"{t.TotalSeconds / 60:N1}m" : t.TotalHours < 120 ? $"{t.TotalMinutes / 60:N1}h" : $"{t.TotalHours / 24:N1}d" : "-";
            });

        private void UiPage_Loaded(object sender, RoutedEventArgs e)
        {
            Timer Restorer = new Timer(500) { AutoReset = true };
            Restorer.Elapsed += (_sender, _e) => Dispatcher.Invoke(() =>
            {
                Logger.Out(LogType.Debug, string.Join(";", Catalog.Function.BotCache));
                if (!Restored && BotWebBrowser.ReadyState == System.Windows.Forms.WebBrowserReadyState.Complete)
                {
                    Catalog.Function.BotCache.ForEach((Text) => AppendText(Text));
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
