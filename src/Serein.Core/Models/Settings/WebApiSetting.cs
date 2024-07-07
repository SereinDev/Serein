using System.ComponentModel;

namespace Serein.Core.Models.Settings;

public class WebApiSetting : INotifyPropertyChanged
{
    public bool Enable { get; set; }

    public string[] UrlPrefixes { get; set; } = [];

    public bool AllowCrossOrigin { get; init; }

    public int MaxRequestsPerSecond { get; init; } = 50;

    public string[] WhiteList { get; init; } = [];

    public string[] AccessTokens { get; init; } = [];

    public CertificateSetting Certificate { get; init; } = new();

#pragma warning disable CS0067
    public event PropertyChangedEventHandler? PropertyChanged;
}
