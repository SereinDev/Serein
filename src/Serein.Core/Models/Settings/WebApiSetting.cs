using System.ComponentModel;

namespace Serein.Core.Models.Settings;

public class WebApiSetting : INotifyPropertyChanged
{
    public bool Enable { get; set; }

    public string[] UrlPrefixes { get; set; } = ["http://127.0.0.1:50000/"];

    public bool AllowCrossOrigin { get; set; }

    public int MaxRequestsPerSecond { get; set; } = 50;

    public string[] WhiteList { get; set; } = [];

    public string[] AccessTokens { get; set; } = [];

    public CertificateSetting Certificate { get; init; } = new();

#pragma warning disable CS0067
    public event PropertyChangedEventHandler? PropertyChanged;
}
