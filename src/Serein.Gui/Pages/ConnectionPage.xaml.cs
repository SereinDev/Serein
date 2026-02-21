using System;
using System.Windows;
using Serein.Gui.ViewModels;
using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace Serein.Gui.Pages;

public partial class ConnectionPage : Page
{
    private readonly ConnectionPageViewModel _viewModel;

    public ConnectionPage(ConnectionPageViewModel viewModel)
    {
        _viewModel = viewModel;
        DataContext = _viewModel;

        InitializeComponent();
        Console.EnableLogLevelHighlight();
        _viewModel.RequestClearConsole += () => Dispatcher.Invoke(Console.Clear);
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        _viewModel.OnLoaded();
    }

    private void Page_Unloaded(object sender, RoutedEventArgs e)
    {
        _viewModel.OnUnloaded();
    }
}
