using System;
using System.Reflection;

namespace Serein.Core;

public sealed class SereinApp
{
    private static SereinApp? s_sereinApp;

    public static SereinApp GetCurrentApp()
    {
        return s_sereinApp ?? throw new InvalidOperationException("没有正在运行的 SereinApp 实例");
    }

    public SereinApp(IServiceProvider serviceProvider)
    {
        s_sereinApp = this;

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
    }

    public IServiceProvider Services { get; }

    public string AssemblyName { get; }

    public Version Version { get; }

    public string FullVersion { get; }

    public AppType Type { get; }

    public bool IsReleaseConfiguration { get; }

    public bool IsSingleFile { get; }
}
