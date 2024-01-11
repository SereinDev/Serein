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

    public bool ColorfulLog { get; set; } = true;

    public bool AwareDPI { get; set; }

    [AlsoNotifyFor(nameof(AutoUpdate))]
    public bool CheckUpdate { get; set; } = true;

    public uint MaxCacheLines { get; set; } = 250;

    public Theme Theme { get; set; }

    public string CliCommandHeader { get; set; } = "//";

    public LogLevel LogLevel { get; set; } = LogLevel.Information;

#pragma warning disable CS0067
    public event PropertyChangedEventHandler? PropertyChanged;
}
