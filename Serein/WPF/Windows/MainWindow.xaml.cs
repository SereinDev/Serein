using Serein.Server;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using Wpf.Ui.Controls;

namespace Serein.Windows
{
    public partial class MainWindow : UiWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void UiWindow_StateChanged(object sender, EventArgs e)
        {
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
        }

        private void UiWindow_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = ServerManager.Status;
        }

        private void UiWindow_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ShowInTaskbar = IsVisible;
        }

        private void Hide_Click(object sender, RoutedEventArgs e)
        {
            ShowInTaskbar = false;
            Hide();
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://serein.cc/") { UseShellExecute = true });
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            if (!ServerManager.Status)
            {
                Close();
            }
        }
    }
}
