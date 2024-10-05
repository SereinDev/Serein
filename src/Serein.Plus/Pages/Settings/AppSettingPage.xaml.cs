using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using iNKORE.UI.WPF.Modern;

using Serein.Core.Models.Settings;
using Serein.Core.Services.Data;

using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace Serein.Plus.Pages.Settings;

public partial class AppSettingPage : Page
{
    private readonly SettingProvider _settingProvider;

    public AppSettingPage(SettingProvider settingProvider)
    {
        InitializeComponent();
        _settingProvider = settingProvider;
        DataContext = _settingProvider;

        ThemePanel.Children
            .Cast<RadioButton>()
            .First(c => c?.Tag?.ToString() == _settingProvider.Value.Application.Theme.ToString())
            .IsChecked = true;
    }

    private void OnPropertyChanged(object? sender, EventArgs e)
    {
        if (IsLoaded)
            _settingProvider.SaveAsyncWithDebounce();
    }

    private void OnThemeRadioButtonChecked(object sender, RoutedEventArgs e)
    {
        var tag = (sender as RadioButton)?.Tag?.ToString();

        _settingProvider.Value.Application.Theme = tag switch
        {
            "Light" => Theme.Light,
            "Dark" => Theme.Dark,
            _ => Theme.Default
        };

        ThemeManager.Current.ApplicationTheme = _settingProvider.Value.Application.Theme switch
        {
            Theme.Light => ApplicationTheme.Light,
            Theme.Dark => ApplicationTheme.Dark,
            _ => null,
        };

        OnPropertyChanged(sender, e);
    }
}
