using System;
using System.Windows;
using System.Windows.Controls;
using iNKORE.UI.WPF.Modern;
using iNKORE.UI.WPF.Modern.Controls;
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

        ReadyToReplaceInfoBar.DataContext = _updateChecker;
        DataContext = _settingProvider;

        _updateChecker.Updated += (_, _) => Dispatcher.Invoke(UpdateVersionInfoBar);
    }

    private void UpdateVersionInfoBar()
    {
        if (_updateChecker.Latest is not null)
        {
            VersionInfoBar.IsOpen = true;
            VersionInfoBar.Severity = InfoBarSeverity.Informational;
            VersionInfoBar.Title = "�����°汾";
            VersionInfoBar.Message = $"��ǰ�汾��{_sereinApp.Version}�����°汾��{_updateChecker.Latest.TagName}";
        }
        else if (_updateChecker.LastResult is not null)
        {
            VersionInfoBar.IsOpen = true;
            VersionInfoBar.Severity = InfoBarSeverity.Success;
            VersionInfoBar.Title = $"��ǰ�������°汾��{_sereinApp.Version}��";
            VersionInfoBar.Message = null;
        }
    }

    private void OnPropertyChanged(object? sender, EventArgs e)
    {
        if (IsLoaded)
        {
            _settingProvider.SaveAsyncWithDebounce();
        }
    }

    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ThemeManager.Current.ApplicationTheme = _settingProvider.Value.Application.Theme switch
        {
            Theme.Light => ApplicationTheme.Light,
            Theme.Dark => ApplicationTheme.Dark,
            _ => null,
        };

        OnPropertyChanged(sender, e);
    }

    private void SettingsCard_Click(object sender, RoutedEventArgs e)
    {
        if (sender is SettingsCard { IsEnabled: true } settingsCard)
        {
                    VersionInfoBar.IsOpen = false;
            settingsCard.IsEnabled = false;
            settingsCard.Content = new ProgressRing { IsActive = true, Width = 25, Height = 25 };

            _updateChecker.CheckAsync().ContinueWith((task) =>
                Dispatcher.Invoke(() =>
                {
                    settingsCard.IsEnabled = true;
                    settingsCard.Content = null;
                    UpdateVersionInfoBar();
                }));
        }
    }
}
