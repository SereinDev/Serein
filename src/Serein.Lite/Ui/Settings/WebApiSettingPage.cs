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

        _urlPrefixesTextBox.Text = string.Join("\r\n", _settingProvider.Value.WebApi.UrlPrefixes);
        _whiteListTextBox.Text = string.Join("\r\n", _settingProvider.Value.WebApi.WhiteList);
        _accessTokensTextBox.Text = string.Join("\r\n", _settingProvider.Value.WebApi.AccessTokens);
    }

    private void SetBindings()
    {
        _certificateEnableCheckBox.DataBindings.Add(
            nameof(_certificateEnableCheckBox.Checked),
            _settingProvider.Value.WebApi.Certificate,
            nameof(CertificateSetting.IsEnabled),
            false,
            DataSourceUpdateMode.OnPropertyChanged
        );
        _autoLoadCertificateCheckBox.DataBindings.Add(
            nameof(_autoLoadCertificateCheckBox.Checked),
            _settingProvider.Value.WebApi.Certificate,
            nameof(CertificateSetting.AutoLoadCertificate),
            false,
            DataSourceUpdateMode.OnPropertyChanged
        );
        _autoRegisterCertificateCheckBox.DataBindings.Add(
            nameof(_autoRegisterCertificateCheckBox.Checked),
            _settingProvider.Value.WebApi.Certificate,
            nameof(CertificateSetting.AutoRegisterCertificate),
            false,
            DataSourceUpdateMode.OnPropertyChanged
        );
        _passwordMaskedTextBox.DataBindings.Add(
            nameof(_passwordMaskedTextBox.Text),
            _settingProvider.Value.WebApi.Certificate,
            nameof(CertificateSetting.Password),
            false,
            DataSourceUpdateMode.OnPropertyChanged
        );
        _pathTextBox.DataBindings.Add(
            nameof(_pathTextBox.Text),
            _settingProvider.Value.WebApi.Certificate,
            nameof(CertificateSetting.Path),
            false,
            DataSourceUpdateMode.OnPropertyChanged
        );

        _isEnableCheckBox.DataBindings.Add(
            nameof(_isEnableCheckBox.Checked),
            _settingProvider.Value.WebApi,
            nameof(WebApiSetting.IsEnabled),
            false,
            DataSourceUpdateMode.OnPropertyChanged
        );
        _allowCrossOriginCheckBox.DataBindings.Add(
            nameof(_allowCrossOriginCheckBox.Checked),
            _settingProvider.Value.WebApi,
            nameof(WebApiSetting.AllowCrossOrigin),
            false,
            DataSourceUpdateMode.OnPropertyChanged
        );
        _maxRequestsPerSecondNumericUpDown.DataBindings.Add(
            nameof(_maxRequestsPerSecondNumericUpDown.Value),
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
        _settingProvider.Value.WebApi.UrlPrefixes = _urlPrefixesTextBox.Text.Split(
            ';',
            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries
        );
        OnPropertyChanged(sender, e);
    }

    private void WhiteListTextBox_TextChanged(object sender, EventArgs e)
    {
        _settingProvider.Value.WebApi.WhiteList = _whiteListTextBox.Text.Split(
            ';',
            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries
        );
        OnPropertyChanged(sender, e);
    }

    private void AccessTokensTextBox_TextChanged(object sender, EventArgs e)
    {
        _settingProvider.Value.WebApi.AccessTokens = _accessTokensTextBox.Text.Split(
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
            if (_isEnableCheckBox.Checked)
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
            _pathTextBox.Text = dialog.FileName;
        }
    }
}
