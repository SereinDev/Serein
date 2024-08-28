using System;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

using Serein.Core;
using Serein.Pro.ViewModels;

namespace Serein.Pro.Views.Settings;

public sealed partial class SettingPage : Page
{
    private Type? _currentPage;
    private readonly IServiceProvider _services;
    private SettingViewModel ViewModel { get; }

    public SettingPage()
    {
        _services = SereinApp.Current!.Services;
        ViewModel = _services.GetRequiredService<SettingViewModel>();

        InitializeComponent();

        NavView.SelectedItem =
            NavView.MenuItems.OfType<NavigationViewItem>().FirstOrDefault((item) => item.Tag as Type == _currentPage) ??
            NavView.MenuItems[0];
        ContentFrame.Navigate(_currentPage ?? ViewModel.Connection);
    }

    private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        if (args.InvokedItemContainer is not NavigationViewItem { Tag: Type type })
            type = typeof(BlankPage);

        if (ContentFrame.CurrentSourcePageType != type)
        {
            ContentFrame.Navigate(type, null, args.RecommendedNavigationTransitionInfo);
            _currentPage = type;
        }
    }
}
