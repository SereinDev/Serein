using System;
using System.IO;
using System.Linq;
using System.Threading;

using EmbedIO;
using EmbedIO.WebApi;

using Microsoft.Extensions.Hosting;

using Serein.Core.Models.Output;
using Serein.Core.Services.Data;
using Serein.Core.Services.Networks.WebApi.Apis;

using Swan.Logging;

namespace Serein.Core.Services.Networks.WebApi;

public class HttpServer(IHost host, ISereinLogger logger, SettingProvider settingProvider)
{
    static HttpServer()
    {
        Logger.UnregisterLogger<ConsoleLogger>();
    }

    private readonly IHost _host = host;
    private readonly ISereinLogger _logger = logger;
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
        _webServer.WithWebApi("/api", (module) => module.WithController(() => new ApiMap(_host)));

        _webServer.Start(_cancellationTokenSource.Token);
    }

    public void Stop()
    {
        if (State == WebServerState.Stopped || _webServer is null)
            throw new InvalidOperationException("Web服务器不在运行中");

        _cancellationTokenSource.Cancel();
        _webServer.Dispose();
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
                options.Certificate = string.IsNullOrEmpty(
                    _settingProvider.Value.WebApi.Certificate.Password
                )
                    ? new(_settingProvider.Value.WebApi.Certificate.Path!)
                    : new(
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
