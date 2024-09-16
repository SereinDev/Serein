using System;
using System.Windows;
using System.Windows.Controls;

using iNKORE.UI.WPF.Modern.Controls;

using Serein.Core.Services.Network.Connection;
using Serein.Plus.Services;
using Serein.Plus.ViewModels;

using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace Serein.Plus.Pages;

public partial class ConnectionPage : Page
{
    private ConnectionViewModel ViewModel { get; }

    private readonly InfoBarProvider _infoBarProvider;
    private readonly WsConnectionManager _wsConnectionManager;

    public ConnectionPage(
        InfoBarProvider infoBarProvider,
        ConnectionViewModel connectionViewModel,
        WsConnectionManager wsConnectionManager
    )
    {
        _infoBarProvider = infoBarProvider;
        ViewModel = connectionViewModel;
        _wsConnectionManager = wsConnectionManager;
        InitializeComponent();
        DataContext = ViewModel;

        Console.EnableLogLevelHighlight();
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
            _infoBarProvider.Enqueue("操作失败", ex.Message, InfoBarSeverity.Error);
        }
    }
}
