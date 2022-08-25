using Serein.Server;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;

namespace Serein.Windows
{
    public partial class MainWindow : UiWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Window.MainWindow = this;
        }

        private void UiWindow_StateChanged(object sender, EventArgs e)
        {
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            PopupEx.VerticalOffset = Height * 0.5 - 60;
        }

        private void UiWindow_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = ServerManager.Status;
        }


        private void Hide_Click(object sender, RoutedEventArgs e)
        {
            ShowInTaskbar = false;
            Hide();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            if (!ServerManager.Status)
            {
                Close();
            }
        }

        /// <summary>
        /// 显示底部通知栏
        /// </summary>
        /// <param name="Title">标题</param>
        /// <param name="Message">信息</param>
        /// <param name="Icon">图标</param>
        public void OpenSnackbar(string Title, string Message, SymbolRegular Icon)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                PopupEx.VerticalOffset = Height * 0.5 - 60; // 自动调整底部距离，使之刚好与底部重合
                Snackbar.Show(Title, Message, Icon);
            }));
        }

        private void UiWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Snackbar.IsShown)
            {
                PopupEx.VerticalOffset = Height * 0.5 - 60;
            }
        }

        private void Help_Click(object sender, RoutedEventArgs e) => Process.Start(new ProcessStartInfo("https://serein.cc/") { UseShellExecute = true });
        private void UiWindow_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) => ShowInTaskbar = IsVisible;
        private void MemberEditor_ButtonRightClick(object sender, RoutedEventArgs e) => MemberEditor.Hide();
        private void MemberEditor_ButtonLeftClick(object sender, RoutedEventArgs e) => Window.Function.Member.Confirm();
    }
}
