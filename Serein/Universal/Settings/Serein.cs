namespace Serein.Settings
{
    internal class Serein
    {
        public bool ColorfulLog = true;
        public bool DPIAware;
        public bool EnableGetUpdate = true;
        public Pages Pages = new();
        public int MaxCacheLines = 250;
        public bool ThemeFollowSystem = true;
        public bool UseDarkTheme;
        public AutoRun AutoRun = new();
        public DevelopmentTool DevelopmentTool = new();
    }

    internal class AutoRun
    {
        public bool StartServer;
        public bool ConnectWS;
    }

    internal class DevelopmentTool
    {
        public bool EnableDebug;
        public bool DetailDebug;
        public int JSEventCoolingDownTime = 15;
        public string NOTE { get; } = "以上设置内容为开发专用选项，请在指导下修改";
    }

    internal class Pages
    {
        public bool
        ServerPanel = true,
        ServerPluginManager = true,
        RegexList = true,
        Task = true,
        Bot = true,
        Member = true,
        JSPlugin = true,
        Settings = true;
    }
}
