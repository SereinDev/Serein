namespace Serein.Core.Models.Settings;

public class AutoRunSetting
{
    public bool StartServer { get; set; }

    public bool ConnectWS { get; set; }

    public int Delay { get; set; } = 5000;
}
