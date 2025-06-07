using System.Collections.Generic;
using Serein.ConnectionProtocols.Models.OneBot;

namespace Serein.Core.Models.Settings;

public class OneBotSetting : NotifyPropertyChangedModelBase
{
    public OneBotVersion Version { get; set; }

    public string AccessToken { get; set; } = string.Empty;

    public bool AutoEscape { get; set; }

    public bool GrantPermissionToGroupOwnerAndAdmins { get; set; }

    public IDictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

    public string Uri { get; set; } = "ws://127.0.0.1:8080";

    public string[] SubProtocols { get; set; } = [];
}
