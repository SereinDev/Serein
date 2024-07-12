using System.Collections.Generic;
using System.ComponentModel;

namespace Serein.Core.Models.Settings;

public class SshSetting : INotifyPropertyChanged
{
    public bool Enable { get; set; }

    public string IpAddress { get; set; } = "127.0.0.1";

    public ushort Port { get; set; } = 22;

    public Dictionary<string, string> Users { get; set; } = new() { ["admin"] = "password" };

#pragma warning disable CS0067
    public event PropertyChangedEventHandler? PropertyChanged;
}
