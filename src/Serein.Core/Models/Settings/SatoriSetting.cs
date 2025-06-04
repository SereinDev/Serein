namespace Serein.Core.Models.Settings;

public class SatoriSetting
{
    public string ApiUrl { get; set; } = "http://127.0.0.1:3000/v1/";

    public string EventUrl { get; set; } = "ws://";

    public string ApiAccessToken { get; set; } = string.Empty;

    public string EventAccessToken { get; set; } = string.Empty;
}
