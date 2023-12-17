using System;
using System.Collections.Generic;

namespace Serein.Core.Models.Settings;

public class BotSetting
{
    public string? AccessToken { get; set; }

    public bool AutoEscape { get; set; }

    public bool AutoReconnect { get; set; }

    public bool EnableLog { get; set; }

    public bool EnbaleOutputData { get; set; }

    public bool EnbaleParseAt { get; set; } = true;

    public bool GivePermissionToAllAdmins { get; set; }

    public string[] GroupList { get; set; } = Array.Empty<string>();

    public string[] PermissionList { get; set; } = Array.Empty<string>();

    public Dictionary<string, string?> Headers { get; set; } = new();

    public string Uri { get; set; } = "ws://127.0.0.1:8080";
}
