using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using Serein.Core;
using Serein.Pro.Helpers;
using Serein.Pro.ViewModels;
using Serein.Pro.Views.Settings;

using Windows.UI.ViewManagement;

namespace Serein.Pro.Views;

public sealed partial class ShellPage : Page
{
    private readonly DispatcherQueue _dispatcherQueue;
    private readonly UISettings _settings;
    private Window MainWindow =>
        (Application.Current as App)!.Host.Services.GetRequiredService<MainWindow>();

    private ShellViewModel ViewModel { get; }

    public ShellPage()
    {
        InitializeComponent();

        _dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        _settings = new();
        _settings.ColorValuesChanged += Settings_ColorValuesChanged;

        ViewModel = SereinApp.Current!.Services.GetRequiredService<ShellViewModel>();

        if (NavView.MenuItems.Count > 0 && NavView.MenuItems[0] is NavigationViewItem { Tag: Type type })
        {
            NavView.SelectedItem = NavView.MenuItems[0];
            ContentFrame.Navigate(type);
        }
    }

    private void NavView_DisplayModeChanged(
        NavigationView sender,
        NavigationViewDisplayModeChangedEventArgs args
    )
    {
        AppTitleBar.Margin = new Thickness
        {
            Left =
                sender.CompactPaneLength
                * (sender.DisplayMode == NavigationViewDisplayMode.Minimal ? 2 : 1),
            Top = AppTitleBar.Margin.Top,
            Right = AppTitleBar.Margin.Right,
            Bottom = AppTitleBar.Margin.Bottom,
        };
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        MainWindow.SetTitleBar(AppTitleBar);
        TitleBarHelper.UpdateTitleBar(RequestedTheme);
    }


    private void Settings_ColorValuesChanged(UISettings sender, object args)
    {
        _dispatcherQueue.TryEnqueue(() => TitleBarHelper.UpdateTitleBar(AppTitleBar.ActualTheme));
    }

    private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        if (args.IsSettingsInvoked)
            ContentFrame.Navigate(typeof(SettingPage), null, args.RecommendedNavigationTransitionInfo);
        else if (args.InvokedItemContainer is NavigationViewItem { Tag: Type type })
            ContentFrame.Navigate(type, null, args.RecommendedNavigationTransitionInfo);
        else
            ContentFrame.Navigate(typeof(BlankPage), null, args.RecommendedNavigationTransitionInfo);
    }
}
