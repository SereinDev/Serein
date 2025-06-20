using System;
using System.Collections.Generic;
using System.Windows.Forms;
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
    }

    private void OnPropertyChanged(object sender, EventArgs e)
    {
        _settingProvider.SaveAsyncWithDebounce();
    }

    private void SetBindings()
    {
        //UriTextBox.DataBindings.Add(
        //    nameof(UriTextBox.Text),
        //    _settingProvider.Value.Connection,
        //    nameof(ConnectionSetting.Uri),
        //    false,
        //    DataSourceUpdateMode.OnPropertyChanged
        //);
        //AccessTokenMaskedTextBox.DataBindings.Add(
        //    nameof(AccessTokenMaskedTextBox.Text),
        //    _settingProvider.Value.Connection,
        //    nameof(ConnectionSetting.AccessToken),
        //    false,
        //    DataSourceUpdateMode.OnPropertyChanged
        //);
        //UseReverseWebSocketCheckBox.DataBindings.Add(
        //    nameof(UseReverseWebSocketCheckBox.Checked),
        //    _settingProvider.Value.Connection,
        //    nameof(ConnectionSetting.UseReverseWebSocket),
        //    false,
        //    DataSourceUpdateMode.OnPropertyChanged
        //);
        //AutoReconnectCheckBox.DataBindings.Add(
        //    nameof(AutoReconnectCheckBox.Checked),
        //    _settingProvider.Value.Connection,
        //    nameof(ConnectionSetting.AutoReconnect),
        //    false,
        //    DataSourceUpdateMode.OnPropertyChanged
        //);
        //OutputDataCheckBox.DataBindings.Add(
        //    nameof(OutputDataCheckBox.Checked),
        //    _settingProvider.Value.Connection,
        //    nameof(ConnectionSetting.OutputData),
        //    false,
        //    DataSourceUpdateMode.OnPropertyChanged
        //);
        //AutoEscapeCheckBox.DataBindings.Add(
        //    nameof(AutoEscapeCheckBox.Checked),
        //    _settingProvider.Value.Connection,
        //    nameof(ConnectionSetting.AutoEscape),
        //    false,
        //    DataSourceUpdateMode.OnPropertyChanged
        //);
        //SaveLogCheckBox.DataBindings.Add(
        //    nameof(SaveLogCheckBox.Checked),
        //    _settingProvider.Value.Connection,
        //    nameof(ConnectionSetting.SaveLog),
        //    false,
        //    DataSourceUpdateMode.OnPropertyChanged
        //);
        //GrantPermissionToOwnerAndAdminsCheckBox.DataBindings.Add(
        //    nameof(GrantPermissionToOwnerAndAdminsCheckBox.Checked),
        //    _settingProvider.Value.Connection,
        //    nameof(ConnectionSetting.GrantPermissionToOwnerAndAdmins),
        //    false,
        //    DataSourceUpdateMode.OnPropertyChanged
        //);
    }

    private void SubProtocolsTextBox_TextChanged(object sender, EventArgs e)
    {
      
        OnPropertyChanged(sender, e);
    }

    private void GroupsTextBox_TextChanged(object sender, EventArgs e)
    {
      
        OnPropertyChanged(sender, e);
    }

    private void AdministratorsTextBox_TextChanged(object sender, EventArgs e)
    {
        OnPropertyChanged(sender, e);
    }
}
