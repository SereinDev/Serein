namespace Serein.Core.Models.Settings;

public class FunctionSetting
{
    public int JSEventMaxWaitingTime { get; set; } = 500;

    public string[] JSGlobalAssemblies { get; set; } = { "System" };

    public string[] JSPatternToSkipLoadingSpecifiedFile { get; set; } = { ".module.js" };

    public string? JSScriptToBeExecutedBeforeRunning { get; set; }

    public bool DisableBinderWhenServerClosed { get; set; }

    public string RegexForCheckingGameID { get; set; } = @"^[a-zA-Z0-9_\s-]{3,16}$";

    public string[] RegexForEnableMatchMuiltLines { get; set; } = { @"players\sonline:", "个玩家在线" };
}
