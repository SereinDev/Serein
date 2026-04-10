using iNKORE.UI.WPF.Modern.Controls;
using Serein.Gui.ViewModels;

namespace Serein.Gui.Pages.Settings;

public partial class AboutPage : Page
{
    public AboutPageViewModel ViewModel { get; }

    public AboutPage(AboutPageViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();
        DataContext = ViewModel;
    }
}
