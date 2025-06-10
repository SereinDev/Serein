using System;
using System.Collections.Generic;
using System.Windows.Navigation;
using iNKORE.UI.WPF.Modern.Controls;
using Microsoft.Extensions.DependencyInjection;
using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace Serein.Plus.Pages.Settings;

public partial class CategoriesPage : Page
{
    private readonly IServiceProvider _services;

    public CategoriesPage(IServiceProvider services)
    {
        _services = services;

        InitializeComponent();
        ConfigureNavigation();
    }

    private void ConfigureNavigation()
    {
        NavigationView.MenuItems = new List<NavigationViewItem>
        {
            new() { Content = "连接", Tag = typeof(ConnectionSettingPage) },
            new() { Content = "反应", Tag = typeof(ReactionSettingPage) },
            new() { Content = "网页", Tag = typeof(WebApiSettingPage) },
            new() { Content = "应用", Tag = typeof(AppSettingPage) },
            new() { Content = "关于", Tag = typeof(AboutPage) },
        };
        NavigationView.SelectedItem = NavigationView.MenuItems[0];
    }

    private void NavigationView_SelectionChanged(
        NavigationView sender,
        NavigationViewSelectionChangedEventArgs args
    )
    {
        if (args.SelectedItem is not NavigationViewItem { Tag: Type type })
        {
            type = typeof(NotImplPage);
        }

        SubFrame.Navigate(_services.GetRequiredService(type));
    }

    private void SubFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
    {
        e.Handled = true;
    }
}
