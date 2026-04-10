using System;
using System.Windows;
using Serein.Gui.ViewModels;
using Page = System.Windows.Controls.Page;

namespace Serein.Gui.Pages.Settings;

public partial class WebApiSettingPage : Page
{
    public WebApiSettingPageViewModel ViewModel { get; }

    public WebApiSettingPage(WebApiSettingPageViewModel viewModel)
    {
        DataContext = ViewModel = viewModel;
        InitializeComponent();
    }

    private void OnPropertyChanged(object? sender, EventArgs e)
    {
        if (IsLoaded)
        {
            ViewModel.SaveSettingsCommand.Execute(null);
        }
    }

    private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
    {
        if (!IsLoaded)
        {
            return;
        }

        ViewModel.ToggleServerStateCommand.Execute(null);
    }
}
