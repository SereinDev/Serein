using System;
using System.Windows.Forms;
using Serein.Core.Models.Settings;
using Serein.Core.Services.Data;
using Serein.Core.Services.Network.Web;
using Serein.Lite.Utils;

namespace Serein.Lite.Ui.Settings;

public partial class WebApiSettingPage : UserControl
{
    private readonly SettingProvider _settingProvider;
    private readonly WebServer _httpServer;

    public WebApiSettingPage(SettingProvider settingProvider, WebServer httpServer)
    {
        _settingProvider = settingProvider;
        _httpServer = httpServer;
        InitializeComponent();
        SetBindings();

        UrlPrefixesTextBox.Text = string.Join("\r\n", _settingProvider.Value.WebApi.UrlPrefixes);
        WhiteListTextBox.Text = string.Join("\r\n", _settingProvider.Value.WebApi.WhiteList);
        AccessTokensTextBox.Text = string.Join("\r\n", _settingProvider.Value.WebApi.AccessTokens);
    }

    private void SetBindings()
    {
        CertificateEnableCheckBox.DataBindings.Add(
            nameof(CertificateEnableCheckBox.Checked),
            _settingProvider.Value.WebApi.Certificate,
            nameof(CertificateSetting.IsEnabled),
            false,
            DataSourceUpdateMode.OnPropertyChanged
        );
        AutoLoadCertificateCheckBox.DataBindings.Add(
            nameof(AutoLoadCertificateCheckBox.Checked),
            _settingProvider.Value.WebApi.Certificate,
            nameof(CertificateSetting.AutoLoadCertificate),
            false,
            DataSourceUpdateMode.OnPropertyChanged
        );
        AutoRegisterCertificateCheckBox.DataBindings.Add(
            nameof(AutoRegisterCertificateCheckBox.Checked),
            _settingProvider.Value.WebApi.Certificate,
            nameof(CertificateSetting.AutoRegisterCertificate),
            false,
            DataSourceUpdateMode.OnPropertyChanged
        );
        PasswordMaskedTextBox.DataBindings.Add(
            nameof(PasswordMaskedTextBox.Text),
            _settingProvider.Value.WebApi.Certificate,
            nameof(CertificateSetting.Password),
            false,
            DataSourceUpdateMode.OnPropertyChanged
        );
        PathTextBox.DataBindings.Add(
            nameof(PathTextBox.Text),
            _settingProvider.Value.WebApi.Certificate,
            nameof(CertificateSetting.Path),
            false,
            DataSourceUpdateMode.OnPropertyChanged
        );

        EnableCheckBox.DataBindings.Add(
            nameof(EnableCheckBox.Checked),
            _settingProvider.Value.WebApi,
            nameof(WebApiSetting.IsEnabled),
            false,
            DataSourceUpdateMode.OnPropertyChanged
        );
        AllowCrossOriginCheckBox.DataBindings.Add(
            nameof(AllowCrossOriginCheckBox.Checked),
            _settingProvider.Value.WebApi,
            nameof(WebApiSetting.AllowCrossOrigin),
            false,
            DataSourceUpdateMode.OnPropertyChanged
        );
        MaxRequestsPerSecondNumericUpDown.DataBindings.Add(
            nameof(MaxRequestsPerSecondNumericUpDown.Value),
            _settingProvider.Value.WebApi,
            nameof(WebApiSetting.MaxRequestsPerSecond),
            false,
            DataSourceUpdateMode.OnPropertyChanged
        );
    }

    private void OnPropertyChanged(object sender, EventArgs e)
    {
        _settingProvider.SaveAsyncWithDebounce();
    }

    private void UrlPrefixesTextBox_TextChanged(object sender, EventArgs e)
    {
        _settingProvider.Value.WebApi.UrlPrefixes = UrlPrefixesTextBox.Text.Split(
            ';',
            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries
        );
        OnPropertyChanged(sender, e);
    }

    private void WhiteListTextBox_TextChanged(object sender, EventArgs e)
    {
        _settingProvider.Value.WebApi.WhiteList = WhiteListTextBox.Text.Split(
            ';',
            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries
        );
        OnPropertyChanged(sender, e);
    }

    private void AccessTokensTextBox_TextChanged(object sender, EventArgs e)
    {
        _settingProvider.Value.WebApi.AccessTokens = AccessTokensTextBox.Text.Split(
            ';',
            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries
        );
        OnPropertyChanged(sender, e);
    }

    private void EnableCheckBox_Click(object sender, EventArgs e)
    {
        OnPropertyChanged(sender, e);

        try
        {
            if (EnableCheckBox.Checked)
            {
                _httpServer.Start();
            }
            else
            {
                _httpServer.Stop();
            }
        }
        catch (Exception ex)
        {
            MessageBoxHelper.ShowWarningMsgBox("切换状态失败：\r\n" + ex.Message);
        }
    }

    private void OpenFileButton_Click(object sender, EventArgs e)
    {
        var dialog = new OpenFileDialog { Title = "选择证书文件" };

        if (dialog.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(dialog.FileName))
        {
            PathTextBox.Text = dialog.FileName;
        }
    }
}
