using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using iNKORE.UI.WPF.Modern.Controls;
using Microsoft.Win32;
using Serein.Core.Models.Server;
using Serein.Core.Services.Servers;
using Serein.Plus.Controls;
using Serein.Plus.Services;
using Serein.Plus.Windows;
using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace Serein.Plus.Pages;

public partial class ServerPage : Page
{
    private readonly MainWindow _mainWindow;
    private readonly InfoBarProvider _infoBarProvider;
    private readonly ServerManager _serverManager;
    private readonly Dictionary<string, PanelTabItem> _panels;

    private CancellationTokenSource? _cancellationTokenSource;

    public ServerPage(
        MainWindow mainWindow,
        InfoBarProvider infoBarProvider,
        ServerManager serverManager
    )
    {
        _mainWindow = mainWindow;
        _infoBarProvider = infoBarProvider;
        _serverManager = serverManager;
        _panels = [];

        InitializeComponent();

        DataContext = this;

        foreach (var (id, server) in _serverManager.Servers)
        {
            Add(id, server);
        }

        _serverManager.ServersUpdated += ServerManager_ServersUpdated;
    }

    public void RaiseMenuItemClickEvent(object sender, RoutedEventArgs e)
    {
        MenuItem_Click(sender, e);
    }

    private void Add(string id, Server server)
    {
        var tabItem = new PanelTabItem(id, server, _serverManager, this, _mainWindow) { Tag = id };

        _panels[id] = tabItem;
        TabControl.Items.Add(tabItem);

        if (TabControl.Items.Count == 1)
        {
            TabControl.SelectedIndex = 0;
        }
    }

    private void ServerManager_ServersUpdated(object? sender, ServersUpdatedEventArgs e)
    {
        Dispatcher.Invoke(() =>
        {
            if (
                e.Type == ServersUpdatedType.Added
                && _serverManager.Servers.TryGetValue(e.Id, out var server)
            )
            {
                Add(e.Id, server);
            }
            else if (_panels.TryGetValue(e.Id, out var page))
            {
                if (TabControl.Items.Contains(page))
                {
                    TabControl.Items.Remove(page);
                }
                _panels.Remove(e.Id);

                _infoBarProvider.Enqueue(
                    $"服务器（Id: {e.Id}）删除成功",
                    string.Empty,
                    InfoBarSeverity.Success
                );
            }
        });
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = new();
        if (_serverManager.Servers.Count == 0)
        {
            _infoBarProvider.Enqueue(
                "当前没有服务器配置",
                "你可以点击上方的加号进行添加",
                InfoBarSeverity.Informational,
                TimeSpan.FromSeconds(5),
                _cancellationTokenSource.Token
            );
        }
    }

    private void Page_Unloaded(object sender, RoutedEventArgs e)
    {
        if (_cancellationTokenSource?.IsCancellationRequested == false)
        {
            _cancellationTokenSource.Cancel();
        }
    }

    private void MenuItem_Click(object sender, RoutedEventArgs e)
    {
        switch ((sender as MenuItem)?.Tag as string)
        {
            case "Import":
                try
                {
                    var dialog = new OpenFileDialog { Filter = "服务器配置文件|*.json" };
                    if (dialog.ShowDialog() != true)
                    {
                        return;
                    }

                    var config = ServerManager.LoadFrom(dialog.FileName);
                    var editor1 = new ServerConfigurationEditor(_serverManager, config)
                    {
                        Owner = _mainWindow,
                    };

                    if (editor1.ShowDialog() != true || string.IsNullOrEmpty(editor1.Id))
                    {
                        return;
                    }

                    _serverManager.Add(editor1.Id, editor1.Configuration);
                }
                catch (Exception ex)
                {
                    _infoBarProvider.Enqueue("导入失败", ex.Message, InfoBarSeverity.Error);
                }
                break;

            case "Add":
                try
                {
                    var editor2 = new ServerConfigurationEditor(_serverManager, new())
                    {
                        Owner = _mainWindow,
                    };

                    if (editor2.ShowDialog() != true || string.IsNullOrEmpty(editor2.Id))
                    {
                        return;
                    }

                    _serverManager.Add(editor2.Id, editor2.Configuration);
                }
                catch (Exception ex)
                {
                    _infoBarProvider.Enqueue("添加失败", ex.Message, InfoBarSeverity.Error);
                }
                break;

            default:
                throw new NotSupportedException();
        }
    }
}
