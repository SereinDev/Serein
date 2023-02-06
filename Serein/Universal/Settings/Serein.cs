﻿namespace Serein.Settings
{
    internal class Serein
    {
        public bool AutoUpdate;
        public bool ColorfulLog = true;
        public bool DPIAware;
        public bool EnableGetUpdate = true;
        public int MaxCacheLines = 250;
        public bool ThemeFollowSystem = true;
        public bool UseDarkTheme;
        public AutoRun AutoRun = new();
        public DevelopmentTool DevelopmentTool = new();
        public Function Function = new();
        public Pages Pages = new();
    }

    internal class AutoRun
    {
        public bool StartServer, ConnectWS;
        public int Delay;
    }

    internal class DevelopmentTool
    {
        public bool EnableDebug, DetailDebug;
        public string NOTE { get; } = "以上设置内容为开发专用选项，请在指导下修改";
    }

    internal class Pages
    {
        public bool
        ServerPanel = true,
        ServerPluginManager = true,
        RegexList = true,
        Schedule = true,
        Bot = true,
        Member = true,
        JSPlugin = true,
        Settings = true;
    }

    internal class Function
    {
        public int JSEventMaxWaitingTime = 500;
        public int JSEventCoolingDownTime = 15;
        public bool DisableBinderWhenServerClosed;
    }
}
