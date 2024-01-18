using System.ComponentModel;

namespace Serein.Core.Models.Settings;

public class FunctionSetting : INotifyPropertyChanged
{
    public int PluginEventMaxWaitingTime { get; set; } = 500;

    public string[] JSGlobalAssemblies { get; set; } = { "System" };

    public string[] JSPatternToSkipLoadingSpecifiedFile { get; set; } = { ".module.js" };

    public bool DisableBinderWhenServerClosed { get; set; }

    public string RegexForCheckingGameID { get; set; } = @"^[a-zA-Z0-9_\s-]{3,16}$";

    public string[] PattenForEnableMatchMuiltLines { get; set; } = { @"players online:", "个玩家在线" };

#pragma warning disable CS0067
    public event PropertyChangedEventHandler? PropertyChanged;
}
