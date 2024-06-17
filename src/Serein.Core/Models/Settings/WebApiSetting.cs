using System;
using System.ComponentModel;

namespace Serein.Core.Models.Settings;

public class WebApiSetting : INotifyPropertyChanged
{
    public bool Enable { get; set; }

    public string[] UrlPrefixes { get; set; } = Array.Empty<string>();

    public bool AllowCrossOrigin { get; init; }

    public int MaxRequestsPerSecond { get; init; } = 50;

    public string[] WhiteList { get; init; } = Array.Empty<string>();

    public string[] AccessTokens { get; init; } = Array.Empty<string>();

    public CertificateSetting Certificate { get; init; } = new();

#pragma warning disable CS0067
    public event PropertyChangedEventHandler? PropertyChanged;
}
