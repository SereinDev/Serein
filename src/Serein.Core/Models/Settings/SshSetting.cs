using System.Collections.Generic;

namespace Serein.Core.Models.Settings;

public class SshSetting : NotifyPropertyChangedModelBase
{
    public bool Enable { get; set; }

    public string IpAddress { get; set; } = "127.0.0.1";

    public ushort Port { get; set; } = 22;

    public Dictionary<string, string> Users { get; set; } = new() { ["admin"] = "password" };
}
