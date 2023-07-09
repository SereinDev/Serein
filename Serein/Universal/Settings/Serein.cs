using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Serein.Settings
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    internal class Serein
    {
        /// <summary>
        /// 自动更新
        /// </summary>
        public bool AutoUpdate;

        /// <summary>
        /// 彩色输出（仅命令行）
        /// </summary>
        public bool ColorfulLog = true;

        /// <summary>
        /// DPI感知（仅Winform）
        /// </summary>
        public bool DPIAware;

        /// <summary>
        /// 自动获取更新
        /// </summary>
        public bool EnableGetUpdate = true;

        /// <summary>
        /// 最大缓存行数（仅Winfom、WPF）
        /// </summary>
        public int MaxCacheLines = 250;
        
        /// <summary>
        /// 主题跟随系统（仅WPF）
        /// </summary>
        public bool ThemeFollowSystem = true;

        /// <summary>
        /// 使用暗黑主题（仅WPF）
        /// </summary>
        public bool UseDarkTheme;

        public AutoRun AutoRun = new();
       
        public DevelopmentTool DevelopmentTool = new();
       
        public Function Function = new();
       
        public PagesDisplayed PagesDisplayed = new();
    }

    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    internal class AutoRun
    {
        public bool StartServer, ConnectWS;

        /// <summary>
        /// 延迟
        /// </summary>
        public int Delay;
    }

    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    internal class DevelopmentTool
    {
        public bool EnableDebug, DetailDebug;
        public string NOTE { get; } = "以上设置内容为开发专用选项，请在指导下修改";
    }

    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
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

    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    internal class Function
    {
        /// <summary>
        /// 禁用心跳
        /// </summary>
        public bool NoHeartbeat;

        /// <summary>
        /// JS事件最大等待时间
        /// </summary>
        public int JSEventMaxWaitingTime = 500;

        /// <summary>
        /// JS事件冷却时间
        /// </summary>
        public int JSEventCoolingDownTime = 15;

        /// <summary>
        /// JS全局程序集
        /// </summary>
        public string[] JSGlobalAssemblies = { "System" };

        /// <summary>
        /// 忽略加载的文件后缀
        /// </summary>
        public string[] JSPatternToSkipLoadingSpecifiedFile = { ".module.js" };

        /// <summary>
        /// JS预执行代码
        /// </summary>
        public string? JSScriptToPreExecute;

        /// <summary>
        /// 服务器关闭时禁用绑定功能
        /// </summary>
        public bool DisableBinderWhenServerClosed;

        /// <summary>
        /// 检查游戏ID的正则
        /// </summary>
        /// <value></value>
        public string RegexForCheckingGameID = @"^[a-zA-Z0-9_\s-]{3,16}$";
    }
}
