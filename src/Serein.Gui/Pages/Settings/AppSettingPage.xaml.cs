using System;
using Serein.Gui.ViewModels;
using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace Serein.Gui.Pages.Settings;

public partial class AppSettingPage : Page
{
    public AppSettingPageViewModel ViewModel { get; }

    public AppSettingPage(AppSettingPageViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();
        DataContext = ViewModel;
    }

    private void OnPropertyChanged(object? sender, EventArgs e)
    {
        if (IsLoaded)
        {
            ViewModel.SettingProvider.SaveAsyncWithDebounce();
        }
    }
}
