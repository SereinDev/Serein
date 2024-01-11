using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using iNKORE.UI.WPF.Modern;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core.Models.Settings;
using Serein.Core.Services.Data;

using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace Serein.Plus.Ui.Pages.Settings;

public partial class AppSettingPage : Page
{
    private readonly IHost _host;

    private IServiceProvider Services => _host.Services;
    private SettingProvider SettingProvider => Services.GetRequiredService<SettingProvider>();

    public AppSettingPage(IHost host)
    {
        _host = host;
        InitializeComponent();
        DataContext = SettingProvider;

        ThemePanel.Children
            .Cast<RadioButton>()
            .First(c => c?.Tag?.ToString() == SettingProvider.Value.Application.Theme.ToString())
            .IsChecked = true;
    }

    private void OnPropertyChanged(object? sender, EventArgs e)
    {
        if (IsLoaded)
            SettingProvider.SaveAsyncWithDebounce();
    }

    private void OnThemeRadioButtonChecked(object sender, RoutedEventArgs e)
    {
        var tag = (sender as RadioButton)?.Tag?.ToString();

        ThemeManager.SetRequestedTheme(
            Application.Current.MainWindow,
            tag switch
            {
                "Light" => ElementTheme.Light,
                "Dark" => ElementTheme.Dark,
                _ => ElementTheme.Default
            }
        );

        OnPropertyChanged(sender, e);
    }
}
