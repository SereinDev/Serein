using System;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace Serein.Core;

public sealed class SereinApp
{
    private static SereinApp? s_sereinApp;
    private readonly ILogger<SereinApp> _logger;

    public static SereinApp GetCurrentApp()
    {
        return s_sereinApp ?? throw new InvalidOperationException("没有正在运行的 SereinApp 实例");
    }

    public SereinApp(ILogger<SereinApp> logger, IServiceProvider serviceProvider)
    {
        s_sereinApp = this;
        _logger = logger;
        Services = serviceProvider;

        AssemblyName = typeof(SereinApp).Assembly.FullName!;
        Version = typeof(SereinApp).Assembly.GetName().Version!;
        FullVersion = typeof(SereinApp)
            .Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!
            .InformationalVersion;
        Type = Assembly.GetEntryAssembly()?.GetName().Name switch
        {
            "Serein.Cli" => AppType.Cli,
            "Serein.Lite" => AppType.Lite,
            "Serein.Plus" => AppType.Plus,
            "Serein.Tests" => AppType.Tests,
            _ => AppType.Unknown,
        };

#if RELEASE
        IsReleaseConfiguration = true;
#endif

        try
        {
#pragma warning disable IL3000
            IsSingleFile = string.IsNullOrEmpty(Assembly.GetEntryAssembly()?.Location);
#pragma warning restore IL3000
        }
        catch { }

        _logger.LogDebug(
            "编译版本: {Version}, 全版本: {FullVersion}, 类型: {Type}, 单文件: {IsSingleFile}, 发布配置: {IsReleaseConfiguration}",
            Version,
            FullVersion,
            Type,
            IsSingleFile,
            IsReleaseConfiguration
        );

        if (!IsReleaseConfiguration)
        {
            _logger.LogWarning(
                "当前为开发版本，可能包含暂未发布的功能或测试代码。不建议在生产环境中使用"
            );
        }

        if (Debugger.IsAttached)
        {
            _logger.LogWarning("当前正在调试模式下运行，可能会影响性能和稳定性");
        }
    }

    [JsonIgnore]
    public IServiceProvider Services { get; }

    public string AssemblyName { get; }

    public Version Version { get; }

    public string FullVersion { get; }

    public AppType Type { get; }

    public bool IsReleaseConfiguration { get; }

    public bool IsSingleFile { get; }
}
