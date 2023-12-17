namespace Serein.Core.Models.Settings;

public class SereinSetting
{
    public bool EnableAutoUpdate { get; set; }

    public bool EnableColorfulLog { get; set; } = true;

    public bool EnableDPIAware { get; set; }

    public bool EnableCheckUpdate { get; set; } = true;

    public uint MaxCacheLines { get; set; } = 250;

    public bool ThemeFollowSystem { get; set; } = true;

    public bool UseDarkTheme { get; set; }
}
