using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;

using iNKORE.UI.WPF.Modern.Controls;

using Serein.Core.Services.Network.Connection;
using Serein.Core.Utils.Extensions;
using Serein.Plus.Services;

using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace Serein.Plus.Pages;

public partial class ConnectionPage : Page
{
    private readonly InfoBarProvider _infoBarProvider;
    private readonly WsConnectionManager _wsConnectionManager;
    private readonly Timer _timer;

    public ConnectionPage(
        InfoBarProvider infoBarProvider,
        WsConnectionManager wsConnectionManager
    )
    {
        _timer = new(1000) { AutoReset = true, Enabled = true };
        _infoBarProvider = infoBarProvider;
        _wsConnectionManager = wsConnectionManager;
        DataContext = _wsConnectionManager;

        InitializeComponent();
        UpdateTimeText();

        Console.EnableLogLevelHighlight();
        _timer.Elapsed += (_, _) => Dispatcher.Invoke(UpdateTimeText);
        _wsConnectionManager.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(_wsConnectionManager.ConnectedAt))
                Dispatcher.Invoke(UpdateTimeText);
        };
    }

    private void UpdateTimeText()
    {
        TimeTextBlock.Text = (DateTime.Now - _wsConnectionManager.ConnectedAt).ToCommonString() ?? "-";
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
