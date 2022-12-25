namespace Serein.Settings
{
    internal class Serein
    {
        public bool ColorfulLog = true;
        public bool DPIAware = false;
        public bool EnableGetUpdate = true;
        public bool ThemeFollowSystem = true;
        public bool UseDarkTheme = false;
        public AutoRun AutoRun = new AutoRun();
        public DevelopmentTool DevelopmentTool = new DevelopmentTool();
    }

    internal class AutoRun
    {
        public bool StartServer = false;
        public bool ConnectWS = false;
    }

    internal class DevelopmentTool
    {
        public bool EnableDebug = false;
        public bool DetailDebug = false;
        public int JSEventCoolingDownTime = 15;
        public int MaxCacheLines = 100;
        public string NOTE { get; } = "以上设置内容为开发专用选项，请在指导下修改";
    }
}
