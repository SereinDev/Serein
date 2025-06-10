namespace Serein.Core.Models.Settings;

public class SatoriSetting : NotifyPropertyChangedModelBase
{
    public string ApiUrl { get; set; } = "http://127.0.0.1:3000/";

    public string EventUrl { get; set; } = "ws://";

    public string ApiAccessToken { get; set; } = string.Empty;

    public string EventAccessToken { get; set; } = string.Empty;
}
