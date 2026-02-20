using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using Serein.Core.Services.Network.Connection;
using Serein.Core.Utils.Extensions;
using MessageBox = iNKORE.UI.WPF.Modern.Controls.MessageBox;
using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace Serein.Gui.Pages;

public partial class ConnectionPage : Page
{
    private readonly ConnectionManager _connectionManager;
    private readonly Timer _timer;

    public ConnectionPage(ConnectionManager connectionManager)
    {
        _timer = new(1000) { AutoReset = true, Enabled = true };
        _connectionManager = connectionManager;
        DataContext = _connectionManager;

        InitializeComponent();
        UpdateTimeText();

        Console.EnableLogLevelHighlight();
        _timer.Elapsed += (_, _) => Dispatcher.Invoke(UpdateTimeText);
        _connectionManager.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(_connectionManager.StartedAt))
            {
                Dispatcher.Invoke(UpdateTimeText);
            }
        };
    }

    private void UpdateTimeText()
    {
        TimeTextBlock.Text = _connectionManager.IsActive
            ? (DateTime.Now - _connectionManager.StartedAt).ToCommonString() ?? "-"
            : "-";
    }

    private void ControlButton_Click(object sender, RoutedEventArgs e)
    {
        var tag = (sender as Control)?.Tag as string;

        try
        {
            if (tag == "Close")
            {
                _connectionManager.Stop();
            }
            else if (tag == "Open")
            {
                _connectionManager.Start();
                Console.Clear();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "操作失败", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        _timer.Start();
        UpdateTimeText();
    }

    private void Page_Unloaded(object sender, RoutedEventArgs e)
    {
        _timer.Stop();
    }
}
