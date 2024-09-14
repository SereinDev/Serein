using System;
using System.Windows.Navigation;

using iNKORE.UI.WPF.Modern.Controls;

using Microsoft.Extensions.DependencyInjection;

using Serein.Plus.ViewModels;

namespace Serein.Plus.Pages;

public partial class PluginPage : Page
{
    private readonly IServiceProvider _services;

    public PluginViewModel ViewModel { get; }

    public PluginPage(IServiceProvider services, PluginViewModel viewModel)
    {
        _services = services;
        ViewModel = viewModel;
        DataContext = ViewModel;

        InitializeComponent();
        NavView.SelectedItem = NavView.MenuItems[0];
        ContentFrame.Navigate(_services.GetRequiredService<PluginConsolePage>());
    }

    private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
    {
        e.Handled = true;
    }

    private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        if (args.InvokedItemContainer.Tag is not Type type)
            type = typeof(NotImplPage);

        ContentFrame.Navigate(_services.GetRequiredService(type));
    }
}
