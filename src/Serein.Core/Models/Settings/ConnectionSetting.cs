using System.Collections.Generic;
using System.ComponentModel;

namespace Serein.Core.Models.Settings;

public class ConnectionSetting : INotifyPropertyChanged
{
    public string? AccessToken { get; set; }

    public bool AutoEscape { get; set; }

    public bool AutoReconnect { get; set; }

    public bool SaveLog { get; set; }

    public bool OutputData { get; set; }

    public bool GivePermissionToAllAdmins { get; set; }

    public long[] Groups { get; set; } = [];

    public long[] Administrators { get; set; } = [];

    public Dictionary<string, string> Headers { get; set; } = [];

    public string Uri { get; set; } = "ws://127.0.0.1:8080";

    public bool UseReverseWebSocket { get; set; }

    public string[] SubProtocols { get; set; } = [];

#pragma warning disable CS0067
    public event PropertyChangedEventHandler? PropertyChanged;
}
