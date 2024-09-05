using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;

using iNKORE.UI.WPF.Modern.Controls;

using Serein.Core.Services.Network.Connection;
using Serein.Plus.ViewModels;

using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace Serein.Plus.Pages;

public partial class ConnectionPage : Page
{
    private readonly Timer _timer;
    private ConnectionViewModel ViewModel { get; }
    private readonly WsConnectionManager _wsConnectionManager;

    public ConnectionPage(
        ConnectionViewModel connectionViewModel,
        WsConnectionManager wsConnectionManager
    )
    {
        ViewModel = connectionViewModel;
        _wsConnectionManager = wsConnectionManager;
        InitializeComponent();
        DataContext = ViewModel;

        _timer = new(2500) { Enabled = true };
        _timer.Elapsed += UpdateInfo;
        _wsConnectionManager.PropertyChanged += UpdateInfo;
    }

    private void ControlButton_Click(object sender, RoutedEventArgs e)
    {
        var tag = (sender as Control)?.Tag?.ToString();

        try
        {
            if (tag == "Close")
                _wsConnectionManager.Stop();
            else if (tag == "Open")
            {
                _wsConnectionManager.Start();
                Console.Clear();
            }
        }
        catch (Exception ex)
        {
            new ContentDialog
            {
                Content = ex.Message,
                PrimaryButtonText = "确定",
                Title = "操作失败",
            }.ShowAsync();
        }
    }

    private void UpdateInfo(object? sender, EventArgs e) { }
}
