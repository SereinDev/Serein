using PropertyChanged;
using Serein.Core.Models.Abstractions;

namespace Serein.Core.Models.Settings;

public class ApplicationSetting : NotifyPropertyChangedModelBase
{
    private bool _autoUpdate;

    public bool AutoUpdate
    {
        get => _autoUpdate && CheckUpdate;
        set => _autoUpdate = value && CheckUpdate;
    }

    [AlsoNotifyFor(nameof(AutoUpdate))]
    public bool CheckUpdate { get; set; } = true;

    public string CustomTitle { get; set; } = "{serein.version}";

    public bool EnableSentry { get; set; } = true;

    public Theme Theme { get; set; }

    public string CliCommandHeader { get; set; } = "//";

    public int PluginEventMaxWaitingTime { get; set; } = 500;

    public string[] JSGlobalAssemblies { get; set; } = ["System"];

    public string[] JSPatternToSkipLoadingSingleFile { get; set; } = [".module.js"];

    public bool DisableBindingManagerWhenServerClosed { get; set; }

    public string RegexForCheckingGameId { get; set; } = @"^[a-zA-Z0-9_\s\-]{3,16}$";

    public string[] PattenForEnableMatchingMuiltLines { get; set; } = [];
}
