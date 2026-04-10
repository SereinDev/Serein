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

    public int MaximumWaitTimeForPluginEvents { get; set; } = 500;

    public string[] JsDefaultAssemblies { get; set; } = ["System"];

    public string[] JsFilesToExcludeFromLoading { get; set; } = [".module.js"];

    public bool DisableBindingManagerWhenAllServersStopped { get; set; }

    public string GameIdValidationPattern { get; set; } = @"^[a-zA-Z0-9_\s\-]{3,16}$";

    public string[] MultiLineMatchingPatterns { get; set; } = [];
}
