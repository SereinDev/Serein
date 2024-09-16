using System;
using System.IO;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using iNKORE.UI.WPF.Modern.Controls;

using Serein.Core.Models.Server;
using Serein.Core.Services.Servers;
using Serein.Core.Utils;
using Serein.Core.Utils.Extensions;
using Serein.Plus.Dialogs;
using Serein.Plus.Pages;
using Serein.Plus.Services;
using Serein.Plus.Windows;

namespace Serein.Plus.Controls;

public partial class PanelTabItem : TabItem
{
    private readonly string _id;
    private readonly Server _server;
    private readonly ServerManager _serverManager;
    private readonly ServerPage _page;
    private readonly InfoBarProvider _infoBarProvider;
    private readonly Timer _timer;
    private readonly object _lock;

    public PanelTabItem(
        string id,
        Server server,
        ServerManager serverManager,
        ServerPage page,
        InfoBarProvider infoBarProvider
    )
    {
        _id = id;
        _lock = new();
        _server = server;
        _serverManager = serverManager;
        _page = page;
        _infoBarProvider = infoBarProvider;
        DataContext = _server.Configuration;
        InitializeComponent();
        UpdateInfo();
        Console.EnableAnsiColor();
        Console.EnableLogLevelHighlight(true);

        _timer = new(2500) { Enabled = true };
        _timer.Elapsed += (_, _) => UpdateInfo();
        _server.ServerStatusChanged += (_, _) => UpdateInfo();
        _server.ServerStatusChanged += OnServerStatusChanged;
        _server.ServerOutput += Output;
    }

    private void Output(object? sender, ServerOutputEventArgs e)
    {
        switch (e.OutputType)
        {
            case ServerOutputType.Raw:
                lock (_lock)
                    Dispatcher.Invoke(
                        () =>
                            Console.AppendLine(
                                _server.Configuration.OutputStyle == OutputStyle.Plain
                                    ? OutputFilter.RemoveColorChars(e.Data)
                                    : e.Data
                            )
                    );
                break;

            case ServerOutputType.InputCommand:
                if (_server.Configuration.OutputCommandUserInput)
                    lock (_lock)
                        Dispatcher.Invoke(() => Console.AppendLine($">{e.Data}"));
                break;

            case ServerOutputType.Information:
                lock (_lock)
                    Dispatcher.Invoke(() => Console.AppendNoticeLine(e.Data));
                break;

            case ServerOutputType.Error:
                lock (_lock)
                    Dispatcher.Invoke(() => Console.AppendErrorLine(e.Data));
                break;
        }
    }

    private void ControlButton_Click(object sender, RoutedEventArgs e)
    {
        var tag = (sender as Control)?.Tag?.ToString();

        try
        {
            switch (tag)
            {
                case "Start":
                    _server.Start();
                    break;

                case "Stop":
                    _server.Stop();
                    break;

                case "Restart":
                    _server.RequestRestart();
                    break;

                case "Terminate":
                    TerminateFlyout.Hide();
                    _server.Terminate();
                    break;

                case "OpenInExplorer":
                    if (File.Exists(_server.Configuration.FileName))
                        _server.Configuration.FileName.OpenInExplorer();
                    else
                        throw new InvalidOperationException("启动文件不存在，无法打开其所在文件夹");
                    break;
            }
        }
        catch (Exception ex)
        {
            _infoBarProvider.Enqueue("操作失败", ex.Message, InfoBarSeverity.Error);
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
        InputBox.Focus();
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
                _ => throw new NotSupportedException(),
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

    private void MenuItem_Click(object sender, RoutedEventArgs e)
    {
        switch ((sender as MenuItem)?.Tag?.ToString())
        {
            case "Edit":
                var config = _server.Configuration.WiseClone();
                var editor = new ServerConfigurationEditor(_serverManager, config, _id);
                if (editor.ShowDialog() != true)
                    return;

                editor.Configuration.WiseCloneTo(_server.Configuration);
                Header = _server.Configuration;
                _serverManager.SaveAll();
                break;
            case "Remove":
                DialogHelper
                    .ShowDeleteConfirmation($"确定要删除服务器配置（Id: {_id}）吗？")
                    .ContinueWith(
                        (task) =>
                        {
                            if (task.Result)
                                try
                                {
                                    _serverManager.Remove(_id);
                                }
                                catch (Exception ex)
                                {
                                    _infoBarProvider.Enqueue(
                                        "删除服务器失败",
                                        ex.Message,
                                        InfoBarSeverity.Error
                                    );
                                }
                        }
                    );
                break;

            default:
                _page.RaiseMenuItemClickEvent(sender, e);
                break;
        }
    }
}
