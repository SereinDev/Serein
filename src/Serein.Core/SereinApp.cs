using System.Reflection;

namespace Serein.Core;

public sealed class SereinApp
{
    private SereinApp() { }

    public static readonly string Version =
        typeof(SereinApp).Assembly.GetName().Version?.ToString() ?? "¿¿¿";
    public static readonly string? FullVersion = typeof(SereinApp)
        .Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
        ?.InformationalVersion;
    public static readonly AppType Type;
    public static readonly bool IsDebugConfiguration;

    static SereinApp()
    {
        Type = Assembly.GetEntryAssembly()?.GetName().Name switch
        {
            "Serein.Cli" => AppType.Cli,
            "Serein.Lite" => AppType.Lite,
            "Serein.Plus" => AppType.Plus,
            "Serein.Tests" => AppType.Tests,
            _ => AppType.Unknown,
        };

#if DEBUG
        IsDebugConfiguration = true;
#endif
    }
}
