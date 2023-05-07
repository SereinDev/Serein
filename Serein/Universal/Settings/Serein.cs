using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serein.Extensions;
using System.Text.RegularExpressions;

namespace Serein.Settings
{
    [JsonObject(MemberSerialization.OptOut, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
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
        public PagesDisplayed PagesDisplayed = new();
    }

    [JsonObject(MemberSerialization.OptOut, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    internal class AutoRun
    {
        public bool StartServer, ConnectWS;
        public int Delay;
    }

    [JsonObject(MemberSerialization.OptOut, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    internal class DevelopmentTool
    {
        public bool EnableDebug, DetailDebug;
        public string NOTE { get; } = "以上设置内容为开发专用选项，请在指导下修改";
    }

    [JsonObject(MemberSerialization.OptOut, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    internal class PagesDisplayed
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

    [JsonObject(MemberSerialization.OptOut, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    internal class Function
    {
        public bool NoHeartbeat;
        public int JSEventMaxWaitingTime = 500;
        public int JSEventCoolingDownTime = 15;
        public bool DisableBinderWhenServerClosed;
        public string RegexForCheckingGameID = @"^[a-zA-Z0-9_\s-]{3,16}$";
    }
}
