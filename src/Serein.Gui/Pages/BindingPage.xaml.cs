using System;
using System.Windows;
using System.Windows.Controls;
using Serein.Gui.ViewModels;

namespace Serein.Gui.Pages;

public partial class BindingPage : Page
{
    private readonly BindingPageViewModel _viewModel;

    public BindingPage(BindingPageViewModel viewModel)
    {
        DataContext = _viewModel = viewModel;

        InitializeComponent();
    }

    private async void Page_Loaded(object sender, RoutedEventArgs e)
    {
        await _viewModel.LoadAsync();
    }
}
