using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;

using iNKORE.UI.WPF.Modern;
using iNKORE.UI.WPF.Modern.Controls;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core.Models.Settings;
using Serein.Core.Services.Data;
using Serein.Plus.Ui.Pages;
using Serein.Plus.Ui.Pages.Function;
using Serein.Plus.Ui.Pages.Server;
using Serein.Plus.Ui.Pages.Settings;

namespace Serein.Plus.Ui.Window;

public partial class MainWindow : System.Windows.Window
{
    private readonly IHost _host = App.Host;
    private IServiceProvider Services => _host.Services;
    private SettingProvider SettingProvider => Services.GetRequiredService<SettingProvider>();

    public MainWindow()
    {
        InitializeComponent();
        ConfigureNavigation();

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

    private void ConfigureNavigation()
    {
        NavigationView.MenuItems = new List<NavigationViewItem>()
        {
            new()
            {
                Icon = new FontIcon { Glyph = SegoeIcons.DirectAccess },
                Tag = nameof(PanelPage),
                Content = "服务器",
                ToolTip = "查看输出、管理服务器状态"
            },
            new()
            {
                Icon = new FontIcon { Glyph = SegoeIcons.List },
                Tag = nameof(MatchPage),
                Content = "匹配",
                ToolTip = "管理匹配列表"
            },
            new()
            {
                Icon = new FontIcon { Glyph = SegoeIcons.EmojiTabFavorites },
                Tag = nameof(SchedulePage),
                Content = "定时任务",
                ToolTip = "管理定时执行的命令"
            },
            new()
            {
                Icon = new FontIcon { Glyph = SegoeIcons.NetworkTower },
                Content = "连接",
                ToolTip = "通过WebSocket连接获取消息"
            },
            new()
            {
                Icon = new FontIcon { Glyph = SegoeIcons.People },
                Content = "成员管理",
                ToolTip = "绑定用户的昵称和游戏名称"
            },
            new()
            {
                Icon = new FontIcon { Glyph = SegoeIcons.Puzzle },
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
            RootFrame.Navigate(Services.GetRequiredService<SettingPage>());
            return;
        }

        Page? page = item.Tag?.ToString() switch
        {
            nameof(PanelPage) => Services.GetRequiredService<PanelPage>(),
            nameof(MatchPage) => Services.GetRequiredService<MatchPage>(),
            nameof(PluginPage) => Services.GetRequiredService<PluginPage>(),
            nameof(SchedulePage) => Services.GetRequiredService<SchedulePage>(),
            _ => Services.GetRequiredService<NotImplPage>(),
        };

        if (page is not null)
            RootFrame.Navigate(page);
    }
}
