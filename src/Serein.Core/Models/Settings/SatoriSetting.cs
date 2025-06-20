using Serein.Core.Models.Abstractions;

namespace Serein.Core.Models.Settings;

public class SatoriSetting : NotifyPropertyChangedModelBase
{
    public string Uri { get; set; } = "http://127.0.0.1:5140/satori/";

    public string AccessToken { get; set; } = string.Empty;
}
