using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using iNKORE.UI.WPF.Modern;
using Serein.Core;
using Serein.Core.Models.Settings;
using Serein.Core.Services.Data;
using Serein.Core.Services.Network;
using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace Serein.Plus.Pages.Settings;

public partial class AppSettingPage : Page
{
    private readonly SereinApp _sereinApp;
    private readonly SettingProvider _settingProvider;
    private readonly UpdateChecker _updateChecker;

    public AppSettingPage(
        SereinApp sereinApp,
        SettingProvider settingProvider,
        UpdateChecker updateChecker
    )
    {
        _sereinApp = sereinApp;
        _settingProvider = settingProvider;
        _updateChecker = updateChecker;

        InitializeComponent();
        UpdateVersionInfoBar();

        DataContext = _settingProvider;

        ThemePanel
            .Children.Cast<RadioButton>()
            .First(c => c?.Tag as string == _settingProvider.Value.Application.Theme.ToString())
            .IsChecked = true;

        _updateChecker.Updated += (_, _) => Dispatcher.Invoke(UpdateVersionInfoBar);
    }

    private void UpdateVersionInfoBar()
    {
        VersionInfoBar.IsOpen = _updateChecker.Latest is not null;
        VersionInfoBar.Message =
            $"当前版本：{_sereinApp.Version}\r\n新版本：{_updateChecker.Latest?.TagName}";
    }

    private void OnPropertyChanged(object? sender, EventArgs e)
    {
        if (IsLoaded)
        {
            _settingProvider.SaveAsyncWithDebounce();
        }
    }

    private void OnThemeRadioButtonChecked(object sender, RoutedEventArgs e)
    {
        var tag = (sender as RadioButton)?.Tag as string;

        _settingProvider.Value.Application.Theme = tag switch
        {
            "Light" => Theme.Light,
            "Dark" => Theme.Dark,
            _ => Theme.Default,
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
