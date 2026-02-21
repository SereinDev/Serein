using System;
using System.Windows.Navigation;
using iNKORE.UI.WPF.Modern.Controls;
using Microsoft.Extensions.DependencyInjection;
using Serein.Gui.ViewModels;

namespace Serein.Gui.Pages;

public partial class PluginPage : Page
{
    private readonly IServiceProvider _services;
    private readonly PluginPageViewModel _viewModel;

    public PluginPage(IServiceProvider services, PluginPageViewModel viewModel)
    {
        _services = services;
        _viewModel = viewModel;
        DataContext = _viewModel;

        InitializeComponent();
        NavView.SelectedItem = NavView.MenuItems[0];
        ContentFrame.Navigate(_services.GetRequiredService(_viewModel.GetDefaultPageType()));
    }

    private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
    {
        e.Handled = true;
    }

    private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        var type = _viewModel.ResolvePageType(args.InvokedItemContainer.Tag);

        ContentFrame.Navigate(_services.GetRequiredService(type));
    }
}
