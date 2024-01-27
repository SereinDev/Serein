using System;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core.Services.Networks;

using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace Serein.Plus.Ui.Pages.Function;

public partial class NetworkPage : Page
{
    private readonly IHost _host;
    private IServiceProvider Services => _host.Services;
    private WsNetwork WsNetwork => Services.GetRequiredService<WsNetwork>();

    public NetworkPage(IHost host)
    {
        _host = host;
        InitializeComponent();
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
            Console.AppendErrorLine(ex.Message);
        }
    }
}
