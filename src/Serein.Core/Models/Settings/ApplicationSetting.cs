using System.ComponentModel;

using Microsoft.Extensions.Logging;

using PropertyChanged;

namespace Serein.Core.Models.Settings;

public class ApplicationSetting : INotifyPropertyChanged
{
    private bool _autoUpdate;

    public bool AutoUpdate
    {
        get => _autoUpdate && CheckUpdate;
        set => _autoUpdate = value && CheckUpdate;
    }

    [AlsoNotifyFor(nameof(AutoUpdate))]
    public bool CheckUpdate { get; set; } = true;

    public string CustomTitle { get; set; } = "{filename}";

    public uint MaxDisplayedLines { get; set; } = 250;

    public Theme Theme { get; set; }

    public string CliCommandHeader { get; set; } = "//";

    public LogLevel LogLevel { get; set; } = LogLevel.Information;

    public int PluginEventMaxWaitingTime { get; set; } = 500;

    public string[] JSGlobalAssemblies { get; set; } = ["System"];

    public string[] JSPatternToSkipLoadingSpecifiedFile { get; set; } = [".module.js"];

    public bool DisableBinderWhenServerClosed { get; set; }

    public string RegexForCheckingGameID { get; set; } = @"^[a-zA-Z0-9_\s-]{3,16}$";

    public string[] PattenForEnableMatchMuiltLines { get; set; } = [@"players online:", "个玩家在线"];

#pragma warning disable CS0067
    public event PropertyChangedEventHandler? PropertyChanged;
}
