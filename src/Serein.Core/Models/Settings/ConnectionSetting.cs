using Serein.ConnectionProtocols.Models;
using Serein.Core.Models.Abstractions;
using Serein.Core.Models.Network.Connection;

namespace Serein.Core.Models.Settings;

public class ConnectionSetting : NotifyPropertyChangedModelBase
{
    public AdapterType Adapter { get; set; } = AdapterType.OneBot_ForwardWebSocket;

    public bool ConnectWhenSettingUp { get; set; }

    public bool SaveLog { get; set; }

    public bool OutputData { get; set; }

    public string[] ListenedIds { get; set; } = [];

    public string[] AdministratorUserIds { get; set; } = [];

    public Self Self { get; set; } = new();

    public OneBotSetting OneBot { get; set; } = new();

    public SatoriSetting Satori { get; set; } = new();
}
