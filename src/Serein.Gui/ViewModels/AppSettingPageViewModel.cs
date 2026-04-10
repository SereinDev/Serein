using System;
using System.ComponentModel;
using System.Windows;
using iNKORE.UI.WPF.Modern;
using iNKORE.UI.WPF.Modern.Controls;
using Microsoft.Extensions.Logging;
using Serein.Core;
using Serein.Core.Models.Abstractions;
using Serein.Core.Models.Settings;
using Serein.Core.Services.Data;
using Serein.Core.Services.Network;
using Serein.Gui.Commands;
using Serein.Gui.Services.Loggers;
using Serein.Gui.Windows;

namespace Serein.Gui.ViewModels;

public class AppSettingPageViewModel : NotifyPropertyChangedModelBase
{
    private readonly SereinApp _sereinApp;
    public SettingProvider SettingProvider { get; }
    public UpdateChecker UpdateChecker { get; }
    private readonly ConsoleLoggerProvider _consoleLoggerProvider;
    private ConsoleWindow? _consoleWindow;

    public RelayCommand SaveSettingsCommand { get; }
    public RelayCommand CheckUpdateCommand { get; }
    public RelayCommand OpenConsoleCommand { get; }

    public ApplicationSetting ApplicationSetting => SettingProvider.Value.Application;

    public string CurrentVersion => _sereinApp.Version.ToString();

    // VersionInfo properties
    public bool IsVersionInfoOpen { get; set; }
    public InfoBarSeverity VersionInfoSeverity { get; set; }
    public string VersionInfoTitle { get; set; } = string.Empty;
    public string? VersionInfoMessage { get; set; }

    public AppSettingPageViewModel(
        SereinApp sereinApp,
        SettingProvider settingProvider,
        UpdateChecker updateChecker,
        ConsoleLoggerProvider consoleLoggerProvider
    )
    {
        _sereinApp = sereinApp;
        SettingProvider = settingProvider;
        UpdateChecker = updateChecker;
        _consoleLoggerProvider = consoleLoggerProvider;

        SaveSettingsCommand = new(SaveSettings);
        CheckUpdateCommand = new(CheckUpdateAsync);
        OpenConsoleCommand = new(OpenConsole);

        UpdateVersionInfoBar();
        UpdateChecker.Updated += (_, _) =>
            Application.Current.Dispatcher.Invoke(UpdateVersionInfoBar);

        // Listen to ApplicationSetting Theme changes
        ApplicationSetting.PropertyChanged += ApplicationSetting_PropertyChanged;
    }

    private void ApplicationSetting_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ApplicationSetting.Theme))
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ThemeManager.Current.ApplicationTheme = ApplicationSetting.Theme switch
                {
                    Theme.Light => ApplicationTheme.Light,
                    Theme.Dark => ApplicationTheme.Dark,
                    _ => null,
                };
            });
        }
        SaveSettings();
    }

    private void SaveSettings()
    {
        SettingProvider.SaveAsyncWithDebounce();
    }

    private void UpdateVersionInfoBar()
    {
        if (UpdateChecker.Latest is not null)
        {
            IsVersionInfoOpen = true;
            VersionInfoSeverity = InfoBarSeverity.Informational;
            VersionInfoTitle = "发现新版本";
            VersionInfoMessage =
                $"当前版本：{CurrentVersion}，最新版本：{UpdateChecker.Latest.TagName}";
        }
        else if (UpdateChecker.LastResult is not null)
        {
            IsVersionInfoOpen = true;
            VersionInfoSeverity = InfoBarSeverity.Success;
            VersionInfoTitle = $"当前已是最新版本（{CurrentVersion}）";
            VersionInfoMessage = null;
        }
    }

    private async void CheckUpdateAsync()
    {
        IsVersionInfoOpen = false;
        await UpdateChecker.CheckAsync();
        UpdateVersionInfoBar();
    }

    private void OpenConsole()
    {
        if (_consoleWindow is null)
        {
            _consoleWindow = new();
            _consoleLoggerProvider.LogWritten += Write;
            _consoleWindow.Closed += (_, _) =>
            {
                _consoleLoggerProvider.LogWritten -= Write;
                _consoleWindow = null;
            };
        }

        _consoleWindow.Show();
        _consoleWindow.Focus();

        void Write(object? _, (LogLevel Level, string Line) args)
        {
            _consoleWindow?.Dispatcher.Invoke(
                () => _consoleWindow.WriteLine(args.Level, args.Line)
            );
        }
    }
}
