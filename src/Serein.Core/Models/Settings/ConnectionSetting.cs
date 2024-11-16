using System.Collections.Generic;

namespace Serein.Core.Models.Settings;

public class ConnectionSetting : NotifyPropertyChangedModelBase
{
    public string AccessToken { get; set; } = string.Empty;

    public bool AutoEscape { get; set; }

    public bool AutoReconnect { get; set; }

    public bool SaveLog { get; set; }

    public bool OutputData { get; set; }

    public bool GrantPermissionToOwnerAndAdmins { get; set; }

    public long[] Groups { get; set; } = [];

    public long[] Administrators { get; set; } = [];

    public Dictionary<string, string> Headers { get; set; } = [];

    public string Uri { get; set; } = "ws://127.0.0.1:8080";

    public bool UseReverseWebSocket { get; set; }

    public string[] SubProtocols { get; set; } = [];
}
