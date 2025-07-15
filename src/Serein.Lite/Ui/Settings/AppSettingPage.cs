using System;
using System.Windows.Forms;
using Serein.Core;
using Serein.Core.Models.Settings;
using Serein.Core.Services.Data;
using Serein.Core.Services.Network;

namespace Serein.Lite.Ui.Settings;

public partial class AppSettingPage : UserControl
{
    private readonly SettingProvider _settingProvider;
    private readonly UpdateChecker _updateChecker;

    public AppSettingPage(
        SereinApp sereinApp,
        SettingProvider settingProvider,
        UpdateChecker updateChecker
    )
    {
        _settingProvider = settingProvider;
        _updateChecker = updateChecker;
        InitializeComponent();
        SetBindings();
        UpdateLatestVersion();

        _versionLabel.Text = $"当前版本：{sereinApp.Version}";

        _jsDefaultAssembliesTextBox.Text = string.Join(
            "\r\n",
            _settingProvider.Value.Application.JsDefaultAssemblies
        );
        _jsFilesToExcludeFromLoadingTextBox.Text = string.Join(
            "\r\n",
            _settingProvider.Value.Application.JsFilesToExcludeFromLoading
        );
        _multiLineMatchingPatternsTextBox.Text = string.Join(
            "\r\n",
            _settingProvider.Value.Application.MultiLineMatchingPatterns
        );
        _updateChecker.Updated += (_, _) => Invoke(() => UpdateLatestVersion());
    }

    private void UpdateLatestVersion()
    {
        _latestVersionLabel.Text = $"最新版本：{_updateChecker.LastResult?.TagName ?? "-"}";
    }

    private void OnPropertyChanged(object sender, EventArgs e)
    {
        _settingProvider.SaveAsyncWithDebounce();
    }

    private void SetBindings()
    {
        _enableSentryCheckBox.DataBindings.Add(
            nameof(_enableSentryCheckBox.Checked),
            _settingProvider.Value.Application,
            nameof(ApplicationSetting.EnableSentry),
            false,
            DataSourceUpdateMode.OnPropertyChanged
        );
        _maximumWaitTimeForPluginEventsNumericUpDown.DataBindings.Add(
            nameof(_maximumWaitTimeForPluginEventsNumericUpDown.Value),
            _settingProvider.Value.Application,
            nameof(ApplicationSetting.MaximumWaitTimeForPluginEvents),
            false,
            DataSourceUpdateMode.OnPropertyChanged
        );
        _gameIdValidationPatternTextBox.DataBindings.Add(
            nameof(_gameIdValidationPatternTextBox.Text),
            _settingProvider.Value.Application,
            nameof(ApplicationSetting.GameIdValidationPattern),
            false,
            DataSourceUpdateMode.OnPropertyChanged
        );
        _customTitleTextBox.DataBindings.Add(
            nameof(_customTitleTextBox.Text),
            _settingProvider.Value.Application,
            nameof(ApplicationSetting.CustomTitle),
            false,
            DataSourceUpdateMode.OnPropertyChanged
        );
        _disableBindingManagerWhenAllServersStoppedCheckBox.DataBindings.Add(
            nameof(_disableBindingManagerWhenAllServersStoppedCheckBox.Checked),
            _settingProvider.Value.Application,
            nameof(ApplicationSetting.DisableBindingManagerWhenAllServersStopped),
            false,
            DataSourceUpdateMode.OnPropertyChanged
        );
        _checkUpdateCheckBox.DataBindings.Add(
            nameof(_checkUpdateCheckBox.Checked),
            _settingProvider.Value.Application,
            nameof(ApplicationSetting.CheckUpdate),
            false,
            DataSourceUpdateMode.OnPropertyChanged
        );
        _autoUpdateCheckBox.DataBindings.Add(
            nameof(_autoUpdateCheckBox.Checked),
            _settingProvider.Value.Application,
            nameof(ApplicationSetting.AutoUpdate),
            false,
            DataSourceUpdateMode.OnPropertyChanged
        );
        _autoUpdateCheckBox.DataBindings.Add(
            nameof(_autoUpdateCheckBox.Enabled),
            _settingProvider.Value.Application,
            nameof(ApplicationSetting.CheckUpdate),
            false,
            DataSourceUpdateMode.OnPropertyChanged
        );
    }

    private void CheckUpdateButton_Click(object sender, EventArgs e)
    {
        _latestVersionLabel.Text = "最新版本：";
        _updateChecker.CheckAsync().ContinueWith((_) => Invoke(UpdateLatestVersion));
    }

    private void JsDefaultAssembliesTextBox_TextChanged(object sender, EventArgs e)
    {
        _settingProvider.Value.Application.JsDefaultAssemblies =
            _jsDefaultAssembliesTextBox.Text.Split(
                "\r\n",
                StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries
            );
    }

    private void JsFilesToExcludeFromLoadingTextBox_TextChanged(object sender, EventArgs e)
    {
        _settingProvider.Value.Application.JsFilesToExcludeFromLoading =
            _jsFilesToExcludeFromLoadingTextBox.Text.Split(
                "\r\n",
                StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries
            );
    }

    private void multiLineMatchingPatternsTextBox_TextChanged(object sender, EventArgs e)
    {
        _settingProvider.Value.Application.MultiLineMatchingPatterns =
            _multiLineMatchingPatternsTextBox.Text.Split(
                "\r\n",
                StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries
            );
    }
}
