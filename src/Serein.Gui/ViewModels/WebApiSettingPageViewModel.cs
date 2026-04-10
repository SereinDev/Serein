using System;
using System.Collections.Specialized;
using iNKORE.UI.WPF.Modern.Controls;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using Serein.Core.Models.Abstractions;
using Serein.Core.Models.Settings;
using Serein.Core.Services.Data;
using Serein.Core.Services.Network.Web;
using Serein.Gui.Commands;
using Serein.Gui.Services;
using Serein.Gui.Windows;

namespace Serein.Gui.ViewModels;

public class WebApiSettingPageViewModel : NotifyPropertyChangedModelBase
{
    private readonly ILogger<WebApiSettingPageViewModel> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly MainWindow _mainWindow;
    private readonly WebServer _httpServer;
    private readonly PageExtractor _pageExtractor;
    private readonly InfoBarProvider _infoBarProvider;
    private readonly SettingProvider _settingProvider;
    private readonly WebAuthenticationProvider _webAuthenticationProvider;

    public WebServer HttpServer => _httpServer;
    public WebApiSetting WebApiSetting => _settingProvider.Value.WebApi;
    public string AuthenticationCountText =>
        $"目前共有{_webAuthenticationProvider.Value.Count}条访问凭证";

    public RelayCommand SaveSettingsCommand { get; }
    public RelayCommand ToggleServerStateCommand { get; }
    public RelayCommand ExtractPagesCommand { get; }
    public RelayCommand OpenEditorCommand { get; }
    public RelayCommand OpenFileCommand { get; }

    public WebApiSettingPageViewModel(
        ILogger<WebApiSettingPageViewModel> logger,
        IServiceProvider serviceProvider,
        MainWindow mainWindow,
        WebServer httpServer,
        PageExtractor pageExtractor,
        InfoBarProvider infoBarProvider,
        SettingProvider settingProvider
    )
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _mainWindow = mainWindow;
        _httpServer = httpServer;
        _pageExtractor = pageExtractor;
        _infoBarProvider = infoBarProvider;
        _settingProvider = settingProvider;
        _webAuthenticationProvider =
            _serviceProvider.GetRequiredService<WebAuthenticationProvider>();
        _webAuthenticationProvider.Value.CollectionChanged += OnWebAuthenticationCollectionChanged;

        SaveSettingsCommand = new(SaveSettings);
        ToggleServerStateCommand = new(ToggleServerState);
        ExtractPagesCommand = new(ExtractPages);
        OpenEditorCommand = new(OpenEditor);
        OpenFileCommand = new(OpenFile);
    }

    private void OnWebAuthenticationCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        RaisePropertyChanged(nameof(AuthenticationCountText));
    }

    private void SaveSettings()
    {
        _settingProvider.SaveAsyncWithDebounce();
    }

    private void ToggleServerState()
    {
        try
        {
            if (_httpServer.State == EmbedIO.WebServerState.Stopped)
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
    }

    private void ExtractPages()
    {
        try
        {
            _pageExtractor.Extract();
            _infoBarProvider.Enqueue(
                "解压成功",
                "需要重启网页服务器以应用更改",
                InfoBarSeverity.Success
            );
        }
        catch (Exception ex)
        {
            _infoBarProvider.Enqueue("解压失败", ex.Message, InfoBarSeverity.Error);
        }
    }

    private void OpenFile()
    {
        var dialog = new OpenFileDialog();

        if (dialog.ShowDialog() == true)
        {
            WebApiSetting.Certificate.Path = dialog.FileName;
            SaveSettings();
        }
    }

    private void OpenEditor()
    {
        var window = new WebAuthenticationEditor(_webAuthenticationProvider)
        {
            Owner = _mainWindow,
        };

        window.ShowDialog();
    }
}
