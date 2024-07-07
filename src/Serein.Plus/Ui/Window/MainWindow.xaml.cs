using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

using Hardcodet.Wpf.TaskbarNotification;

using iNKORE.UI.WPF.Modern;
using iNKORE.UI.WPF.Modern.Controls;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core;
using Serein.Core.Models.Plugins;
using Serein.Core.Models.Server;
using Serein.Core.Models.Settings;
using Serein.Core.Services;
using Serein.Core.Services.Commands;
using Serein.Core.Services.Data;
using Serein.Core.Services.Plugins;
using Serein.Core.Services.Servers;
using Serein.Plus.Ui.Commands;
using Serein.Plus.Ui.Dialogs;
using Serein.Plus.Ui.Pages;
using Serein.Plus.Ui.Pages.Function;
using Serein.Plus.Ui.Pages.Server;
using Serein.Plus.Ui.Pages.Settings;

using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace Serein.Plus.Ui.Window;

public partial class MainWindow : System.Windows.Window
{
    private readonly IHost _host = SereinApp.Current!;
    private IServiceProvider Services => _host.Services;
    private ServerManager Servers => Services.GetRequiredService<ServerManager>();
    private CommandParser CommandParser => Services.GetRequiredService<CommandParser>();
    private SettingProvider SettingProvider => Services.GetRequiredService<SettingProvider>();
    private EventDispatcher EventDispatcher => Services.GetRequiredService<EventDispatcher>();
    private bool _isTopMost;
    private readonly Timer _timer;

