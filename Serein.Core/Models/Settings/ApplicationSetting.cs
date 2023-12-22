using Microsoft.Extensions.Logging;

namespace Serein.Core.Models.Settings;

public class ApplicationSetting
{
    public bool EnableAutoUpdate { get; set; }

    public bool ColorfulLog { get; set; } = true;

    public bool EnableDPIAware { get; set; }

    public bool EnableCheckUpdate { get; set; } = true;

    public uint MaxCacheLines { get; set; } = 250;

    public bool ThemeFollowSystem { get; set; } = true;

    public bool UseDarkTheme { get; set; }

    public string CliCommandHeader { get; set; } = "//";

    public LogLevel LogLevel { get; set; } = LogLevel.Information;
}
