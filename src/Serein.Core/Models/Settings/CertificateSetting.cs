using System.ComponentModel;

namespace Serein.Core.Models.Settings;

public class CertificateSetting : INotifyPropertyChanged
{
    public bool Enable { get; init; }

    public bool AutoRegisterCertificate { get; init; }

    public bool AutoLoadCertificate { get; init; }

    public string? Path { get; init; }

    public string? Password { get; init; }

#pragma warning disable CS0067
    public event PropertyChangedEventHandler? PropertyChanged;
}