    public MainWindow()
    {
        InitializeComponent();
        ConfigureNavigation();
        UpdateTitle();
        TaskbarIcon.ContextMenu.DataContext = this;
        TaskbarIcon.DoubleClickCommand = new TaskbarIconDoubleClickCommand(this);

        _timer = new(2500) { Enabled = true };
        _timer.Elapsed += (_, _) => UpdateTitle();

        TaskbarIcon.TrayBalloonTipClicked += (_, _) => ShowWindow();
        SettingProvider.Value.Application.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(SettingProvider.Value.Application.CustomTitle))
                UpdateTitle();
        };
    }

    private void OnDeactivated(object sender, EventArgs e)
    {
        Topmost = _isTopMost;
    }

    private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        UpdateTitle();
        ShowInTaskbar = IsVisible;
        HideMenuItem.IsChecked = !IsVisible;

        if (Topmost && !IsVisible)
            Topmost = false;
    }

    private void MenuItem_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not MenuItem item)
            return;

        var tag = item.Tag?.ToString();

        switch (tag)
        {
            case "TopMost":
                _isTopMost = item.IsChecked;
                break;

            case "Exit":
                Close();
                break;

            case "Hide":
                if (HideMenuItem.IsChecked)
                    Hide();
                else
                    ShowWindow();
                break;
        }
    }

    public void ShowWindow()
    {
        Show();
        Activate();
        WindowState = WindowState.Normal;
    }

    private void ConfigureNavigation()
    {
        NavigationView.MenuItems = new List<NavigationViewItem>()
        {
            new()
            {
                // Icon = new FontIcon { Glyph = SegoeIcons.DirectAccess },
                Tag = nameof(ServerPage),
                Content = "服务器",
                ToolTip = "查看输出、管理服务器状态"
            },
            new()
            {
                // Icon = new FontIcon { Glyph = SegoeIcons.List },
                Tag = nameof(MatchPage),
                Content = "匹配",
                ToolTip = "管理匹配列表"
            },
            new()
            {
                // Icon = new FontIcon { Glyph = SegoeIcons.EmojiTabFavorites },
                Tag = nameof(SchedulePage),
                Content = "定时任务",
                ToolTip = "管理定时执行的命令"
            },
            new()
            {
                // Icon = new FontIcon { Glyph = SegoeIcons.NetworkTower },
                Tag = nameof(ConnectionPage),
                Content = "连接",
                ToolTip = "通过WebSocket连接获取消息"
            },
            new()
            {
                // Icon = new FontIcon { Glyph = SegoeIcons.People },
                Content = "成员管理",
                ToolTip = "绑定用户的昵称和游戏名称"
            },
            new()
            {
                // Icon = new FontIcon { Glyph = SegoeIcons.Puzzle },
                Tag = nameof(PluginPage),
                Content = "插件",
                ToolTip = "扩展Serein玩法和功能"
            }
        };
        NavigationView.SelectedItem = NavigationView.MenuItems.OfType<NavigationViewItem>().First();
    }

    private void OnNavigationViewLoaded(object sender, RoutedEventArgs e)
    {
        if (NavigationView.SettingsItem is NavigationViewItem item)
        {
            item.Content = "设置";
            item.ToolTip = "调整Serein的设置内容";
        }
    }

    private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
    {
        e.Handled = true;
    }

    private void OnNavigating(object sender, NavigatingCancelEventArgs e)
    {
        e.Cancel = e.NavigationMode != NavigationMode.New;
    }

    private void OnNavigationViewSelectionChanged(
        object sender,
        NavigationViewSelectionChangedEventArgs e
    )
    {
        if (e.SelectedItem is not NavigationViewItem item || item is null)
            return;

        if (e.IsSettingsSelected)
        {
            RootFrame.Navigate(Services.GetRequiredService<CategoriesPage>());
            return;
        }

        Page? page = item.Tag?.ToString() switch
        {
            nameof(ServerPage) => Services.GetRequiredService<ServerPage>(),
            nameof(MatchPage) => Services.GetRequiredService<MatchPage>(),
            nameof(PluginPage) => Services.GetRequiredService<PluginPage>(),
            nameof(ConnectionPage) => Services.GetRequiredService<ConnectionPage>(),
            nameof(SchedulePage) => Services.GetRequiredService<SchedulePage>(),
            _ => Services.GetRequiredService<NotImplPage>(),
        };

        if (page is not null)
            RootFrame.Navigate(page);
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        ThemeManager.SetRequestedTheme(
            this,
            SettingProvider.Value.Application.Theme switch
            {
                Theme.Light => ElementTheme.Light,
                Theme.Dark => ElementTheme.Dark,
                _ => ElementTheme.Default
            }
        );
    }

    public void OnClosing(object sender, CancelEventArgs e)
    {
        Hide();

        if (!Servers.AnyRunning)
        {
            TaskbarIcon.Visibility = Visibility.Hidden;
            EventDispatcher.Dispatch(Event.SereinClosed);
            return;
        }

        e.Cancel = true;
        TaskbarIcon.ShowBalloonTip("服务器进程仍在运行中", "已自动最小化至托盘，点击托盘图标即可复原窗口", BalloonIcon.None);
    }

    private void OnContentRendered(object sender, EventArgs e)
    {
        if (SereinApp.StartForTheFirstTime)
            new WelcomeDialog().ShowAsync();

        _host.StartAsync();
    }

    // private void OnServerStatusChanged(object? sender, EventArgs e)
    // {
    //     Dispatcher.Invoke(() =>
    //     {
    //         if (IsVisible)
    //             return;

    //         TaskbarIcon.ShowBalloonTip(
    //             "服务器状态变更",
    //             ServerManager.Status == ServerStatus.Running
    //                 ? "服务器已启动"
    //                 : $"服务器进程已于{ServerManager.ServerInfo?.ExitTime:T}已退出",
    //             BalloonIcon.None
    //         );
    //     });
    // }

    private void UpdateTitle()
    {
        Dispatcher.Invoke(() =>
        {
            var text = CommandParser.ApplyVariables(
                SettingProvider.Value.Application.CustomTitle,
                null,
                true
            );

            TaskbarIcon.ToolTipText = Title = !string.IsNullOrEmpty(text.Trim())
                ? $"Serein.Plus - {text}"
                : "Serein.Plus";
        });
    }
}
