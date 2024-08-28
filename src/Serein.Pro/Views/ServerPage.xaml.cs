using System;
using System.Linq;
using System.Threading;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using Serein.Core;
using Serein.Core.Models.Server;
using Serein.Core.Services.Servers;
using Serein.Pro.Controls;
using Serein.Pro.Services;

namespace Serein.Pro.Views;

public sealed partial class ServerPage : Page
{
    private readonly IServiceProvider _services;
    private readonly ServerManager _serverManager;
    private readonly DispatcherQueue _dispatcherQueue;
    private readonly InfoBarProvider _infoBarProvider;
    private readonly MenuBarItemFlyout _tabMenuBarItem;
    private readonly MainWindow _mainWindow;
    private CancellationTokenSource? _cancellationTokenSource;

    public ServerPage()
    {
        _services = SereinApp.Current!.Services;
        _serverManager = _services.GetRequiredService<ServerManager>();
        _infoBarProvider = _services.GetRequiredService<InfoBarProvider>();
        _mainWindow = _services.GetRequiredService<MainWindow>();
        _dispatcherQueue = DispatcherQueue.GetForCurrentThread();

        var item1 = new MenuFlyoutItem
        {
            Icon = new SymbolIcon(Symbol.Edit),
            Text = "编辑"
        };
        var item2 = new MenuFlyoutItem
        {
            Icon = new SymbolIcon(Symbol.Delete),
            Text = "删除"
        };
        item1.Click += MenuFlyoutItem_Click;
        item2.Click += MenuFlyoutItem_Click;

        _tabMenuBarItem = new()
        {
            Items = { item1, item2 }
        };

        InitializeComponent();

        foreach ((var id, var server) in _serverManager.Servers)
            AddPanel(id, server);

        _serverManager.ServersUpdated += ServerManager_ServersUpdated;
    }

    private void ServerManager_ServersUpdated(object? sender, ServersUpdatedEventArgs e)
    {
        if (e.Type == ServersUpdatedType.Added &&
            _serverManager.Servers.TryGetValue(e.Id, out var server))
            _dispatcherQueue.TryEnqueue(() => AddPanel(e.Id, server));
        else
            _dispatcherQueue.TryEnqueue(() => RemovePanel(e.Id));
    }

    private void AddPanel(string id, Server server)
    {
        TabView.TabItems.Add(new TabViewItem
        {
            Content = new ServerPanel(id, server, _infoBarProvider),
            Header = string.IsNullOrEmpty(server.Configuration.Name)
                    ? $"未命名-{id}"
                    : server.Configuration.Name,
            Tag = id,
            IsClosable = false,
            ContextFlyout = _tabMenuBarItem
        });
    }

    private void RemovePanel(string id)
    {
        var tabItems = TabView.TabItems.OfType<TabViewItem>();
        foreach (var item in tabItems)
        {
            if (item.Tag is string tag && tag == id)
                TabView.TabItems.Remove(item);
        }
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        if (_cancellationTokenSource is null || _cancellationTokenSource.IsCancellationRequested)
            _cancellationTokenSource = new();

        if (_serverManager.Servers.Count == 0)
            _infoBarProvider.ShowInfoBar(
                "当前没有服务器配置",
                "你可以点击页面上方的“+”添加或导入配置",
                InfoBarSeverity.Informational,
                TimeSpan.FromSeconds(10),
                _cancellationTokenSource.Token
                );

    }

    private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
    {
        var tag = (sender as MenuFlyoutItem)?.Tag as string;

        switch (tag)
        {
            case "Delete":
                try
                {
                    if (TabView.SelectedItem is TabViewItem { Tag: string t } && _serverManager.Remove(t))
                        RemovePanel(t);
                }
                catch (Exception ex)
                {
                    _infoBarProvider.ShowInfoBar(
                       "删除服务器配置失败",
                       ex.Message,
                       InfoBarSeverity.Warning
                       );
                }
                break;

            default:
                throw new NotSupportedException();
        }
    }

    private void TabView_AddTabButtonClick(TabView sender, object args)
    {
        TabViewMenuFlyout.ShowAt(sender);
    }

    private void Page_Unloaded(object sender, RoutedEventArgs e)
    {
        _cancellationTokenSource?.Cancel();
    }
}
