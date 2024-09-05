using System;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;

using iNKORE.UI.WPF.Modern.Controls;

using Microsoft.Extensions.DependencyInjection;

using Serein.Plus.Pages.Settings;
using Serein.Plus.ViewModels;

namespace Serein.Plus.Pages;

public partial class ShellPage : Page
{
    private readonly IServiceProvider _services;
    public ShellViewModel ViewModel { get; }

    public ShellPage(IServiceProvider services, ShellViewModel shellViewModel)
    {
        _services = services;
        ViewModel = shellViewModel;
        DataContext = ViewModel;

        InitializeComponent();
        NavView.SelectedItem = NavView.MenuItems[0];
        ContentFrame.Navigate(_services.GetRequiredService<HomePage>());
    }

    private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
    {
        e.Handled = true;
    }

    private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        if (args.InvokedItemContainer.Tag is not Type type)
            type = args.IsSettingsInvoked ? typeof(CategoriesPage) : typeof(NotImplPage);

        ContentFrame.Navigate(_services.GetRequiredService(type), args.RecommendedNavigationTransitionInfo);
    }

    private void NavView_DisplayModeChanged(NavigationView sender, NavigationViewDisplayModeChangedEventArgs args)
    {
        var currMargin = AppTitleBar.Margin;
        AppTitleBar.Margin =  new Thickness(sender.CompactPaneLength, currMargin.Top, currMargin.Right, currMargin.Bottom);
    }

    private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
    {
        NavView.IsBackEnabled = ContentFrame.CanGoBack;
    }
}
