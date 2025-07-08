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

        _jsGlobalAssembliesTextBox.Text = string.Join(
            "\r\n",
            _settingProvider.Value.Application.JSGlobalAssemblies
        );
        _jsPatternToSkipLoadingSingleFileTextBox.Text = string.Join(
            "\r\n",
            _settingProvider.Value.Application.JSPatternToSkipLoadingSingleFile
        );
        _pattenForEnableMatchingMuiltLinesTextBox.Text = string.Join(
            "\r\n",
            _settingProvider.Value.Application.PattenForEnableMatchingMuiltLines
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
        _pluginEventMaxWaitingTimeNumericUpDown.DataBindings.Add(
            nameof(_pluginEventMaxWaitingTimeNumericUpDown.Value),
            _settingProvider.Value.Application,
            nameof(ApplicationSetting.PluginEventMaxWaitingTime),
            false,
            DataSourceUpdateMode.OnPropertyChanged
        );
        _regexForCheckingGameIDTextBox.DataBindings.Add(
            nameof(_regexForCheckingGameIDTextBox.Text),
            _settingProvider.Value.Application,
            nameof(ApplicationSetting.RegexForCheckingGameId),
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
        _disableBindingManagerWhenServerClosedCheckBox.DataBindings.Add(
            nameof(_disableBindingManagerWhenServerClosedCheckBox.Checked),
            _settingProvider.Value.Application,
            nameof(ApplicationSetting.DisableBindingManagerWhenServerClosed),
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

    private void JSGlobalAssembliesTextBox_TextChanged(object sender, EventArgs e)
    {
        _settingProvider.Value.Application.JSGlobalAssemblies =
            _jsGlobalAssembliesTextBox.Text.Split(
                "\r\n",
                StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries
            );
    }

    private void JSPatternToSkipLoadingSingleFileTextBox_TextChanged(object sender, EventArgs e)
    {
        _settingProvider.Value.Application.JSPatternToSkipLoadingSingleFile =
            _jsPatternToSkipLoadingSingleFileTextBox.Text.Split(
                "\r\n",
                StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries
            );
    }

    private void PattenForEnableMatchingMuiltLinesTextBox_TextChanged(object sender, EventArgs e)
    {
        _settingProvider.Value.Application.PattenForEnableMatchingMuiltLines =
            _pattenForEnableMatchingMuiltLinesTextBox.Text.Split(
                "\r\n",
                StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries
            );
    }
}
