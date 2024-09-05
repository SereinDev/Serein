using System;
using System.Threading;
using System.Windows;

using iNKORE.UI.WPF.Modern.Controls;

using Serein.Core.Services.Servers;
using Serein.Plus.Services;

using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace Serein.Plus.Pages;

public partial class ServerPage : Page
{
    private readonly InfoBarProvider _infoBarProvider;
    private readonly ServerManager _serverManager;

    private CancellationTokenSource? _cancellationTokenSource;

    public ServerPage(InfoBarProvider infoBarProvider, ServerManager serverManager)
    {
        _infoBarProvider = infoBarProvider;
        _serverManager = serverManager;

        InitializeComponent();
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        _cancellationTokenSource = new();
        if (_serverManager.Servers.Count == 0)
            _infoBarProvider.Enqueue(
                "当前没有服务器配置",
                "你可以点击上方的加号进行添加",
                InfoBarSeverity.Informational,
                TimeSpan.FromSeconds(5),
                _cancellationTokenSource.Token
                );
    }

    private void Page_Unloaded(object sender, RoutedEventArgs e)
    {
        if (_cancellationTokenSource?.IsCancellationRequested == false)
            _cancellationTokenSource.Cancel();
    }
}
