using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using iNKORE.UI.WPF.Modern.Controls;

using Microsoft.Extensions.Hosting;

using Serein.Core.Models.Server;
using Serein.Core.Services.Servers;

namespace Serein.Plus.Controls;

public partial class PanelTabItem : TabItem
{
    private readonly IHost _host;
    private readonly string _id;
    private readonly Server _server;
    private readonly Timer _timer;

    public PanelTabItem(IHost host, string id, Server server)
    {
        _host = host;
        _id = id;
        _server = server;

        InitializeComponent();
        UpdateInfo();
        Console.EnableAnsiColor();

        _timer = new(2500) { Enabled = true };
        _timer.Elapsed += (_, _) => UpdateInfo();
        _server.ServerStatusChanged += (_, _) => UpdateInfo();
        _server.ServerStatusChanged += OnServerStatusChanged;
    }

    private void ControlButton_Click(object sender, RoutedEventArgs e)
    {
        var tag = (sender as Control)?.Tag?.ToString();

        try
        {
            switch (tag)
            {
                case "start":
                    _server.Start();
                    break;

                case "stop":
                    _server.Stop();
                    break;

                case "restart":
                    break;

                case "terminate":
                    TerminateFlyout.Hide();
                    _server.Terminate();
                    break;
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
        _server.Input(InputBox.Text);
        InputBox.Text = string.Empty;
    }

    private void OnServerStatusChanged(object? sender, EventArgs e)
    {
        if (_server.Status == ServerStatus.Running)
            Dispatcher.Invoke(Console.Clear);
    }

    private const string EmptyHolder = "-";

    private void UpdateInfo()
    {
        Dispatcher.Invoke(() =>
        {
            if (!IsLoaded)
                return;

            Status.Text = _server.Status switch
            {
                ServerStatus.Unknown => "未启动",
                ServerStatus.Stopped => "已关闭",
                ServerStatus.Running => "运行中",
                _ => throw new NotSupportedException()
            };

            Players.Text =
                _server.Status == ServerStatus.Running
                    ? $"{_server.ServerInfo?.Stat?.CurrentPlayersInt}/{_server.ServerInfo?.Stat?.MaximumPlayersInt}"
                    : EmptyHolder;

            CPUUsage.Text =
                _server.Status == ServerStatus.Running && _server.ServerInfo is not null
                    ? _server.ServerInfo.CPUUsage.ToString("N1") + "%"
                    : EmptyHolder;

            Version.Text =
                _server.Status == ServerStatus.Running && _server.ServerInfo?.Stat is not null
                    ? _server.ServerInfo?.Stat.Version
                    : EmptyHolder;

            if (_server.Status == ServerStatus.Running && _server.ServerInfo?.StartTime is not null)
            {
                var span =
                    DateTime.Now - _server.ServerInfo.StartTime
                    ?? throw new NullReferenceException();
                RunTime.Text = $"{span.Days}d{span.Hours}h{span.Minutes}m";
            }
            else
                RunTime.Text = EmptyHolder;
        });
    }
}
