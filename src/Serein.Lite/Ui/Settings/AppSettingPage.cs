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

    public AppSettingPage(SettingProvider settingProvider, UpdateChecker updateChecker)
    {
        _settingProvider = settingProvider;
        _updateChecker = updateChecker;
        InitializeComponent();
        SetBindings();
        UpdateLatestVersion();

        VersionLabel.Text = $"当前版本：{SereinApp.Version}";

        JSGlobalAssembliesTextBox.Text = string.Join(
            "\r\n",
            _settingProvider.Value.Application.JSGlobalAssemblies
        );
        JSPatternToSkipLoadingSingleFileTextBox.Text = string.Join(
            "\r\n",
            _settingProvider.Value.Application.JSPatternToSkipLoadingSingleFile
        );
        PattenForEnableMatchingMuiltLinesTextBox.Text = string.Join(
            "\r\n",
            _settingProvider.Value.Application.PattenForEnableMatchingMuiltLines
        );
        _updateChecker.Updated += (_, _) => Invoke(() => UpdateLatestVersion());
    }

    private void UpdateLatestVersion()
    {
        LatestVersionLabel.Text = $"最新版本：{_updateChecker.Newest?.TagName ?? "-"}";
    }

    private void OnPropertyChanged(object sender, EventArgs e)
    {
        _settingProvider.SaveAsyncWithDebounce();
    }

    private void SetBindings()
    {
        PluginEventMaxWaitingTimeNumericUpDown.DataBindings.Add(
            nameof(PluginEventMaxWaitingTimeNumericUpDown.Value),
            _settingProvider.Value.Application,
            nameof(ApplicationSetting.PluginEventMaxWaitingTime),
            false,
            DataSourceUpdateMode.OnPropertyChanged
        );
        RegexForCheckingGameIDTextBox.DataBindings.Add(
            nameof(RegexForCheckingGameIDTextBox.Text),
            _settingProvider.Value.Application,
            nameof(ApplicationSetting.RegexForCheckingGameId),
            false,
            DataSourceUpdateMode.OnPropertyChanged
        );
        CustomTitleTextBox.DataBindings.Add(
            nameof(CustomTitleTextBox.Text),
            _settingProvider.Value.Application,
            nameof(ApplicationSetting.CustomTitle),
            false,
            DataSourceUpdateMode.OnPropertyChanged
        );
        DisableBinderWhenServerClosedCheckBox.DataBindings.Add(
            nameof(DisableBinderWhenServerClosedCheckBox.Checked),
            _settingProvider.Value.Application,
            nameof(ApplicationSetting.DisableBinderWhenServerClosed),
            false,
            DataSourceUpdateMode.OnPropertyChanged
        );
        CheckUpdateCheckBox.DataBindings.Add(
            nameof(CheckUpdateCheckBox.Checked),
            _settingProvider.Value.Application,
            nameof(ApplicationSetting.CheckUpdate),
            false,
            DataSourceUpdateMode.OnPropertyChanged
        );
        AutoUpdateCheckBox.DataBindings.Add(
            nameof(AutoUpdateCheckBox.Checked),
            _settingProvider.Value.Application,
            nameof(ApplicationSetting.AutoUpdate),
            false,
            DataSourceUpdateMode.OnPropertyChanged
        );
        AutoUpdateCheckBox.DataBindings.Add(
            nameof(AutoUpdateCheckBox.Enabled),
            _settingProvider.Value.Application,
            nameof(ApplicationSetting.CheckUpdate),
            false,
            DataSourceUpdateMode.OnPropertyChanged
        );
    }

    private void CheckUpdateButton_Click(object sender, EventArgs e)
    {
        LatestVersionLabel.Text = "最新版本：";
        _updateChecker.CheckAsync().ContinueWith((_) => Invoke(UpdateLatestVersion));
    }

    private void JSGlobalAssembliesTextBox_TextChanged(object sender, EventArgs e)
    {
        _settingProvider.Value.Application.JSGlobalAssemblies =
            JSGlobalAssembliesTextBox.Text.Split(
                "\r\n",
                StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries
            );
    }

    private void JSPatternToSkipLoadingSingleFileTextBox_TextChanged(object sender, EventArgs e)
    {
        _settingProvider.Value.Application.JSPatternToSkipLoadingSingleFile =
            JSPatternToSkipLoadingSingleFileTextBox.Text.Split(
                "\r\n",
                StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries
            );
    }

    private void PattenForEnableMatchingMuiltLinesTextBox_TextChanged(object sender, EventArgs e)
    {
        _settingProvider.Value.Application.PattenForEnableMatchingMuiltLines =
            PattenForEnableMatchingMuiltLinesTextBox.Text.Split(
                "\r\n",
                StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries
            );
    }
}
