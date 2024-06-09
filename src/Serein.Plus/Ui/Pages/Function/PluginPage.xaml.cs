using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Navigation;

using iNKORE.UI.WPF.Modern;
using iNKORE.UI.WPF.Modern.Controls;
using iNKORE.UI.WPF.Modern.Media.Animation;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Serein.Plus.Ui.Pages.Function;

public partial class PluginPage : Page
{
    private readonly IHost _host;
    private IServiceProvider Services => _host.Services;
    private readonly NavigationTransitionInfo _transitionInfo;

    public PluginPage(IHost host)
    {
        _host = host;
        _transitionInfo = new SlideNavigationTransitionInfo()
        {
            Effect = SlideNavigationTransitionEffect.FromLeft
        };

        InitializeComponent();
        ConfigureNavigation();
    }

    private void ConfigureNavigation()
    {
        NavigationView.MenuItems = new List<NavigationViewItem>
        {
            new()
            {
                // Icon = new FontIcon { Glyph = SegoeIcons.OEM },
                Tag = nameof(PluginConsolePage),
                Content = "输出",
                ToolTip = "查看插件输出信息"
            },
            new()
            {
                // Icon = new FontIcon { Glyph = SegoeIcons.AllApps },
                Tag = nameof(PluginListPage),
                Content = "管理",
                ToolTip = "管理已经加载的插件列表"
            },
            new()
            {
                // Icon = new FontIcon { Glyph = SegoeIcons.Package },
                Content = "市场",
                ToolTip = "从社区下载插件"
            }
        };
        NavigationView.SelectedItem = NavigationView.MenuItems.OfType<NavigationViewItem>().First();
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

        Page? page = item.Tag?.ToString() switch
        {
            nameof(PluginListPage) => Services.GetRequiredService<PluginListPage>(),
            nameof(PluginConsolePage) => Services.GetRequiredService<PluginConsolePage>(),
            _ => Services.GetRequiredService<NotImplPage>(),
        };

        if (page is not null)
            RootFrame.Navigate(page, _transitionInfo);
    }
}
