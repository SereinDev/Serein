using System;
using System.IO;
using System.Linq;
using System.Threading;

using EmbedIO;
using EmbedIO.WebApi;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serein.Core.Services.Data;
using Serein.Core.Services.Network.WebApi.Apis;

using Swan.Logging;

using MS = Microsoft.Extensions.Logging;

namespace Serein.Core.Services.Network.WebApi;

public sealed class HttpServer(
    IServiceProvider serviceProvider,
    ILogger<HttpServer> logger,
    SettingProvider settingProvider
)
{
    static HttpServer()
    {
        Logger.NoLogging();
    }

    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly MS.ILogger _logger = logger;
    private readonly SettingProvider _settingProvider = settingProvider;
    private WebServer? _webServer;
    private CancellationTokenSource _cancellationTokenSource = new();

    public WebServerState State => _webServer?.State ?? WebServerState.Stopped;

    public void Start()
    {
        if (State != WebServerState.Stopped && State != WebServerState.Created)
        {
            throw new InvalidOperationException("WebApi服务器正在运行中");
        }

        if (_cancellationTokenSource.IsCancellationRequested)
        {
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = new();
        }

        _webServer = new WebServer(CreateOptions());

        if (_settingProvider.Value.WebApi.AllowCrossOrigin)
        {
            _webServer.WithCors();
        }

        _webServer.WithModule(_serviceProvider.GetRequiredService<BroadcastWebSocketModule>());
        _webServer.WithModule(new AuthGate(_settingProvider));
        _webServer.WithModule(_serviceProvider.GetRequiredService<IPBannerModule>());
        _webServer.WithWebApi(
            "/api",
            (module) =>
                module
                    .HandleHttpException(ApiHelper.HandleHttpException)
                    .HandleUnhandledException(ApiHelper.HandleException)
                    .WithController(() => _serviceProvider.GetRequiredService<ApiMap>())
        );

        _webServer.Start(_cancellationTokenSource.Token);
        _logger.LogInformation("WebApi服务器已启动");
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
            throw new InvalidOperationException("WebApi服务器不在运行中");
        }
        _cancellationTokenSource.Cancel();
        _webServer.Dispose();
        _logger.LogInformation("WebApi服务器已停止");
    }

    private WebServerOptions CreateOptions()
    {
        var options = new WebServerOptions();

        _settingProvider.Value.WebApi.UrlPrefixes.ToList().ForEach(options.AddUrlPrefix);
        if (_settingProvider.Value.WebApi.Certificate.Enable)
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
