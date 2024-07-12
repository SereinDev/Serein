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

public class HttpServer(IHost host, MS.ILogger logger, SettingProvider settingProvider)
{
    static HttpServer()
    {
        Logger.NoLogging();
    }

    private readonly IHost _host = host;
    private readonly MS.ILogger _logger = logger;
    private readonly SettingProvider _settingProvider = settingProvider;
    private WebServer? _webServer;
    private WebServerState State => _webServer?.State ?? WebServerState.Stopped;
    private CancellationTokenSource _cancellationTokenSource = new();

    public void Start()
    {
        if (State != WebServerState.Stopped)
            throw new InvalidOperationException("Web服务器正在运行中");

        if (_cancellationTokenSource.IsCancellationRequested)
            _cancellationTokenSource = new();

        _webServer = new WebServer(CreateOptions());

        if (_settingProvider.Value.WebApi.AllowCrossOrigin)
            _webServer.WithCors();

        _webServer.WithModule(new AuthGate(_settingProvider));
        _webServer.WithModule(_host.Services.GetRequiredService<IPBannerModule>());
        _webServer.WithWebApi(
            "/api",
            (module) =>
                module
                    .HandleHttpException(ApiHelper.HandleHttpException)
                    .HandleUnhandledException(ApiHelper.HandleException)
                    .WithController(() => _host.Services.GetRequiredService<ApiMap>())
        );

        _webServer.Start(_cancellationTokenSource.Token);
        _logger.LogInformation("Http服务器已启动");
    }

    public void Stop()
    {
        if (State == WebServerState.Stopped || _webServer is null)
            throw new InvalidOperationException("Web服务器不在运行中");

        _cancellationTokenSource.Cancel();
        _webServer.Dispose();
        _logger.LogInformation("Http服务器已停止");
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
                return options;

            if (File.Exists(_settingProvider.Value.WebApi.Certificate.Path))
                options.Certificate = new(
                    _settingProvider.Value.WebApi.Certificate.Path!,
                    _settingProvider.Value.WebApi.Certificate.Password
                );
            else
                throw new InvalidOperationException(
                    $"证书文件“{_settingProvider.Value.WebApi.Certificate.Path}”不存在"
                );
        }

        return options;
    }
}
