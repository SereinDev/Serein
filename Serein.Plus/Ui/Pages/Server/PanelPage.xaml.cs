using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core.Models.Server;
using Serein.Core.Services.Server;
using Serein.Plus.Utils.Extensions;

using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace Serein.Plus.Ui.Pages.Server;

public partial class PanelPage : Page
{
    private readonly IHost _host;
    private ServerManager ServerManager => _host.Services.GetRequiredService<ServerManager>();
    private readonly Timer _timer;

    public PanelPage(IHost host)
    {
        _host = host;
        InitializeComponent();

        _timer = new(2500) { Enabled = true };
        _timer.Elapsed += (_, _) => UpdateInfo();

        ServerManager.PropertyChanged += (_, _) => UpdateInfo();
        DataContext = ServerManager;
        Console.EnableAnsiColor();
        UpdateInfo();
    }

    private void ControlButton_Click(object sender, RoutedEventArgs e)
    {
        var tag = (sender as Control)?.Tag?.ToString();

        try
        {
            switch (tag)
            {
                case "start":
                    ServerManager.Start();
                    break;

                case "stop":
                    ServerManager.Stop();
                    break;

                case "restart":
                    break;

                case "terminate":
                    TerminateFlyout.Hide();
                    ServerManager.Terminate();
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.AppendErrorLine(ex.Message);
        }
    }

    private void Enter_Click(object sender, RoutedEventArgs e)
    {
        Input();
        InputBox.Focus();
    }

    private void InputBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
            Input();
    }

    private void Input()
    {
        ServerManager.Input(InputBox.Text);
        InputBox.Text = string.Empty;
    }

    private const string EmptyHolder = "-";

    private void UpdateInfo()
    {
        Dispatcher.Invoke(() =>
        {
            if (!IsLoaded)
                return;

            Status.Text = ServerManager.Status switch
            {
                ServerStatus.Unknown => "未启动",
                ServerStatus.Stopped => "已关闭",
                ServerStatus.Running => "运行中",
                _ => throw new NotSupportedException()
            };

            Players.Text =
                ServerManager.Status == ServerStatus.Running
                    ? $"{ServerManager.ServerInfo?.Motd?.CurrentPlayersInt}/{ServerManager.ServerInfo?.Motd?.MaximumPlayersInt}"
                    : EmptyHolder;

            CPUUsage.Text =
                ServerManager.Status == ServerStatus.Running && ServerManager.ServerInfo is not null
                    ? ServerManager.ServerInfo.CPUUsage.ToString("N1") + "%"
                    : EmptyHolder;

            Version.Text =
                ServerManager.Status == ServerStatus.Running
                && ServerManager.ServerInfo?.Motd is not null
                    ? ServerManager.ServerInfo?.Motd.Version
                    : EmptyHolder;

            if (
                ServerManager.Status == ServerStatus.Running
                && ServerManager.ServerInfo?.StartTime is not null
            )
            {
                var span =
                    DateTime.Now - ServerManager.ServerInfo.StartTime
                    ?? throw new NullReferenceException();
                RunTime.Text = $"{span.Days}d{span.Hours}h{span.Minutes}m";
            }
            else
                RunTime.Text = EmptyHolder;
        });
    }
}
