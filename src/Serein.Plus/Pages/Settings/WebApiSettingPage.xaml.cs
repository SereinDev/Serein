using System;
using System.Windows;
using iNKORE.UI.WPF.Modern.Controls;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using Serein.Core.Services.Data;
using Serein.Core.Services.Network.Web;
using Serein.Plus.Services;

namespace Serein.Plus.Pages.Settings;

public partial class WebApiSettingPage : System.Windows.Controls.Page
{
    private readonly ILogger<WebApiSettingPage> _logger;
    private readonly WebServer _httpServer;
    private readonly InfoBarProvider _infoBarProvider;
    private readonly SettingProvider _settingProvider;

    public WebApiSettingPage(
        ILogger<WebApiSettingPage> logger,
        WebServer httpServer,
        InfoBarProvider infoBarProvider,
        SettingProvider settingProvider
    )
    {
        _logger = logger;
        _httpServer = httpServer;
        _infoBarProvider = infoBarProvider;
        _settingProvider = settingProvider;
        DataContext = _settingProvider;
        InitializeComponent();
    }

    private void OnPropertyChanged(object sender, EventArgs e)
    {
        if (IsLoaded)
        {
            _settingProvider.SaveAsyncWithDebounce();
        }
    }

    private void OpenFileButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog();
        if (dialog.ShowDialog() == true)
        {
            _settingProvider.Value.WebApi.Certificate.Path = dialog.FileName;
        }
        OnPropertyChanged(sender, e);
    }

    private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
    {
        try
        {
            if (_settingProvider.Value.WebApi.IsEnabled)
            {
                _httpServer.Start();
                _infoBarProvider.Enqueue(
                    "网页服务器开启成功",
                    string.Empty,
                    InfoBarSeverity.Success
                );
            }
            else
            {
                _httpServer.Stop();
                _infoBarProvider.Enqueue(
                    "网页服务器关闭成功",
                    string.Empty,
                    InfoBarSeverity.Success
                );
            }
        }
        catch (Exception ex)
        {
            _infoBarProvider.Enqueue("切换状态失败", ex.Message, InfoBarSeverity.Error);
            _logger.LogError(ex, "切换状态失败");
        }
        OnPropertyChanged(sender, e);
    }
}
