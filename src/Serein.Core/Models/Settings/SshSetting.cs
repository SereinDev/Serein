using System.ComponentModel;

namespace Serein.Core.Models.Settings;

public class SshSetting : INotifyPropertyChanged
{
    public bool Enable { get; set; }

    public string IpAddress { get; set; } = string.Empty;

    public ushort Port { get; set; } = 22;

#pragma warning disable CS0067
    public event PropertyChangedEventHandler? PropertyChanged;
}
