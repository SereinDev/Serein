using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Navigation;

using iNKORE.UI.WPF.Modern.Controls;
using iNKORE.UI.WPF.Modern.Media.Animation;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace Serein.Plus.Ui.Pages.Settings;

public partial class CategoriesPage : Page
{
    private readonly IHost _host;
    private SlideNavigationTransitionInfo TransitionInfo { get; }
    private IServiceProvider Services => _host.Services;

    public CategoriesPage(IHost host)
    {
        _host = host;
        TransitionInfo = new SlideNavigationTransitionInfo
        {
            Effect = SlideNavigationTransitionEffect.FromLeft
        };

        InitializeComponent();
        ConfigureNavigation();
    }

    private void ConfigureNavigation()
    {
        NavigationView.MenuItems = new List<NavigationViewItem>()
        {
            new()
            {
                // Icon = new FontIcon { Glyph = SegoeIcons.NetworkTower },
                Content = "连接",
                Tag = nameof(NetworkSettingPage)
            },
            new()
            {
                // Icon = new FontIcon { Glyph = SegoeIcons.EmojiTabSmilesAnimals },
                Content = "反应",
                Tag = nameof(ReactionSettingPage)
            },
            new()
            {
                // Icon = new FontIcon { Glyph = SegoeIcons.AppIconDefault },
                Content = "应用",
                Tag = nameof(AppSettingPage)
            },
            new()
            {
                // Icon = new FontIcon { Glyph = SegoeIcons.Page },
                Content = "页面",
                Tag = nameof(PageSettingPage)
            },
            new()
            {
                // Icon = new FontIcon { Glyph = SegoeIcons.Info },
                Content = "关于",
                Tag = nameof(AboutPage)
            },
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
            nameof(AboutPage) => Services.GetRequiredService<AboutPage>(),
            nameof(AppSettingPage) => Services.GetRequiredService<AppSettingPage>(),
            nameof(NetworkSettingPage) => Services.GetRequiredService<NetworkSettingPage>(),
            nameof(PageSettingPage) => Services.GetRequiredService<PageSettingPage>(),
            nameof(ReactionSettingPage) => Services.GetRequiredService<ReactionSettingPage>(),
            _ => Services.GetRequiredService<NotImplPage>(),
        };

        if (page is not null)
            SubFrame.Navigate(page, TransitionInfo);
    }
}
