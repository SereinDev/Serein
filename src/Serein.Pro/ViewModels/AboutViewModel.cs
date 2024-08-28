using Serein.Core;

namespace Serein.Pro.ViewModels;
public class AboutViewModel
{
    public string Version { get; } = SereinApp.Version;

    public string DetailedVersion { get; } = SereinApp.FullVersion ?? "未知";

    public string AssemblyName { get; } = typeof(AboutViewModel).Assembly.ToString();

    public string AssemblyPath { get; } = typeof(AboutViewModel).Assembly.Location;
}
