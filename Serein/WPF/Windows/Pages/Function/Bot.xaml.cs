using Serein.Base;
using Serein.Core;
using Serein.Utils;
using System;
using System.Timers;
using System.Windows;
using Serein.Extensions;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace Serein.Windows.Pages.Function
{
    public partial class Bot : UiPage
    {
        private readonly Timer UpdateInfoTimer = new Timer(2000) { AutoReset = true };

        public Bot()
        {
            InitializeComponent();
            ResourcesManager.InitConsole();
            BotWebBrowser.ScriptErrorsSuppressed = true;
            BotWebBrowser.IsWebBrowserContextMenuEnabled = false;
            BotWebBrowser.WebBrowserShortcutsEnabled = false;
            BotWebBrowser.Navigate(@"file:\\\" + Global.PATH + $"console\\console.html?type=bot&theme={(Theme.GetAppTheme() == ThemeType.Light ? "light" : "dark")}");
            UpdateInfoTimer.Elapsed += (_, _) => UpdateInfos();
            UpdateInfoTimer.Start();
            Catalog.Function.Bot = this;
        }

        private bool Restored;

        private void Connect_Click(object sender, RoutedEventArgs e)
            => Websocket.Open();
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
                Time.Content = Websocket.Status ? t.ToCustomString() : "-";
            });

        private void UiPage_Loaded(object sender, RoutedEventArgs e)
        {
            Timer restorer = new Timer(500) { AutoReset = true };
            restorer.Elapsed += (_, _) => Dispatcher.Invoke(() =>
            {
                Logger.Output(LogType.Debug, string.Join(";", Catalog.Function.BotCache));
                if (!Restored && BotWebBrowser.ReadyState == System.Windows.Forms.WebBrowserReadyState.Complete)
                {
                    Catalog.Function.BotCache.ForEach((Text) => AppendText(Text));
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
