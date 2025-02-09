using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using EmbedIO;
using EmbedIO.WebApi;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serein.Core.Services.Data;
using Serein.Core.Services.Network.Web.Apis;
using Serein.Core.Services.Network.Web.WebSockets;
using Serein.Core.Utils;
using Swan.Logging;

namespace Serein.Core.Services.Network.Web;

public sealed class WebServer
{
    static WebServer()
    {
        Logger.NoLogging();
    }

    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<WebServer> _logger;
    private readonly SettingProvider _settingProvider;
    private readonly List<IDisposable> _disposables;
    private EmbedIO.WebServer? _webServer;
    private CancellationTokenSource _cancellationTokenSource = new();

    public WebServer(
        IServiceProvider serviceProvider,
        ILogger<WebServer> logger,
        SettingProvider settingProvider,
        PageExtractor pageExtractor
    )
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _settingProvider = settingProvider;
        _disposables = [];

        if (!Directory.Exists(PathConstants.WebRoot) && !pageExtractor.TryExtract())
        {
            File.WriteAllText(
                Path.Combine(PathConstants.WebRoot, "index.html"),
                $"<p>你可以在 https://github.com/SereinDev/Web 仓库下载最新的构建或最新的版本，手动将解压后的文件复制到文件夹“{PathConstants.WebRoot}”下</p>"
            );
        }
    }

    public WebServerState State => _webServer?.State ?? WebServerState.Stopped;

    public void Start()
    {
        if (State != WebServerState.Stopped && State != WebServerState.Created)
        {
            throw new InvalidOperationException("网页服务器正在运行中");
        }

        if (_cancellationTokenSource.IsCancellationRequested)
        {
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = new();
        }

        _webServer = new EmbedIO.WebServer(CreateOptions());

        if (_settingProvider.Value.WebApi.AllowCrossOrigin)
        {
            _webServer.WithCors();
        }

        _webServer.WithModule(new AuthGate(_settingProvider));

        var serverWebSocketModule = _serviceProvider.GetRequiredService<ServerWebSocketModule>();
        _webServer.WithModule(serverWebSocketModule);
        _disposables.Add(serverWebSocketModule);

        var connectionWebSocketModule =
            _serviceProvider.GetRequiredService<ConnectionWebSocketModule>();
        _webServer.WithModule(connectionWebSocketModule);
        _disposables.Add(connectionWebSocketModule);

        var ipBannerModule = _serviceProvider.GetRequiredService<IpBannerModule>();
        _webServer.WithModule(ipBannerModule);
        _disposables.Add(ipBannerModule);

        _webServer.WithWebApi(
            "/api",
            (module) =>
                module
                    .HandleHttpException(ApiHelper.HandleHttpException)
                    .HandleUnhandledException(ApiHelper.HandleException)
                    .WithController(() => _serviceProvider.GetRequiredService<ApiMap>())
        );
        _webServer.WithStaticFolder("/", PathConstants.WebRoot, true);

        _webServer.Start(_cancellationTokenSource.Token);
        _logger.LogInformation("网页服务器已启动");
        _logger.LogInformation(
            "当前监听的Url： {}{}",
            Environment.NewLine,
            string.Join(Environment.NewLine, _settingProvider.Value.WebApi.UrlPrefixes)
        );
    }

    public void Stop()
    {
        if (
            State == WebServerState.Stopped
            || State == WebServerState.Created
            || _webServer is null
        )
        {
            throw new InvalidOperationException("网页服务器不在运行中");
        }

        _disposables.ForEach((d) => d.Dispose());
        _disposables.Clear();

        _cancellationTokenSource.Cancel();
        _webServer.Dispose();
        _logger.LogInformation("网页服务器已停止");
    }

    private WebServerOptions CreateOptions()
    {
        var options = new WebServerOptions();

        _settingProvider.Value.WebApi.UrlPrefixes.ToList().ForEach(options.AddUrlPrefix);
        if (_settingProvider.Value.WebApi.Certificate.IsEnabled)
        {
            options.AutoLoadCertificate = _settingProvider
                .Value
                .WebApi
                .Certificate
                .AutoLoadCertificate;
            options.AutoRegisterCertificate = _settingProvider
                .Value
                .WebApi
                .Certificate
                .AutoRegisterCertificate;

            if (string.IsNullOrEmpty(_settingProvider.Value.WebApi.Certificate.Path))
            {
                return options;
            }
            options.Certificate = File.Exists(_settingProvider.Value.WebApi.Certificate.Path)
                ? new(
                    _settingProvider.Value.WebApi.Certificate.Path!,
                    _settingProvider.Value.WebApi.Certificate.Password
                )
                : throw new InvalidOperationException(
                    $"证书文件“{_settingProvider.Value.WebApi.Certificate.Path}”不存在"
                );
        }

        return options;
    }
}
