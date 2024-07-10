using System.IO;
using System.Reflection;

using Serein.Core.Utils;

namespace Serein.Core;

public partial class SereinApp
{
    public static readonly string Version =
     typeof(SereinApp).Assembly.GetName().Version?.ToString() ?? "¿¿¿";
    public static readonly string? FullVersion = typeof(SereinApp)
        .Assembly
        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
        ?.InformationalVersion;
    public static readonly AppType Type;
    public static readonly bool StartForTheFirstTime = !File.Exists(PathConstants.SettingFile);
    public static SereinApp? Current { get; private set; }

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
