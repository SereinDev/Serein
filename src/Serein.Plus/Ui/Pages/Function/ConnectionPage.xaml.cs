using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;

using iNKORE.UI.WPF.Modern.Controls;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core.Services.Network.Connection;

using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace Serein.Plus.Ui.Pages.Function;

public partial class ConnectionPage : Page
{
    private readonly IHost _host;
    private IServiceProvider Services => _host.Services;
    private WsConnectionManager WsNetwork => Services.GetRequiredService<WsConnectionManager>();
    private readonly Timer _timer;

    public ConnectionPage(IHost host)
    {
        _host = host;
        InitializeComponent();

        _timer = new(2500) { Enabled = true };
        _timer.Elapsed += UpdateInfo;
        WsNetwork.PropertyChanged += UpdateInfo;
    }

    private void ControlButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var tag = (sender as Control)?.Tag?.ToString();

            if (tag == "Close")
                WsNetwork.Stop();
            else if (tag == "Open")
            {
                WsNetwork.Start();
                Console.Clear();
            }
        }
        catch (Exception ex)
        {
            new ContentDialog
            {
                Content = ex.Message,
                PrimaryButtonText = "确定",
                Title = "操作失败"
            }.ShowAsync();
        }
    }

    private const string EmptyHolder = "-";

    private void UpdateInfo(object? sender, EventArgs e)
    {
        Dispatcher.Invoke(() =>
        {
            if (!WsNetwork.Active)
            {
                Status.Text = "未连接";
                Account.Text = Time.Text = EmptyHolder;
            }
            else
            {
                Status.Text = "已连接";
            }
        });
    }
}
