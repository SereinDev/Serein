using System;
using System.Windows.Forms;
using Serein.ConnectionProtocols.Models;
using Serein.ConnectionProtocols.Models.OneBot;
using Serein.Core.Models.Network.Connection;
using Serein.Core.Models.Settings;
using Serein.Core.Services.Data;

namespace Serein.Lite.Ui.Settings;

public partial class ConnectionSettingPage : UserControl
{
    private readonly SettingProvider _settingProvider;

    public ConnectionSettingPage(SettingProvider settingProvider)
    {
        _settingProvider = settingProvider;

        InitializeComponent();
        SetBindings();

        _adapterComboBox.SelectedIndex = (int)_settingProvider.Value.Connection.Adapter;
        _oneBotVersionComboBox.SelectedIndex = (int)
            _settingProvider.Value.Connection.OneBot.Version;
    }

    private void OnPropertyChanged(object sender, EventArgs e)
    {
        _settingProvider.SaveAsyncWithDebounce();
    }

    private void SetBindings()
    {
        _outputDataCheckBox.DataBindings.Add(
            nameof(_outputDataCheckBox.Checked),
            _settingProvider.Value.Connection,
            nameof(ConnectionSetting.OutputData),
            false,
            DataSourceUpdateMode.OnPropertyChanged
        );
        _saveLogCheckBox.DataBindings.Add(
            nameof(_saveLogCheckBox.Checked),
            _settingProvider.Value.Connection,
            nameof(ConnectionSetting.SaveLog),
            false,
            DataSourceUpdateMode.OnPropertyChanged
        );
        _connectWhenSettingUpcheckBox.DataBindings.Add(
            nameof(_connectWhenSettingUpcheckBox.Checked),
            _settingProvider.Value.Connection,
            nameof(ConnectionSetting.ConnectWhenSettingUp),
            false,
            DataSourceUpdateMode.OnPropertyChanged
        );

        _selfUserIdTextBox.DataBindings.Add(
            nameof(_selfUserIdTextBox.Text),
            _settingProvider.Value.Connection.Self,
            nameof(Self.UserId),
            false,
            DataSourceUpdateMode.OnPropertyChanged
        );
        _selfPlatformTextBox.DataBindings.Add(
            nameof(_selfPlatformTextBox.Text),
            _settingProvider.Value.Connection.Self,
            nameof(Self.Platform),
            false,
            DataSourceUpdateMode.OnPropertyChanged
        );

        _webSocketUriTextBox.DataBindings.Add(
            nameof(_webSocketUriTextBox.Text),
            _settingProvider.Value.Connection.OneBot,
            nameof(OneBotSetting.Uri),
            false,
            DataSourceUpdateMode.OnPropertyChanged
        );
        _oneBotAccessTokenMaskedTextBox.DataBindings.Add(
            nameof(_oneBotAccessTokenMaskedTextBox.Text),
            _settingProvider.Value.Connection.OneBot,
            nameof(OneBotSetting.AccessToken),
            false,
            DataSourceUpdateMode.OnPropertyChanged
        );
        _autoReconnectCheckBox.DataBindings.Add(
            nameof(_autoReconnectCheckBox.Checked),
            _settingProvider.Value.Connection.OneBot,
            nameof(OneBotSetting.AutoReconnect),
            false,
            DataSourceUpdateMode.OnPropertyChanged
        );
        _autoEscapeCheckBox.DataBindings.Add(
            nameof(_autoEscapeCheckBox.Checked),
            _settingProvider.Value.Connection.OneBot,
            nameof(OneBotSetting.AutoEscape),
            false,
            DataSourceUpdateMode.OnPropertyChanged
        );
        _grantPermissionToGroupOwnerAndAdminsCheckBox.DataBindings.Add(
            nameof(_grantPermissionToGroupOwnerAndAdminsCheckBox.Checked),
            _settingProvider.Value.Connection.OneBot,
            nameof(OneBotSetting.GrantPermissionToGroupOwnerAndAdmins),
            false,
            DataSourceUpdateMode.OnPropertyChanged
        );

        _satoriUriTextBox.DataBindings.Add(
            nameof(_satoriUriTextBox.Text),
            _settingProvider.Value.Connection.Satori,
            nameof(SatoriSetting.Uri),
            false,
            DataSourceUpdateMode.OnPropertyChanged
        );
        _satoriAccessTokenMaskedTextBox.DataBindings.Add(
            nameof(_satoriAccessTokenMaskedTextBox.Text),
            _settingProvider.Value.Connection.Satori,
            nameof(SatoriSetting.AccessToken),
            false,
            DataSourceUpdateMode.OnPropertyChanged
        );
    }

    private void AdapterComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        _settingProvider.Value.Connection.Adapter = (AdapterType)_adapterComboBox.SelectedIndex;

        OnPropertyChanged(sender, e);
    }

    private void AdministratorUserIdsTextBox_TextChanged(object sender, EventArgs e)
    {
        _settingProvider.Value.Connection.AdministratorUserIds =
            _administratorUserIdsTextBox.Text.Split(
                ';',
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
            );

        OnPropertyChanged(sender, e);
    }

    private void ListenedIdsTextBox_TextChanged(object sender, EventArgs e)
    {
        _settingProvider.Value.Connection.ListenedIds = _listenedIdsTextBox.Text.Split(
            ';',
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
        );

        OnPropertyChanged(sender, e);
    }

    private void OneBotVersionComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        _settingProvider.Value.Connection.OneBot.Version = (OneBotVersion)
            _oneBotVersionComboBox.SelectedIndex;

        OnPropertyChanged(sender, e);
    }

    private void WebSocketSubProtocolsTextBox_TextChanged(object sender, EventArgs e)
    {
        _settingProvider.Value.Connection.OneBot.SubProtocols =
            _webSocketSubProtocolsTextBox.Text.Split(
                "\r\n",
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
            );

        OnPropertyChanged(sender, e);
    }
}
