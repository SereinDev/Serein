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

    static SereinApp()
    {
        Type = Assembly.GetEntryAssembly()?.GetName().Name switch
        {
            "Serein.Cli" => AppType.Cli,
            "Serein.Lite" => AppType.Lite,
            "Serein.Plus" => AppType.Plus,
            _ => AppType.Unknown,
        };
    }
}
