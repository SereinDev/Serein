using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Navigation;

using iNKORE.UI.WPF.Modern.Controls;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace Serein.Plus.Pages.Settings;

public partial class CategoriesPage : Page
{
    private readonly IHost _host;
    private IServiceProvider Services => _host.Services;

    public CategoriesPage(IHost host)
    {
        _host = host;
        InitializeComponent();
        ConfigureNavigation();
    }

    private void ConfigureNavigation()
    {
        NavigationView.MenuItems = new List<NavigationViewItem>()
        {
            new()
            {
                Content = "连接",
                Tag = nameof(ConnectionSettingPage)
            },
            new()
            {
                Content = "反应",
                Tag = nameof(ReactionSettingPage)
            },
            new()
            {
                Content = "应用",
                Tag = nameof(AppSettingPage)
            },
            new()
            {
                Content = "页面",
                Tag = nameof(PageSettingPage)
            },
            new()
            {
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

        Page page = item.Tag?.ToString() switch
        {
            nameof(AboutPage) => Services.GetRequiredService<AboutPage>(),
            nameof(AppSettingPage) => Services.GetRequiredService<AppSettingPage>(),
            nameof(ConnectionSettingPage) => Services.GetRequiredService<ConnectionSettingPage>(),
            nameof(PageSettingPage) => Services.GetRequiredService<PageSettingPage>(),
            nameof(ReactionSettingPage) => Services.GetRequiredService<ReactionSettingPage>(),
            _ => Services.GetRequiredService<NotImplPage>(),
        };

        SubFrame.Navigate(page);
    }
}
