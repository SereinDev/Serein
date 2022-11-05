namespace Serein.Settings
{
    internal class Serein
    {
        public bool ColorfulLog { get; set; } = true;
        public bool Debug { get; set; } = false;
        public bool DPIAware { get; set; } = false;
        public bool EnableGetUpdate { get; set; } = true;
        public int MaxCacheLines { get; set; } = 100;
        public bool ThemeFollowSystem { get; set; } = true;
        public bool UseDarkTheme { get; set; } = false;
    }
}
