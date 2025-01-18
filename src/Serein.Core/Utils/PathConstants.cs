namespace Serein.Core.Utils;

/// <summary>
/// 路径常量
/// </summary>
public static class PathConstants
{
    public static readonly string Root = "Serein";

    public static readonly string ConsoleHistory = "Serein/.console-input-history";

    public static readonly string LogDirectory = "Serein/logs";

    public static readonly string ServerLogDirectory = "Serein/logs/servers/{0}";

    public static readonly string ConnectionLogDirectory = "Serein/logs/connection";

    public static readonly string AppLogFile = "Serein/logs/app/{0}#{1}.log";

    public static readonly string BindingRecordsFile = "Serein/binding-records.sqlite.db";

    public static readonly string MatchesFile = "Serein/matches.json";

    public static readonly string PermissionGroupsFile = "Serein/permission-groups.json";

    public static readonly string SchedulesFile = "Serein/schedules.json";

    public static readonly string SettingFile = "Serein/setting.json";

    public static readonly string PluginsDirectory = "Serein/plugins";

    public static readonly string PluginInfoFileName = "plugin-info.json";

    public static readonly string LocalStorageFileName = "local-storage.json";

    public static readonly string JsPluginConfigFileName = "js-config.json";

    public static readonly string ServerConfigFile = "Serein/servers/{0}.json";

    public static readonly string ServerConfigDirectory = "Serein/servers";
    public static readonly string WebRoot = "Serein/web";
}
