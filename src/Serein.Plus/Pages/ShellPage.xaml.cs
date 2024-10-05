using System;
using System.Windows;
using System.Windows.Navigation;

using iNKORE.UI.WPF.Modern.Controls;

using Microsoft.Extensions.DependencyInjection;

using Serein.Plus.Pages.Settings;
using Serein.Plus.Services;
using Serein.Plus.ViewModels;

namespace Serein.Plus.Pages;

public partial class ShellPage : Page
{
    private readonly IServiceProvider _services;
    private readonly TitleUpdater _titleUpdater;

    public ShellViewModel ViewModel { get; }

    public ShellPage(IServiceProvider services, TitleUpdater titleUpdater, ShellViewModel shellViewModel)
    {
        _services = services;
        _titleUpdater = titleUpdater;
        ViewModel = shellViewModel;

        InitializeComponent();

        DataContext = ViewModel;
        AppTitleBarText.DataContext = _titleUpdater;
        NavView.SelectedItem = NavView.MenuItems[0];
        ContentFrame.Navigate(_services.GetRequiredService<ServerPage>());
    }

    private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
    {
        e.Handled = true;
    }

    private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        if (args.InvokedItemContainer.Tag is not Type type)
            type = args.IsSettingsInvoked ? typeof(CategoriesPage) : typeof(NotImplPage);

        var page = _services.GetRequiredService(type);

        ContentFrame.Navigate(page, args.RecommendedNavigationTransitionInfo);
    }

    private void NavView_DisplayModeChanged(NavigationView sender, NavigationViewDisplayModeChangedEventArgs args)
    {
        var currMargin = AppTitleBar.Margin;
        AppTitleBar.Margin = new Thickness(sender.CompactPaneLength, currMargin.Top, currMargin.Right, currMargin.Bottom);
    }

    private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
    {
        NavView.IsBackEnabled = ContentFrame.CanGoBack;
        ContentFrame.RemoveBackEntry();
    }
}
