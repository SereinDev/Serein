using System.Windows;
using Serein.Gui.ViewModels;
using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace Serein.Gui.Pages;

public partial class ServerPage : Page
{
    private readonly ServerPageViewModel _viewModel;

    public ServerPage(ServerPageViewModel viewModel)
    {
        InitializeComponent();
        DataContext = _viewModel = viewModel;
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
